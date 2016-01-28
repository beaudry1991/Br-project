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
    public class Loading_produitController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Loading_produit
        public ActionResult Index()
        {
            return View(db.Loading_produit.ToList());
        }

        // GET: Loading_produit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading_produit loading_produit = db.Loading_produit.Find(id);
            if (loading_produit == null)
            {
                return HttpNotFound();
            }
            return View(loading_produit);
        }

        // GET: Loading_produit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Loading_produit/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_produit,qte_caisse_delivre,qte_caisse_retourne,qte_bout_retourne,id_loading")] Loading_produit loading_produit)
        {
            if (ModelState.IsValid)
            {
                db.Loading_produit.Add(loading_produit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loading_produit);
        }

        // GET: Loading_produit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading_produit loading_produit = db.Loading_produit.Find(id);
            if (loading_produit == null)
            {
                return HttpNotFound();
            }
            return View(loading_produit);
        }

        // POST: Loading_produit/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_produit,qte_caisse_delivre,qte_caisse_retourne,qte_bout_retourne,id_loading")] Loading_produit loading_produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loading_produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loading_produit);
        }

        // GET: Loading_produit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading_produit loading_produit = db.Loading_produit.Find(id);
            if (loading_produit == null)
            {
                return HttpNotFound();
            }
            return View(loading_produit);
        }

        // POST: Loading_produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loading_produit loading_produit = db.Loading_produit.Find(id);
            db.Loading_produit.Remove(loading_produit);
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
