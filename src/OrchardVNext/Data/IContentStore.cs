using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrchardVNext.Data {
    public interface IContentStore : IDependency {
        // Similar to REST interfaces, where the Id is returned.
        Task<int> Store<T>(T data) where T : StorageDocument;
        Task<T> Get<T>(int id) where T : StorageDocument;
        Task<IEnumerable<T>> GetMany<T>(int[] id) where T : StorageDocument;
        Task Delete<T>(int id) where T : StorageDocument;
    }
}
