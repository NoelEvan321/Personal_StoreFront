using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Personal_StoreFront.DATA.EF.Models//.Metadata
{
    public class CardConditionMetadata
    {
        public int CardConditionId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        public string Condition { get; set; } = null!;
        [StringLength(500, ErrorMessage = "*Max of 500 characters")]
        [Display(Name = "Description")]
        public string? ConditionDescription { get; set; }
    }

    public class CategoryMetadata
    {
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        public string CategoryName { get; set; } = null!;
        [StringLength(500, ErrorMessage = "*Max of 500 characters")]
        public string? Description { get; set; }
    }

    public class OrderDetailMetadata
    {
        public string UserID { get; set; } = null!;

        [Display(Name = "Order Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "*Max 100 characters")]
        [Display(Name = "Ship To")]
        public string ShipToName { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [Display(Name = "City")]
        public string ShipCity { get; set; } = null!;

        [StringLength(2, ErrorMessage = "*Must be 2 characters")]
        [Display(Name = "State")]
        public string? ShipState { get; set; }

        [StringLength(5, ErrorMessage = "*Must be 5 characters")]
        [Display(Name = "Zip")]
        [DataType(DataType.PostalCode)]
        public string? ShipZip { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [Display(Name = "Country")]
        public string  ShipCountry { get; set; } = null!;

        [Display(Name = "Is Active?")]
        public bool? IsActive { get; set; }

    }

    //public class OrderProductMetadata
    //{

    //}

    public class ProductMetadata
    {
        public int ProductId { get; set; }
        public int? CategoryID { get; set; }

        [StringLength(200, ErrorMessage = "*Max of 200 characters.")]
        [Display(Name = "Product")]//could write a ternary to state if carddesc isn't null, read as "card" else product.
        [Required]
        public string ProductName { get; set; } = null!;
        public int? CardConditionID { get; set; }

        [StringLength(150, ErrorMessage = "*Max of 150 characters.")]
        [Display(Name = "Product Set")]
        public string? ProductSeries { get; set; }

        [StringLength(500, ErrorMessage = "*Max of 500 characters.")]
        [Display(Name = "Card Description")]
        public string? CardDescription { get; set; }//TODO: Create a placeholder "only needed if product is card"

        [StringLength(500, ErrorMessage = "*Max of 500 characters.")]
        [Display(Name = "Product Description")]
        public string? ProductDescription { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        [Display(Name = "Price")]
        [Range(0, (double)decimal.MaxValue)]
        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        [Display(Name = "In Stock")]
        public int UnitsInStock { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        [Display(Name = "On Order")]
        public int UnitsOnOrder { get; set; }

        [StringLength(75, ErrorMessage = "*Max of 75 characters.")]
        [Display(Name = "Picture")]
        public string? ProductPicture { get; set; }

        public int? TypeID { get; set; }


    }

    public class ProductTypeMetadata
    {
        [Required]
        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; } = null!;

        [StringLength(500, ErrorMessage = "*Max of 500 characters.")]
        [Display(Name = "Description")]
        public string? ProductTypeDescription { get; set; }
    }

    public class UsersDetailMetadata
    {
        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(150, ErrorMessage = "*Max of 150 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        public string? City { get; set; }

        [StringLength(10, ErrorMessage = "*Please enter your 2 character state code")]
        public string? State { get; set; }//TODO: Create a placeholder "2 character State Code"

        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [Display(Name = "User's Country")]
        public string? CustomerCountry { get; set; }//TODO: Need to make changes to DB names

        [StringLength(5, ErrorMessage = "*Max of 5 characters.")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(24, ErrorMessage = "*Max of 24 characters.")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(50, ErrorMessage = "*Max of 50 characters.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }


}
