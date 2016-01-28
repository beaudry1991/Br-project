using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class Chauffeur
    {
        public int id { get; set; }
        [DisplayName("FirstName*")]
        [Required(ErrorMessage = "Please provide the Driver's Firstname!", AllowEmptyStrings = false)]
        public string prenom { get; set; }
        [DisplayName("LastName*")]
        [Required(ErrorMessage = "Please provide the Driver's lastname!", AllowEmptyStrings = false)]
        public string nom { get; set; }
        [DisplayName("Mail")]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Please provide a valid mail address")]
        public string mail { get; set; }
        [DisplayName("Phone*")]
        [Required(ErrorMessage = "Please provide the Driver's Phone number!", AllowEmptyStrings = false)]
        [RegularExpression(@"^(509)?(\+509)?[2-4]{1}[0-9]{1}[ -.]?([0-9]{2}[ -.]?){3}$", ErrorMessage = "Please provide a valid Phone number")]
        public string phone { get; set; }
        [DisplayName("Profil")]
        public string photo { get; set; }
        [DisplayName("Address*")]
        [Required(ErrorMessage = "Please provide the Driver's Adress!", AllowEmptyStrings = false)]
        public string adresse { get; set; }

        [DisplayName("HelperI")]
        public string aide1 { get; set; }
        [DisplayName("HelperII")]
        public string aide2 { get; set; }
        [DisplayName("Licence#*")]
        [Required(ErrorMessage = "Please provide the licence's number!", AllowEmptyStrings = false)]
        [RegularExpression(@"[a-zA-Z]{2}(-)[0-9]{5}(-)[tbTB]{2}$", ErrorMessage = "Please provide a valid licence's number")]
        public string noPermis { get; set; }
        [DisplayName("Emission*")]
        [Required(ErrorMessage = "Please provide the Date Emission!", AllowEmptyStrings = false)]
        [DataType(DataType.Date)]
        public DateTime dateEmission { get; set; }
        [DisplayName("Expire*")]
        [Required(ErrorMessage = "Please provide the Date Expire!", AllowEmptyStrings = false)]
        [DataType(DataType.Date)]
        public DateTime dateExpire { get; set; }
        
    }
}