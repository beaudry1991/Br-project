using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Produits")]
    public class Produits
    {
        public int id { get; set; }
        [Display(Name = "Nom Produit")]
        [Required(AllowEmptyStrings = false)]
        public string nom { get; set; }
        [Display(Name = "Type D'Emballage")]
        [Required(AllowEmptyStrings = false)]
        public int id_emballage { get; set; }
        [Display(Name = "Categorie")]
        [Required(AllowEmptyStrings = false)]
        public int id_categorie { get; set; }
        public int id_utilisateur { get; set; }
        public DateTime date_prod { get; set; }
    }
}