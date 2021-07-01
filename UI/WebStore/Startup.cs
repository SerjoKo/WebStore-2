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
using WebStore.Domain.Entitys.Identity;
using WebStore.Inerfaces.Services;
using WebStore.Inerfaces.TestAPI;
using WebStore.Inftastructure.MidleWare;
using WebStore.Services.Data;
using WebStore.Services.Services.InSQL;
using WebStore.Services.Servicess.InCookies;
using WebStore.Servicess.Interfaces;
using WebStore.WebApi.Clients.Employees;
using WebStore.WebApi.Clients.Order;
using WebStore.WebApi.Clients.Products;
using WebStore.WebApi.lients.Values;

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

            services.AddTransient<WSDBInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                //opt.User.AllowedUserNameCharacters = "����� ����������� ��������";
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

            services.AddScoped<ICartService, InCookiesCartService>();
            
            #region ����� ����� ��� �������
            //services.AddHttpClient<IValuesService, ValuesClient>
            //    (client => client.BaseAddress = new Uri(Configuration["WebApi"]));

            //services.AddHttpClient<IEmployeesData, EmployeesClient>
            //    (client => client.BaseAddress = new Uri(Configuration["WebApi"]));

            //services.AddHttpClient<IProductData, ProductClient>
            //    (client => client.BaseAddress = new Uri(Configuration["WebApi"]));

            //services.AddHttpClient<IOrderService, OrderClient>
            //    (client => client.BaseAddress = new Uri(Configuration["WebApi"]));
            #endregion
            
            services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))
               .AddTypedClient<IValuesService, ValuesClient>()
               .AddTypedClient<IEmployeesData, EmployeesClient>()
               .AddTypedClient<IProductData, ProductClient>()
               .AddTypedClient<IOrderService, OrderClient>()
                ;

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}