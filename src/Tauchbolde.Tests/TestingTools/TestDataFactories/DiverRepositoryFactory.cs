using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Tests.TestingTools.TestDataFactories
{
    internal abstract class DiverRepositoryFactory
    {
        public static IDiverRepository CreateRepository()
        {
            var result = A.Fake<IDiverRepository>();

            A.CallTo(() => result.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == DiverFactory.JohnDoeDiverId
                        ? DiverFactory.CreateJohnDoe()
                        : null));

            A.CallTo(() => result.GetAllTauchboldeUsersAsync(A<bool>._))
                .ReturnsLazily(() => Task.FromResult<ICollection<Diver>>(
                    new List<Diver>
                    {
                        DiverFactory.CreateJohnDoe(),
                    }));

            A.CallTo(() => result.GetAllDiversAsync())
                .ReturnsLazily(() => Task.FromResult<ICollection<Diver>>(
                    new List<Diver>
                    {
                        DiverFactory.CreateJohnDoe(),
                        DiverFactory.CreateJaneDoe(),
                    }));

            A.CallTo(() => result.FindByUserNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == DiverFactory.JohnDoeUserName
                        ? DiverFactory.CreateJohnDoe()
                        : null));

            return result;
        }
    }
}