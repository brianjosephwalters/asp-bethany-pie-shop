using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public DateTime BirthDay { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<string> Claims { get; set; }
    }
}
