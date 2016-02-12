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
    public class Produit_BloqueController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();


        // GET: Produit_Bloque
        public ActionResult Index()
        {
            // 
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction1 = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction1))
                {
                    
                    return View(db.Produit_Bloque.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }

        // GET: Produit_Bloque/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            if (produit_Bloque == null)
            {
                return HttpNotFound();
            }
            return View(produit_Bloque);
        }

        // GET: Produit_Bloque/Create
        public ActionResult Create()
        {
            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();
            ViewBag.tmpdepot = 0;
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();
            ViewBag.tmpProd = 0;


            return View();
        }

        // POST: Produit_Bloque/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_produit,id_depot,raison,qtite_caisse_donnee,qtite_caisse_retour,qtite_bout_donnee,qtite_bout_retour")] Produit_Bloque produit_Bloque)
        {
            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();
            ViewBag.tmpdepot = 0;
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();
            ViewBag.tmpProd = 0;


            if (ModelState.IsValid)
            {
                var verification = db.Depot_Produit.Where(b => b.id_depot.Equals(produit_Bloque.id_depot) && b.id_produit.Equals(produit_Bloque.id_produit)).FirstOrDefault();
                if (verification.qtite_produit_dispo < produit_Bloque.qtite_caisse_donnee || verification.qtite_bouteille < produit_Bloque.qtite_bout_donnee)
                {
                    ViewBag.bloque = "Vous ne pouvez pas bloquer plus de produits qu vous en avez!";
                    return View(produit_Bloque);
                }
                produit_Bloque.id_admin = int.Parse(Session["IdUser"].ToString());
                produit_Bloque.date_donnee = DateTime.Now;
                produit_Bloque.date_retour = DateTime.Now;
                produit_Bloque.etat = 0;
                db.Produit_Bloque.Add(produit_Bloque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produit_Bloque);
        }

        // GET: Produit_Bloque/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            if (produit_Bloque == null)
            {
                return HttpNotFound();
            }
            return View(produit_Bloque);
        }

        // GET: Produit_Bloque/Debloque/5
        public ActionResult Debloque(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            if (produit_Bloque == null)
            {
                return HttpNotFound();
            }
            return View(produit_Bloque);
        }
        // GET: Produit_Bloque/Debloque/5
        public ActionResult Bloque(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            if (produit_Bloque == null)
            {
                return HttpNotFound();
            }
            var deblo = db.Produit_Bloque.Where(b => b.id.Equals(produit_Bloque.id)).FirstOrDefault();
            deblo.etat = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
            // return View(produit_Bloque);
        }

        // POST: Produit_Bloque/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_produit,id_depot,raison,qtite_caisse_donnee,qtite_caisse_retour,etat,date_donnee,date_retour,qtite_bout_donnee,qtite_bout_retour,id_admin")] Produit_Bloque produit_Bloque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produit_Bloque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produit_Bloque);
        }
        // Debloque
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Debloque([Bind(Include = "id,qtite_caisse_retour,qtite_bout_retour")] Produit_Bloque produit_Bloque)
        {
            var deblo = db.Produit_Bloque.Where(b => b.id.Equals(produit_Bloque.id)).FirstOrDefault();
           

            
            deblo.qtite_bout_retour = produit_Bloque.qtite_bout_retour;
            deblo.qtite_caisse_retour = produit_Bloque.qtite_caisse_retour;
                 deblo.etat = 1;
                deblo.date_retour = DateTime.Now;
               // db.Produit_Bloque.Add(produit_Bloque);
              //  db.Entry(produit_Bloque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
           
        }
        // GET: Produit_Bloque/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            if (produit_Bloque == null)
            {
                return HttpNotFound();
            }
            return View(produit_Bloque);
        }

        // POST: Produit_Bloque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produit_Bloque produit_Bloque = db.Produit_Bloque.Find(id);
            db.Produit_Bloque.Remove(produit_Bloque);
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
