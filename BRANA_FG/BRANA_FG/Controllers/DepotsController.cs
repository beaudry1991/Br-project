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
    public class DepotsController : Controller
    {
        private BddContext db = new BddContext();
        private BddContext ddd = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();

        // GET: Depots
        public ActionResult Index()
        {
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction))
                {
                    Session["idadmin"] = Session["IdUser"];
                    return View(db.Depots.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
            //return View(db.Depots.ToList());
        }

        // GET: Depots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depots depots = db.Depots.Find(id);
            if (depots == null)
            {
                return HttpNotFound();
            }
            return View(depots);
        }

        // GET: Depots/Create
        public ActionResult Create()
        {
            if (Session.Keys.Count == 0)
            {
                return RedirectToAction("Index", "Logins");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {

                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }
            //return View();
        }

        // POST: Depots/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,adresse,typeDepot")] Depots depots)
        {
            if (ModelState.IsValid)
            {
                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeProduit = Fonct.listproduit().Count();

                depots.dateCreation = DateTime.Now;
                db.Depots.Add(depots);
                db.SaveChanges();



                for (int b = 0; b < ViewBag.sizeProduit; b++)
                {
                    Depot_Produit add_pp = new Depot_Produit();

                    add_pp.id_depot = depots.id;
                    add_pp.id_produit = ViewBag.produitlist[b].id;
                    add_pp.qtite_produit_dispo = 0;
                    add_pp.qtite_bouteille = 0;
                    ddd.Depot_Produit.Add(add_pp);
                    ddd.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(depots);
        }

        // GET: Depots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depots depots = db.Depots.Find(id);
            TempData["datetempon"] = depots.dateCreation;
            if (depots == null)
            {
                return HttpNotFound();
            }
            return View(depots);
        }

        // POST: Depots/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,adresse,typeDepot")] Depots depots)
        {
            if (ModelState.IsValid)
            {
                depots.dateCreation = (DateTime)TempData["datetempon"];
                db.Entry(depots).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depots);
        }

        // GET: Depots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depots depots = db.Depots.Find(id);
            if (depots == null)
            {
                return HttpNotFound();
            }
            return View(depots);
        }

        // POST: Depots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depots depots = db.Depots.Find(id);
            db.Depots.Remove(depots);
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
