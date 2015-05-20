using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data.EF {
    public class EFContentIndexProvider : IContentIndexProvider {
        private readonly DataContext _dataContext;
        private readonly IEnumerable<IContentStore> _contentStores;

        public EFContentIndexProvider(DataContext dataContext,
            IEnumerable<IContentStore> contentStores) {
            _dataContext = dataContext;
            _contentStores = contentStores;
        }

        public void Store<T>(T data) where T : DocumentRecord {
            //Logger.Debug("Adding {0} to index {1}.", typeof(DocumentRecord).Name, map);

            //// TODO: Remove with dynamic tables
            //_dataContext.Set<DocumentIndexRecord>().Add(new DocumentIndexRecord {
            //    ContentId = document.Id,
            //    Map = expression.Map.ToString(),
            //    Sort = expression.Sort.ToString(),
            //    Value = data
            //});

            //Logger.Debug("Added {0} to index {1}.", typeof(DocumentRecord).Name, map);
        }

        public ContentIndex<T> Query<T, TF>(
            Expression<Func<TF, bool>> map,
            Expression<Action<IEnumerable<TF>>> sort) where T : DocumentRecord {

            var mapValue = map.ToString();
            var sortValue = sort.ToString();

            bool exists = _dataContext
                .Set<DocumentIndexExistanceRecord>()
                .Any(x => x.Map == mapValue && x.Sort == sortValue);

            if (!exists) {
                // TODO: Build Index
            }

            var contentIds = _dataContext
                .Set<DocumentIndexRecord>()
                .Where(x => x.Map == mapValue && x.Sort == sortValue)
                .Select(o => o.ContentId)
                .ToArray();

            IEnumerable<T> records = _contentStores
                .Select(x => x.GetMany<T>(contentIds))
                .SelectMany(x => x.Result);

            return new ContentIndex<T> {
                Records = records
            };
        }

        public async Task DeIndex(int id) {
            var set = _dataContext.Set<DocumentIndexRecord>();

            await Task.Run(() => set.RemoveRange(set.Where(x => x.ContentId == id)));
        }

        [Persistent]
        private class DocumentIndexExistanceRecord {
            public int Id { get; set; }
            public string Map { get; set; }
            public string Sort { get; set; }
        }

        [Persistent]
        private class DocumentIndexRecord {
            public int Id { get; set; }
            public string Map { get; set; }
            public string Sort { get; set; }
            public int ContentId { get; set; }
            public string Value { get; set; }
        }
    }
}