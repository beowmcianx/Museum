using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Museum.Models;

namespace Museum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;

        public IndexModel(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel 
        {
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            Username = user.UserName;
            FullName = user.FullName;
            Email = user.Email;
            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber
            };

            var roles = await _userManager.GetRolesAsync(user);
            Role = roles.FirstOrDefault() ?? "No Role";

            return Page();
        }   

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            user.PhoneNumber = Input.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error updating profile.");
                return Page();
            }

            return RedirectToPage();
        }
    }
}
