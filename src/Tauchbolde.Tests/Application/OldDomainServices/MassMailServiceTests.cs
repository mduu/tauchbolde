using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.OldDomainServices;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.OldDomainServices
{

    public class MassMailServiceTests
    {
        [Fact]
        public void Test_CreateReceiverString_WithOne()
        {
            var diver = new Diver
            {
                Fullname = "John Doe",
                User = new IdentityUser { Email = "john.doe@example.com" },
            };
            var massMailService = new MassMailService();
            
            var result = massMailService.CreateReceiverString(new [] { diver });

            result.Should().NotBeNull();
            result.Should().Be("\"John Doe\"<john.doe@example.com>");
        }        
        
        [Fact]
        public void Test_CreateReceiverString_WithTwo()
        {
            var diver1 = new Diver
            {
                Fullname = "John Doe",
                User = new IdentityUser { Email = "john.doe@example.com" },
            };            
            var diver2 = new Diver
            {
                Fullname = "Jane Doe",
                User = new IdentityUser { Email = "jane.doe@example.com" },
            };
            var massMailService = new MassMailService();
            
            var result = massMailService.CreateReceiverString(new [] { diver1, diver2 });

            result.Should().NotBeNull();
            result.Should().Be("\"John Doe\"<john.doe@example.com>;\"Jane Doe\"<jane.doe@example.com>");
        }
        
        [Fact]
        public void Test_CreateReceiverString_WithThree()
        {
            var diver1 = new Diver
            {
                Fullname = "John Doe",
                User = new IdentityUser { Email = "john.doe@example.com" },
            };            
            var diver2 = new Diver
            {
                Fullname = "Jane Doe",
                User = new IdentityUser { Email = "jane.doe@example.com" },
            };
            var diver3 = new Diver
            {
                Fullname = "Robin Doe",
                User = new IdentityUser { Email = "robin.doe@example.com" },
            };
            var massMailService = new MassMailService();
            
            var result = massMailService.CreateReceiverString(new [] { diver1, diver2, diver3 });

            result.Should().NotBeNull();
            result.Should().Be("\"John Doe\"<john.doe@example.com>;\"Jane Doe\"<jane.doe@example.com>;\"Robin Doe\"<robin.doe@example.com>");
        }
    }
}
