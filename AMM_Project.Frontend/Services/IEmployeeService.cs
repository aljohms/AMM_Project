using AMM_Project.Frontend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public interface IEmployeeService
    {
        Task DeleteAsync(long id);
        Employee Find(long id);
        Task<Employee> FindAsync(long id);
        IQueryable<Employee> GetAll(int? count = null, int? page = null);
        Task<Employee[]> GetAllAsync(int? count = null, int? page = null);
        Task SaveAsync(Employee employee);
    }
}
