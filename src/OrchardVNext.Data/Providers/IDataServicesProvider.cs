using OrchardVNext.DependencyInjection;

namespace OrchardVNext.Data.Providers {
    public interface IDataServicesProvider : ITransientDependency {
        string ProviderName { get; }
    }
}