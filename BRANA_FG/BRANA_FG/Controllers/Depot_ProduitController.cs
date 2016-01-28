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
    public class Depot_ProduitController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Depot_Produit
        public ActionResult Index()
        {
            return View(db.Depot_Produit.ToList());
        }

        // GET: Depot_Produit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_Produit depot_Produit = db.Depot_Produit.Find(id);
            if (depot_Produit == null)
            {
                return HttpNotFound();
            }
            return View(depot_Produit);
        }

        // GET: Depot_Produit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Depot_Produit/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_depot,id_produit,qtite_produit_dispo,qtite_bouteille")] Depot_Produit depot_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Depot_Produit.Add(depot_Produit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depot_Produit);
        }

        // GET: Depot_Produit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_Produit depot_Produit = db.Depot_Produit.Find(id);
            if (depot_Produit == null)
            {
                return HttpNotFound();
            }
            return View(depot_Produit);
        }

        // POST: Depot_Produit/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_depot,id_produit,qtite_produit_dispo,qtite_bouteille")] Depot_Produit depot_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depot_Produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depot_Produit);
        }

        // GET: Depot_Produit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_Produit depot_Produit = db.Depot_Produit.Find(id);
            if (depot_Produit == null)
            {
                return HttpNotFound();
            }
            return View(depot_Produit);
        }

        // POST: Depot_Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depot_Produit depot_Produit = db.Depot_Produit.Find(id);
            db.Depot_Produit.Remove(depot_Produit);
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
