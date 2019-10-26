using System;
using System.Collections.Generic;
using ApprovalTests.Core.Exceptions;
using ApprovalTests.Reporters;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase;
using Tauchbolde.InterfaceAdapters.Administration.EditRoles;
using Tauchbolde.Tests.TestingTools;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.Administration
{
    [UseReporter(typeof(DiffReporter))]
    public class MvcEditRolesPresenterTests
    {
        private readonly MvcEditRolesPresenter presenter = new MvcEditRolesPresenter();

        [Fact]
        public void Output_Success()
        {
            // Arrange
            var interactorOutput = CreateValidInteractorOutput();

            // Act
            presenter.Output(interactorOutput);
            var viewModel = presenter.GetViewModel();

            // Assert
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

        private GetEditRolesOutput CreateValidInteractorOutput() => 
            new GetEditRolesOutput(
                DiverFactory.JohnDoeUserName,
                DiverFactory.JohnDoeFullname,
                new List<string> { "role1", "role2," }, 
                new List<string> { "role1" });
    }
}