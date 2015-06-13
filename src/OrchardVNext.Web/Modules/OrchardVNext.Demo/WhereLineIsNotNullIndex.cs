using System;
using System.Linq;
using System.Collections;
using OrchardVNext.Data;
using OrchardVNext.ContentManagement;
using System.Collections.Generic;
using System.Linq.Expressions;
using OrchardVNext.Demo.Models;

namespace OrchardVNext.Demo.Controllers {
    public class WhereLineIsNotNullIndex : IIndex<TestContentPartA> {
        public Expression<Func<IEnumerable<TestContentPartA>, IEnumerable<int>>> Map {
            get {
                Expression<Func<IEnumerable<TestContentPartA>, IEnumerable<int>>> Map =
                    contentitems => contentitems.Where(x => x.Line != null).Select(x => x.Id);

                return Map;
            }
        }
    }
}