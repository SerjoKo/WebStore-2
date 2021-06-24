﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context.WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entitys.Identity;

namespace WebStore.Services.Data
{
    public class WSDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WSDBInitializer> _Logger;

        public WSDBInitializer(
            WebStoreDB db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WSDBInitializer> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _Logger = Logger;
        }

        public void Initialize()
        {
            _Logger.LogInformation("Инициализация БД...");
            var timer = Stopwatch.StartNew();

            //_db.Database.EnsureDeleted();
            //_db.Database.EnsureCreated();

            if (_db.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Миграция БД...");
                _db.Database.Migrate();
                _Logger.LogInformation("Миграция БД выполнена за {0}c", timer.Elapsed.TotalSeconds);
            }
            else
                _Logger.LogInformation("Миграция БД не требуется. {0}c", timer.Elapsed.TotalSeconds);

            try
            {
                InitializeProducts();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Ошибка при инициализации товаров в БД");
                throw;
            }

            try
            {
                InitializeIdentityAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Ошибка при инициализации данных БД системы Identity");
                throw;
            }


            try
            {
                AddEmploees();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Ошибка при инициализации данных БД сотрудников");
                throw;
            }

            _Logger.LogInformation("Инициализация БД завершена за {0} с", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any())
            {
                _Logger.LogInformation("БД не нуждается в инициализации товаров");
                return;
            }

            #region Грохнул
            // Секции        
            //Logger.LogInformation("Инициализации таблицы секций");

            //using (db.Database.BeginTransaction())
            //{
            //    db.Sections.AddRange(TestData.Sections);

            //    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
            //    db.SaveChanges();
            //    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

            //    db.Database.CommitTransaction();
            //}

            //Logger.LogInformation("Инициализации таблицы секций завершена");

            //// Бренды        
            //Logger.LogInformation("Инициализации таблицы брендов");

            //using (db.Database.BeginTransaction())
            //{
            //    db.Brands.AddRange(TestData.Brands);

            //    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
            //    db.SaveChanges();
            //    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

            //    db.Database.CommitTransaction();
            //}

            //Logger.LogInformation("Инициализации таблицы брендов завершена");

            //// Товары
            //Logger.LogInformation("Инициализации таблицы товаров");
            #endregion

            var timer = Stopwatch.StartNew();

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var section in TestData.Sections.Where(s => s.ParentId != null))
                section.Parent = sections_pool[(int)section.ParentId!];

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brand_id)
                    product.Brand = brands_pool[brand_id];

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
                brand.Id = 0;


            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Инициализация товаров выполнена за. {0} c", timer.Elapsed.TotalSeconds);
        }

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Инициализация БД системы Identity");
            var timer = Stopwatch.StartNew();

            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Роль {0} отсутствует. Создаю...", RoleName);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Роль {0} создана успешно", RoleName);
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Пользователь {0} отсутствует. Создаю...", User.Administrator);

                var admin = new User
                {
                    UserName = User.Administrator
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.AdmPass);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Пользователь {0} успешно создан", User.Administrator);

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("Пользователь {0} наделён ролью {1}",
                        User.Administrator, Role.Administrators);
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description).ToArray();
                    _Logger.LogError("Учётная запись администратора не создана по причине: {0}",
                        string.Join(",", errors));

                    throw new InvalidOperationException($"Ошибка при создании пользователя {User.Administrator}:{string.Join(",", errors)}");
                }
            }

            _Logger.LogInformation("Инициализация данных БД системы Identity выполнена за {0} c",
                timer.Elapsed.TotalSeconds);
        }

        private void AddEmploees()
        {
            if (_db.Employees.Any())
                return;

            TestData.Employees.ForEach(e => e.Id = 0);
            _db.Employees.AddRange(TestData.Employees);
            _db.SaveChanges();
        }
    }
}