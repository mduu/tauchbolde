using System;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.UseCases.Logbook.Publish;

namespace Tauchbolde.UseCases.Logbook
{
    public static class LogbookUseCasesRegistration
    {
        public static void RegisterServices([NotNull] IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddMediatR(typeof(PublishLogbookEntryHandler));
        }
    }
}