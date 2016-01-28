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
    public class Transfert_ProduitController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Transfert_Produit
        public ActionResult Index()
        {
            return View(db.Transfert_Produit.ToList());
        }

        // GET: Transfert_Produit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert_Produit transfert_Produit = db.Transfert_Produit.Find(id);
            if (transfert_Produit == null)
            {
                return HttpNotFound();
            }
            return View(transfert_Produit);
        }

        // GET: Transfert_Produit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transfert_Produit/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_transfert,qtite_caisse,id_produit,qtite_bout")] Transfert_Produit transfert_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Transfert_Produit.Add(transfert_Produit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transfert_Produit);
        }

        // GET: Transfert_Produit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert_Produit transfert_Produit = db.Transfert_Produit.Find(id);
            if (transfert_Produit == null)
            {
                return HttpNotFound();
            }
            return View(transfert_Produit);
        }

        // POST: Transfert_Produit/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_transfert,qtite_caisse,id_produit,qtite_bout")] Transfert_Produit transfert_Produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transfert_Produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transfert_Produit);
        }

        // GET: Transfert_Produit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert_Produit transfert_Produit = db.Transfert_Produit.Find(id);
            if (transfert_Produit == null)
            {
                return HttpNotFound();
            }
            return View(transfert_Produit);
        }

        // POST: Transfert_Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transfert_Produit transfert_Produit = db.Transfert_Produit.Find(id);
            db.Transfert_Produit.Remove(transfert_Produit);
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
