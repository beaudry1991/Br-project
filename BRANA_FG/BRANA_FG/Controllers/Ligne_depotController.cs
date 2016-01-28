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
                var fonction = "super_FG"; var fonction1 = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
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
        public ActionResult Create([Bind(Include = "id,id_ligne_production,id_depot,qtite_caisse,date_ligne_depot,id_produit")] Ligne_depot ligne_depot)
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
                Depot_Produit dep_pro = new Depot_Produit();
                var Idp1 = Request["id_produit"];
                int valID1 = int.Parse(Idp1.ToString());

                var Iddep = Request["id_depot"];
               int valDep = int.Parse(Iddep.ToString());

                var query = from lign_depot in db.Depot_Produit
                            where lign_depot.id_produit == valID1  && lign_depot.id_depot== valDep
                            select lign_depot;

                foreach (Depot_Produit lo in query)
                {
                    lo.qtite_produit_dispo += ligne_depot.qtite_caisse;
             
                }
                db.SaveChanges();



                ligne_depot.id_superviseur = int.Parse(Session["iddataclock"].ToString());
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
