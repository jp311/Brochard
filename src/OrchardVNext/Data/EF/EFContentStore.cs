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

        public async Task Store<T>(T document) where T : DocumentRecord {
            if (document.Id == 0) {
                _dataContext.Add(document);
            }
            else {
                _dataContext.Update(document);
            }
            
            await _dataContext.SaveChangesAsync();
        }

        public async Task<T> Get<T>(int id) where T : DocumentRecord {
            return await _dataContext.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetMany<T>(int[] id) where T : DocumentRecord {
            return await _dataContext.Set<T>().Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public async Task Delete<T>(int id) where T : DocumentRecord {
            await Task.Run(() => {
                var entity = Get<T>(id);

                _dataContext.Remove(entity.Result);
            });
        }
    }
}