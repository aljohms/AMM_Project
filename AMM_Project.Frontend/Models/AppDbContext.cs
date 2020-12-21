using Microsoft.EntityFrameworkCore;
using AMM_Project.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AMM_Project.Frontend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
           // this.EnsureSeedData();

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Business> Business { get; set; }


        public virtual DbSet<Branch> Branch { get; set; }


        public virtual DbSet<Employee> Employee { get; set; }


        public virtual DbSet<BranchItem> BranchItem { get; set; }


        public virtual DbSet<EmployeeItem> EmployeeItem { get; set; }
    }
}
