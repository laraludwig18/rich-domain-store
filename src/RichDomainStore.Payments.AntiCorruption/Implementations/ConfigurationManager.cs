using System;
using System.Linq;
using RichDomainStore.Payments.AntiCorruption.Interfaces;

namespace RichDomainStore.Payments.AntiCorruption.Implementations
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}