using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBusinessService businessService;


        public IndexModel(ILogger<IndexModel> logger, IBusinessService businessService)
        {
            _logger = logger;
            this.businessService = businessService;
        }
        public IList<Business> businesses;
        public bool isNewBusiness { get { return Id == null; } }

        [FromRoute]
        public long? Id { set; get; }
        [BindProperty]
        public Business Business { set; get; }
        [BindProperty]
        public IFormFile Image { get; set; }
        public async Task OnGetAsync()
        {
            Business = await businessService.FindAsync(Id.GetValueOrDefault()) ?? new Business();
            businesses = await businessService.GetAllAsync();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //Validate From [Check for requred fields and errors then populate the corresponding message]
            if (!ModelState.IsValid)
            {
                return Page();
            }


            //Recipe.Id = Id.GetValueOrDefault();
            var business = await businessService.FindAsync(Id.GetValueOrDefault()) ?? new Business();//if the recipe doesnt exist create a new one
            //get data from the Bind Property Model
            business.Name = Business.Name;
            business.ActivityField = Business.ActivityField;
            business.ContactPerson = Business.ContactPerson;
            business.ContactNumber = Business.ContactNumber;

            //Only update image when its changed or uploaded
            if (Image != null)
            {
                //Create a new instance of the system.IO.MemoryStream object and wrap it in a Using statement to be sure that it's cleaned up after we're done with it.
                using (var Stream = new System.IO.MemoryStream())
                {
                    //In order to retrieve the uploaded data from the image property, first copy it to a temporary stream and then read the image's bytes from that stream.
                    await Image.CopyToAsync(Stream);
                    //Then copy the uploaded image bytes from the image property to the image stream using its CopyToAsync method
                    //Convert it to bytes and assign them to the recipe object by calling the memory streams ToArray method. 
                    business.Image = Stream.ToArray();
                    //Get the image's content type from the meta data on the uploaded image file property and copy that to the recipe as well. 
                    business.ImageContentType = Image.ContentType;
                }
            }

            await businessService.SaveAsync(business);
            return RedirectToPage();
        }
    }
}
