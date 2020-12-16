using AMM_Project.Frontend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public  class EmployeeItemService : IEmployeeItemService
    {
        private readonly AppDbContext _context;

        public EmployeeItemService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Backend")
                .Options;

            _context = new AppDbContext(options);
        }

        public EmployeeItemService(AppDbContext context)
        {
            _context = context;
        }

        public EmployeeItem Find(long id)
        {
            return _context.EmployeeItem.Include(x=>x.Employee).ThenInclude(x=>x.Branch).ThenInclude(x=>x.Business).FirstOrDefault(x => x.Id == id);
        }

        public Task<EmployeeItem> FindAsync(long id)
        {
            return _context.EmployeeItem.Include(x => x.Employee).ThenInclude(x => x.Branch).ThenInclude(x => x.Business).FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<EmployeeItem[]> GetAllAsync( int? count = null, int? page = null)
        {
            return GetAll(count, page).ToArrayAsync();
        }
        public IQueryable<EmployeeItem> GetAll(int? count = null, int? page = null)
        {
            var actualCount = count.GetValueOrDefault(10);

            return _context.EmployeeItem.Include(x => x.Employee).Include(x => x.Employee).ThenInclude(x => x.Branch).ThenInclude(x => x.Business)
                    .Skip(actualCount * page.GetValueOrDefault(0))
                    .Take(actualCount);
        }
        public async Task SaveAsync(EmployeeItem employeeItem)
        {
            var isNew = employeeItem.Id == default(long);

            _context.Entry(employeeItem).State = isNew ? EntityState.Added : EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
