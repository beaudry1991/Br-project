using BRANA_FG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BRANA_FG.Controllers
{
    public class RapportInventaireController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();
        // GET: RapportInventaire
        public ActionResult Index()
        {
            if (Session.Keys.Count == 0)
            {
                return RedirectToAction("Index", "Logins");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {
                    //Session["idadmin"] = Session["IdUser"];
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }
        }



        public ActionResult rapportJournalier()
        {
           
            
            if (Request["date1"] != null || Request["date1"]=="") {
                    ViewBag.date1 = Request["date1"];
                ViewBag.date2 = Request["date2"];

                string a = Request["date1"];
                string b = Request["date2"];


                ViewBag.rapp_journalier = new Func<string, string, List<Rapp_Quotidient>>(Rapport);
                ViewBag.rapp_jour_count = Rapport(a, b).Count();
            }else
            {
                ViewBag.rapp_jour_count = 0;
            }

           




            if (Session.Keys.Count == 0)
            {
                return RedirectToAction("Index", "Logins");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {
                    //Session["idadmin"] = Session["IdUser"];
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }



            // return View();
        }



        public List<Rapp_Quotidient> Rapport(string d1 , string d2)
        {
            DateTime date1 = Convert.ToDateTime(d1);
            DateTime date2 = Convert.ToDateTime(d2);

            List<Rapp_Quotidient> rp = new List<Rapp_Quotidient>();

            var list = from deb in db.Debut_Inventaire
                       where deb.date_debut_inventaire >= date1 && deb.date_debut_inventaire <= date2
                       join prod in db.Produits on deb.id_produit equals prod.id
                       join super in db.Utilisateurs on deb.id_superviseur equals super.id
                       select new
                       {
                           date = deb.date_debut_inventaire,
                           caisse_th = deb.caisse_theor,
                           bout_th = deb.bout_theor,
                           id_pro = deb.id_produit,
                           depot_afect = super.depot,
                           shift = super.shift,
                           nom = prod.nom,
                           caisse_ph = deb.qtite_trouvee,
                           bout_ph = deb.qtite_bouteille,
                           id_sup = deb.id_superviseur

                       };


            foreach (var a in list)
            {

                List<ListSomme> listSomme = Fonct.ListTotal(a.id_pro, a.date, a.id_sup);
                List<ListSommeTransf> listSomme_tra = Fonct.ListTotalTransf(a.id_pro, a.date, a.id_sup);
              //  List<ListCasses> listSommeCasse = Fonct.listCaPeSh(a.id_pro, a.date, a.id_sup);
                List<FinInventaire_qtite> list_inventaire = Fonct.Fin_inv_produit(a.id_sup, a.id_pro, a.date);



                if (list_inventaire.Count() == 0)
                {
                    list_inventaire.Add(new FinInventaire_qtite() { caisse_th = 0, bouteille_th = 0, caisse_ph = 0, bouteille_ph = 0 });
                }
                ViewBag.tota = list_inventaire.Count();

                int v = 0, w = 0;
                List<Produit_dispo> dispo = Fonct.Stock_dispos(a.depot_afect, a.id_pro);

                if (dispo.Count() > 0)
                {
                    v = dispo[0].bouteille;
                    w = dispo[0].caisse;
                }

                rp.Add(new Rapp_Quotidient()
                {
                    date = a.date,

                    sku = a.nom,
                    open_stck_caisse_ph = a.caisse_ph,
                    open_stck_bout_ph = a.bout_ph,
                    open_stck_caisse_th = a.caisse_th,
                    open_stck_bout_th = a.bout_th,

                    shift = a.shift,

                    load_caisse = listSomme[0].somme_caisse_load,
                    load_bout = listSomme[0].somme_bouteille_load,
                    unload_caisse = listSomme[0].somme_caisse_unload,
                    unload_bout = listSomme[0].somme_bouteille_unload,

                    transf_caisse = listSomme_tra[0].somme_caisse,
                    transf_bout = listSomme_tra[0].somme_bouteille,



                    //perte = (listSommeCasse.Count() > 1) ? listSommeCasse[1].perte : 0,
                    //shortf = (listSommeCasse.Count() > 1) ? listSommeCasse[2].short_fill : 0,
                    //bouteille1 = (listSommeCasse.Count() > 1) ? listSommeCasse[0].bouteille : 0,
                    //bouteille2 = (listSommeCasse.Count() > 1) ? listSommeCasse[1].bouteille : 0,
                    //bouteille3 = (listSommeCasse.Count() > 1) ? listSommeCasse[2].bouteille : 0,
                    //casse = (listSommeCasse.Count() > 1) ? listSommeCasse[0].casse : 0,

                    clo_stck_caisse_ph = list_inventaire[0].caisse_ph,
                    clo_stck_bout_ph = list_inventaire[0].bouteille_ph,

                    clo_stck_caisse_th = list_inventaire[0].caisse_th,
                    clo_stck_bout_th = list_inventaire[0].bouteille_th

                });






                //    PC.Add(new ProduitList() { id = a.id, nom = a.nom, volume = a.vol, emballage = a.nombalage, qte_par_caisse = a.qt_p_c });

            }





            return rp;
        }






        public ActionResult rapportUtilisateur()
        {
            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.userNomPre = Fonct.ListSuperviseurs();

            ViewBag.tmpSup = 0;
            var valeur = Request["valeur"];


            ViewBag.affiche_loading_Count = afficheLoading(int.Parse(valeur)).Count();
            ViewBag.affiche_loading = new Func<int, List<Tuple<string, string, string>>>(afficheLoading);

            ViewBag.affiche_transfert_Count = afficheTransfert(int.Parse(valeur)).Count();
            ViewBag.afficheTransfert = new Func<int, List<Tuple<string, string, int>>>(afficheTransfert);

            ViewBag.afficheApprov_Count = afficheApprov(int.Parse(valeur)).Count();
            ViewBag.afficheApprov = new Func<int, List<Tuple<string, string, string>>>(afficheApprov);

            //ViewBag.afficheCount = afficheCount.Count();
            return View();
        }

        public List<Tuple<string, string, string>> afficheLoading(int b)
        {
            List<Tuple<string, string, string>> PC = new List<Tuple<string, string, string>>();

            VehiculeList user = new VehiculeList();

            var vehiculo = from load in db.Loadings
                           where load.id_superviseur == b
                           select new
                           {
                               load.date_loading,
                               load.numero_sp,
                               load.numero_emb

                           };

                PC.Add(Tuple.Create("", "", ""));

                foreach (var a in vehiculo)
                    {
                
                       PC.Add(Tuple.Create(a.date_loading.ToString(), a.numero_sp, a.numero_emb));
                
                    }
            return PC;
        }

        public List<Tuple<string, string, int>> afficheTransfert(int b)
        {
            List<Tuple<string, string, int>> PC = new List<Tuple<string, string, int>>();

            VehiculeList user = new VehiculeList();

            var vehiculo = from trasf in db.Transferts
                           where trasf.id_superviseur == b
                           select new
                           {
                               trasf.date_transfert,
                               trasf.num_transfert,
                               trasf.id_chauffeur

                           };

            PC.Add(Tuple.Create("", "", 0));

            foreach (var a in vehiculo)
            {

                PC.Add(Tuple.Create(a.date_transfert.ToString(), a.num_transfert.ToString(), a.id_chauffeur));

            }
            return PC;
        }

        public List<Tuple<string, string, string>> afficheApprov(int b)
        {
            List<Tuple<string, string, string>> PC = new List<Tuple<string, string, string>>();

            VehiculeList user = new VehiculeList();

            var vehiculo = from app in db.Ligne_depot
                           where app.id_superviseur == b
                           join lig in db.Ligne_Production on app.id_ligne_production equals lig.id
                           join prod in db.Produits on app.id_produit equals prod.id
                           select new
                           {
                              date =  app.date_ligne_depot,
                              lign = lig.nom,
                              produit =  prod.nom


                           }; 

            PC.Add(Tuple.Create("", "", ""));

            foreach (var a in vehiculo)
            {

                PC.Add(Tuple.Create(a.date.ToString(), a.lign.ToString(), a.produit.ToString()));

            }
            return PC;
        }
    }
}