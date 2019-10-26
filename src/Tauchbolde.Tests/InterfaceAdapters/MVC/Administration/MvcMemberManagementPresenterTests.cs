using System;
using System.Collections.Generic;
using ApprovalTests.Reporters;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.MemberManagement;
using Tauchbolde.Tests.TestingTools;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.MVC.Administration
{
    [UseReporter(typeof(DiffReporter))]
    public class MvcMemberManagementPresenterTests
    {
        private readonly MvcMemberManagementPresenter presenter;

        public MvcMemberManagementPresenterTests()
        {
            presenter = new MvcMemberManagementPresenter();
        }

        [Fact]
        public void Output_Success()
        {
            // Arrange
            var output = CreateOutput();

            // Act
            presenter.Output(output);
            var viewModel = presenter.GetViewModel();

            // Assert
            viewModel.Should().NotBeNull();
            ApprovalsExtensions.VerifyObjectJson(viewModel);
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

        private MemberManagementOutput CreateOutput() =>
            new MemberManagementOutput(
                new List<MemberManagementOutput.Member>
                {
                    new MemberManagementOutput.Member(
                        DiverFactory.JohnDoeDiverId,
                        DiverFactory.JohnDoeFullname,
                        DiverFactory.JohnDoeUserName,
                        DiverFactory.JohnDoeEmail,
                        true,
                        false,
                        new[] {"admin", "tauchbold"})
                },
                new []{ "user33", "user44"});
    }
}