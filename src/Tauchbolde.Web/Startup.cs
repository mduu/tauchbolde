using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tauchbolde.Common;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Services;

namespace Tauchbolde.Web
{
    public class Startup
    {
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Caching Middleware
            services.AddResponseCaching();

            // ASP.Net Core Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");

            services.AddAuthentication()
                    .AddMicrosoftAccount(options =>
            {
                options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            });

            // ASP.Net Core MVC
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("OneDay",
                    new CacheProfile()
                    {
                        Duration = 86400 // 24h cache
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            });

            // Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.RequireTauchbold, policy => policy.RequireRole(Rolenames.Tauchbold));
            });

            // EF
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Tauchbolde.Web")));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<ApplicationDbContext>();

            // Repos
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IPostImageRepository, PostImageRepository>();

            // DomainServices
            services.AddTransient<IParticipationService, ParticipationService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<INotificationFormatter, HtmlNotificationFormatter>();

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddTransient<INotificationSubmitter, ConsoleNotificationSubmitter>();
            }
            else
            {
                services.AddTransient<INotificationSubmitter, SmtpNotificationSubmitter>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
