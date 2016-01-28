using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Debut_inventaire")]
    public class Debut_Inventaire
    {
        public int id { get; set; }

       
        public int id_superviseur { get; set; }

        public System.DateTime date_debut_inventaire { get; set; }

        public int id_produit { get; set; }

        public int qtite_bouteille { get; set; }

        public int qtite_trouvee { get; set; }

        public int id_depot_fg { get; set; }

        public int caisse_theor { get; set; }

        public int bout_theor { get; set; }
    }

}