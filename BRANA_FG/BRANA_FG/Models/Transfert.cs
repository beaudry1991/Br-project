using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Transfert_dep")]
    public class Transfert
    {
        public int id { get; set; }
        
        [Display(Name = "Depot Source")]
        [Required(AllowEmptyStrings = false)]
        public int id_depot_source { get; set; }
        [Display(Name = "Depot Destination")]
        [Required(AllowEmptyStrings = false)]
        public int id_depot_dest { get; set; }
        [Display(Name = "Qte Palette")]
        [Required(AllowEmptyStrings = false)]
        public int quantite_palette { get; set; }
        [Display(Name = "Chauffeur")]
        [Required(AllowEmptyStrings = false)]
        public int id_chauffeur { get; set; }
        [Display(Name = "Vehicule")]
        [Required(AllowEmptyStrings = false)]
        public int id_vehicule { get; set; }
        [Display(Name = "Superviseur")]
        [Required(AllowEmptyStrings = false)]
        public int id_superviseur { get; set; }
        
        [Display(Name = "Num transfert")]
        public string num_transfert { get; set; }
        [Display(Name = "Date")]
        public DateTime date_transfert { get; set; }
        public int process { get; set; }
        public int id_superviseur_recu { get; set; }
    }
}