using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BRANA_FG.Models
{
    public class VehiculesController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Vehicules
        public ActionResult Index()
        {
            if (Session.Keys.Count == 0)
            {
                return Redirect("~/Logins/Index");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {

                    return View(db.Vehicules.ToList());

                }
                else
                {
                    return Redirect("~/Logins/Index");
                }
            }
           
        }

        // GET: Vehicules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.Vehicules.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // GET: Vehicules/Create
        public ActionResult Create()
        {
            if (Session.Keys.Count == 0)
            {
                return Redirect("~/Logins/Index");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {

                    return View();

                }
                else
                {
                    return Redirect("~/Logins/Index");
                }
            }
            //return View();
        }

        // POST: Vehicules/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,plaque,model,marque,annee,capacite,numero")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                var logged = db.Vehicules.Where(lg => lg.numero.Equals(vehicule.numero) || lg.plaque.Equals(vehicule.plaque)).FirstOrDefault();
                if (logged != null)
                {
                    ViewBag.alert = "This truck is already created, please check the plaque number or the truck number.";
                    return View();
                }
                db.Vehicules.Add(vehicule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicule);
        }

        // GET: Vehicules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.Vehicules.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: Vehicules/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,plaque,model,marque,annee,capacite,numero")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicule);
        }

        // GET: Vehicules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = db.Vehicules.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: Vehicules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicule vehicule = db.Vehicules.Find(id);
            db.Vehicules.Remove(vehicule);
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
