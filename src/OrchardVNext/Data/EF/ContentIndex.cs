using System;
using System.Linq;
using System.Collections.Generic;

namespace OrchardVNext.Data.EF {
    public class ContentIndex<T> {
        public IEnumerable<T> Records { get; set; }

        public IEnumerable<T> Reduce(Func<T, bool> reduce) {
            return Records.Where(reduce);
        }
    }
}