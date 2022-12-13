using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personal_StoreFront.DATA.EF.Models;

namespace Personal_StoreFront.UI.MVC.Controllers
{
    public class UsersDetailsController : Controller
    {
        private readonly Personal_StoreFrontContext _context;

        public UsersDetailsController(Personal_StoreFrontContext context)
        {
            _context = context;
        }

        // GET: UsersDetails
        public async Task<IActionResult> Index()
        {
              return View(await _context.UsersDetails.ToListAsync());
        }

        // GET: UsersDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UsersDetails == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usersDetail == null)
            {
                return NotFound();
            }

            return View(usersDetail);
        }

        // GET: UsersDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsersDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Address,City,State,CustomerCountry,Zip,Phone,Email")] UsersDetail usersDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usersDetail);
        }

        // GET: UsersDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UsersDetails == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails.FindAsync(id);
            if (usersDetail == null)
            {
                return NotFound();
            }
            return View(usersDetail);
        }

        // POST: UsersDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,FirstName,LastName,Address,City,State,CustomerCountry,Zip,Phone,Email")] UsersDetail usersDetail)
        {
            if (id != usersDetail.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersDetailExists(usersDetail.UserId))
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
            return View(usersDetail);
        }

        // GET: UsersDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UsersDetails == null)
            {
                return NotFound();
            }

            var usersDetail = await _context.UsersDetails
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usersDetail == null)
            {
                return NotFound();
            }

            return View(usersDetail);
        }

        // POST: UsersDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UsersDetails == null)
            {
                return Problem("Entity set 'Personal_StoreFrontContext.UsersDetails'  is null.");
            }
            var usersDetail = await _context.UsersDetails.FindAsync(id);
            if (usersDetail != null)
            {
                _context.UsersDetails.Remove(usersDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersDetailExists(string id)
        {
          return _context.UsersDetails.Any(e => e.UserId == id);
        }
    }
}
