using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Unloading")]
    public class Unloading
    {
        public int id { get; set; }
        [Display(Name = "# Embarquement")]
        [Required(AllowEmptyStrings = false)]
        public int id_loading { get; set; }
        [Display(Name = "Quantite Palette")]
        [Required(AllowEmptyStrings = false)]
        public int qtite_palette_retr { get; set; }
        [Display(Name = "Heure Debut")]
        [Required(AllowEmptyStrings = false)]
        public DateTime time_unloading_debut { get; set; }
        [Display(Name = "Heure Fin")]
        [Required(AllowEmptyStrings = false)]
        public DateTime time_unloading_fin { get; set; }
        [Display(Name = "Remarque")]
        public string remarque { get; set; }
        
    }
}