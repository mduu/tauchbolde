using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.SharedKernel
{
    public static class SharedKernelRegistrations
    {
        public static void RegisterServices([NotNull] IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}