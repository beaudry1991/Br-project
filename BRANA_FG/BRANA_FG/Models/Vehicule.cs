using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class Vehicule
    {
        public int id { get; set; }
        [DisplayName("Plaque number*")]
        [Required(ErrorMessage = "Please provide the Plaque number!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Za-z]{2}[ -.]?[0-9]{5}$", ErrorMessage = "Please provide a valid Plaque number")]
        public string plaque { get; set; }
        [DisplayName("Model")]
        public string model { get; set; }
        [DisplayName("Marque")]
        public string marque { get; set; }
        // [DataType(DataType.Int)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Please provide a valid year")]
        [DisplayName("Year*")]
        //[MaxLength(4,ErrorMessage="Please provide a year with 2 or 4 digits")]
        //[MinLength(2,ErrorMessage="Please provide a year with 2 or 4 digits")]
        public int annee { get; set; }
        [DisplayName("Truck Capacity*")]
        [Required(ErrorMessage = "Please provide the truck capacity!", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Please provide the truck capacity")]
        public int capacite { get; set; }
        [DisplayName("Truck number*")]
        [Required(ErrorMessage = "Please provide the truck number!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[BbTtWw]{1}[ -.]?[0-9]{3}$", ErrorMessage = "Please provide a valid Truck number")]
        public string numero { get; set; }
        // public string numeroVehicule { get; set; }
    }
}