using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OrchardVNext.ContentManagement;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data {
    public interface IContentStorageManager : IDependency {
        void Store<T>(T data) where T : DocumentRecord;
        T Get<T>(int id, VersionOptions versionOption) where T : DocumentRecord;
        IEnumerable<T> GetMany<T>(int[] ids, VersionOptions versionOption) where T : DocumentRecord;

        IEnumerable<T> Query<T, TF>(Expression<Func<TF, bool>> map,
            Expression<Action<IEnumerable<TF>>> sort,
            Func<TF, bool> reduce,
            VersionOptions versionOption) where T : DocumentRecord;

        void Delete<T>(int id) where T : DocumentRecord;
    }

    public static class ContentStorageManagerExtensions {
        public static IEnumerable<T> Query<T>(this IContentStorageManager manager, 
            Expression<Func<T, bool>> map,
            Expression<Action<IEnumerable<T>>> sort,
            Func<T, bool> reduce) where T : DocumentRecord {
            return manager.Query<T, T>(map, sort, reduce, VersionOptions.Published);
        }

        public static IEnumerable<T> Query<T>(this IContentStorageManager manager,
            Expression<Func<T, bool>> map,
            Expression<Action<IEnumerable<T>>> sort) where T : DocumentRecord {
            return manager.Query<T, T>(map, sort, (r) => true, VersionOptions.Published);
        }

        public static IEnumerable<T> Query<T>(this IContentStorageManager manager,
            Expression<Func<T, bool>> map) where T : DocumentRecord {
            return manager.Query<T, T>(map, (s) => s.OrderBy(p => p.Id) , (r) => true, VersionOptions.Published);
        }
    }
}