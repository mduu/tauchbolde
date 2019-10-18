using ApprovalTests.Reporters;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase;
using Tauchbolde.InterfaceAdapters.Profile.GetEditAvatar;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;
using static Tauchbolde.Tests.TestingTools.ApprovalsExtensions;

namespace Tauchbolde.Tests.InterfaceAdapters.Profile
{
    [UseReporter(typeof(DiffReporter))]
    public class MvcGetEditAvatarPresenterTests
    {
        private readonly MvcGetEditAvatarPresenter presenter = new MvcGetEditAvatarPresenter();

        [Fact]
        public void Output_Success_Approval()
        {
            // Arrange
            var output = new GetEditAvatarOutput(DiverFactory.JohnDoeDiverId, DiverFactory.JohnDoeFullname);
            
            // Act
            presenter.Output(output);
            var viewModel = presenter.GetViewModel();

            // Assert
            viewModel.Realname.Should().Be(DiverFactory.JohnDoeFullname);
            viewModel.UserId.Should().Be(DiverFactory.JohnDoeDiverId);
            VerifyObjectJson(viewModel);
        }
    }
}