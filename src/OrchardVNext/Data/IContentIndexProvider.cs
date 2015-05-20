using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement.Records;
using OrchardVNext.Data.EF;

namespace OrchardVNext.Data {
    public interface IContentIndexProvider : IDependency {
        void Store<T>(T data) where T : DocumentRecord;

        ContentIndex<T> Query<T, TF>(
            Expression<Func<TF, bool>> map,
            Expression<Action<IEnumerable<TF>>> sort) where T : DocumentRecord;

        Task DeIndex(int id);
    }
}