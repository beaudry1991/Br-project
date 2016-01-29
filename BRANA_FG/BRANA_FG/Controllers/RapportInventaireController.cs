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
            List<Rapp_Quotidient> rp = new List<Rapp_Quotidient>();

            //Rapp_Quotidient rapport = new Rapp_Quotidient();


            


            var list = from deb in db.Debut_Inventaire
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

                List < ListSomme > listSomme = Fonct.ListTotal(a.id_pro, a.date, a.id_sup);
                List<ListSommeTransf> listSomme_tra = Fonct.ListTotalTransf(a.id_pro, a.date, a.id_sup);
                List<ListCasses> listSommeCasse = Fonct.listCaPeSh(a.id_pro, a.date, a.id_sup);
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

                    

                    perte = (listSommeCasse.Count() > 1) ? listSommeCasse[1].perte : 0,
                    shortf = (listSommeCasse.Count() > 1) ? listSommeCasse[2].short_fill : 0,
                    bouteille1 = (listSommeCasse.Count() > 1) ? listSommeCasse[0].bouteille : 0,
                    bouteille2 = (listSommeCasse.Count() > 1) ? listSommeCasse[1].bouteille : 0,
                    bouteille3 = (listSommeCasse.Count() > 1) ? listSommeCasse[2].bouteille : 0,
                    casse = (listSommeCasse.Count() > 1) ? listSommeCasse[0].casse : 0,

                    clo_stck_caisse_ph = list_inventaire[0].caisse_ph,
                    clo_stck_bout_ph = list_inventaire[0].bouteille_ph,

                    clo_stck_caisse_th = list_inventaire[0].caisse_th,
                    clo_stck_bout_th = list_inventaire[0].bouteille_th

                });
                    

                

                

                //    PC.Add(new ProduitList() { id = a.id, nom = a.nom, volume = a.vol, emballage = a.nombalage, qte_par_caisse = a.qt_p_c });

            }





            ViewBag.rapp_journalier = rp;
            ViewBag.rapp_jour_count = rp.Count();

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

        public ActionResult rapportUtilisateur()
        {

            return View();
        }
    }
}