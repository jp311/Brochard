using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using OrchardVNext.Logging;
#if DNXCORE50
using System.Reflection;
#endif

namespace OrchardVNext.DependencyInjection {
    public interface IContainerFactory {
        IServiceProvider CreateContainer(IReadOnlyList<DependencyBlueprint> dependencies, Action<IServiceCollection> additionalDependencies);
    }

    public class ContainerFactory : IContainerFactory {
        private readonly IServiceProvider _serviceProvider;

        public ContainerFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IServiceProvider CreateContainer(
            IReadOnlyList<DependencyBlueprint> dependencies, 
            Action<IServiceCollection> additionalDependencies) {

            IServiceCollection serviceCollection = new ServiceCollection();

            additionalDependencies(serviceCollection);

            foreach (var dependency in dependencies
                .Where(t => typeof(IModule).IsAssignableFrom(t.Type))) {

                Logger.Debug("IModule Type: {0}", dependency.Type);

                // TODO: Rewrite to get rid of reflection.
                var instance = (IModule)Activator.CreateInstance(dependency.Type);
                instance.Configure(serviceCollection);
            }

            foreach (var dependency in dependencies
                .Where(t => !typeof(IModule).IsAssignableFrom(t.Type))) {
                foreach (var interfaceType in dependency.Type.GetInterfaces()
                    .Where(itf => typeof(IDependency).IsAssignableFrom(itf))) {
                    Logger.Debug("Type: {0}, Interface Type: {1}", dependency.Type, interfaceType);

                    if (typeof(ISingletonDependency).IsAssignableFrom(interfaceType)) {
                        serviceCollection.AddSingleton(interfaceType, dependency.Type);
                    }
                    else if (typeof(IUnitOfWorkDependency).IsAssignableFrom(interfaceType)) {
                        serviceCollection.AddScoped(interfaceType, dependency.Type);
                    }
                    else if (typeof(ITransientDependency).IsAssignableFrom(interfaceType)) {
                        serviceCollection.AddTransient(interfaceType, dependency.Type);
                    }
                    else {
                        serviceCollection.AddScoped(interfaceType, dependency.Type);
                    }
                }
            }

            return new WrappingServiceProvider(_serviceProvider, serviceCollection);
        }

        private class WrappingServiceProvider : IServiceProvider {
            private readonly IServiceProvider _services;

            // Need full wrap for generics like IOptions
            public WrappingServiceProvider(IServiceProvider fallback, IServiceCollection replacedServices) {
                var services = new ServiceCollection();
                var manifest = fallback.GetRequiredService<IRuntimeServices>();
                foreach (var service in manifest.Services) {
                    services.AddTransient(service, sp => fallback.GetService(service));
                }

                services.AddSingleton<IRuntimeServices>(sp => new HostingManifest(services));
                services.Add(replacedServices);

                _services = services.BuildServiceProvider();
            }

            public object GetService(Type serviceType) {
                return _services.GetService(serviceType);
            }


            // Manifest exposes the fallback manifest in addition to ITypeActivator, IHostingEnvironment, and ILoggerFactory
            private class HostingManifest : IRuntimeServices {
                public HostingManifest(IServiceCollection hostServices) {
                    Services = new Type[] {
                    typeof(IHostingEnvironment),
                    typeof(ILoggerFactory),
                    typeof(IHttpContextAccessor),
                    typeof(IApplicationLifetime)
                }.Concat(hostServices.Select(s => s.ServiceType)).Distinct();
                }

                public IEnumerable<Type> Services { get; }
            }
        }
    }
}