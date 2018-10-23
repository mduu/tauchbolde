using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Mozilla;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public class DiversService : IDiverService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DiversService"/> class.
        /// </summary>
        /// <param name="applicationDbContext">Application db context.</param>
        public DiversService(ApplicationDbContext applicationDbContext)
        {
            if (applicationDbContext == null) throw new ArgumentNullException(nameof(applicationDbContext));

            _applicationDbContext = applicationDbContext;
        }

        /// <inheritdoc/>
        public async Task<ICollection<Diver>> GetMembersAsync(IDiverRepository diverRepository)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }

            return await diverRepository.GetAllTauchboldeUsersAsync();
        }        

        /// <inheritdoc/>
        public async Task<Diver> GetMemberAsync(IDiverRepository diverRepository, string userName)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }

            return await diverRepository.FindByUserNameAsync(userName);
        }
        
        public async Task UpdateUserProfil(IDiverRepository diverRepository, Diver profile)
        {
            if (diverRepository == null) { throw new ArgumentNullException(nameof(diverRepository)); }
            if (profile == null) { throw new ArgumentNullException(nameof(profile)); }

            var diver = await diverRepository.FindByUserNameAsync(profile.User.UserName);
            if (diver == null)
            {
                throw new InvalidOperationException("Profile (Diver) nicht gefunden!");
            }

            diver.Fullname = profile.Fullname;
            diver.Firstname = profile.Firstname;
            diver.Lastname = profile.Lastname;
            diver.Education = profile.Education;
            diver.Experience = profile.Experience;
            diver.Slogan = profile.Slogan;
            diver.WebsiteUrl = profile.WebsiteUrl;
            diver.TwitterHandle = profile.TwitterHandle;
            diver.FacebookId = profile.FacebookId;
            diver.SkypeId = profile.SkypeId;
            diver.MobilePhone = profile.MobilePhone;

            diverRepository.Update(diver);
        }
    }
}
