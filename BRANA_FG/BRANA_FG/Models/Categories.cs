using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Categorie")]
    public class Categorie
    {
        public int id { get; set; }

        [Display(Name = "Nom Categorie")]
        [Required(AllowEmptyStrings = false)]
        public string nom { get; set; }
    }
}