using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class ListLoading
    {
        public int id { get; set; }
       
        public string superviseur { get; set; }
      
        public string chauffeur { get; set; }

       

        public string vehicule { get; set; }
        public string numero_sp { get; set; }
       
        public string numero_emb { get; set; }
        public DateTime heure_debut { get; set; }
      
        public DateTime heure_fin { get; set; }
        public int nbre_palette { get; set; }

        public string client { get; set; }
        public string type_emb { get; set; }
        public int retour { get; set; }
        public System.DateTime date_loading { get; set; }

       

        public string data_clock { get; set; }
       
        public string remarque { get; set; }
        public string depot { get; set; }
      
        public string destination { get; set; }
    }
}