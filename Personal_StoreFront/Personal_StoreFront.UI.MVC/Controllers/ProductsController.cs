using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personal_StoreFront.DATA.EF.Models;

using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Personal_StoreFront.UI.MVC.Controllers;

namespace Personal_StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly Personal_StoreFrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(Personal_StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var personal_StoreFrontContext = _context.Products.Include(p => p.CardCondition).Include(p => p.Category).Include(p => p.Type);
            return View(await personal_StoreFrontContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> TiledView()
        {
            var products =
                _context.Products //SELECT * FROM Products WHERE IsDiscontinued != true 
                    .Include(p => p.CardCondition)
                    .Include(p => p.Category)
                    .Include(p => p.Type);
                //.Include(p => p.OrderProducts);//Similar to a JOIN - gives access to properties from OrderProduct

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            if (ViewData["Title"].ToString() != "In Stock")
            {
                var product = await _context.Products
                    .Include(p => p.CardCondition)
                    .Include(p => p.Category)
                    .Include(p => p.Type)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            else
            {
                var product = await _context.Products.Where(p => p.UnitsInStock > 0)//TODO: Add "In Stock" View
                    .Include(p => p.CardCondition)
                    .Include(p => p.Category)
                    .Include(p => p.Type)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CardConditionId"] = new SelectList(_context.CardConditions, "CardConditionId", "Condition");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "ProductType1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ProductName,CardConditionId,ProductSeries,CardDescription,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,ProductPicture,TypeId,Image")] Product product)
        {
            if (ModelState.IsValid)
            {

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardConditionId"] = new SelectList(_context.CardConditions, "CardConditionId", "Condition", product.CardConditionId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "ProductType1", product.TypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CardConditionId"] = new SelectList(_context.CardConditions, "CardConditionId", "Condition", product.CardConditionId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "ProductType1", product.TypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ProductName,CardConditionId,ProductSeries,CardDescription,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,ProductPicture,TypeId,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CardConditionId"] = new SelectList(_context.CardConditions, "CardConditionId", "Condition", product.CardConditionId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "ProductType1", product.TypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CardCondition)
                .Include(p => p.Category)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'Personal_StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
