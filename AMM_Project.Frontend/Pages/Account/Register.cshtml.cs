using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using AMM_Project.Frontend.Models;

namespace AMM_Project.Frontend.Pages.Account
{
    public class RegisterModel : PageModel
    {
        /* The user manager class is an important part of asp.net core identity because it has all the APIs for managing users.
       I've injected it into the RegisterModel's constructor. It's dependencies were already registered when we added identity services to our start up class. */
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
           
        }
        //Input Model
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirsrName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string RePassword { get; set; }
            //[Required]
            //[DataType(DataType.Text)]
            //[Display(Name = "First name")]
            //public string FirstName { get; set; }
            //[Required]
            //[DataType(DataType.Text)]
            //[Display(Name = "Last name")]
            //public string LastName { get; set; }

        }
        public async Task<IActionResult> OnPost()
        {
            //If Form Submitted Successfully
            if (ModelState.IsValid)
            {
                //create an instance of the identity user class.
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = Input.Username,
                    Email = Input.Username,
                    FirstName = Input.FirsrName,
                    LastName = Input.LastName
                    
                    //Name = Input.Name,
                    //DOB = Input.DOB,
                };
                //call the user managers create async method, which takes in the user object and the password
                //Create async returns an identity result object with a succeeded boolean property. This will tell us if the user was successfully created.
                var result = await _userManager.CreateAsync(user, Input.Password);
                //if user created successfulley return to loging
                if(result.Succeeded)
                {
                    return RedirectToPage("/account/login");
                }
                else
                {
                    //If the result wasn't successful, there's an errors property that will list the reasons why it failed. And, we'll go ahead and iterate through that errors collection.
                    foreach(var errors in result.Errors)
                    {
                        // return the errors in our model and display them in the views validation summary
                        ModelState.AddModelError(string.Empty, errors.Description);
                        
                    }
                    return Page();
                }
            }
            else
            {
                return Page();

            }
        }
    }
}
