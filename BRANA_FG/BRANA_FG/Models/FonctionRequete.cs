using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BRANA_FG.Models
{
    public class FonctionRequete : InterfaceFonction
    {

        private BddContext bdd; // declaration de l'objet bd 

        public FonctionRequete()
        {   /*Instanciation de l'objet bd*/
            bdd = new BddContext();
        }

        public List<SuperviseurLIst> ListSuperviseurs()
        {
            List<SuperviseurLIst> PC = new List<SuperviseurLIst>();

            SuperviseurLIst user = new SuperviseurLIst();

            var utilisator = from prod in bdd.Utilisateurs
                             select new
                             {
                                 prod.id,
                                 prod.nom,
                                 prod.prenom,
                              

                             };


            foreach (var a in utilisator)
            {

                PC.Add(new SuperviseurLIst() { id = a.id, nom = a.nom, prenom = a.prenom});

            }
            return PC;

        }


        public List<ChauffeurList> ListChauffeur()
        {
            List<ChauffeurList> PC = new List<ChauffeurList>();

            ChauffeurList user = new ChauffeurList();

            var chauffeuro = from prod in bdd.Chauffeurs
                             select new
                             {
                                 prod.id,
                                 prod.nom,
                                 prod.prenom

                             };


            foreach (var a in chauffeuro)
            {

                PC.Add(new ChauffeurList() { id = a.id, nom = a.nom, prenom = a.prenom });

            }
            return PC;

        }

        public List<Emballage> listEmballage()
        {
            List<Emballage> PC = new List<Emballage>();

            var emb = from embal in bdd.Emballages
                      select new {
                          embal.id,
                          embal.nom
                      };
            foreach (var a in emb)
            {

                PC.Add(new Emballage() { id = a.id, nom = a.nom});

            }
            return PC;

        }



        public List<Categorie> listCategorie()
        {
            List<Categorie> PC = new List<Categorie>();

            var cat = from catego in bdd.Categories
                      select new
                      {
                          catego.id,
                          catego.nom
                      };
            foreach (var a in cat)
            {

                PC.Add(new Categorie() { id = a.id, nom = a.nom });

            }
            return PC;

        }



        public List<VehiculeList> ListVehicule()
        {
            List<VehiculeList> PC = new List<VehiculeList>();

            VehiculeList user = new VehiculeList();

            var vehiculo = from prod in bdd.Vehicules
                             select new
                             {
                                 prod.id,
                                 prod.numero
                         
                             };


            foreach (var a in vehiculo)
            {

                PC.Add(new VehiculeList() { id = a.id, numeroCamion = a.numero });

            }
            return PC;

        }




        public List<ProduitList> listproduit()
        {
            List<ProduitList> PC = new List<ProduitList>();

            ProduitList user = new ProduitList();

            var produito = from prod in bdd.Produits
                           join emb in bdd.Emballages on prod.id_emballage equals emb.id                          
                           select new
                             {
                                 prod.id,
                                 prod.nom,
                                 nombalage = emb.nom,
                                 vol = emb.volume,
                                 qt_p_c = emb.qte_par_caisse
                                 

                             };


            foreach (var a in produito)
            {

                PC.Add(new ProduitList() { id = a.id, nom = a.nom, volume = a.vol, emballage = a.nombalage, qte_par_caisse = a.qt_p_c });

            }
            return PC;

        }




        public string nomEmballage(int id)
        {
           // throw new NotImplementedException();
            Emballage emballage = bdd.Emballages.Find(id);
            return emballage.nom;
        }

        /*Password Updating*/
        public void UpdatingPassword(string mail, string newpassword)
        {
            Login log = bdd.Logins.Where(pl => pl.mail.Equals(mail)).FirstOrDefault();
            if (log != null)
            {
                log.password = newpassword;
                bdd.SaveChanges();
            }
        }

        public List<LigneProductionList> ListLigneProduction()
        {

            List<LigneProductionList> PC = new List<LigneProductionList>();

            LigneProductionList user = new LigneProductionList();

            var lignePro = from prod in bdd.Ligne_Production
                             select new
                             {
                                 prod.id,
                                 prod.nom

                             };


            foreach (var a in lignePro)
            {

                PC.Add(new LigneProductionList() { id = a.id, nom = a.nom });

            }
            return PC;
        }

        public List<ListDepots> ListDepot()
        {
            List<ListDepots> PC = new List<ListDepots>();

            ListDepots user = new ListDepots();

            var listdep = from prod in bdd.Depots
                           select new
                           {
                               prod.id,
                               prod.nom

                           };


            foreach (var a in listdep)
            {

                PC.Add(new ListDepots() { id = a.id, nom = a.nom });

            }
            return PC;
        }

        public int id_loadingWrNumEmb(string numEmb)
        {
            int b = 0;
            var listdep = from load in bdd.Loadings
                          where load.numero_emb.Equals(numEmb)
                          select load;
            foreach (var a in listdep)
            {
                b = a.id;
            }
            return b;
        }

        public int idtransfert_numTrans(string numTrans)
        {
            int b = 0;
            var listdep = from load in bdd.Transferts
                          where load.num_transfert.Equals(numTrans)
                          select load;
            foreach (var a in listdep)
            {
                b = a.id;
            }
            
            return b;
        }

        public int qtite_dispo_ligne_depot(int produit, int depot)
        {
            var data = bdd.Ligne_depot.Where(d => d.id_depot == depot && d.id_produit == produit);
            var somme = data.Sum(d => d.qtite_caisse);
            return somme;
        }

        public List<ListRetour> ListRetour(int idtransfert)
        {
                List<ListRetour> PC = new List<ListRetour>();
            
            var lisProd = from prodlis in bdd.Transfert_Produit
                             where prodlis.id_transfert.Equals(idtransfert) 
                             select new
                              {
                                   prodlis.id,
                                   prodlis.id_produit,
                                   prodlis.qtite_bout,
                                   prodlis.qtite_caisse
                                   

                               };


                foreach (var a in lisProd)
                {

                    PC.Add(new ListRetour() { id = a.id, id_produit = a.id_produit, qtite_bout =a.qtite_bout, qtite_caisse =a.qtite_caisse });

               }
               return PC;
            }

        public List<ListUnload> ListUnload (string idloading)
        {
            List<ListUnload> PC = new List<ListUnload>();

            var lisUnl = from Unlis in bdd.Loading_produit
                          where Unlis.id_loading.Equals(idloading)
                          select new
                          {
                             Unlis.id_produit,
                             Unlis.id
                           
                          };


            foreach (var a in lisUnl)
            {

                PC.Add(new ListUnload() {id=a.id, id_produit = a.id_produit } );

            }
            return PC;
        }

        public List<ListSomme> ListTotal(int id, DateTime date, int superv)
        {
            List<ListSomme> PC = new List<ListSomme>();

            var list = from value in bdd.Loadings
                     join gala in bdd.Loading_produit on value.numero_emb equals gala.id_loading
                     where gala.id_produit == id && (value.heure_debut.Year == date.Year && value.heure_debut.Month == date.Month && value.heure_debut.Day == date.Day) && value.id_superviseur == superv
                       group gala by gala.id_produit into kiko
                     select new
                     {
                         caisse_load = kiko.Sum(x => x.qte_caisse_delivre),
                         bouteille_load = kiko.Sum(x => x.qte_bouteille_delivre),
                         caisse_unload = kiko.Sum(x => x.qte_caisse_retourne),
                         bouteille_unload = kiko.Sum(x => x.qte_bout_retourne)
                     };


            if (list.Count() == 0)
            {
                PC.Add(new ListSomme() { somme_caisse_load = 0, somme_bouteille_load = 0, somme_bouteille_unload = 0, somme_caisse_unload = 0 });

            }
            else {
                foreach (var a in list)
                {

                    PC.Add(new ListSomme() { somme_caisse_load = a.caisse_load, somme_bouteille_load = a.bouteille_load, somme_bouteille_unload = a.bouteille_unload, somme_caisse_unload = a.caisse_unload });
                }
            }
     

            return PC;
        }

        public List<ListSommeTransf> ListTotalTransf(int idpro, DateTime date, int superv)
        {
            List<ListSommeTransf> PC = new List<ListSommeTransf>();

            var list = from value in bdd.Transferts
                       join gala in bdd.Transfert_Produit on value.id equals gala.id_transfert
                       where gala.id_produit == idpro  && (value.date_transfert.Year== date.Year && value.date_transfert.Month == date.Month && value.date_transfert.Day == date.Day) && value.id_superviseur == superv
                       group gala by gala.id_produit into kiko
                       select new
                       {
                           caisse_load = kiko.Sum(x => x.qtite_caisse),
                           bouteille_load = kiko.Sum(x => x.qtite_bout)
                           
                       };


            if (list.Count() == 0)
            {
                PC.Add(new ListSommeTransf() { somme_caisse = 0, somme_bouteille = 0 });

            }
            else {
                foreach (var a in list)
                {

                    PC.Add(new ListSommeTransf() { somme_caisse = a.caisse_load, somme_bouteille = a.bouteille_load });
                }
            }


            return PC;
        }

        public List<ListCasses> listCaPeSh(int id, DateTime date, int superv)
        {
            List<ListCasses> PC = new List<ListCasses>();

            var list = from value in bdd.Casses
                       where value.id_produit == id && (value.date_casse.Year == date.Year && value.date_casse.Month == date.Month && value.date_casse.Day == date.Day) && value.id_superviseur == superv
                       group value by new { value.type , value.id_produit}  into kiko
                       select new
                       {
                           
                           short_f = kiko.Sum(x => x.qtite_shortfill),
                           casse = kiko.Sum(x => x.qtite_casse),
                           bouteille = kiko.Sum(x => x.qtite_bout),
                           perte = kiko.Sum(x => x.qtite_perte)
                       };


            if (list.Count() == 0)
            {
                PC.Add(new ListCasses() {  casse = 0, perte = 0, bouteille = 0, short_fill = 0 });

            }
            else {
                foreach (var a in list)
                {

                    PC.Add(new ListCasses() {   perte = a.perte, casse = a.casse, bouteille = a.bouteille, short_fill = a.short_f });
                }
            }


            return PC;
        }

        public List<Produit_dispo> ProduitDispos()
        {
            List<Produit_dispo> PC = new List<Produit_dispo>();

            var list = from value in bdd.Depot_Produit
                       //where value.id_produit == 1
            //          // join emb in bdd.Produits on value.id_produit equals emb.
                      group value by value.id_produit  into kiko
                      select new
                      {

                         cais = kiko.Sum(x =>x.qtite_produit_dispo),
                          bout = kiko.Sum(x =>x.qtite_bouteille)

                      };
            if (list.Count() == 0)
            {
                new Produit_dispo() { caisse = 0, bouteille = 0 };

            }
            else {
                foreach (var a in list)
                {

                    PC.Add(new Produit_dispo() { caisse=a.cais, bouteille = a.bout });
                    }
                }
            
            return PC;
        }

        public List<Produit_dispo> Stock_dispos(string iddep, int idprod)
        {
            List<Produit_dispo> PC = new List<Produit_dispo>();
            int id = 0;
            List<ListDepots> dep = this.ListDepot();
            foreach(ListDepots a in dep)
            {
                if(a.nom.Equals(iddep))
                {
                    id = a.id;
                }
            }


            var list = from value in bdd.Depot_Produit
                           where value.id_produit == idprod && value.id_depot == id
                           group value by  value.id_produit into kiko
                           select new
                           {
                               cais = kiko.Sum(x => x.qtite_produit_dispo),
                               bout = kiko.Sum(x => x.qtite_bouteille)
                           };
            if (list.Count() == 0)
            {
                new Produit_dispo() { caisse = 0, bouteille = 0 };

            }
            else {
                foreach (var a in list)
                {

                    PC.Add(new Produit_dispo() { caisse = a.cais, bouteille = a.bout });
                }
            }

            return PC;
        }

        public List<FinInventaire_qtite> Fin_inv_produit(int idsup, int idprod, DateTime date)
        {
            List<FinInventaire_qtite> PC = new List<FinInventaire_qtite>();
          

            var list = from value in bdd.Fin_Inventaire
                       where value.id_produit == idprod && value.id_superviseur == idsup && (value.date_fin_inventaire.Day == date.Day && value.date_fin_inventaire.Month == date.Month && value.date_fin_inventaire.Year == date.Year)
                       select new
                       {
                           cais_th = value.qtite_caisse_theo,
                           bout_th = value.qtite_bout_theo,
                           cais_ph = value.qtite_laissee,
                           bout_ph = value.qtite_bout
                       };
            if (list.Count() == 0)
                {
                new FinInventaire_qtite() { caisse_th= 0 , bouteille_th = 0, caisse_ph = 0, bouteille_ph = 0 };

                }
            else {
                foreach (var a in list)
                {
                PC.Add(new FinInventaire_qtite() { caisse_th = a.cais_th, bouteille_th = a.bout_th, caisse_ph = a.cais_ph, bouteille_ph = a.bout_ph });
                }
            }
           
            return PC;
        }
    }




}