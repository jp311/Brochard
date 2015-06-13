using System;
using System.Collections.Generic;
using System.Linq;

namespace OrchardVNext.Data {
    public class ContentIndexResult<T> {
        public IEnumerable<T> Records { get; set; }

        public IEnumerable<T> Reduce(Func<T, bool> reduce) {
            return Records.Where(reduce);
        }
    }
}