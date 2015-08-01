using Microsoft.Data.Entity;

namespace OrchardVNext.Data.EF.Providers {
    public class SqlServerDataServicesProvider : IConfigurableDataServiceProvider {
        public string ProviderName {
            get { return "SqlServer"; }
        }

        public void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilders, string connectionString) {
            optionsBuilders.UseSqlServer(connectionString);
        }
    }
}