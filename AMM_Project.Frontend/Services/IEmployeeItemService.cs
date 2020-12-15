using AMM_Project.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public interface IEmployeeItemService
    {

        EmployeeItem Find(long id);
        Task<EmployeeItem> FindAsync(long id);
        IQueryable<EmployeeItem> GetAll( int? count = null, int? page = null);

        Task<EmployeeItem[]> GetAllAsync( int? count = null, int? page = null);
        Task SaveAsync(EmployeeItem employeeItem);
    }
}
