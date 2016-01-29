using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    [Table("Loading")]
    public class Loading
    {
        public int id { get; set; }
        [Display(Name = "Superviseur")]
        [Required(AllowEmptyStrings = false)]
        public int id_superviseur { get; set; }
        [Display(Name = "Chaufeur")]
        [Required(AllowEmptyStrings = false)]
        public int id_chauffeur { get; set; }

        [Display(Name = "Client")]
        [Required(AllowEmptyStrings = false)]
        public string client { get; set; }

        [DisplayName("Vehicule#*")]
        [Required(ErrorMessage = "Choisissez le numero du Camoin!", AllowEmptyStrings = false)]
        public int id_vehicule { get; set; }

        [DisplayName("SP#*")]
        [Required(ErrorMessage = "Saisissez le numero du SP!", AllowEmptyStrings = false)]
        [RegularExpression(@"^(SP)?(sp)?(Sp)?(sP)?[ -.]?[0-9]{8}$", ErrorMessage = "Le numero SP saisi est incorrect!")]
        public string numero_sp { get; set; }
        [DisplayName("Emb#*")]
        [Required(ErrorMessage = "Saisissez le numero du Debarquement!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Le numero Deb saisi est incorrect!")]
        public string numero_emb { get; set; }
        [Display(Name = "Nombre de Palette")]
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Le nombre de palette doit etre un entier positif!")]
        public int nbre_palette { get; set; }

        [DisplayName("Type Embarquement*")]
        [Required(ErrorMessage = "Selectionnez le type D'Embarquement", AllowEmptyStrings = false)]
        public string type_emb { get; set; }
        public int retour { get; set; }
        public System.DateTime date_loading { get; set; }

        [Display(Name = "Heure Debut")]
        [Required(AllowEmptyStrings = false)]
        public DateTime heure_debut { get; set; }
        [Display(Name = "Heure Fin")]
        [Required(AllowEmptyStrings = false)]
        public DateTime heure_fin { get; set; }
        public int id_data_clock { get; set; }
        //[Display(Name = "Quantite Casse")]
        //[Required(AllowEmptyStrings = false)]
        //public int qtite_casse { get; set; }
        [Display(Name = "Remarque")]
        [Required(AllowEmptyStrings = false)]
        public string remarque { get; set; }
        public int id_depot { get; set; }
        [Display(Name = "Destination")]
        public string destination { get; set; }
    }
}