using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class ProduitList
    {
        public int id { get; set; }
        public string nom { get; set; }
        public decimal volume { get; set; }
        public string emballage { get; set; }
        public int qte_par_caisse { get; set; }

    }
}