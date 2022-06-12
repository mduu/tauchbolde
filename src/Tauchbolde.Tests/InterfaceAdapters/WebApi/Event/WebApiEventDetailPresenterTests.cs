using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Domain.Types;
using Tauchbolde.InterfaceAdapters.WebApi.Presenters.Event.Details;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.WebApi.Event
{
    [UseReporter(typeof(DiffReporter))]
    public class WebApiEventDetailPresenterTests
    {
        private readonly WebApiEventDetailPresenter presenter;

        public WebApiEventDetailPresenterTests()
        {
            presenter = new WebApiEventDetailPresenter();
        }

        [Fact]
        public void Output_Success_ApproveApiSurface()
        {
            // Arrange
            GetEventDetailsOutput output = new GetEventDetailsOutput(
                new Guid("9F3C4AB1-C27A-4A6A-B95A-D83A9251F961"),
                "The Title",
                new Guid("0A3D24CE-FC83-4355-91EF-E593D168FC1A"),
                "John Doe",
                "The Location",
                "The MeetingPoint",
                "The description",
                new DateTime(2019, 10, 5, 19, 0, 0),
                new DateTime(2019, 10, 5, 22, 0, 0),
                false,
                false,
                new List<ParticipantOutput>
                {
                    new ParticipantOutput(
                        "Name",
                        "Email",
                        "AvatarId",
                        "Team 1",
                        ParticipantStatus.Accepted,
                        1,
                        "The Note")
                },
                new List<CommentOutput>
                {
                    new CommentOutput(
                        new Guid("93F4FAD3-425C-4447-89DE-AEB25E862E02"),
                        new Guid("4F969046-7899-4FBE-8C54-1482C2BF24D0"),
                        "John Doe",
                        "john.doe@company.com",
                        "avatarId",
                        new DateTime(2019, 10, 3, 12, 42, 0),
                        new DateTime(2019, 10, 4, 13, 42, 0),
                        "The comment text",
                        true,
                        true)
                },
                true,
                ParticipantStatus.Accepted,
                "The current Note",
                "Current Team 1",
                1);
      
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
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("output");
        }
    }
}