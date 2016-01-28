using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Emballage")]
    public class Emballage
    {
        public int id { get; set; }

        [Display(Name = "Nom Emballage")]
        [Required(AllowEmptyStrings = false)]
        public string nom { get; set; }

        [Display(Name = "Volume")]
        [Required(AllowEmptyStrings = false)]
        public decimal volume { get; set; }

        [Display(Name = "Quantite Par Caisse")]
        [Required(AllowEmptyStrings = false)]
        public int qte_par_caisse { get; set; }

        public int id_utilisateur { get; set; }
        public System.DateTime date_emb { get; set; }
    }
}