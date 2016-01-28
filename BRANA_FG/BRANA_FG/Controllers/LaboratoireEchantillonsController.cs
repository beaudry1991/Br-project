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
    public class LaboratoireEchantillonsController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();
        // GET: LaboratoireEchantillons
        public ActionResult Index()
        {
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG"; var fonction1 = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View(db.LaboratoireEchantillons.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }

            //if (Session["iddataclock"] == null)
            //{
            //    Session["iddataclock"] = 1;
            //}
            //return View(db.LaboratoireEchantillons.ToList());
        }

        // GET: LaboratoireEchantillons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LaboratoireEchantillon laboratoireEchantillon = db.LaboratoireEchantillons.Find(id);
            if (laboratoireEchantillon == null)
            {
                return HttpNotFound();
            }
            return View(laboratoireEchantillon);
        }

        // GET: LaboratoireEchantillons/Create
        public ActionResult Create()
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG"; var fonction1 = "admin_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }

        // POST: LaboratoireEchantillons/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom_recepteur,id_produit,qtite_donnee,qtite_retour")] LaboratoireEchantillon laboratoireEchantillon)
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();


            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"];
            }
            if (ModelState.IsValid)
            {
                laboratoireEchantillon.date_donnee = DateTime.Now;
                Utilisateur info_superv = db.Utilisateurs.Find(int.Parse(Session["iddataclock"].ToString()));

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;

                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }
                if (id_depot != 0)
                {
                    laboratoireEchantillon.id_depot = id_depot;
                    laboratoireEchantillon.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                    db.LaboratoireEchantillons.Add(laboratoireEchantillon);
                    db.SaveChanges();


                    var query1 = from lign_depot in db.Depot_Produit
                                 where lign_depot.id_produit == laboratoireEchantillon.id_produit && lign_depot.id_depot == laboratoireEchantillon.id_depot
                                 select lign_depot;

                    foreach (Depot_Produit lo in query1)
                    {
                        lo.qtite_produit_dispo -= laboratoireEchantillon.qtite_donnee;

                    }
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }

            return View(laboratoireEchantillon);
        }

        // GET: LaboratoireEchantillons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LaboratoireEchantillon laboratoireEchantillon = db.LaboratoireEchantillons.Find(id);
            
            TempData["datetempon"] = laboratoireEchantillon.date_donnee;
            ViewBag.superviseuredit = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.produitedit = new SelectList(db.Produits, "id", "nom");
            if (laboratoireEchantillon == null)
            {
                return HttpNotFound();
            }
            return View(laboratoireEchantillon);
        }

        // POST: LaboratoireEchantillons/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom_recepteur,date_retour,id_produit,qtite_donnee,qtite_retour")] LaboratoireEchantillon laboratoireEchantillon)
        {
            if (ModelState.IsValid)
            {
                laboratoireEchantillon.date_donnee = (DateTime)TempData["datetempon"];
                laboratoireEchantillon.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                Utilisateur info_superv = db.Utilisateurs.Find(laboratoireEchantillon.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {
                    laboratoireEchantillon.id_depot = id_depot;
                    db.Entry(laboratoireEchantillon).State = EntityState.Modified;
                     db.SaveChanges();
            }
                return RedirectToAction("Index");
            }
            return View(laboratoireEchantillon);
        }

        // GET: LaboratoireEchantillons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LaboratoireEchantillon laboratoireEchantillon = db.LaboratoireEchantillons.Find(id);
            if (laboratoireEchantillon == null)
            {
                return HttpNotFound();
            }
            return View(laboratoireEchantillon);
        }

        // POST: LaboratoireEchantillons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LaboratoireEchantillon laboratoireEchantillon = db.LaboratoireEchantillons.Find(id);
            db.LaboratoireEchantillons.Remove(laboratoireEchantillon);
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
