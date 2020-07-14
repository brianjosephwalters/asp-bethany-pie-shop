using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BethanysPieShop.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please enter your address.")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }
        
        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Please enter your zip code.")]
        [StringLength(10, MinimumLength = 4)]
        public string ZipCode { get; set; }
        
        [Required(ErrorMessage = "Please enter your city.")]
        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter your country.")]
        [StringLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }

    }
}
