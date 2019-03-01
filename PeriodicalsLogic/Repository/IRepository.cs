using Periodicals.DAL.Publishings;
using System.Collections.Generic;

namespace Periodicals.DAL.Repository
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllHosts();
        IEnumerable<T> GetAllAuthors();
        IEnumerable<Magazine> GetAllMagazines();
        void CreateHost(T item);
        void CreateHost(T item, string password = "");
        void CreateAuthor(T item, string password = "");
        void CreateMagazine(Magazine item);
        void UpdateHost(T item);
        void UpdateMagazine(Magazine item);
        void DeleteUser(int id);
        void DeleteMagazine(int id);
    }
}
