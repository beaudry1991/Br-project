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
    public class ProduitsController : Controller
    {
        private BddContext db = new BddContext();
        private BddContext ddd = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();


        // GET: Produits
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

                    return View(db.Produits.ToList());

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }
            //return View(db.Produits.ToList());
        }

        // GET: Produits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produits produits = db.Produits.Find(id);
            if (produits == null)
            {
                return HttpNotFound();
            }
            return View(produits);
        }

        // GET: Produits/Create
        public ActionResult Create()
        {
            //ViewBag.utilisateur = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.emballage = Fonct.listEmballage();
            ViewBag.sizeEmballage = Fonct.listEmballage().Count();
            ViewBag.categorie = Fonct.listCategorie();
            ViewBag.sizeCategorie = Fonct.listCategorie().Count();
            if (Session.Keys.Count == 0)
            {
                return RedirectToAction("Index", "Logins");
            }
            else
            {
                if (Session["fonction"].ToString().Equals("admin_FG"))
                {
                    Session["idadmin"] = Session["IdUser"];
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Logins");
                }
            }

            //if (Session["idadmin"]== null){
            //Session["idadmin"] = 1;
            //}
            //return View();
        }

        // POST: Produits/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,id_emballage,id_categorie")] Produits produits)
        {
            if (ModelState.IsValid)
            {

                ViewBag.depotlist = Fonct.ListDepot();
                ViewBag.sizeDepot = Fonct.ListDepot().Count();

                //var idd = Request["id"];
                //int idd_int = int.Parse(idd.ToString());
                ViewBag.emballage = Fonct.listEmballage();
                ViewBag.sizeEmballage = Fonct.listEmballage().Count();

                ViewBag.categorie = Fonct.listCategorie();
                ViewBag.sizeCategorie = Fonct.listCategorie().Count();



                produits.date_prod = DateTime.Now;
                produits.id_utilisateur = int.Parse(Session["idadmin"].ToString());
                db.Produits.Add(produits);
                db.SaveChanges();

                for (int b = 0; b < ViewBag.sizeDepot; b++)
                {
                    Depot_Produit add_pp = new Depot_Produit();

                    add_pp.id_depot = ViewBag.depotlist[b].id;
                    add_pp.id_produit = produits.id;
                    add_pp.qtite_produit_dispo = 0;
                    add_pp.qtite_bouteille = 0;
                    ddd.Depot_Produit.Add(add_pp);
                    ddd.SaveChanges();
                  }









                return RedirectToAction("Index");
            }

            return View(produits);
        }

        // GET: Produits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produits produits = db.Produits.Find(id);
            ViewBag.utilisateur = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.emballage = new SelectList(db.Emballages, "id", "nom");
            ViewBag.categorie = new SelectList(db.Categories, "id", "nom");
            TempData["datetempon"] = produits.date_prod;
            if (Session["idadmin"] == null)
            {
                Session["idadmin"] = Session["IdUser"];
            }
            if (produits == null)
            {
                return HttpNotFound();
            }
            return View(produits);
        }

        // POST: Produits/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,id_emballage,id_categorie,id_utilisateur,date_prod")] Produits produits)
        {
            if (ModelState.IsValid)
            {
                produits.date_prod = DateTime.Now;
                produits.id_utilisateur = int.Parse(Session["idadmin"].ToString());
                produits.date_prod = (DateTime)TempData["datetempon"];

                db.Entry(produits).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produits);
        }

        // GET: Produits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produits produits = db.Produits.Find(id);
            if (produits == null)
            {
                return HttpNotFound();
            }
            return View(produits);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produits produits = db.Produits.Find(id);
            db.Produits.Remove(produits);
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
