using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BlogApp.Entity;
using BlogApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        public readonly BlobContext _context;
        public UserController(BlobContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (@User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Post");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == model.Email && p.Password == model.Password);
                if (user != null)
                {
                    var userclaims = new List<Claim>();
                    userclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                    userclaims.Add(new Claim(ClaimTypes.Name, user.Name ?? ""));
                    userclaims.Add(new Claim(ClaimTypes.GivenName, user.UserName ?? ""));
                    userclaims.Add(new Claim(ClaimTypes.UserData, user.Image ?? ""));

                    if (user.Email == "aslan@gmail.com")
                    {
                        userclaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userclaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authproperties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authproperties
                    );

                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    ModelState.AddModelError("", "email veya şifre yanlış");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        public IActionResult Register()
        {
            if (@User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Post");
            }

            return View();
        }



          [HttpPost]
        public async Task<IActionResult> Register(RegisterView model, IFormFile imagefile)
        {
            var allowexcepsion = new[] { ".jpg", ".png", ".jpeg" };
            var randomfilename = "";
            var path = "";
            var excension = "";

            if (imagefile != null)
            {
                excension = Path.GetExtension(imagefile.FileName).ToLower();  // Dosya uzantısını almak için .jpg gibi
                randomfilename = $"{Guid.NewGuid()}{excension}";

               
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", randomfilename);

               
                if (!allowexcepsion.Contains(excension))
                {
                    ModelState.AddModelError("", "Geçersiz dosya formatı. Lütfen bir resim yükleyin.");
                }
                else
                {
                  
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                }
            }
            else
            {
               
                ModelState.AddModelError("", "Lütfen bir resim yükleyin.");
                
            }

            // Model geçerli ise kullanıcıyı ekle
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == model.UserName || p.Email == model.Email);
                if (user == null)
                {
                    await _context.Users.AddAsync(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "~/img/" + randomfilename  
                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı veya e-posta zaten kullanılıyor.");
                    return View(model);
                }
            }

           
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profil(string username)
        {
            if (username == null)
            {
                return NotFound();
            }
            var user = await _context.Users
            .Include(p => p.posts)
            .ThenInclude(p => p.comments)
            .Include(p => p.comments)
            .ThenInclude(p => p.post)
            .FirstOrDefaultAsync(p => p.UserName == username);
            return View(user);
        }

    }
}