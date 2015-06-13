using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrchardVNext.Data {
    public interface IContentStorageManager : IDependency {
        void Store(StorageDocument data);

        ContentIndexResult<TContent> Query<TContent>() where TContent : StorageDocument;
        ContentIndexResult<TContent> Query<TContent, IIndex>() 
            where TContent : StorageDocument 
            where IIndex : IIndex<TContent>;

        void Delete<T>(int id) where T : StorageDocument;
    }

    public interface IIndex<TContent> where TContent : StorageDocument {
        Expression<Func<IEnumerable<TContent>, IEnumerable<int>>> Map { get; }
    }
}