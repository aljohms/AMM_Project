using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMM_Project.Frontend.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly IBranchService branchService;
        private readonly IEmployeeService employeeService;


        public EmployeesModel( IBranchService branchService, IEmployeeService employeeService)
        {
            this.branchService = branchService;
            this.employeeService = employeeService;
        }
        [BindProperty]
        public Employee Employee { set; get; }
        public IList<Employee> employees;
        public class ViewContent
        {
            public long BusnissId { get; set; }
            public long BranchId { get; set; }
            public string BranchName { get; set; }
            public string BusinessName { get; set; }
        }
        public ViewContent viewContent = new ViewContent();

        [FromRoute]
        public long? Id { get; set; }
        public IActionResult OnGet()
        {
            if (Id.HasValue)
            {
                var getBranch = branchService.Find(Id.Value);
                employees = employeeService.GetAllAsync().Result.Where(x => x.BranchId == Id.Value).ToList();
                if (getBranch != null)
                {
                    viewContent.BranchName = getBranch.Name;
                    viewContent.BusnissId = getBranch.BusinessId;
                    viewContent.BusinessName = getBranch.Business.Name;
                    viewContent.BranchId = getBranch.Id;
                    return null;
                }
                RedirectToPage("/Index");
            }
             return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
             OnGet();
            //Validate From [Check for requred fields and errors then populate the corresponding message]
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Recipe.Id = Id.GetValueOrDefault();
            //var branch =  new Branch();//if the recipe doesnt exist create a new one
            var employee = await employeeService.FindAsync(Employee.Id) ?? new Employee();//if the recipe doesnt exist create a new one
                                                                                          //get data from the Bind Property Model
            employee.FirstName = Employee.FirstName;
            employee.LastName = Employee.LastName;
            employee.ContactNumber = Employee.ContactNumber;
            employee.Email = Employee.Email;
            employee.Position = Employee.Position;
            employee.Position = Employee.Position;
            employee.BranchId = Id.Value;



            await employeeService.SaveAsync(employee);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            await employeeService.DeleteAsync(Employee.Id);
            return RedirectToPage();
        }
    }
}
