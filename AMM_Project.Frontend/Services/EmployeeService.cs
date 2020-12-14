using AMM_Project.Frontend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Backend")
                .Options;

            _context = new AppDbContext(options);
        }

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(long id)
        {
            _context.Employee.Remove(new Employee { Id = id });
            await _context.SaveChangesAsync();
        }

        public Employee Find(long id)
        {
            return _context.Employee.FirstOrDefault(x => x.Id == id);
        }

        public Task<Employee> FindAsync(long id)
        {
            return _context.Employee.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Employee> GetAll(int? count = null, int? page = null)
        {
            var actualCount = count.GetValueOrDefault(10);

            return _context.Employee
                    .Skip(actualCount * page.GetValueOrDefault(0))
                    .Take(actualCount);
        }

        public Task<Employee[]> GetAllAsync(int? count = null, int? page = null)
        {
            return GetAll(count, page).ToArrayAsync();
        }

        public async Task SaveAsync(Employee employee)
        {
            var isNew = employee.Id == default(long);

            _context.Entry(employee).State = isNew ? EntityState.Added : EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
