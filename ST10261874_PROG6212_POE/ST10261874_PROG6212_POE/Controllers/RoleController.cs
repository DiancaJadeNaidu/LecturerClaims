using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10261874_PROG6212_POE.Areas.Identity.Data;
using ST10261874_PROG6212_POE.Models;

namespace ST10261874_PROG6212_POE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; // role manager for handling role operations
        private readonly UserManager<ApplicationUser> _userManager; // user manager for handling user operations

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager; // initialize role manager
            _userManager = userManager; // initialize user manager
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            try
            {
                if (ModelState.IsValid && !string.IsNullOrEmpty(roleName)) // validate model state and role name
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName); // check if role already exists
                    if (!roleExists) // if the role does not exist
                    {
                        var result = await _roleManager.CreateAsync(new IdentityRole(roleName)); // create new role
                        if (result.Succeeded) // if role creation is successful
                        {
                            return RedirectToAction(nameof(ManageUserRoles)); // redirect to ManageUserRoles action
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (for production, you should use a logging framework)
                ModelState.AddModelError("", $"Error creating role: {ex.Message}");
            }

            return RedirectToAction(nameof(ManageUserRoles)); // return to ManageUserRoles if creation fails
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserRoles()
        {
            try
            {
                var roles = new[] { "Admin", "Lecturer", "Programme Coordinator", "Academic Manager" }; // predefined roles

                var users = await _userManager.Users.ToListAsync(); // get list of all users
                var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(); // get list of all roles

                // Ensure roles are in the database
                foreach (var role in roles) // iterate over predefined roles
                {
                    if (!await _roleManager.RoleExistsAsync(role)) // check if role exists
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role)); // create role if it does not exist
                    }
                }

                var userRolesViewModelList = new List<UserRoleViewModel>(); // list to hold user role view models

                foreach (var user in users) // iterate over all users
                {
                    var userRoles = await _userManager.GetRolesAsync(user); // get roles for each user

                    var userRolesViewModel = new UserRoleViewModel
                    {
                        Email = user.Email, // set user email
                        Roles = userRoles.ToList(), // set user roles
                        AvailableRoles = allRoles // set all available roles
                    };

                    userRolesViewModelList.Add(userRolesViewModel); // add view model to the list
                }

                return View(userRolesViewModelList); // return the view with user roles data
            }
            catch (Exception ex)
            {
                // Log the exception (for production, you should use a logging framework)
                ModelState.AddModelError("", $"Error managing user roles: {ex.Message}");
                return View(new List<UserRoleViewModel>()); // Return an empty list if an error occurs
            }
        }

        // POST: Roles/AssignRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles(string email, IList<string> roles)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) // check if email is null or empty
                {
                    ModelState.AddModelError("", "Email cannot be null or empty."); // add model state error
                    return RedirectToAction(nameof(ManageUserRoles)); // redirect to ManageUserRoles action
                }

                var user = await _userManager.FindByEmailAsync(email); // find user by email
                if (user == null) // check if user is not found
                {
                    return NotFound(); // return 404 error if user does not exist
                }

                // Remove all roles first
                var currentRoles = await _userManager.GetRolesAsync(user); // get current roles of the user
                await _userManager.RemoveFromRolesAsync(user, currentRoles); // remove all current roles

                // Add new roles if provided
                if (roles != null && roles.Any()) // check if roles are provided
                {
                    await _userManager.AddToRolesAsync(user, roles); // add new roles to the user
                }
            }
            catch (Exception ex)
            {
                // Log the exception (for production, you should use a logging framework)
                ModelState.AddModelError("", $"Error assigning roles: {ex.Message}");
            }

            return RedirectToAction(nameof(ManageUserRoles)); // redirect to ManageUserRoles action
        }
    }
}
