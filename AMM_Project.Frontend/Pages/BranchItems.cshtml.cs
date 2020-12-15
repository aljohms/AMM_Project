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
    public class BranchItemsModel : PageModel
    {
        private readonly IBranchItemService branchItemService;
        private readonly IBranchService branchService;

        public BranchItemsModel(IBranchItemService branchItemService, IBranchService branchService)
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
        public long? Id { get; set; }
        [BindProperty]
        public IFormFile Attachment { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id.HasValue)
            {
                branchItems = await branchItemService.GetAllAsync(Id.Value);
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
            var branchItem = await branchItemService.FindAsync(BranchItem.Id) ?? new BranchItem();//if the recipe doesnt exist create a new one
            branchItem.DocumentTitle = BranchItem.DocumentTitle;
            branchItem.DocumentNumber = BranchItem.DocumentNumber;
            branchItem.ExpDate = BranchItem.ExpDate;
            branchItem.AnnualCost = BranchItem.AnnualCost;
            branchItem.Notified = false;
            if (Attachment != null)
            {
                //Create a new instance of the system.IO.MemoryStream object and wrap it in a Using statement to be sure that it's cleaned up after we're done with it.
                using (var Stream = new System.IO.MemoryStream())
                {
                    //In order to retrieve the uploaded data from the image property, first copy it to a temporary stream and then read the image's bytes from that stream.
                    await Attachment.CopyToAsync(Stream);
                    //Then copy the uploaded image bytes from the image property to the image stream using its CopyToAsync method
                    //Convert it to bytes and assign them to the recipe object by calling the memory streams ToArray method. 
                    branchItem.Attachment = Stream.ToArray();
                    //Get the image's content type from the meta data on the uploaded image file property and copy that to the recipe as well. 
                    branchItem.FileName = Attachment.FileName;
                }
            }

            branchItem.BranchId = Id.Value;
            await branchItemService.SaveAsync(branchItem);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetDownloadDbAsync(long? id)
        {
            if (id == null)
            {
                return Page();
            }

            var requestFile = await branchItemService.FindAsync(id.Value);

            if (requestFile == null)
            {
                return Page();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(requestFile.Attachment, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(requestFile.FileName));
        }
    }
}


        