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
    public class ChauffeursController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Chauffeurs
        public ActionResult Index()
        {

            if (Session.Keys.Count == 0)
            {
                return RedirectToAction("Index", "Logins");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {

                    return View(db.Chauffeurs.ToList());

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }
            //return View(db.Chauffeurs.ToList());
        }

        // GET: Chauffeurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chauffeur chauffeur = db.Chauffeurs.Find(id);
            if (chauffeur == null)
            {
                return HttpNotFound();
            }
            return View(chauffeur);
        }

        // GET: Chauffeurs/Create
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

        // POST: Chauffeurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,prenom,nom,mail,phone,photo,adresse,aide1,aide2,noPermis,dateEmission,dateExpire")] Chauffeur chauffeur)
        {




            if (ModelState.IsValid)
            {   /*Verification de l'existence du chauffeur via son numero de permis*/
                var logged = db.Chauffeurs.Where(lg => lg.noPermis.Equals(chauffeur.noPermis)).FirstOrDefault();
                if (logged != null)
                {
                    ViewBag.chauffeur = "This driver is already registered";
                    return View();
                }

                /*Verification de l'existence du chauffeur via son  mail or phone number*/

                var log = db.Chauffeurs.Where(lg => lg.phone.Equals(chauffeur.phone)).FirstOrDefault();
                if (log != null)
                {
                    ViewBag.chauffeur = "This phone number: " + chauffeur.phone + " is already used";
                    return View();
                }
                if (chauffeur.mail != null)
                {
                    var logmail = db.Chauffeurs.Where(lg => lg.mail.Equals(chauffeur.mail)).FirstOrDefault();
                    if (logmail != null)
                    {
                        ViewBag.chauffeur = "This mail: " + chauffeur.mail + " is already used";
                        return View();
                    }
                }
                /*Date expiration licence*/
                if (chauffeur.dateExpire.Year != chauffeur.dateEmission.AddYears(5).Year)
                {
                    ViewBag.chauffeur = "Please verify the Expire date or the emission date";
                    return View();
                }
                if (chauffeur.dateExpire.Date <= DateTime.Now.Date)
                {
                    ViewBag.chauffeur = "Please verify the Expire date of the licence";
                    return View();
                }
                
                db.Chauffeurs.Add(chauffeur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chauffeur);
            
        }

        // GET: Chauffeurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chauffeur chauffeur = db.Chauffeurs.Find(id);
            if (chauffeur == null)
            {
                return HttpNotFound();
            }
            return View(chauffeur);
        }

        // POST: Chauffeurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,prenom,nom,mail,phone,photo,adresse,aide1,aide2,noPermis,dateEmission,dateExpire")] Chauffeur chauffeur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chauffeur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chauffeur);
        }

        // GET: Chauffeurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chauffeur chauffeur = db.Chauffeurs.Find(id);
            if (chauffeur == null)
            {
                return HttpNotFound();
            }
            return View(chauffeur);
        }

        // POST: Chauffeurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chauffeur chauffeur = db.Chauffeurs.Find(id);
            db.Chauffeurs.Remove(chauffeur);
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
