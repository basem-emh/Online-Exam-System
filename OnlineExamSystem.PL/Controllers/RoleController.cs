using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.DAL.Entities.Identity;
using OnlineExamSystem.PL.ViewModels.Identity;

namespace OnlineExamSystem.PL.Controllers
{
    [Authorize("Admin")]
    public class RoleController : Controller
    {
        #region Services
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var mappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
            return View(mappedRole);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role); 

            return View(mappedRole);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }
        #endregion

        #region Update Role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(mappedRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
                return View(roleVM);

            var role = await _roleManager.FindByIdAsync(roleVM.Id);
            if (role == null)
                return NotFound();

            var existingRole = await _roleManager.FindByNameAsync(roleVM.RoleName);
            if (existingRole != null && existingRole.Id != role.Id)
            {
                ModelState.AddModelError(nameof(RoleViewModel.RoleName), "This role name is already used.");
                return View(roleVM);
            }

            role.Name = roleVM.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(roleVM);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var findUser = await _roleManager.FindByIdAsync(id);

                if (findUser is null)
                    return BadRequest();

                await _roleManager.DeleteAsync(findUser);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(HomeController.Error));
            }
        }
        #endregion

        #region Manage Users
        [HttpGet]
        public async Task<IActionResult> ManageUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            
            var users = await _userManager.Users.ToListAsync();
            
            if (users is null)
                return NotFound();

            var usersInRole = new List<UserInRoleViewModel>();
            foreach (var user in users)
            {
                var usrInRole = new UserInRoleViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    usrInRole.IsSelected = true;
                else
                    usrInRole.IsSelected = false;
                usersInRole.Add(usrInRole);
            }
            ViewBag.RoleId = roleId;
            return View(usersInRole);
        }
        
        [HttpPost]
        public async Task<IActionResult> ManageUsers(string roleId , List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserID);
            
                    if (role is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.AddToRoleAsync(appUser, role.Name);

                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }
                return RedirectToAction(nameof(Edit),new{id = roleId });
            }

            return View(users);
        }
        #endregion
    }
}
