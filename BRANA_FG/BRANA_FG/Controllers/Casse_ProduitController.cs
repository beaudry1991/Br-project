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
    public class Casse_ProduitController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Casse_Produit
        public ActionResult Index()
        {
            return View(db.Casse_Produit.ToList());
        }

        // GET: Casse_Produit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse_Produit casse_Produit = db.Casse_Produit.Find(id);
            if (casse_Produit == null)
            {
                return HttpNotFound();
            }
            return View(casse_Produit);
        }

        // GET: Casse_Produit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Casse_Produit/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_casse,id_produit,qtite_caisse,qtite_bout")] Casse_Produit casse_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Casse_Produit.Add(casse_Produit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(casse_Produit);
        }

        // GET: Casse_Produit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse_Produit casse_Produit = db.Casse_Produit.Find(id);
            if (casse_Produit == null)
            {
                return HttpNotFound();
            }
            return View(casse_Produit);
        }

        // POST: Casse_Produit/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_casse,id_produit,qtite_caisse,qtite_bout")] Casse_Produit casse_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(casse_Produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(casse_Produit);
        }

        // GET: Casse_Produit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse_Produit casse_Produit = db.Casse_Produit.Find(id);
            if (casse_Produit == null)
            {
                return HttpNotFound();
            }
            return View(casse_Produit);
        }

        // POST: Casse_Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Casse_Produit casse_Produit = db.Casse_Produit.Find(id);
            db.Casse_Produit.Remove(casse_Produit);
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
