using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Ligne_depot")]
    public class Ligne_depot
    { 
        public int id { get; set; }
        [Display(Name = "Ligne Production")]
        [Required(AllowEmptyStrings = false)]
        public int id_ligne_production { get; set; }

        [Display(Name = "Depot")]
        [Required(AllowEmptyStrings = false)]
        public int id_depot { get; set; }

        [Display(Name = "Quantite Caisse")]
        [Required(AllowEmptyStrings = false)]
        public int qtite_caisse { get; set; }

        [DisplayName("Date")]
        [Required(AllowEmptyStrings = false)]
        public DateTime date_ligne_depot { get; set; }

        [DisplayName("Superviseur")]
        [Required(AllowEmptyStrings = false)]
        public int id_superviseur { get; set; }

        
        
        [DisplayName ("Produit")]
        [Required(AllowEmptyStrings = false)]
        public int id_produit { get; set; }

    }
}