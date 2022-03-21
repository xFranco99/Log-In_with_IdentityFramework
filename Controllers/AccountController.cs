using Log_In.Email;
using Log_In.Models;
using Log_In.Models.Tables;
using Log_In.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Log_In.Controllers
{
    public class AccountController : Controller
    {
        private AppDBContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                      SignInManager<IdentityUser> signInManager, 
                                      AppDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult CheckMail()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                var stud = new Studente();
                stud.UserId = user.Id;

                _context.Studenti.Add(stud);
                _context.SaveChanges();

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                EmailHelper emailHelper = new EmailHelper();
                bool emailResponse = emailHelper.SendEmail(user.Email, confirmationLink);

                if (emailResponse)
                {
                    ViewBag.email = user.Email;
                    return View("CheckMail");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid e-mail address");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {

            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false);
                
                if(result.Succeeded)
                {
                    bool emailStatus = await _userManager.IsEmailConfirmedAsync(appUser);

                    if (result.Succeeded && emailStatus)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (!emailStatus)
                    {

                        ModelState.AddModelError(string.Empty, "Email is unconfirmed, please confirm it first");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }

            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserInfo()
        {
            var user = _userManager.GetUserAsync(User);

            ViewBag.Name = user.Result.UserName;
            ViewBag.Email = user.Result.Email;

            var stud = _context.Studenti
                .FirstOrDefault(
                s => s.UserId == user.Result.Id);

            ViewBag.Stud = stud;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("ChangePassword");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return View("ChangePasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }

            return View(model);
        }

        public IActionResult DeleteUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel delete)
        {
            if (delete.Confirm)
            {
                var user = _userManager.GetUserAsync(User);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {user.Result.UserName} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    var delStud = _context.Studenti.FirstOrDefault(
                        s => s.UserId == user.Result.Id);

                    if (delStud != null)
                    {
                        //delete user and then student tuple
                        var result = await _userManager.DeleteAsync(user.Result);

                        _context.Remove(delStud);
                        _context.SaveChanges();

                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"User with Id = {user.Result.UserName} cannot be found";
                        return View("NotFound");
                    }

                    
                }
            }
            return RedirectToAction("UserInfo");
        }

        [HttpGet]
        public IActionResult UpdateUserInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(StudenteViewModel stud)
        {
            bool modify = false;

            var user = await _userManager.GetUserAsync(User);

            var upStud = _context.Studenti.FirstOrDefault(
                s => s.UserId == user.Id);

            if (stud.Nome != null)
            {
                upStud.Nome = stud.Nome;
                modify = true;
            }

            if (stud.Cognome != null)
            {
                upStud.Cognome = stud.Cognome;
                modify = true;
            }

            if (stud.DataNascita != DateTime.MinValue)
            {
                upStud.DataNascita = stud.DataNascita;
                modify = true;
            }

            if (modify) { upStud.DataModifica = DateTime.Now; }


            _context.Update(upStud);
            _context.SaveChanges();

            return RedirectToAction("UserInfo");
        }

        public IActionResult FakeBuy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FakeBuy(Studente stud)
        {
            var user = await _userManager.GetUserAsync(User);

            var upStud = _context.Studenti.FirstOrDefault(
                s => s.UserId == user.Id);

            upStud.DataAquisto = DateTime.Now;
            _context.Update(upStud);
            _context.SaveChanges();

            return RedirectToAction("UserInfo");
        }
    }
}
