using Tauchbolde.Common.Model;
using Xunit;

namespace Tauchbolde.Tests.Mocks
{
    public class DiverTests
    {
        [Theory]
        [InlineData("myname", "myname")]
        [InlineData("@myname", "myname")]
        [InlineData("_myname", "_myname")]
        [InlineData("@@myname", "@myname")]
        [InlineData("", "")]
        public void GetTwitterUrl(string twitterHandle, string expectedUrl)
        {
            var diver = new Diver { TwitterHandle = twitterHandle };
            var twitterUrl = diver.GetTwitterUrl();

            Assert.Equal(expectedUrl, twitterUrl);
        }
    }
}
