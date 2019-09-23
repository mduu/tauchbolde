using System;
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

    }
}