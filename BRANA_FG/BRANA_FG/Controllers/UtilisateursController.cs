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
    public class UtilisateursController : Controller
    {
        private BddContext db = new BddContext();
        FonctionRequete Fonct = new FonctionRequete();

        // GET: Utilisateurs
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

                    return View(db.Utilisateurs.ToList());

                }
               else
                {
                   return Redirect("~/Logins/Index");
                }
            }
           // return View(db.Utilisateurs.ToList());
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
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
                //   ViewBag.depot = new SelectList(db.Depots, "nom", "nom", " ");
                ViewBag.sizeDP = Fonct.ListDepot().Count();
                ViewBag.listdep = Fonct.ListDepot();
                ViewBag.tmpdepot = 0;

                return View();
               }
                else
                {
                    return Redirect("~/Logins/Index");
                }
            }
            //return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,prenom,mail,phone,adresse,fonction,shift,depot")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                ViewBag.sizeDP = Fonct.ListDepot().Count();
                ViewBag.listdep = Fonct.ListDepot();
                //assurance d'email unique
                var mail = db.Utilisateurs.Where(a => a.mail.Equals(utilisateur.mail)).FirstOrDefault();
                if (mail != null)
                {
                    //reste a ajoute un message d'erreur
                    ViewBag.error = "This Email is already  used";
                    ViewBag.depot = new SelectList(db.Depots, "nom", "nom", " ");
                    return View(utilisateur);
                }
                /* function required*/
                if (utilisateur.fonction.Equals("Select one"))
                {
                    //reste a ajoute un message d'erreur
                    ViewBag.error = "Please select a function for this User";
                    ViewBag.depot = new SelectList(db.Depots, "nom", "nom", " ");
                    return View(utilisateur);
                }

                /*Shift and depot required for supervisor depot and unloading*/
                if ((utilisateur.fonction.Equals("Superviseur FG") || utilisateur.fonction.Equals("Data Clock")) && (utilisateur.shift.Equals("Superviseur Debarquement") || IsNullOrWhiteSpace(utilisateur.depot)))
                {
                    //reste a ajoute un message d'erreur
                    ViewBag.error = "Please select a shift and a location to affect this user";
                    ViewBag.depot = new SelectList(db.Depots, "nom", "nom", " ");
                    return View(utilisateur);
                }
                db.Utilisateurs.Add(utilisateur);
                db.SaveChanges();
                /*Last user Insert ID*/
                int ID = db.Utilisateurs.Max(item => item.id);

               // Idal dal = new Dal();
               // var pass = "Password"; // Password Crypted
              //  dal.createNewlogin(Request["mail"], pass, ID);
                ModelState.Clear();
                ViewBag.Message = "Succesfully Registration Done !";
                return RedirectToAction("Index");

            }

            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();
            ViewBag.depot = new SelectList(db.Depots, "nom", "nom", " ");
            return View(utilisateur);
            //if (ModelState.IsValid)
            //{
            //    db.Utilisateurs.Add(utilisateur);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(utilisateur);
        }


        private bool IsNullOrWhiteSpace(string parameter)
        {
            return String.IsNullOrEmpty(parameter) || parameter.Trim().Length == 0;
        }


        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,prenom,mail,phone,adresse,fonction,shift,depot")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(utilisateur);
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
