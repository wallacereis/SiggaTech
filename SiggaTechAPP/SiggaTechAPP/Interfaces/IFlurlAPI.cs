using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SiggaTechAPP.Interfaces
{
    public interface IFlurlAPI<T> where T : class
    {
        /*-------------------------- List Result --------------------------*/
        Task<List<T>> GetAllItemsAsync(string entity);
        Task<List<T>> GetAllItemsByIdAsync(string entity, int id);
        Task<List<T>> GetAllItemsAsync(string entity, string parameter, string value);

        /*-------------------------- Unique Result --------------------------*/
        Task<T> GetItemByIdAsync(string entity, int id);
        Task<T> GetItemByEmailAsync(string entity, string email);
        Task<List<T>> GetItemByLoginAsync(string entity, string emailParameter, string passwordParameter);

        /*-------------------------- File Result --------------------------*/
        Task<byte[]> GetFileAsync(string file, string token);

        /*-------------------------- CRUD Result --------------------------*/
        Task<T> AddItemAsync(string entity, T obj);
        Task<T> UpdateItemAsync(string entity, int id, T obj);
        Task<T> DeleteItemAsync(string entity, int id);

        /*-------------------------- Upload Result --------------------------*/
        Task<T> UploadItemAsync(string entity, Stream file, int id);
    }
}
