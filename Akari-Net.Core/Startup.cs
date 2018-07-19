using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Akari_Net.Core.Extensions;
using Akari_Net.Core.Models.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Akari_Net.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             {
                 // Password settings
                 var passSettings = Configuration.GetSection("PasswordSettings");

                 options.Password.RequireDigit = passSettings.GetValue<bool>("RequireDigit");
                 options.Password.RequiredLength = passSettings.GetValue<int>("RequiredLength");
                 options.Password.RequiredUniqueChars = passSettings.GetValue<int>("RequiredUniqueChars");
                 options.Password.RequireLowercase = passSettings.GetValue<bool>("RequireLowercase");
                 options.Password.RequireNonAlphanumeric = passSettings.GetValue<bool>("RequireNonAlphanumeric");
                 options.Password.RequireUppercase = passSettings.GetValue<bool>("RequireUppercase");
                 
                 //Lockout settings
                 var lockoutSettings = Configuration.GetSection("Lockout");
                 options.Lockout.MaxFailedAccessAttempts = lockoutSettings.GetValue<int>("MaxFailedAccessAttempts");
                 options.Lockout.DefaultLockoutTimeSpan = lockoutSettings.GetValue<TimeSpan>("DefaultLockoutTimeSpan");
             })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicies(new PoliciesManager());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                             .AddRazorPagesOptions(options =>
                             {
                                 options.AllowAreas = true;
                                 options.Conventions.AuthorizeAreaFolder("Usuarios", "/Account/Manage");
                                 options.Conventions.AuthorizeAreaPage("Usuarios", "/Account/Logout");
                             });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Usuarios/Account/Login";
                options.LogoutPath = $"/Usuarios/Account/Logout";
                options.AccessDeniedPath = $"/Usuarios/Account/AccessDenied";
            });

            services.AddResponseCompression();

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IPoliciesManager, PoliciesManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider, loggerFactory);
        }

        private void CreateRoles(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "WebMaster", "Podólogo", "Administracion","Usuario" };

            try
            {
                foreach (var roleName in roleNames)
                {
                    var roleExist = RoleManager.RoleExistsAsync(roleName);
                    roleExist.Wait();
                    if (!roleExist.Result)
                    {
                        //create the roles and seed them to the database: Question 1
                        RoleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("RoleCreation");
                logger.LogCritical(ex, "Error al crear los roles");
            }
        }
    }
}
