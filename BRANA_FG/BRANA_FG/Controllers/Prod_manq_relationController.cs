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
    public class Prod_manq_relationController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Prod_manq_relation
        public ActionResult Index()
        {
            return View(db.Prod_manq_relation.ToList());
        }

        // GET: Prod_manq_relation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prod_manq_relation prod_manq_relation = db.Prod_manq_relation.Find(id);
            if (prod_manq_relation == null)
            {
                return HttpNotFound();
            }
            return View(prod_manq_relation);
        }

        // GET: Prod_manq_relation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prod_manq_relation/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,id_produit,id_prod_manq,qtite_caisse,qtite_bout")] Prod_manq_relation prod_manq_relation)
        {
            if (ModelState.IsValid)
            {
                db.Prod_manq_relation.Add(prod_manq_relation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prod_manq_relation);
        }

        // GET: Prod_manq_relation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prod_manq_relation prod_manq_relation = db.Prod_manq_relation.Find(id);
            if (prod_manq_relation == null)
            {
                return HttpNotFound();
            }
            return View(prod_manq_relation);
        }

        // POST: Prod_manq_relation/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,id_produit,id_prod_manq,qtite_caisse,qtite_bout")] Prod_manq_relation prod_manq_relation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prod_manq_relation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prod_manq_relation);
        }

        // GET: Prod_manq_relation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prod_manq_relation prod_manq_relation = db.Prod_manq_relation.Find(id);
            if (prod_manq_relation == null)
            {
                return HttpNotFound();
            }
            return View(prod_manq_relation);
        }

        // POST: Prod_manq_relation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prod_manq_relation prod_manq_relation = db.Prod_manq_relation.Find(id);
            db.Prod_manq_relation.Remove(prod_manq_relation);
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
