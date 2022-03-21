using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class ExportIcalStreamInteractorTests
    {
        private readonly Guid validEventId = new("66F6BCD0-FB87-417A-9CD3-A26263C00B87");
        private readonly ExportIcalStreamInteractor interactor;
        private readonly IEventRepository repository = A.Fake<IEventRepository>();
        private readonly IExportIcalStreamOutputPort outputPort = A.Fake<IExportIcalStreamOutputPort>();
        private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();

        public ExportIcalStreamInteractorTests()
        {
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    new Tauchbolde.Domain.Entities.Event
                    {
                        Id = (Guid)call.Arguments[0],
                        Name = "Test-Event",
                        Description = "Description",
                        Location = "The Dive Spot",
                        MeetingPoint = "5 Minutes before",
                        StartTime = new DateTime(2019, 8, 1, 19, 0, 0),
                    }));

            interactor = new ExportIcalStreamInteractor(repository, telemetryService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new ExportIcalStream(validEventId, outputPort);
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<ExportIcalStreamOutput>.That.Matches(o => o.IcalStream != null)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.IcalRequested, A<object>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }
        
        [Fact]
        public async Task Handle_EventNotFound_MustFail()
        {
            // Arrange
            var request = new ExportIcalStream(validEventId, outputPort);
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(() => Task.FromResult<Tauchbolde.Domain.Entities.Event>(null));
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<ExportIcalStreamOutput>._))
                .MustNotHaveHappened();
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.IcalRequested, A<object>._))
                .MustNotHaveHappened();
        }
    }
}