using AMM_Project.Frontend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public  class BranchItemService :IBranchItemService
    {
        private readonly AppDbContext _context;

        public BranchItemService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Backend")
                .Options;

            _context = new AppDbContext(options);
        }

        public BranchItemService(AppDbContext context)
        {
            _context = context;
        }

        public BranchItem Find(long id)
        {
            return _context.BranchItem.Include(x=>x.Branch).ThenInclude(x=>x.Business).FirstOrDefault(x => x.Id == id);
        }

        public Task<BranchItem> FindAsync(long id)
        {
            return _context.BranchItem.FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<BranchItem[]> GetAllAsync(long id, int? count = null, int? page = null)
        {
            return GetAll(id, count, page).ToArrayAsync();
        }
        public IQueryable<BranchItem> GetAll(long id, int? count = null, int? page = null)
        {
            var actualCount = count.GetValueOrDefault(10);

            return _context.BranchItem.Include(x => x.Branch).ThenInclude(x=>x.Business).Where(x => x.Branch.Id == id)
                    .Skip(actualCount * page.GetValueOrDefault(0))
                    .Take(actualCount);
        }
        public async Task SaveAsync(BranchItem branchItem)
        {
            var isNew = branchItem.Id == default(long);

            _context.Entry(branchItem).State = isNew ? EntityState.Added : EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
