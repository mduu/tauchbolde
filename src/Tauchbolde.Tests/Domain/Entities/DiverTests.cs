using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Domain.Entities
{
    public class DiverTests
    {
        [Theory]
        [InlineData("myname", "https://twitter.com/myname")]
        [InlineData("@myname", "https://twitter.com/myname")]
        [InlineData("_myname", "https://twitter.com/_myname")]
        [InlineData("@@myname", "https://twitter.com/@myname")]
        [InlineData("", "")]
        public void GetTwitterUrl(string twitterHandle, string expectedUrl)
        {
            var diver = new Diver { TwitterHandle = twitterHandle };
            var twitterUrl = diver.GetTwitterUrl();

            Assert.Equal(expectedUrl, twitterUrl);
        }

        [Theory]
        [InlineData("myname", "https://facebook.com/myname")]
        [InlineData("@myname", "https://facebook.com/@myname")]
        [InlineData("_myname", "https://facebook.com/_myname")]
        [InlineData("@@myname", "https://facebook.com/@@myname")]
        [InlineData("", "")]
        public void GetFacebookUrl(string facebookId, string expectedUrl)
        {
            var diver = new Diver { FacebookId = facebookId };
            var facebookUrl = diver.GetFacebookeUrl();

            Assert.Equal(expectedUrl, facebookUrl);
        }
    }
}
