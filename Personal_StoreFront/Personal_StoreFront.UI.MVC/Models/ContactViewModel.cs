using System.ComponentModel.DataAnnotations;

namespace Personal_StoreFront.UI.MVC.Models
{
    public class ContactViewModel
    {
        //We can use Data Annotations to add validations to our model.
        //This is useful when we have required fields or need certain types of info.
        //Not only do they need to fill out the fields, but also email is email and not like last name on accident
        //Wait. Why does this only need properties?
        [Required(ErrorMessage = "*Name is required")]
        public string Name { get; set; }//Must be alphabetic characters
        [Required(ErrorMessage = "*Email is required")]
        [DataType(DataType.EmailAddress)] //remember that DataType is an enum hints the DataType.list of options
        public string Email { get; set; }//Must contain an @ and an extension(.com, .net, etc)
        [Required(ErrorMessage = "*Subject is required")]
        public string Subject { get; set; }//Must contain text
        [Required(ErrorMessage = "*Message is required")]
        [DataType(DataType.MultilineText)] //Notes that the field should have a bigger textbox (textarea)
        public string Message { get; set; } //Must contain text

    }
}
