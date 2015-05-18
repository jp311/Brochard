using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Data {
    public interface IContentStore : IDependency {
        Task Store<T>(T data) where T : DocumentRecord;
        Task<T> Get<T>(int id) where T : DocumentRecord;
        Task<IEnumerable<T>> GetMany<T>(int[] id) where T : DocumentRecord;
        Task Delete<T>(int id) where T : DocumentRecord;
    }
}