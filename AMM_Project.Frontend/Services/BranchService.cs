using AMM_Project.Frontend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public class BranchService : IBranchService
    {
        private readonly AppDbContext _context;

        public BranchService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Backend")
                .Options;

            _context = new AppDbContext(options);
        }

        public BranchService(AppDbContext context)
        {
            _context = context;
        }
        public Branch Find(long id)
        {
            return _context.Branch.Include(x=>x.Business).FirstOrDefault(x => x.Id == id);
        }

        public Task<Branch> FindAsync(long id)
        {
            return _context.Branch.Include(x=>x.Business).FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<Branch[]> GetAllAsync( int? count = null, int? page = null)
        {
            return GetAll(count, page).ToArrayAsync();
        }
        public IQueryable<Branch> GetAll( int? count = null, int? page = null)
        {
            var actualCount = count.GetValueOrDefault(10);

            return _context.Branch.Include(x=>x.Business)
                    .Skip(actualCount * page.GetValueOrDefault(0))
                    .Take(actualCount);
        }
        public async Task SaveAsync(Branch branch)
        {
            var isNew = branch.Id == default(long);

            _context.Entry(branch).State = isNew ? EntityState.Added : EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
