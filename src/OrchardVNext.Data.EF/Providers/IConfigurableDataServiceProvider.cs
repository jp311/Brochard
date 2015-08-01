using Microsoft.Data.Entity;
using OrchardVNext.Data.Providers;

namespace OrchardVNext.Data.EF.Providers {
    public interface IConfigurableDataServiceProvider : IDataServicesProvider {
        void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilders, string connectionString);
    }
}