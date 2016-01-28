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
    public class Ligne_ProductionController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Ligne_Production
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
                    return View(db.Ligne_Production.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }

            //if (Session["idadmin"] == null)
            //{
            //    Session["idadmin"] = 1;
            //}
            //return View(db.Ligne_Production.ToList());
        }

        // GET: Ligne_Production/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_Production ligne_Production = db.Ligne_Production.Find(id);
            if (ligne_Production == null)
            {
                return HttpNotFound();
            }
            return View(ligne_Production);
        }

        // GET: Ligne_Production/Create
        public ActionResult Create()
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
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }

        // POST: Ligne_Production/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom")] Ligne_Production ligne_Production)
        {
            if (Session["idadmin"] == null)
            {
                Session["idadmin"] = Session["IdUser"]; 
            }
            if (ModelState.IsValid)
            {
                ligne_Production.id_utilisateur= int.Parse(Session["idadmin"].ToString());
                ligne_Production.date_ligne_prod = DateTime.Now;
                db.Ligne_Production.Add(ligne_Production);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ligne_Production);
        }

        // GET: Ligne_Production/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_Production ligne_Production = db.Ligne_Production.Find(id);
            TempData["datetempon"] = ligne_Production.date_ligne_prod;
            if (ligne_Production == null)
            {
                return HttpNotFound();
            }
            return View(ligne_Production);
        }

        // POST: Ligne_Production/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom")] Ligne_Production ligne_Production)
        {
            if (ModelState.IsValid)
            {
                ligne_Production.date_ligne_prod = (DateTime)TempData["datetempon"];
                ligne_Production.id_utilisateur = int.Parse(Session["idadmin"].ToString());
                db.Entry(ligne_Production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ligne_Production);
        }

        // GET: Ligne_Production/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ligne_Production ligne_Production = db.Ligne_Production.Find(id);
            if (ligne_Production == null)
            {
                return HttpNotFound();
            }
            return View(ligne_Production);
        }

        // POST: Ligne_Production/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ligne_Production ligne_Production = db.Ligne_Production.Find(id);
            db.Ligne_Production.Remove(ligne_Production);
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
