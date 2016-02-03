using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BRANA_FG.Models;

namespace BRANA_FG.Controllers
{
    public class Ligne_depotController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();
        // GET: Ligne_depot
        public ActionResult Index()
        {
            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();
            ViewBag.tmpdepot = 0;

            if (Request["date1"] != null || Request["date1"] == "")
            {
                ViewBag.date1 = Request["date1"];
                ViewBag.date2 = Request["date2"];
               

                string a = Request["date1"];
                string b = Request["date2"];
                var vv = Request["select"];
                int d = int.Parse(vv);
                ViewBag.select = d;

                ViewBag.Approvisionnement_count = Approvisionnement(a, b, d).Count();
            ViewBag.Approvisionnement =  new Func<string, string, int, List<Tuple<int, string, string, int, DateTime, string, string>>>(Approvisionnement);
            } else
            {
                ViewBag.Approvisionnement_count = 0;
            }


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG"; var fonction1 = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View(db.Ligne_depot.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }
        public List<Tuple<int, string, string, int, DateTime, string, string>> Approvisionnement(string c, string b, int d)
        {
            List<Tuple<int, string, string, int, DateTime, string, string>> PC = new List<Tuple<int, string, string, int, DateTime, string, string>>();

            Ligne_depot l_d = new Ligne_depot();
            DateTime date1 = Convert.ToDateTime(c);
            DateTime date2 = Convert.ToDateTime(b);

            var ligne_dep = from ld in db.Ligne_depot
                            where (ld.date_ligne_depot >= date1 && ld.date_ligne_depot<=date2) && ld.id_ligne_production == d
                            join l_p in db.Ligne_Production on ld.id_ligne_production equals l_p.id 
                            join dep in db.Depots on ld.id_depot equals dep.id
                            join sup in db.Utilisateurs on ld.id_superviseur equals sup.id
                            join prod in db.Produits on ld.id_superviseur equals prod.id
                            select new
                           {
                              id = ld.id,
                              produit =  prod.nom,
                              depot = dep.nom,
                              super_nom = sup.nom,
                              super_pre =  sup.prenom,
                              ligne = l_p.nom,
                              quantite = ld.qtite_caisse,
                              date = ld.date_ligne_depot

                           };

            PC.Add(Tuple.Create(0, "", "", 0, date1, "", ""));

            foreach (var a in ligne_dep)
            {

                PC.Add(Tuple.Create(a.id, a.ligne, a.super_nom+" "+a.super_pre, a.quantite, a.date, a.produit, a.depot ));

            }
            return PC;
        }
        // GET: Ligne_depot/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_depot ligne_depot = db.Ligne_depot.Find(id);
            if (ligne_depot == null)
            {
                return HttpNotFound();
            }
            return View(ligne_depot);
        }

        // GET: Ligne_depot/Create
        public ActionResult Create()
        {
            ViewBag.sizeLP = Fonct.ListLigneProduction().Count();
            ViewBag.ligneProduction = Fonct.ListLigneProduction();
            ViewBag.tmpLigneP = 0;

            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();
            ViewBag.tmpdepot = 0;

            ViewBag.sizeSup = Fonct.ListSuperviseurs().Count();
            ViewBag.superviseur = Fonct.ListSuperviseurs();
            ViewBag.tmpSup = 0;

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();
            ViewBag.tmpPro = 0;


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG"; 

                if (Session["fonction"].ToString().Equals(fonction) && Session["verify_Inv"] != null)
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }

        // POST: Ligne_depot/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_ligne_production,qtite_caisse,id_produit")] Ligne_depot ligne_depot)
        {
            ViewBag.sizeLP = Fonct.ListLigneProduction().Count();
            ViewBag.ligneProduction = Fonct.ListLigneProduction();

            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();

            ViewBag.sizeSup = Fonct.ListSuperviseurs().Count();
            ViewBag.superviseur = Fonct.ListSuperviseurs();

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();

            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"];
            }

            if (ModelState.IsValid)
            {
                var sup = (int)Session["iddataclock"];
                int va = int.Parse(sup.ToString());

                Utilisateur info_superv = db.Utilisateurs.Find(va);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();


                Depot_Produit dep_pro = new Depot_Produit();
                var Idp1 = Request["id_produit"];
                int valID1 = int.Parse(Idp1.ToString());

                
               int valDep = info_depot.id;

                var query = from lign_depot in db.Depot_Produit
                            where lign_depot.id_produit == valID1  && lign_depot.id_depot== valDep
                            select lign_depot;

                foreach (Depot_Produit lo in query)
                {
                    lo.qtite_produit_dispo += ligne_depot.qtite_caisse;
             
                }
                db.SaveChanges();


                ligne_depot.id_depot = info_depot.id;
                ligne_depot.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                ligne_depot.date_ligne_depot = DateTime.Now;
                db.Ligne_depot.Add(ligne_depot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ligne_depot);
        }

        // GET: Ligne_depot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_depot ligne_depot = db.Ligne_depot.Find(id);
            if (ligne_depot == null)
            {
                return HttpNotFound();
            }
            
                TempData["tempom"] = ligne_depot.qtite_caisse;
              

        
            
            return View(ligne_depot);
        }

        // POST: Ligne_depot/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_ligne_production,id_depot,qtite_caisse,date_ligne_depot,id_superviseur,id_produit")] Ligne_depot ligne_depot)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ligne_depot).State = EntityState.Modified;
                db.SaveChanges();
                var query = from lign_depot in db.Depot_Produit
                            where lign_depot.id_produit == ligne_depot.id_produit && lign_depot.id_depot == ligne_depot.id_depot
                            select lign_depot;

                foreach (Depot_Produit lo in query)
                {
                    lo.qtite_produit_dispo -= (int)TempData["tempom"];
                    lo.qtite_produit_dispo += ligne_depot.qtite_caisse;

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ligne_depot);
        }

        // GET: Ligne_depot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_depot ligne_depot = db.Ligne_depot.Find(id);
            if (ligne_depot == null)
            {
                return HttpNotFound();
            }
            return View(ligne_depot);
        }

        // POST: Ligne_depot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ligne_depot ligne_depot = db.Ligne_depot.Find(id);
            db.Ligne_depot.Remove(ligne_depot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
