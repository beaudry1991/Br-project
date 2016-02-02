using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("laboratoire_echantillon")]
    public class LaboratoireEchantillon
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Select the Name Please", AllowEmptyStrings = false)]
        [Display(Name = "Superviseur*")]
        public int id_superviseur { get; set; }

        [Display(Name = "Recepteur")]
        [Required(ErrorMessage = "Recepteur Name", AllowEmptyStrings = false)]
        public string nom_recepteur { get; set; }

        [Display(Name = "Date")]
        public System.DateTime date_donnee{ get; set; }
       

        [Required(ErrorMessage = "Select The Product", AllowEmptyStrings = false)]
        [Display(Name = "Produit*")]
        public int id_produit { get; set; }

        [Display(Name = "Depot")]
        public int id_depot { get; set; }
      
        
        [Display(Name = "Quantite Caisse")]
        public int qtite_donnee { get; set; }

        [Required(ErrorMessage = "Enter a value", AllowEmptyStrings = false)]
        [Display(Name = "Quantite Bouteille")]
        public int qtite_bout { get; set; }

        
    }
}