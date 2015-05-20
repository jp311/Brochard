using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OrchardVNext.ContentManagement;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data {
    public class DefaultContentStorageManager : IContentStorageManager {
        private readonly IEnumerable<IContentStore> _contentStores;
        private readonly IContentIndexProvider _contentIndexProvider;

        public DefaultContentStorageManager(IEnumerable<IContentStore> contentStores,
            IContentIndexProvider contentIndexProvider) {
            _contentStores = contentStores;
            _contentIndexProvider = contentIndexProvider;
        }

        public void Store<T>(T data) where T : DocumentRecord {
            _contentStores.Invoke(store => {
                var task = store.Store(data);
            });
        }

        public T Get<T>(int id, VersionOptions versionOption) where T : DocumentRecord {
            var tasks = _contentStores.Select(x => x.Get<T>(id)).ToList();

            return tasks.SingleOrDefault(x => x.Result != null)?.Result;
        }

        public IEnumerable<T> GetMany<T>(int[] ids, VersionOptions versionOption) where T : DocumentRecord {
            // TODO: Combine results from multiple stores
            var tasks = _contentStores.Select(x => x.GetMany<T>(ids)).ToList();
            
            return tasks.SingleOrDefault(x => x.Result != null)?.Result;
        }

        public IEnumerable<T> Query<T, TF>(Expression<Func<TF, bool>> map,
            Expression<Action<IEnumerable<TF>>> sort,
            Func<T, bool> reduce, 
            VersionOptions versionOption) where T : DocumentRecord {

            return _contentIndexProvider.Query<T, TF>(map, sort).Reduce(reduce);
        }

        public void Delete<T>(int id) where T : DocumentRecord {
            var tasks = _contentStores.Select(x => x.Delete<T>(id)).ToList();

            foreach (var item in tasks) {
                item.Start();
            }

            _contentIndexProvider.DeIndex(id).Start();
        }
    }
}