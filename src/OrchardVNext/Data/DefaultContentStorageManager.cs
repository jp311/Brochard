using System;
using System.Collections.Generic;

namespace OrchardVNext.Data {
    public class DefaultContentStorageManager : IContentStorageManager {
        private readonly IEnumerable<IContentStore> _contentStores;
        private readonly IContentIndexProvider _contentIndexProvider;

        public DefaultContentStorageManager(IEnumerable<IContentStore> contentStores,
            IContentIndexProvider contentIndexProvider) {
            _contentStores = contentStores;
            _contentIndexProvider = contentIndexProvider;
        }

        public void Store(StorageDocument data) {
            _contentStores.Invoke(store => {
                store.Store(data);
            });
        }

        public ContentIndexResult<TContent> Query<TContent>() where TContent : StorageDocument {
            throw new NotImplementedException();
        }

        public ContentIndexResult<TContent> Query<TContent, IIndex>() where TContent : StorageDocument {
            throw new NotImplementedException();
        }
    }
}