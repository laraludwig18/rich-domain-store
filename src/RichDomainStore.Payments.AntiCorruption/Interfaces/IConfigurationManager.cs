namespace RichDomainStore.Payments.AntiCorruption.Interfaces
{
    public interface IConfigurationManager
    {
        string GetValue(string node);
    }
}