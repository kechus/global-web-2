using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalWeb.Data;
using GlobalWeb.Models;

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


        [HttpPost]
        public async Task<IActionResult> LoginView([Bind("Id,Email,Password,Role,IsDeleted")] LoginUser loginUser)
        {
            var foundUser = await _context.LoginUser
                .FirstOrDefaultAsync(m => m.Email == loginUser.Email && m.Password == loginUser.Password);

           if(null == foundUser)
            {
                return Problem("User not found");
            } 
            
            return RedirectToAction("Index"); 
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
