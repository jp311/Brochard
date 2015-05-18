using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data.EF {
    public class EFContentStore : IContentStore {
        private readonly DataContext _dataContext;

        public EFContentStore(DataContext dataContext) {
            _dataContext = dataContext;
        }

        public async Task Store<T>(T document) where T : DocumentRecord {
            _dataContext.Add(document);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<T> Get<T>(int id) where T : DocumentRecord {
            return await Task.FromResult(_dataContext.Set<T>().SingleOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<T>> GetMany<T>(int[] id) where T : DocumentRecord {
            return await Task.FromResult(_dataContext.Set<T>().Where(x => id.Contains(x.Id)).AsEnumerable());
        }

        public async Task Delete<T>(int id) where T : DocumentRecord {
            await Task.Run(() => {
                var entity = Get<T>(id);

                _dataContext.Remove(entity);
            });
        }
    }
}