using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMM_Project.Frontend.Pages
{
    public class EmployeeItemsModel : PageModel
    {
        private readonly IEmployeeItemService employeeItemService;
        //private readonly IBranchService branchService;

        public EmployeeItemsModel(IEmployeeItemService employeeItemService, IBranchService branchService)
        {
            this.employeeItemService = employeeItemService;
            //this.branchService = branchService;
        }
        public class ViewContent
        {
            public long BusnissId { get; set; }
            public long BranchId { get; set; }
            public string BranchName { get; set; }
            public string BusinessName { get; set; }
            public string EmployeeName { get; set; }


        }
        public ViewContent viewContent = new ViewContent();
        public IList<EmployeeItem> employeeItems;
        [BindProperty]
        public EmployeeItem EmployeeItem { set; get; }
        [FromRoute]
        public long? Id { get; set; }
        [BindProperty]
        public IFormFile Attachment { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id.HasValue)
            {
                var getEmployeeItems = employeeItemService.GetAllAsync().Result.Where(x => x.EmployeeId == Id.Value);
                employeeItems = getEmployeeItems.ToList();
                var breadCrumbsItems = getEmployeeItems.FirstOrDefault();
                if (breadCrumbsItems != null)
                {
                    viewContent.BranchName = breadCrumbsItems.Employee.Branch.Name;
                    viewContent.BusnissId = breadCrumbsItems.Employee.Branch.BusinessId;
                    viewContent.BusinessName = breadCrumbsItems.Employee.Branch.Business.Name;
                    viewContent.BranchId = breadCrumbsItems.Employee.Branch.Id;
                    viewContent.EmployeeName = breadCrumbsItems.Employee.FirstName + " " + breadCrumbsItems.Employee.LastName;
                    return null;
                }

                RedirectToPage("/Index");
            }
             return RedirectToPage("/Index");

        }
        public async Task<IActionResult> OnPostAsync()
        {
           await  OnGetAsync();
            //Validate From [Check for requred fields and errors then populate the corresponding message]
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Recipe.Id = Id.GetValueOrDefault();
            //var branch =  new Branch();//if the recipe doesnt exist create a new one
            var employeeItem = await employeeItemService.FindAsync(EmployeeItem.Id) ?? new EmployeeItem();//if the recipe doesnt exist create a new one
            employeeItem.DocumentTitle = EmployeeItem.DocumentTitle;
            employeeItem.DocumentNumber = EmployeeItem.DocumentNumber;
            employeeItem.ExpDate = EmployeeItem.ExpDate;
            employeeItem.ItemAnnualExpectedCost = EmployeeItem.ItemAnnualExpectedCost;
            employeeItem.Notified = false;
            if (Attachment != null)
            {
                //Create a new instance of the system.IO.MemoryStream object and wrap it in a Using statement to be sure that it's cleaned up after we're done with it.
                using (var Stream = new System.IO.MemoryStream())
                {
                    //In order to retrieve the uploaded data from the image property, first copy it to a temporary stream and then read the image's bytes from that stream.
                    await Attachment.CopyToAsync(Stream);
                    //Then copy the uploaded image bytes from the image property to the image stream using its CopyToAsync method
                    //Convert it to bytes and assign them to the recipe object by calling the memory streams ToArray method. 
                    employeeItem.Attachment = Stream.ToArray();
                    //Get the image's content type from the meta data on the uploaded image file property and copy that to the recipe as well. 
                    employeeItem.FileName = Attachment.FileName;
                }
            }

            employeeItem.EmployeeId = Id.Value;
            await employeeItemService.SaveAsync(employeeItem);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetDownloadDbAsync(long? id)
        {
            if (id == null)
            {
                return Page();
            }

            var requestFile = await employeeItemService.FindAsync(id.Value);

            if (requestFile == null)
            {
                return Page();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(requestFile.Attachment, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(requestFile.FileName));
        }
    }
}


        