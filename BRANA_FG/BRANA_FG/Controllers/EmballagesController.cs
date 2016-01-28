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
    public class EmballagesController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Emballages
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
                    return View(db.Emballages.ToList());
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
            //return View(db.Emballages.ToList());
        }

        // GET: Emballages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emballage emballage = db.Emballages.Find(id);
            if (emballage == null)
            {
                return HttpNotFound();
            }
            return View(emballage);
        }

        // GET: Emballages/Create
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

        // POST: Emballages/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,volume,qte_par_caisse")] Emballage emballage)
        {

            if (Session["idadmin"] == null)
            {
                Session["idadmin"] = Session["IdUser"]; ;
            }

            if (ModelState.IsValid)
            {
               emballage.id_utilisateur = int.Parse(Session["idadmin"].ToString());
                emballage.date_emb = DateTime.Now;
                db.Emballages.Add(emballage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emballage);
        }

        // GET: Emballages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emballage emballage = db.Emballages.Find(id);
            TempData["datetempon"] = emballage.date_emb;
            if (emballage == null)
            {
                return HttpNotFound();
            }
            return View(emballage);
        }

        // POST: Emballages/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,volume,qte_par_caisse")] Emballage emballage)
        {
            if (ModelState.IsValid)
            {
                emballage.date_emb = (DateTime)TempData["datetempon"];
                emballage.id_utilisateur= int.Parse(Session["idadmin"].ToString());
                db.Entry(emballage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emballage);
        }

        // GET: Emballages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emballage emballage = db.Emballages.Find(id);
            if (emballage == null)
            {
                return HttpNotFound();
            }
            return View(emballage);
        }

        // POST: Emballages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emballage emballage = db.Emballages.Find(id);
            db.Emballages.Remove(emballage);
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
