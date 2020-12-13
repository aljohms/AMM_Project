using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMM_Project.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMM_Project.Frontend.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly IBranchService branchService;

        public EmployeesModel( IBranchService branchService)
        {
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

        [FromRoute]
        public long? Id { get; set; }
        public IActionResult OnGet()
        {
            if (Id.HasValue)
            {
                var _branchService = branchService.Find(Id.Value);
                if (_branchService != null)
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
    }
}
