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
            public class ViewContent
             {
            public long BusnissId { get; set; }
            public long BranchId { get; set; }

            public string BranchName { get; set; }
            public string BusinessName { get; set; }
            }
            public ViewContent viewContent = new ViewContent();
           
            public IList<BranchItem> branchItems;
            [BindProperty]
            public BranchItem BranchItem { set; get; }
            [FromRoute]
            public long? Id { set; get; }
            public async Task<IActionResult> OnGet()
            {
                if (Id.HasValue)
                {
                    branchItems = await branchItemService.GetAllAsync(Id.Value);
                    var _branchService = branchService.Find(Id.Value);
                    if (branchItems != null && _branchService != null)
                    {
                        viewContent.BranchName = _branchService.Name;
                        viewContent.BusnissId = _branchService.BusinessId;
                        viewContent.BusinessName = _branchService.Business.Name;
                        viewContent.BranchId = _branchService.Id;
                    return null;
                    }
                RedirectToPage("/Index");
                }
            return RedirectToPage("/Index");

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
