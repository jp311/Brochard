using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement.Records;
using Microsoft.Data.Entity;

namespace OrchardVNext.Data.EF {
    public class EFContentStore : IContentStore {
        private readonly DataContext _dataContext;

        public EFContentStore(DataContext dataContext) {
            _dataContext = dataContext;
        }

        public async Task<int> Store<T>(T data) where T : StorageDocument {
            if (_dataContext.Entry(data).State == EntityState.Detached) {
                _dataContext.Add(data);
            }
            else {
                _dataContext.Update(data);
            }

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<T> Get<T>(int id) where T : StorageDocument {
           return await _dataContext
               .Set<T>()
               .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetMany<T>(int[] id) where T : StorageDocument {
           return await _dataContext.Set<T>().Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public async Task Delete<T>(int id) where T : StorageDocument {
           await Task.Run(() => {
               _dataContext.Set<T>().Remove(Get<T>(id).Result);
           });
        }
    }
}
