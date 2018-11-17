using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Common.Model;
using Tauchbolde.Common;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using Tauchbolde.Common.DomainServices.SMTPSender;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;

namespace Tauchbolde.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureServices(services);
            ApplicationServices.RegisterDevelopment(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureServices(services);
            ApplicationServices.RegisterProduction(services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpSenderConfiguration>(Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            ConfigureDatabase(services);

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.Name = GlobalConstants.AuthCookieName;
            });

            // External authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.RequireTauchbold, policy => policy.RequireRole(Rolenames.Tauchbold));
                options.AddPolicy(PolicyNames.RequireAdministrator, policy => policy.RequireRole(Rolenames.Administrator));
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("de-CH");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("de-CH") };
                options.RequestCultureProviders = new List<IRequestCultureProvider>();
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(
                    async context => await Task.FromResult(new ProviderCultureResult("de"))
                ));
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowTwitter", builder => builder
                    .WithOrigins("https://platform.twitter.com")
                    .WithOrigins("https://twitter.com"));
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            ApplicationServices.Register(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("TauchboldeConnection");
            var builder = new SqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrWhiteSpace(Configuration["DbUser"]))
            {
                builder.UserID = Configuration["DbUser"];
            }
            if (!string.IsNullOrWhiteSpace(Configuration["DbPassword"]))
            {
                builder.Password = Configuration["DbPassword"];
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.ConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseRequestLocalization()
                .UseCors()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
