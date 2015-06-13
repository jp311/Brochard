using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OrchardVNext.Data.EF;

namespace OrchardVNext.Data {
    public interface IContentIndexProvider : IDependency {
        void Store<T>(T data) where T : IStorageDocument;

        ContentIndex<T> Query<T>(Expression<Func<T, bool>> map) where T : IStorageDocument;

        // ContentIndex<T> Query<T>(
        //     Expression<Func<T, bool>> map,
        //     Expression<Action<IEnumerable<T>>> sort) where T : StorageObject;
        //
        // Task DeIndex(int id);
    }
}
