using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Depot_Produit")]
    public class Depot_Produit
    {
        public int id { get; set; }
        public int id_depot { get; set; }
        public int id_produit { get; set; }
        public int qtite_produit_dispo { get; set; }
        public int qtite_bouteille { get; set; }

    }
}