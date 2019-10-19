using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tauchbolde.Application.Services.Core;

namespace Tauchbolde.Tests.TestingTools.TestDataFactories
{
    public static class CurrentUserFactory
    {
        public static ICurrentUser CreateCurrentUser()
        {
            var result = A.Fake<ICurrentUser>();

            A.CallTo(() => result.Username).Returns(DiverFactory.JohnDoeUserName);

            A.CallTo(() => result.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJohnDoe()));

            A.CallTo(() => result.GetIsDiverOrAdmin(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == DiverFactory.JohnDoeDiverId));

            return result;
        }
    }
}