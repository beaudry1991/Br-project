using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Transfert_Produit")]
    public class Transfert_Produit
    {
        public int id { get; set; }
        public int id_transfert { get; set; }
        public int qtite_caisse { get; set; }
        public int id_produit { get; set; }
        public int qtite_bout { get; set; }
    }
}