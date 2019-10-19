using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tauchbolde.Application.DataGateways;

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
            
            return result;
        }
    }
}