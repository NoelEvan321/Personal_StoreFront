using Microsoft.AspNetCore.Mvc;

using Personal_StoreFront.DATA.EF.Models; //Grants access to the context
using Microsoft.AspNetCore.Identity; //Grants access to UserManager
using Personal_StoreFront.UI.MVC.Models; //Grants access to the CartitemViewModel class
using Newtonsoft.Json; //Provides easier management of the Shopping Cart

namespace Personal_StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly Personal_StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(Personal_StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            //Retrieve the contents from the Session shopping cart (stored as JSON) 
            //and convert them to C# via Newtonsoft.Json. After converting to C#, we 
            //can pass the collection of cart contents back to the strongly-typed 
            //view for display.

            //Retrieve the cart contents
            var sessionCart = HttpContext.Session.GetString("cart");

            //Create a shell for the local (C# version) of the cart
            Dictionary<int, CartitemViewModel> shoppingCart = null;

            //Check to see if the sessionCart was null
            if (sessionCart == null || sessionCart.Count() == 0)
            {
                //User either hasn't put anything in the cart, or they have removed 
                //all items from the cart. So, we'll set shoppingCart to an empty Dictionary
                shoppingCart = new Dictionary<int, CartitemViewModel>();

                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;

                //Deserialize the cart contents from JSON into C#
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartitemViewModel>>(sessionCart);
            }


            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            //Empty shell for LOCAL shopping cart variable
            //int (key) => ProductId
            //CartitemViewModel (value) => Product & Qty
            Dictionary<int, CartitemViewModel> shoppingCart = null;




            var sessionCart = HttpContext.Session.GetString("cart");

            //Check to see if the Session object exists
            //If not, instantiate the new local collection
            if (string.IsNullOrEmpty(sessionCart))
            {
                //If the session didn't exist yet, instantiate the collection as a new object
                shoppingCart = new Dictionary<int, CartitemViewModel>();
            }
            else
            {
                //Cart already exists - transfer the existing cart items from the Session into our 
                //local shoppingCart.
                //DeserializeObject() is a method that converts JSON to C# - we MUST tell this method 
                //which C# class to convert the JSON into (here, that's our Dictionary<int, CartitemViewModel>)
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartitemViewModel>>(sessionCart);
            }

            //Add newly selected products to the cart
            //Retrieve the product from the database
            Product product = _context.Products.Find(id);

            //Instantiate the CIVM object so we can add to the cart
            CartitemViewModel civm = new CartitemViewModel(1, product); //Add 1 of the selected product to the cart

            //If the product was already in the cart, increase the quantity by 1.
            //Otherwise, add the new CIVM object to the cart.
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }

            //Update the session version of the cart
            //Take the local copy and Serialize it as JSON
            //Then assign that value to our session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");

        }


        public IActionResult RemoveFromCart(int id)
        {
            //Retrieve the cart from the session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert the JSON to C#
            Dictionary<int, CartitemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartitemViewModel>>(sessionCart);

            //Remove the item
            shoppingCart.Remove(id);

            //If there are no remaining items in the cart, remove it from session
            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //Otherwise, update the session with the new cart contents
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }


        public IActionResult UpdateCart(int productId, int qty)
        {
            //Retrieve the cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert from JSON to C#
            Dictionary<int, CartitemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartitemViewModel>>(sessionCart);

            //Update the qty for our specific cart item
            if (qty == 0)
            {
                //shoppingCart[productId].Qty = 1;

                shoppingCart.Remove(productId);
            }
            else
            {
                shoppingCart[productId].Qty = qty;
            }

            //Update the session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> SubmitOrder()
        {

            //Retrieve the current user's ID
            //This is a mechanism provided by Identity to retrieve the UserID in the 
            //current HttpContext. If you need to retrieve an ID in ANY controller you can use this:
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            //Retrieve the userdetails record
            UsersDetail ud = _context.UsersDetails.Find(userId);

            //Create an Order object and assign the values
            OrderDetail o = new OrderDetail()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShipToName = ud.FirstName + " " + ud.LastName,
                ShipCity = ud.City,
                ShipState = ud.State,
                ShipCountry =ud.CustomerCountry,
                ShipZip = ud.Zip,
                IsActive = true
            };

            //Add the Order object to the _context
            _context.OrderDetails.Add(o);

            //Retrieve the session cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert the cart to C#
            Dictionary<int, CartitemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartitemViewModel>>(sessionCart);

            //Create an OrderProduct object for each item in the cart
            foreach (var item in shoppingCart)
            {
                //Create and assign the OrderProduct object
                OrderProduct op = new OrderProduct()
                {
                    ProductId = item.Key,
                    OrderId = o.OrderId,
                    UnitQuantity = (short)item.Value.Qty,
                    ProductPrice = item.Value.Product.ProductPrice                  
                };

                //Only need to add items to an existing entity if the items are a related record (like from a linking table)
                o.OrderProducts.Add(op);
            }

            //Commit the changes and save to the database
            _context.SaveChanges();

            return RedirectToAction("Index", "Orders");

        }


    }
}
