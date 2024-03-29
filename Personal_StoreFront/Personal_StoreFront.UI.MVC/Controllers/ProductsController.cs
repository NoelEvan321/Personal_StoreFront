﻿using System;
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
using Personal_StoreFront.UI.MVC.Utilities;

using X.PagedList;
using MailKit.Search;

namespace Personal_StoreFront.UI.MVC.Controllers
{
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
        public async Task<IActionResult> TiledView(string searchTerm, int categoryId = 0, int page = 1)
        {
            int pageSize = 3;
            var products =
                _context.Products //SELECT * FROM Products WHERE IsDiscontinued != true 
                    .Include(p => p.CardCondition)
                    .Include(p => p.Category)
                    .Include(p => p.Type).ToList();
            //.Include(p => p.OrderProducts);//Similar to a JOIN - gives access to properties from OrderProduct
            #region Optional Category Filter

            //Create a ViewData object to send a list of categories to the View
            //(This is similar to what you see in the pruducts/crete or products/edit actions)
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
                ViewBag.Category = categoryId;
            }

            #endregion

            #region Optional Search Filter

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchTermForUs = searchTerm.ToLower();

                products = products.Where(p =>
                p.ProductName.ToLower().Contains(searchTermForUs) ||
                p.CardCondition.Condition.ToLower().Contains(searchTermForUs) ||
                p.Type.ProductType1.ToLower().Contains(searchTermForUs) ||
                p.Category.CategoryName.ToLower().Contains(searchTermForUs) ||
                p.ProductDescription.ToLower().Contains(searchTermForUs)).ToList();



                //Store the number of results
                ViewBag.NbrResults = products.Count;
                //Store the search term
                ViewBag.SearchTerm = searchTerm;

            }
            else
            {
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }

            #endregion

            return View(products.ToPagedList(page, pageSize));
        }

        // GET: Products/Details/5
        //[AllowAnonymous]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }
        //    //if (ViewData["Title"].ToString() != "In Stock")
        //    //{
        //    //    var product = await _context.Products
        //    //        .Include(p => p.CardCondition)
        //    //        .Include(p => p.Category)
        //    //        .Include(p => p.Type)
        //    //        .FirstOrDefaultAsync(m => m.ProductId == id);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(product);
        //    }
        //    else
        //    {
        //        var product = await _context.Products.Where(p => p.UnitsInStock > 0)//TODO: Add "In Stock" View
        //            .Include(p => p.CardCondition)
        //            .Include(p => p.Category)
        //            .Include(p => p.Type)
        //            .FirstOrDefaultAsync(m => m.ProductId == id);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(product);
        //    }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.CardCondition)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Products/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ProductName,CardConditionId,ProductSeries,CardDescription,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,ProductPicture,TypeId,Image")] Product product)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - CREATE
                //Check to see if a file was uploaded
                if (product.Image != null)
                {
                    //Check the file type 
                    //- retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(product.Image.FileName);

                    //- Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //- verify the uploaded file has an extension matching one of the extensions in the list above
                    //- AND verify file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)//underscores don't change the number, they just make it easier to read
                    {
                        //Generate a unique filename
                        product.ProductPicture = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwroot/images)
                        //To access wwwroot, add a property to the controller for the _webHostEnvironment (see the top of this class for our example)
                        //Retrieve the path to wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        //variable for the full image path --> this is where we will save the image
                        string fullImagePath = webRootPath + "/images/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);//transfer file from the request to server memory
                            using (var img = Image.FromStream(memoryStream))//add a using statement for the Image class (using System.Drawing)
                            {
                                //now, send the image to the ImageUtility for resizing and thumbnail creation
                                //items needed for the ImageUtility.ResizeImage()
                                //1) (int) maximum image size
                                //2) (int) maximum thumbnail image size
                                //3) (string) full path where the file will be saved
                                //4) (Image) an image
                                //5) (string) filename
                                int maxImageSize = 500;//in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductPicture, img, maxImageSize, maxThumbSize);
                                //myFile.Save("path/to/folder", "filename"); - how to save something that's NOT an image

                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    product.ProductPicture = "noimage.png";
                }

                #endregion

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ProductName,CardConditionId,ProductSeries,CardDescription,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,ProductPicture,TypeId,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region EDIT File Upload
                //retain old image file name so we can delete if a new file was uploaded
                string oldImageName = product.ProductPicture;

                //Check if the user uploaded a file
                if (product.Image != null)
                {
                    //get the file's extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //list valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    //check the file's extension against the list of valid extensions
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //generate a unique file name
                        product.ProductPicture = Guid.NewGuid() + ext;
                        //build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/images/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;
                                ImageUtility.ResizeImage(fullPath, product.ProductPicture, img, maxImageSize, maxThumbSize);
                            }
                        }

                    }
                }
                #endregion

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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
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
