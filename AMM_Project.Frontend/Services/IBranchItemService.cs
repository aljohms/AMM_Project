using AMM_Project.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public interface IBranchItemService
    {

        BranchItem Find(long id);
        Task<BranchItem> FindAsync(long id);
        IQueryable<BranchItem> GetAll( int? count = null, int? page = null);

        Task<BranchItem[]> GetAllAsync(int? count = null, int? page = null);
        Task SaveAsync(BranchItem branchItem);
        Task DeleteAsync(long id);

    }
}
