using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Produit_Bloque")]
    public class Produit_Bloque
    {
        public int id { get; set; }
        [Display(Name = "Produit")]
        [Required(AllowEmptyStrings = false)]
        public int id_produit { get; set; }

        [Display(Name = "Depot")]
        [Required(AllowEmptyStrings = false)]
        public int id_depot { get; set; }

        [Display(Name = "Raison")]
        [Required(AllowEmptyStrings = false)]
        public string raison { get; set; }

        [Display(Name = "Quantite Caisse")]
        [Required(AllowEmptyStrings = false)]
        public int qtite_caisse_donnee { get; set; }

        public int qtite_caisse_retour { get; set; }

        public int etat { get; set; }

        public DateTime date_donnee { get; set; }

        public DateTime date_retour  {get; set;}

        [Display(Name = "Quantite Bouteille")]
        public int qtite_bout_donnee { get; set; }

        public int qtite_bout_retour { get; set; }

        public int id_admin { get; set; }

    }
}