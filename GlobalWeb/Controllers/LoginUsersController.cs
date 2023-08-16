using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalWeb.Data;
using GlobalWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GlobalWeb.Controllers
{
    public class LoginUsersController : Controller
    {
        private readonly GlobalWebContext _context;

        public LoginUsersController(GlobalWebContext context)
        {
            _context = context;
        }

        // GET: LoginUsers
        public async Task<IActionResult> Index()
        {
              return _context.LoginUser != null ? 
                          View(await _context.LoginUser.Where(user=>user.IsDeleted == false).ToListAsync()) :
                          Problem("Entity set 'GlobalWebContext.LoginUser'  is null.");
        }

        // GET: LoginUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoginUser == null)
            {
                return NotFound();
            }

            var loginUser = await _context.LoginUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginUser == null)
            {
                return NotFound();
            }

            return View(loginUser);
        }

        // GET: LoginUsers/Create
        public IActionResult Create()
        {
            return View();
        }


        static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                }

                return builder.ToString();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginView([Bind("Id,Email,Password,Role,IsDeleted")] LoginUser loginUser)
        {
            var foundUser = await _context.LoginUser
                .FirstOrDefaultAsync(m => m.Email == loginUser.Email && m.Password == CalculateMD5Hash(loginUser.Password));

            if (null == foundUser)
            {
                return Problem("User not found");
            } 

            List<Claim> c = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, foundUser.Email),
                new Claim(ClaimTypes.Role, foundUser.Role)
            };
            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties p = new();

            p.AllowRefresh = true;
            p.IsPersistent = true;

            p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);

            return RedirectToAction("Index", "Students"); 
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginView");
        }

        public IActionResult LoginView()
        {
            return View();
        }


        // POST: LoginUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Role,IsDeleted")] LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                loginUser.Password = CalculateMD5Hash(loginUser.Password);
                _context.Add(loginUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loginUser);
        }

        // GET: LoginUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoginUser == null)
            {
                return NotFound();
            }

            var loginUser = await _context.LoginUser.FindAsync(id);
            if (loginUser == null)
            {
                return NotFound();
            }
            return View(loginUser);
        }

        // POST: LoginUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Role,IsDeleted")] LoginUser loginUser)
        {
            if (id != loginUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    loginUser.Password = CalculateMD5Hash(loginUser.Password);
                    _context.Update(loginUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginUserExists(loginUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loginUser);
        }

        // GET: LoginUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoginUser == null)
            {
                return NotFound();
            }

            var loginUser = await _context.LoginUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginUser == null)
            {
                return NotFound();
            }

            return View(loginUser);
        }

        // POST: LoginUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoginUser == null)
            {
                return Problem("Entity set 'GlobalWebContext.LoginUser'  is null.");
            }
            var loginUser = await _context.LoginUser.FindAsync(id);
            if (loginUser != null)
            {
                loginUser.IsDeleted = true;
                _context.LoginUser.Update(loginUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginUserExists(int id)
        {
          return (_context.LoginUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
