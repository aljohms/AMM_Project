using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AMM_Project.Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMM_Project.Frontend.Pages.Account
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public List<ApplicationUser> users { set; get; }      
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string UserId { get; set; }

        }
        public void OnGet()
        {
            users = _userManager.Users.ToList();

        }
      public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                users = _userManager.Users.ToList();
                return Page();
            }

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

           var user = _userManager.Users.Where(x => x.Id == Input.UserId).FirstOrDefault();
           await _userManager.RemovePasswordAsync(user);
           var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                users = _userManager.Users.ToList();
                return Page();
            }

            // await _signInManager.RefreshSignInAsync(user);//logs in user
            StatusMessage = "Password for "+user.UserName+" has been reset successfully.";

            return RedirectToPage();
        }
        //public async Task<IActionResult> OnPostReset()
        //{
        //    var user = _userManager.Users.Where(x => x.Id == UserId).FirstOrDefault();
        //    if(user!=null)
        //    {
        //        var result = await _userManager.RemovePasswordAsync(user);
        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddPasswordAsync(user, "12345678");
        //            Message = "Password Rest for User"+user.UserName+" Sucessful!";
        //            return Page();
        //        }
        //        return Page();
        //    }
        //    return Page();
        //}

    }
}
