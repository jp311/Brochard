using System;
using System.Collections.Generic;

namespace OrchardVNext.DependencyInjection {
    public class DependencyBlueprintItem {
        public Type Type { get; set; }
        public Feature Feature { get; set; }
    }

    public class DependencyBlueprint : DependencyBlueprintItem {
        public IEnumerable<DependencyParameter> Parameters { get; set; }
    }

    public class DependencyParameter {
        public string Component { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}