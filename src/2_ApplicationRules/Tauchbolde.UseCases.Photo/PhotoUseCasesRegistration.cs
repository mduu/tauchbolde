using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.UseCases.Logbook
{
    public static class PhotoUseCasesRegistration
    {
        public static void RegisterServices([NotNull] IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            // services.AddMediatR(typeof(PublishLogbookEntryHandler));
        }
    }
}