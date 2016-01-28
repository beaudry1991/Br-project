using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{

	public class Loading_produit
	{
        public int id { get; set; }
        public int id_produit { get; set; }
        public int qte_caisse_delivre { get; set; }
        public int qte_bouteille_delivre { get; set; }
        public int qte_caisse_retourne { get; set; }
        public int qte_bout_retourne { get; set; }
        public string id_loading { get; set; }
	}
}