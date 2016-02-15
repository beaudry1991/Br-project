using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Casse")]
    public class Casse
    {
        public int id { get; set; }

       
        public int id_superviseur { get; set; }

        public System.DateTime date_casse { get; set; }

        
        public int id_produit { get; set; }
        
       
        
        public int qtite_casse { get; set; }
        
        public int id_depot { get; set; }

        [Display(Name = "Motif")]
        [Required(AllowEmptyStrings = false)]
        public string motif { get; set; }

        [Display (Name = "Type")]
        [Required(AllowEmptyStrings = false)]
        public string type { get; set; }

        public int qtite_bout { get; set; }

        public int qtite_shortfill { get; set; }
        public int qtite_perte { get; set; }

    }
}