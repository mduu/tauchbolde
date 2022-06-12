using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.InterfaceAdapters.WebApi.Presenters.Event.List;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.WebApi.Event
{
    [UseReporter(typeof(DiffReporter))]
    public class WebApiEventListPresenterTests
    {
        [NotNull] private readonly WebApiEventListPresenter presenter;

        public WebApiEventListPresenterTests()
        {
            presenter = new WebApiEventListPresenter();
        }

        [Fact]
        public void Output_Success_ApproveApiSurface()
        {
            // Arrange
            var output = new GetEventListOutput(
                new List<GetEventListOutput.EventRow>
                {
                    new GetEventListOutput.EventRow(
                        new Guid("C76468FE-86AE-43EC-B2D5-28FC61DA04AA"),
                        new DateTime(2019, 10, 5, 19, 0, 0),
                        null,
                        "The Title",
                        "The Location",
                        "The MeetingPoint"
                    )
                });
        
            // Act
            presenter.Output(output);
            var jsonObject = presenter.GetJsonObject();

            // Assert
            var json = JsonConvert.SerializeObject(jsonObject);
            Approvals.VerifyJson(json);
        }

        [Fact]
        public void Output_NullOutput_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Action act = () => presenter.Output(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("interactorOutput");
        }
    }
}