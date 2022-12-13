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
    public class CardConditionsController : Controller
    {
        private readonly Personal_StoreFrontContext _context;

        public CardConditionsController(Personal_StoreFrontContext context)
        {
            _context = context;
        }

        // GET: CardConditions
        public async Task<IActionResult> Index()
        {
              return View(await _context.CardConditions.ToListAsync());
        }

        // GET: CardConditions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CardConditions == null)
            {
                return NotFound();
            }

            var cardCondition = await _context.CardConditions
                .FirstOrDefaultAsync(m => m.CardConditionId == id);
            if (cardCondition == null)
            {
                return NotFound();
            }

            return View(cardCondition);
        }

        // GET: CardConditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CardConditions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardConditionId,Condition,ConditionDescription")] CardCondition cardCondition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardCondition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cardCondition);
        }

        // GET: CardConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CardConditions == null)
            {
                return NotFound();
            }

            var cardCondition = await _context.CardConditions.FindAsync(id);
            if (cardCondition == null)
            {
                return NotFound();
            }
            return View(cardCondition);
        }

        // POST: CardConditions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardConditionId,Condition,ConditionDescription")] CardCondition cardCondition)
        {
            if (id != cardCondition.CardConditionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardCondition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardConditionExists(cardCondition.CardConditionId))
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
            return View(cardCondition);
        }

        // GET: CardConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CardConditions == null)
            {
                return NotFound();
            }

            var cardCondition = await _context.CardConditions
                .FirstOrDefaultAsync(m => m.CardConditionId == id);
            if (cardCondition == null)
            {
                return NotFound();
            }

            return View(cardCondition);
        }

        // POST: CardConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CardConditions == null)
            {
                return Problem("Entity set 'Personal_StoreFrontContext.CardConditions'  is null.");
            }
            var cardCondition = await _context.CardConditions.FindAsync(id);
            if (cardCondition != null)
            {
                _context.CardConditions.Remove(cardCondition);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardConditionExists(int id)
        {
          return _context.CardConditions.Any(e => e.CardConditionId == id);
        }
    }
}
