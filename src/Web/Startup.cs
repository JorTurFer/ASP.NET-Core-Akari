using Akari_Net.Core.Areas.Pacientes.Hubs;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Usuarios.Models.Entities;
using Akari_Net.Core.Areas.Usuarios.Models.Services;
using Akari_Net.Core.Models.Entities;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Web.Areas.Facturas.Extensions;
using Web.Areas.Pacientes.Models.Services;
using Web.Areas.Usuarios.Data;
using Web.Models.Services;

namespace Web
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
            //Añado application insights
            services.AddApplicationInsightsTelemetry();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights();
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //Añado el contexto de usuarios
            var builderUsers = new SqlConnectionStringBuilder(
            Configuration.GetConnectionString("Akari"))
            {
                Password = Configuration["ConnectionStringPassword"],
                UserID = Configuration["ConnectionStringUser"],
                InitialCatalog = Configuration["UsuariosDB"]
            };

            services.AddDbContext<UsersDbContext>(options =>
               options.UseSqlServer(builderUsers.ConnectionString, x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Identity")));

            //Añado el contexto de pacientes
            var builderPacientes = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("Akari"))
            {
                Password = Configuration["ConnectionStringPassword"],
                UserID = Configuration["ConnectionStringUser"],
                InitialCatalog = Configuration["PacientesDB"]
            };

            services.AddDbContext<PatientsDbContext>(options =>
            {
                //options.UseLazyLoadingProxies();
                options.UseSqlServer(builderPacientes.ConnectionString,
                    x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Patients"));
            });

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
           .AddEntityFrameworkStores<UsersDbContext>()
           .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPermissions(new PermissionService());
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
            services.AddSingleton<ISmsSender, SmsSender>();
            services.AddHostedService<TimedHostedService>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<SMSServiceOptions>(Configuration);
            services.Configure<AccountConfirmationOptions>(Configuration);
            services.AddSingleton<IPermissionService, PermissionService>();
            services.AddScoped<IPacientesService, PacientesService>();
            services.AddScoped<ICalendarioServices, CalendarioServices>();

            services.AddFacturas();

            services.AddSignalR()
                .AddMessagePackProtocol();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            var logger = loggerFactory.CreateLogger(typeof(Startup));
            logger.LogCritical("Application Started");

            app.UseHttpsRedirection();

            //Añado el redireccionamiento para las cabeceras
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var cultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseSignalR(routes =>
            {
                routes.MapHub<CalendarioHub>("/CalendarioHub");
            });

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
