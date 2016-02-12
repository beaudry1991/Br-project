using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Produit_manquant")]
    public class Produit_manquant
    {
        public int id { get; set; }
        public int id_depot { get; set; }
        public int id_user { get; set; }
        public int id_chauffeur { get; set; }
        public string num_emb { get; set; }
    }
}