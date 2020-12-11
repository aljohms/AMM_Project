using AMM_Project.Frontend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public interface IBusinessService
    {
        Task DeleteAsync(long id);
        Business Find(long id);
        Task<Business> FindAsync(long id);
        IQueryable<Business> GetAll(int? count = null, int? page = null);
        Task<Business[]> GetAllAsync(int? count = null, int? page = null);
        Task SaveAsync(Business business);
    }
}
