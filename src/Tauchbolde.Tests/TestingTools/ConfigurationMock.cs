using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Tauchbolde.Tests.TestingTools
{
    internal class ConfigurationMock : IConfiguration
    {
        public IDictionary<string, string> Values = new Dictionary<string, string>();
        
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