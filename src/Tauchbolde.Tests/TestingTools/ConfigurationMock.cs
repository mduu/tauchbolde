using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Tauchbolde.Web.Core.TokenHandling;

namespace Tauchbolde.Tests.TestingTools
{
    internal class ConfigurationMock : IConfiguration
    {
        public IDictionary<string, string> Values = new Dictionary<string, string>
        {
            {nameof(TokenConfiguration.TokenSecurityKey), "the_token"}
        };

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public string this[string key]
        {
            get => Values.ContainsKey(key) ? Values[key] : null;
            set => Values[key] = value;
        }
    }
}