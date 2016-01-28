using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class Rapp_Quotidient
    {
        public DateTime date { get; set; }
        public string shift { get; set; }
        public string sku { get; set; }

        public int open_stck_caisse_ph { get; set; }
        public int open_stck_bout_ph { get; set; }

        public int open_stck_caisse_th { get; set; }
        public int open_stck_bout_th { get; set; }


        public int load_caisse { get; set; }
        public int load_bout { get; set; }

        public int unload_caisse { get; set; }
        public int unload_bout { get; set; }

        public int transf_bout { get; set; }
        public int transf_caisse { get; set; }



       
        public int perte { get; set; }
        public int shortf { get; set; }
        public int casse { get; set; }

        public int bouteille1 { get; set; }
        public int bouteille2 { get; set; }
        public int bouteille3 { get; set; }

        public int clo_stck_caisse_ph { get; set; }
        public int clo_stck_bout_ph { get; set; }

        public int clo_stck_caisse_th { get; set; }
        public int clo_stck_bout_th { get; set; }
    }
}