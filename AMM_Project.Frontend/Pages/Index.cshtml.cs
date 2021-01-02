using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
using AMM_Project.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBusinessService _businessService;
        private readonly IBranchItemService _branchItemService;
        private readonly IEmployeeItemService _employeeItemService;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };

        public IndexModel(IBusinessService businessService, IBranchItemService branchItemService, IEmployeeItemService employeeItemService, IConfiguration config)
        {
            _businessService = businessService;
            _branchItemService = branchItemService;
            _employeeItemService = employeeItemService;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");


        }

        public IList<Business> businesses; 
        //Input from View
        [BindProperty]
        public Business Business { set; get; }
        [BindProperty]
        public IFormFile Image { get; set; }

        public class Upcoming
        {
            public string Business { get; set; }
            public string Branch { get; set; }
            public string Details { get; set; }
            public DateTime date { get; set; }
        }
        public IList<Upcoming> upcomings = new List<Upcoming>();
        public  void OnGetAsync()
        {
            businesses =  _businessService.GetAll().ToList();
            var employeeItems =  _employeeItemService.GetAll();
            var branchItems =  _branchItemService.GetAll();
            if (branchItems != null)
            {
                foreach (var item in branchItems)
                {
                    if (item.ExpDate.HasValue && item.ExpDate.Value.AddDays(-30).Date <= DateTime.Today)
                    {
                        Upcoming upcoming = new Upcoming();
                        upcoming.date = item.ExpDate.Value;
                        upcoming.Business = item.Branch.Business.Name;
                        upcoming.Branch = item.Branch.Name;
                        upcoming.Details = item.DocumentTitle;
                        upcomings.Add(upcoming);
                    }
                }
            }
            if (employeeItems != null)
            {
                foreach (var item in employeeItems)
                {
                    if (item.ExpDate.HasValue && item.ExpDate.Value.AddDays(-30).Date <= DateTime.Today)
                    {
                        Upcoming upcoming = new Upcoming();
                        upcoming.date = item.ExpDate.Value;
                        upcoming.Business = item.Employee.Branch.Business.Name;
                        upcoming.Branch = item.Employee.Branch.Name;
                        upcoming.Details = item.DocumentTitle + " " + item.Employee.FirstName + " " + item.Employee.LastName;
                        upcomings.Add(upcoming);
                    }
                }
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            businesses = await _businessService.GetAllAsync();



            //Validate Form Check for required fields and errors then populate the corresponding message
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var business = await _businessService.FindAsync(Business.Id) ?? new Business();//if the business doesnt exist create a new one 
            //Check if the business Name unique if not return error
            if(businesses.FirstOrDefault(x=>x.Name== Business.Name) !=null && business.Id!= businesses.FirstOrDefault(x => x.Name == Business.Name).Id)
            {
                ModelState.AddModelError("Business.Name","Business Already Exists");
                businesses = await _businessService.GetAllAsync();
                return Page();
            }
            //get data from the Bind Property Model
            business.Id = Business.Id;
            business.Name = Business.Name;
            business.ActivityField = Business.ActivityField;
            business.ContactPerson = Business.ContactPerson;
            business.ContactNumber = Business.ContactNumber;

            //Only update image when its changed or uploaded
            if (Image != null)
            {
                var formFileContent =
               await FileHelpers.ProcessFormFile<Business>(
                   Image, ModelState, _permittedExtensions,
                   _fileSizeLimit);

                // Perform a second check to catch ProcessFormFile method
                // violations. If any validation check fails, return to the
                // page.
                if (!ModelState.IsValid)
                {
                   // Result = "Please correct the form.";

                    return Page();
                }
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
            await _businessService.SaveAsync(business);
            return RedirectToPage();
        }
             
        public async Task<IActionResult> OnPostDelete()
        {
            await _businessService.DeleteAsync(Business.Id);
            return RedirectToPage();
        }
    }
}
