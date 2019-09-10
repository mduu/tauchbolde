using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Tauchbolde.InterfaceAdapters.Event.Details;
using Tauchbolde.InterfaceAdapters.Event.List;
using Tauchbolde.InterfaceAdapters.Event.RecentAndUpcomingEvents;
using Tauchbolde.InterfaceAdapters.MailHtmlFormatting;
using Tauchbolde.InterfaceAdapters.TextFormatting;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.InterfaceAdapters
{
    public static class InterfaceAdaptersRegistration
    {
        public static void RegisterServices([NotNull] IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddScoped<ITextFormatter, MarkdownDigFormatter>();
            
            services.AddTransient<INotificationFormatter, HtmlFormatter>();
            services.AddTransient<ICssStyleFormatter, CssStyleFormatter>();
            services.AddTransient<IHtmlListFormatter, HtmlListFormatter>();
            services.AddTransient<IHtmlHeaderFormatter, HtmlHeaderFormatter>();
            services.AddTransient<IHtmlFooterFormatter, HtmlFooterFormatter>();
            services.AddTransient<IEventDetailsOutputPort, MvcEventDetailPresenter>();
            services.AddTransient<IEventListOutputPort, MvcEventListPresenter>();
            services.AddTransient<IRecentAndUpcomingEventsOutputPort, MvcRecentAndUpcomingEventsPresenter>();
        }
    }
}