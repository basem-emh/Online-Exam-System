using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.BLL.Custom_Models;
using OnlineExamSystem.DAL.Entities.Identity;
using OnlineExamSystem.PL.ViewModels.Identity;

namespace OnlineExamSystem.PL.Controllers
{
    [Authorize("Admin")]
    public class UserController : Controller
    {
        #region Services
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        } 
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();
            return View(users);
        } 
        #endregion

        #region Sign Up // Register
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUpdateUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(userViewModel.UserName);

            if (user is { })
            {
                ModelState.AddModelError(nameof(AddUpdateUserViewModel.UserName), "This username is already in use with another account");
                return View(userViewModel);
            }

            user = new ApplicationUser()
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Email= userViewModel.Email,
                UserName = userViewModel.UserName,
                PhoneNumber = userViewModel.PhoneNumber,
                AddedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(userViewModel);
        }
        #endregion

        #region Update User
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            
            if (user is null)
                return NotFound();

            var userViewModel = new AddUpdateUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddUpdateUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return View(userViewModel);

            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            
            if (user is null)
                return NotFound();

            var existingUser = await _userManager.FindByNameAsync(userViewModel.UserName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(nameof(AddUpdateUserViewModel.UserName), "This username is already in use with another account");
                return View(userViewModel);
            }

            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;
            user.UserName = userViewModel.UserName;
            user.PhoneNumber = userViewModel.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(userViewModel.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, userViewModel.Password);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(userViewModel);
                }
            }

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(userViewModel);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var userDetails = new UserDetailsDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = _userManager.GetRolesAsync(user).Result,
                AddedAt = user.AddedAt
            };
            return View(userDetails);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var findUser = await _userManager.FindByIdAsync(id);

                if (findUser is null)
                    return BadRequest();

                await _userManager.DeleteAsync(findUser);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(HomeController.Error));
            }
        }
        #endregion
    }
}
