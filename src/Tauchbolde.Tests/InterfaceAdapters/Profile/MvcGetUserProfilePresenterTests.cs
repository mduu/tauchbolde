using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Tauchbolde.Application.UseCases.Profile;
using Tauchbolde.InterfaceAdapters.Profile;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.Profile
{
    [UseReporter(typeof(DiffReporter))]
    public class MvcGetUserProfilePresenterTests
    {
        private readonly MvcGetUserProfilePresenter presenter;

        public MvcGetUserProfilePresenterTests()
        {
            presenter = new MvcGetUserProfilePresenter();
        }

        [Fact]
        public void Output_NullOutput_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Action act = () => presenter.Output(null);

            // Arrange
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("interactorOutput");
        }

        [Fact]
        public void Output_Success()
        {
            // Arrange
            var output = CreateUserProfileOutput();

            // Act
            presenter.Output(output);
            var viewModel = presenter.GetViewModel();

            // Arrange
            viewModel.Should().NotBeNull();
            Approvals.VerifyJson(JsonConvert.SerializeObject(viewModel));
        }

        private GetUserProfileOutput CreateUserProfileOutput() =>
            new GetUserProfileOutput(
                true,
                DiverFactory.JohnDoeDiverId,
                new[] {"member", "admin"},
                DiverFactory.JohnDoeEmail,
                DiverFactory.JohnDoeAvatarId,
                DiverFactory.JohnDoeFullname,
                DiverFactory.JohnDoeFirstName,
                DiverFactory.JohnDoeLastName,
                new DateTime(2007, 7, 1),
                "My Slogan",
                "My Education",
                "My Experience",
                "+41 777 66 55",
                "https://tauchbolde.ch",
                "@johndoe",
                "johndoe1",
                "john.doe"
            );
    }
}