using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data {
    public interface IContentIndexProvider : IDependency {
        void Store<T>(T data) where T : DocumentRecord;

        IEnumerable<int> Query<TF>(
            Expression<Func<TF, bool>> map,
            Expression<Action<IEnumerable<TF>>> sort);

        Task DeIndex(int id);
    }
}