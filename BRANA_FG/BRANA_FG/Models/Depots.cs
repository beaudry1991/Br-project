using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Depots")]
    public class Depots
    {
        public int id { get; set; }

        [Display(Name = "Nom Depot")]
        [Required(AllowEmptyStrings = false)]
        public string nom { get; set; }

        [Display(Name = "Adresse")]
        [Required(AllowEmptyStrings = false)]
        public string adresse { get; set; }

        public System.DateTime dateCreation { get; set; }

        [Display(Name = "Type Depot")]
        [Required(AllowEmptyStrings = false)]
        public string typeDepot { get; set; }
    }

}