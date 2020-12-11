using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
namespace AMM_Project.Frontend.Pages
{
    public class BranchHomeModel : PageModel
    {
       
            private readonly IBranchItemService branchItemService;
            private readonly IBranchService branchService;

            public BranchHomeModel(IBranchItemService branchItemService,IBranchService branchService)
            {
                this.branchItemService = branchItemService;
                this.branchService = branchService;
             }
            public string branchName()
            {
                     return branchService.Find(Id.Value).Name;
            }
            public IList<BranchItem> branchItems;
            [BindProperty]
            public BranchItem BranchItem { set; get; }
            [FromRoute]
            public long? Id { set; get; }
            public async Task OnGet()
            {
                if (Id.HasValue)
                branchItems = await branchItemService.GetAllAsync(Id.Value);
            }
            public async Task<IActionResult> OnPostAsync()
            {
                //Validate From [Check for requred fields and errors then populate the corresponding message]
                if (!ModelState.IsValid)
                {
                    return Page();
                }


                var branchItem = new BranchItem();
                //branch.Name = Branch.Name;
                //branch.Location = Branch.Location;
                //branch.BusinessId = Id.Value;



                await branchItemService.SaveAsync(branchItem);
                return RedirectToPage();
            }
        }
    
}
