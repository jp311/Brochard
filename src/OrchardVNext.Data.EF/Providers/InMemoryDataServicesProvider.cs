using Microsoft.Data.Entity;

namespace OrchardVNext.Data.EF.Providers
{
    public class InMemoryDataServicesProvider : IConfigurableDataServiceProvider {
        public string ProviderName {
            get { return "InMemory"; }
        }

        public void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilders, string connectionString) {
            optionsBuilders.UseInMemoryDatabase(persist: true);
        }
    }
}
