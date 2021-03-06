﻿using AMM_Project.Frontend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public interface IBranchService
    {
        Branch Find(long id);
        Task<Branch> FindAsync(long id);
        IQueryable<Branch> GetAll( int? count = null, int? page = null);

        Task<Branch[]> GetAllAsync( int? count = null, int? page = null);
        Task SaveAsync(Branch branch);
        Task DeleteAsync(long id);


    }
}
