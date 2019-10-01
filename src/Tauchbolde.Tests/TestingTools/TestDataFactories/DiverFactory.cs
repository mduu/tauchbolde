using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Tests.TestingTools.TestDataFactories
{
    internal static class DiverFactory
    {
        internal static Guid JohnDoeDiverId => new Guid("7023BEDD-179F-4D8C-AE4B-9038B6C6BF3E");
        internal const string JohnDoeUserName = "john.doe";
        internal const string JohnDoeEmail = "john.doe@company.com";
        internal const string JohnDoeAvatarId = "john.doe-1.jpg";
        internal static Guid JaneDoeDiverId => new Guid("C2E0E0C7-BE27-494C-B647-5E5AA2E635AB");
        internal const string JaneDoeUserName = "jane.doe";
        internal const string JaneDoeEmail = "jane.doe@company.com";
        internal const string JaneDoeAvatarId = "jane.doe-1.jpg";
        
        internal static Diver CreateJohnDoe() =>
            new Diver
            {
                Id = JohnDoeDiverId,
                Fullname = "John Doe",
                User = new IdentityUser(JohnDoeUserName)
                {
                    Email = JohnDoeEmail
                },
                AvatarId = JohnDoeAvatarId
            };

        internal static Diver CreateJaneDoe() =>
            new Diver
            {
                Id = JaneDoeDiverId,
                Fullname = "Jane Doe",
                User = new IdentityUser(JaneDoeUserName)
                {
                    Email = JaneDoeEmail
                },
                AvatarId = JaneDoeAvatarId
            };

        internal static IEnumerable<Diver> GetTauchbolde() =>
            new List<Diver> { CreateJohnDoe(), CreateJaneDoe() };
    }
}