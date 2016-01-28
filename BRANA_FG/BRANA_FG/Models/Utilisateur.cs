using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Utilisateurs")]
    public class Utilisateur
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please provide the Lastname!", AllowEmptyStrings = false)]
        [DisplayName("Last Name*")]

        public string nom { get; set; }
        [DisplayName("First Name*")]
        [Required(ErrorMessage = "Please provide the Firstname!", AllowEmptyStrings = false)]
        public string prenom { get; set; }

        [DisplayName("Email *")]
        [Required(ErrorMessage = "Please provide a valid mail address!", AllowEmptyStrings = false)]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Please provide a valid mail address")]
        public string mail { get; set; }
        [DisplayName("Phone*")]
        [Required(ErrorMessage = "Please provide A valid Phone number!", AllowEmptyStrings = false)]
        public string phone { get; set; }
        [DisplayName("Address*")]
        [Required(ErrorMessage = "Please provide the user's Location!", AllowEmptyStrings = false)]
        public string adresse { get; set; }
        [DisplayName("Fonction*")]
        [Required(ErrorMessage = "Please provide the User's Fonction!", AllowEmptyStrings = false)]
        public string fonction { get; set; }

        public string shift { get; set; }
        [DisplayName("Depot Affection ")]
        public string depot { get; set; }


    }
}