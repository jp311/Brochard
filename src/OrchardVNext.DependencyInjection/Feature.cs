using System;
using System.Collections.Generic;

namespace OrchardVNext.DependencyInjection {
    public class Feature {
        public FeatureDescriptor Descriptor { get; set; }
        public IEnumerable<Type> ExportedTypes { get; set; }
    }
}