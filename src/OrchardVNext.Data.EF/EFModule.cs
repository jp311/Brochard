using Microsoft.Framework.DependencyInjection;
using OrchardVNext.DependencyInjection;

namespace OrchardVNext.Data.EF {
    public class EFModule : IModule {
        public void Configure(IServiceCollection serviceCollection) {
            serviceCollection.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<DataContext>();
        }
    }
}