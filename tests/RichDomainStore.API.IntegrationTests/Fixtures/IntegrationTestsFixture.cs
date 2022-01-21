using System.Net.Http;
using RichDomainStore.API.IntegrationTests.Configurations;
using Xunit;

namespace RichDomainStore.API.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(IntegrationTestsCollection))]
    public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture> { }

    public class IntegrationTestsFixture
    {
        public readonly ApiFactory<Startup> Factory;
        public HttpClient Client;
        public DatabaseFixture DatabaseFixture;
        
        public IntegrationTestsFixture()
        {
            DatabaseFixture = new DatabaseFixture();

            Factory = new ApiFactory<Startup>(DatabaseFixture);
            Client = Factory.CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }
}