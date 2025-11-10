using HMS.Interfaces.Services;
using HMS.Models.DTOs.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var staff = await _userService.GetAllHospitalStaffs(CancellationToken.None);

            return View(staff);
        }

        public IActionResult StaffIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DoctorDashboard()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            var loginResponse = await _userService.LoginAsync(model, CancellationToken.None);
            var checkRole = "";
            if (loginResponse.Status)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.Data.FirstName),
                    new Claim(ClaimTypes.GivenName, loginResponse.Data.FullName),
                    new Claim(ClaimTypes.Email, loginResponse.Data.Email),
                    new Claim(ClaimTypes.NameIdentifier, loginResponse.Data.UserId.ToString()),
                };

                foreach (var role in loginResponse.Data.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    checkRole = role.Name;
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authenticationProperties = new AuthenticationProperties();
                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, authenticationProperties);
                if (checkRole == "Patient")
                {
                    return RedirectToAction("Index", "Patient");
                }
                else if (checkRole == "Doctor")
                {
                    return RedirectToAction("DoctorDashboard", "User");
                }
                return RedirectToAction("StaffIndex", "User");

            }
            else
            {
                ViewBag.ErrorMessage = loginResponse.Message;
                return View(model);
            }
           
        }
    }
}
