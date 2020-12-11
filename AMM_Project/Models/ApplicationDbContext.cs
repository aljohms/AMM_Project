
using Microsoft.EntityFrameworkCore;
using AMM_Project.Models;

namespace AMM_Project.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Buisness> Buisness { get; set; }


        public DbSet<AMM_Project.Models.Branch> Branch { get; set; }


        public DbSet<AMM_Project.Models.Employee> Employee { get; set; }


        public DbSet<AMM_Project.Models.BranchItem> BranchItem { get; set; }


        public DbSet<AMM_Project.Models.EmployeeItem> EmployeeItem { get; set; }

    }
}
