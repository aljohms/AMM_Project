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
        private readonly IEmployeeService employeeService;

        public EmployeeItemsModel(IEmployeeItemService employeeItemService, IEmployeeService employeeService)
        {
            this.employeeItemService = employeeItemService;
            this.employeeService = employeeService;
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
                employeeItems = employeeItemService.GetAllAsync().Result.Where(x => x.EmployeeId == Id.Value).ToList();
                var breadCrumbsItems = await employeeService.FindAsync(Id.Value);
                if (breadCrumbsItems != null)
                {
                    viewContent.BranchName = breadCrumbsItems.Branch.Name;
                    viewContent.BusnissId = breadCrumbsItems.Branch.BusinessId;
                    viewContent.BusinessName = breadCrumbsItems.Branch.Business.Name;
                    viewContent.BranchId = breadCrumbsItems.Branch.Id;
                    viewContent.EmployeeName = breadCrumbsItems.FirstName + " " + breadCrumbsItems.LastName;
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
            if (employeeItems.FirstOrDefault(x => x.DocumentTitle == EmployeeItem.DocumentTitle) != null && employeeItem.Id != employeeItems.FirstOrDefault(x => x.DocumentTitle == EmployeeItem.DocumentTitle).Id)
            {
                ModelState.AddModelError("EmployeeItem.DocumentTitle", "Document  Already Exists");
                await OnGetAsync();
                return Page();
            }
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


        