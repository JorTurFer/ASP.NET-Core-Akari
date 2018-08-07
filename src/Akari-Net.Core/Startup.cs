using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Akari_Net.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

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

            //Añado el contexto de usuarios
            var builderUsers = new MySqlConnectionStringBuilder(
            Configuration.GetConnectionString("Akari"));
            builderUsers.Password = Configuration["ConnectionStringPassword"];
            builderUsers.UserID = Configuration["ConnectionStringUser"];
            builderUsers.Database = Configuration["UsuariosDB"];

            services.AddDbContext<UsuariosDbContext>(options =>
               options.UseMySql(builderUsers.ConnectionString));

            //Añado el contexto de pacientes
            var builderPacientes = new MySqlConnectionStringBuilder(
            Configuration.GetConnectionString("Akari"));
            builderPacientes.Password = Configuration["ConnectionStringPassword"];
            builderPacientes.UserID = Configuration["ConnectionStringUser"];
            builderPacientes.Database = Configuration["PacientesDB"];
            
            services.AddDbContext<PacientesDbContext>(options =>
              options.UseMySql(builderPacientes.ConnectionString));

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

                 //Confirmacion de correo electronico
                 options.SignIn.RequireConfirmedEmail = Configuration.GetValue<bool>("EmailConfirmationRequired");
             })
           .AddEntityFrameworkStores<UsuariosDbContext>()
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
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<AccountConfirmationOptions>(Configuration);
            services.AddSingleton<IPoliciesManager, PoliciesManager>();
            services.AddScoped<IPacientesService, PacientesService>();
            services.AddScoped<ICalendarioServices, CalendarioServices>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            //Añado el redireccionamiento para las cabeceras
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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
        }
    }
}
