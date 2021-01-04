using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMM_Project.Frontend.Services;
using AMM_Project.Frontend.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMM_Project.Frontend.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IBranchService branchService;
        private readonly IBusinessService businessService;
        private readonly IBranchItemService branchItemService;
        private readonly IEmployeeItemService employeeItemService;


        public HomeModel(IBranchService branchService, IBusinessService businessService, IBranchItemService branchItemService, IEmployeeItemService employeeItemService)
        {
            this.branchService = branchService;
            this.businessService = businessService;
            this.branchItemService = branchItemService;
            this.employeeItemService = employeeItemService;
        }
        public IList<Branch> branches;
       
        [BindProperty]
        public Branch Branch { set; get; }
        [FromRoute]
        public long? Id { set; get; }
        public string currentBusiness { set; get; }
        public class Upcoming
        {
            public string Branch { get; set; }
            public string Details { get; set; }
            public DateTime date { get; set; }
        }
        public IList<Upcoming> upcomings = new List<Upcoming>();
        public async Task<IActionResult> OnGet()
        {
            if (Id.HasValue)
            {
                branches =  branchService.GetAll().Where(x=>x.BusinessId == Id.Value).ToList();
                var getBusinessName =  businessService.Find(Id.Value);
                var employeeItems =  employeeItemService.GetAll();
                var branchItems = await branchItemService.GetAllAsync();
                if (branchItems != null)
                {
                    foreach (var item in branchItems.Where(x=>x.Branch.BusinessId==Id.Value))
                    {
                        if (item.ExpDate.HasValue && item.ExpDate.Value.AddDays(-30).Date <= DateTime.Today)
                        {
                            Upcoming upcoming = new Upcoming();
                            upcoming.date = item.ExpDate.Value;
                            upcoming.Branch = item.Branch.Name;
                            upcoming.Details = item.DocumentTitle;
                            upcomings.Add(upcoming);
                        }
                    }
                }
                if (employeeItems != null)
                {
                    foreach (var item in employeeItems.Where(x=>x.Employee.Branch.BusinessId==Id.Value))
                    {
                        if (item.ExpDate.HasValue && item.ExpDate.Value.AddDays(-30).Date <= DateTime.Today)
                        {
                            Upcoming upcoming = new Upcoming();
                            upcoming.date = item.ExpDate.Value;
                            upcoming.Branch = item.Employee.Branch.Name;
                            upcoming.Details = item.DocumentTitle + " " + item.Employee.FirstName + " " + item.Employee.LastName;
                            upcomings.Add(upcoming);
                        }
                    }
                }
                if (getBusinessName != null && branches != null)
                {
                    currentBusiness = getBusinessName.Name;
                    return null;
                }
                else
                    return RedirectToPage("/Index");
            }
                return RedirectToPage("/Index");
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await OnGet();

            //Validate From [Check for requred fields and errors then populate the corresponding message]
            if (!ModelState.IsValid)
            {
                return Page();
            }


            //var branch =  new Branch();//if doesnt exist create a new one
            var branch = await branchService.FindAsync(Branch.Id) ?? new Branch();//if t doesnt exist create a new one
            if (branches.FirstOrDefault(x => x.Name == Branch.Name) != null && branch.Id != branches.FirstOrDefault(x => x.Name == Branch.Name).Id)
            {
                ModelState.AddModelError("Branch.Name", "Branch Already Exists");
                await OnGet();
                return Page();
            }
            //get data from the Bind Property Model
            branch.Name = Branch.Name;
            branch.Location = Branch.Location;
            branch.BusinessId = Id.Value;

  

            await branchService.SaveAsync(branch);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            await branchService.DeleteAsync(Branch.Id);
            return RedirectToPage("/Index");
        }
    }
}
