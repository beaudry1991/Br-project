using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BRANA_FG.Models
{
    public class BddContext : DbContext
    {
       
        public DbSet<Categorie> Categories { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Emballage> Emballages { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Ligne_Production> Ligne_Production { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Produits> Produits { get; set; }

    

        public System.Data.Entity.DbSet<BRANA_FG.Models.Debut_Inventaire> Debut_Inventaire { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Fin_Inventaire> Fin_Inventaire { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Casse> Casses { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.LaboratoireEchantillon> LaboratoireEchantillons { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Loading> Loadings { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Transfert> Transferts { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Unloading> Unloadings { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Utilisateur> Utilisateurs { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Depots> Depots { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Login> Logins { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Chauffeur> Chauffeurs { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Vehicule> Vehicules { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Loading_produit> Loading_produit { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Ligne_depot> Ligne_depot { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Depot_Produit> Depot_Produit { get; set; }

        public System.Data.Entity.DbSet<BRANA_FG.Models.Transfert_Produit> Transfert_Produit { get; set; }
    }
}