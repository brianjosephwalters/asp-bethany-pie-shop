using BethanysPieShop.Auth;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BethanysPieShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        [HttpPost]
        public async Task<ActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid) return View(addUserViewModel);

            var user = new ApplicationUser()
            {
                UserName = addUserViewModel.UserName,
                Email = addUserViewModel.Email,
                BirthDay = addUserViewModel.BirthDay,
                City = addUserViewModel.City,
                Country = addUserViewModel.Country
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
            var editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                BirthDay = user.BirthDay,
                City = user.City,
                Country = user.Country
            };

            var claims = await _userManager.GetClaimsAsync(user);
            editUserViewModel.Claims = claims.Select(c => c.Value).ToList();
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

            if (user != null)
            {
                user.Email = editUserViewModel.Email;
                user.UserName = editUserViewModel.UserName;
                user.BirthDay = editUserViewModel.BirthDay;
                user.City = editUserViewModel.City;
                user.Country = editUserViewModel.Country;

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
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
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


        public IActionResult RoleManagement() => View(_roleManager.Roles);

        public IActionResult AddRole() => View();

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel addRoleViewModel)
        {
            if (!ModelState.IsValid) return View(addRoleViewModel);

            var role = new IdentityRole()
            {
                Name = addRoleViewModel.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addRoleViewModel);
        }


        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            var editRoleViewModel = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleViewModel.Users.Add(user.UserName);
                }
            }
            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
            {
                role.Name = editRoleViewModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleManagement", _roleManager.Roles);
                }

                ModelState.AddModelError("", "Role not updated, something went wrong.");
                return View(editRoleViewModel);
            }

            return RedirectToAction("RoleManagement", _roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleManagement", _roleManager.Roles);
                }
                else
                {
                    ModelState.AddModelError("", "This role cannot be found");
                }
            }
            return RedirectToAction("RoleManagement", _roleManager.Roles);
        }


        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            var addUserToRoleViewModel = new UserRoleViewModel()
            {
                RoleId = role.Id
            };

            foreach (var user in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        public async Task<IActionResult> ManageClaimsForUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            var claimsManagementViewModel = new ClaimsManagementViewModel()
            {
                UserId = user.Id,
                AllClaimsList = BethanysPieShopClaimTypes.ClaimsList
            };

            return View(claimsManagementViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaimsForUser(ClaimsManagementViewModel claimsManagementViewModel)
        {
            var user = await _userManager.FindByIdAsync(claimsManagementViewModel.UserId);

            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(claimsManagementViewModel.ClaimId, claimsManagementViewModel.ClaimId));

            if (result.Succeeded)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            ModelState.AddModelError("", "User not updated, something went wrong");

            return View(claimsManagementViewModel);
        }


    }
}
