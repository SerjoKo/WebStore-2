using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context.WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entitys.Identity;
using WebStore.Inerfaces.Services;
using WebStore.Inftastructure.MidleWare;
using WebStore.Servicess.InCookies;
using WebStore.Servicess.InMemory;
using WebStore.Servicess.InSQL;
using WebStore.Servicess.Interfaces;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<ITestService, TestService>();
            //services.AddScoped<IPrinter, DebugPrinter>();

            var database_name = Configuration["Database"];

            switch (database_name)
            {
                case "WSDBSQL":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlServer(
                            Configuration.GetConnectionString("WSDBSQL")));
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlite(
                            Configuration.GetConnectionString("Sqlite"),
                            o => o.MigrationsAssembly("WebStore.DAL.SqLite")));
                    break;
            }

            //services.AddDbContext<WebStoreDB>(opt =>
            //    opt.UseSqlServer(
            //        Configuration.GetConnectionString("WSDBSQL")));

            services.AddTransient<WSDBInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt => {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                //opt.User.AllowedUserNameCharacters = "Набор разрешенных символов";
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WS_C";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(5);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //services.AddDbContext<WebStoreDB>(opt => 
            //    opt.UseSqlServer(Configuration.GetConnectionString("WSDBSQL")));

            services.AddScoped<IEmployeesData, SqlEmployeesData>();
            //services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            // оставить на всякий случай
            //services.AddSingleton<IProductData, InMemoryProductData>();
            //services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, InCookiesCartService>();

            if (Configuration["ProductsDataSource"] == "db")
                services.AddScoped<IProductData, SqlProductData>();
            else
                services.AddSingleton<IProductData, InMemoryProductData>();

            services.AddScoped<IOrderService, SqlOrderService>();
            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();

            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

           
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            //var test_service = services.GetRequiredService<ITestService>();

            //test_service.Test();

            using (var scope = services.CreateScope())
                scope.ServiceProvider.GetRequiredService<WSDBInitializer>().Initialize();
                

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseMiddleware<TestMidleWare>();

            app.UseWelcomePage("/WelcomePage");

            //var greetings = Configuration["Greetings"];
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    //await context.Response.WriteAsync(greetings);
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });


                endpoints.MapControllerRoute(
                    name: "areas", 
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    //interface ITestService
    //{
    //    void Test();
    //}

    //class TestService : ITestService
    //{
    //    private IPrinter _Printer;

    //    public TestService(IPrinter Printer)
    //    {
    //        _Printer = Printer;
    //    }

    //    public void Test()
    //    {
    //        _Printer.Printer("Запуск теста");
    //        //Debug.WriteLine("Запуск теста");
    //    }
    //}

    //interface IPrinter
    //{
    //    void Printer(string str);
    //}

    //class DebugPrinter : IPrinter
    //{
    //    public DebugPrinter()
    //    {

    //    }

    //    public void Printer(string str)
    //    {
    //        Debug.WriteLine(str);
    //    }
    //}
}