using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Casse_produit")]
    public class Casse_Produit
    {
        public int id { get; set; }

        public int id_casse { get; set; }

       
        public int id_produit { get; set; }

        public int qtite_caisse { get; set; }

        public int qtite_bout { get; set; }
    }
}