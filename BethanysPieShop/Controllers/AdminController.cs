using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BethanysPieShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserManagement()
        {
            return View(_userManager.Users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        public async Task<ActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid) return View(addUserViewModel);

            var user = new IdentityUser()
            {
                UserName = addUserViewModel.UserName,
                Email = addUserViewModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }
            
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addUserViewModel);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string userName, string email)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                user.UserName = userName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("UserManagement", _userManager.Users);
                }

                ModelState.AddModelError("", "User not updated, something went wrong.");
                return View(user);
            }

            return RedirectToAction("UserManagement", _userManager.Users);
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UserManagement");
                else
                    ModelState.AddModelError("", "Something went wrong while deleting the user");

            }
            else
            {
                ModelState.AddModelError("", "This user cannot be found.");
            }
            return View(_userManager.Users);
        }
    }
}
