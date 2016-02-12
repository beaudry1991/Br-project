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
  
        public int id_depot { get; set; }

        [Display(Name = "Motif")]
        [Required(AllowEmptyStrings = false)]
        public string motif { get; set; }

        [Display (Name = "Type")]
        [Required(AllowEmptyStrings = false)]
        public string type { get; set; }

        [Display(Name = "Endroit")]
        //[Required(AllowEmptyStrings = false)]
        public string endroit { get; set; }

        [Display(Name = "Personnes Blesseés")]
      //  [Required(AllowEmptyStrings = false)]
        public string injured_person { get; set; }


        [Display(Name = "Chauffeur Fork Lift")]
      //  [Required(AllowEmptyStrings = false)]
        public string chauffeur_forklift { get; set; }

        [Display(Name = "Description")]
     //   [Required(AllowEmptyStrings = false)]
        public string description { get; set; }

        [Display(Name = "PPE utilisé")]
       // [Required(AllowEmptyStrings = false)]
        public string safety_material { get; set; }

    }
}