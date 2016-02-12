using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Prod_manq_relation")]
    public class Prod_manq_relation
    {
        public int Id { get; set; }
        public int id_produit { get; set; }
        public int id_prod_manq { get; set; }
        public int qtite_caisse { get; set; }
        public int qtite_bout { get; set; }
    }
}