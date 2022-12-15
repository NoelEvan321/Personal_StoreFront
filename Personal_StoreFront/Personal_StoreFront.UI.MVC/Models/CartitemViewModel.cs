using Personal_StoreFront.DATA.EF.Models;

namespace Personal_StoreFront.UI.MVC.Models
{
    public class CartitemViewModel
    {
        //expect to see the item I added to the cart plus a quantity indicator
        public int Qty { get; set; }
        public Product Product { get; set; }
        //The above is an example of a concept called "Containment":
        //This is a use of a complex datatype as a field/property i another class.

        //Complex Datatype: Any class with multiple properties (DateTime, Product, ContactViewModel, etc.)
        //Primitive Datatype: Any class that stores a single value and nothing more. (int, char, decimal, etc.)

        //Constructor
        public CartitemViewModel(int qty, Product product)
        {
            
            Qty = qty;
            Product = product;
        }
    }
}
