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
    public class BusinessHomeModel : PageModel
    {
        private readonly IBranchService branchService;
        private readonly IBusinessService businessService;



        public BusinessHomeModel(IBranchService branchService, IBusinessService businessService)
        {
            this.branchService = branchService;
            this.businessService = businessService;
        }
        public IList<Branch> branches;
       
        [BindProperty]
        public Branch Branch { set; get; }
        [FromRoute]
        public long? Id { set; get; }
        public string currentBusiness()
        {
            return businessService.Find(Id.Value).Name;
        }
        public async Task OnGet()
        {
            if(Id.HasValue)
            branches = await branchService.GetAllAsync(Id.Value);

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //Validate From [Check for requred fields and errors then populate the corresponding message]
            if (!ModelState.IsValid)
            {
                return Page();
            }


            //Recipe.Id = Id.GetValueOrDefault();
            var branch =  new Branch();//if the recipe doesnt exist create a new one
            //get data from the Bind Property Model
            branch.Name = Branch.Name;
            branch.Location = Branch.Location;
            branch.BusinessId = Id.Value;

  

            await branchService.SaveAsync(branch);
            return RedirectToPage();
        }
    }
}
