using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Razor.Compilation;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Dnx.Runtime;
using OrchardVNext.Environment.Extensions.Loaders;
using OrchardVNext.Mvc.Razor;

namespace OrchardVNext.Mvc {
    public class MvcModule : IModule {
        private readonly IServiceProvider _serviceProvider;
        public MvcModule(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void Configure(IServiceCollection serviceCollection) {
            serviceCollection.AddMvc();
            serviceCollection.AddSingleton<ICompilationService, DefaultRoslynCompilationService>();

            serviceCollection.Configure<RazorViewEngineOptions>(options => {
                var expander = new ModuleViewLocationExpander();
                options.ViewLocationExpanders.Add(expander);
            });

            var p = _serviceProvider.GetService<IOrchardLibraryManager>();
            serviceCollection.AddInstance<IAssemblyProvider>(new OrchardMvcAssemblyProvider(p, _serviceProvider, _serviceProvider.GetService<IAssemblyLoaderContainer>()));
        }
    }
}