using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Fin_Inventaire")]
    public class Fin_Inventaire
    {
        
       
            public int id { get; set; }

        [Display(Name = "Superviseur")]
        [Required(AllowEmptyStrings = false)]
        public int id_superviseur { get; set; }
            public System.DateTime date_fin_inventaire { get; set; }

        [Display(Name = "Produit")]
        [Required(AllowEmptyStrings = false)]
        public int id_produit { get; set; }
          //  public int id_data_clock { get; set; }
        [Display(Name = "Quantite")]
        [Required(AllowEmptyStrings = false)]
        public int qtite_laissee { get; set; }
            public int id_depot_fg { get; set; }
        public int qtite_bout { get; set; }
        public int qtite_caisse_theo { get; set; }
        public int qtite_bout_theo { get; set; }
    }
}