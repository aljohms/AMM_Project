using AMM_Project.Frontend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _context;

        //public BusinessService()
        //{
        //    var options = new DbContextOptionsBuilder<AppDbContext>()
        //        .UseInMemoryDatabase("Backend")
        //        .Options;

        //    _context = new AppDbContext(options);
        //}

        public BusinessService(AppDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(long id)
        {
            _context.Business.Remove(new Business { Id = id });
            await _context.SaveChangesAsync();
        }

        public Business Find(long id)
        {
            return _context.Business.Include(x=>x.Branches).FirstOrDefault(x => x.Id == id);
        }

        public Task<Business> FindAsync(long id)
        {
            return _context.Business.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Business> GetAll(int? count = null, int? page = null)
        {
            var actualCount = count.GetValueOrDefault(10);

            return _context.Business
                    .Skip(actualCount * page.GetValueOrDefault(0))
                    .Take(actualCount);
        }

        public Task<Business[]> GetAllAsync(int? count = null, int? page = null)
        {
            return GetAll(count, page).ToArrayAsync();
        }

        public async Task SaveAsync(Business business)
        {
            var isNew = business.Id == default(long);

            _context.Entry(business).State = isNew ? EntityState.Added : EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
