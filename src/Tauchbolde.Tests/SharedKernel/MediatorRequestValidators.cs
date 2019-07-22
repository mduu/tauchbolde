using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.SharedKernel
{
    public class MediatorRequestValidators
    {
        private readonly IMediator mediator;

        public MediatorRequestValidators()
        {
            var services = new ServiceCollection();
            SharedKernelRegistrations.RegisterServices(services);
            services.AddMediatR(typeof(MyRequest).Assembly);
            services.AddTransient<IValidator<MyRequest>, MyRequestValidator>();
            var container = services.BuildServiceProvider();

            mediator = container.GetService<IMediator>();
        }

        [Fact]
        public async Task ValidateRequest_Fail()
        {
            var result = await mediator.Send(new MyRequest(1), CancellationToken.None);

            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task ValidateRequest_Success()
        {
            var result = await mediator.Send(new MyRequest(2), CancellationToken.None);

            result.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}