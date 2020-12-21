using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using AMM_Project.Frontend.Models;

namespace AMM_Project.Frontend.Pages.Account
{
    public class LoginModel : PageModel
    {
        //First declare the SignInManager class, which also takes an IdentityUser object.
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        //Inject the sign in manager class into the LoginModel constructor
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            //The sign in manager class provides the authentication APIs we need to implement login and logout
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    //if the Url is not local redirect to index "prevent Open redirect attacks".
                    if(!Url.IsLocalUrl(returnUrl))
                    {
                        return RedirectToPage("/Index");
                    }
                  
                    //Continue to Url

                    return LocalRedirect(returnUrl);
                }
                else
                {
                 // return the errors in our model and display them in the views validation summary
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");                  
                    return Page();
                }

                
                    
                
            }          
                return Page();
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
           // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}
