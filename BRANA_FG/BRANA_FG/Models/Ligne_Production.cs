using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Ligne_production")]
    public class Ligne_Production
    {
        public int id { get; set; }
        [Display(Name = "Nom Ligne Production")]
        [Required(AllowEmptyStrings = false)]
        public string nom { get; set; }

        public int id_utilisateur { get; set; }
        public System.DateTime date_ligne_prod { get; set; }
    }
}