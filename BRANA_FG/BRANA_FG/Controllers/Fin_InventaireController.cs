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
    public class Fin_InventaireController : Controller
    {
        private BddContext db = new BddContext();
        FonctionRequete Fonct = new FonctionRequete();
        // GET: Fin_Inventaire
        public ActionResult Index()
        {

            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction))
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View(db.Fin_Inventaire.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
            
        }

        // GET: Fin_Inventaire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fin_Inventaire fin_Inventaire = db.Fin_Inventaire.Find(id);
            if (fin_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(fin_Inventaire);
        }

        // GET: Fin_Inventaire/Create
        public ActionResult Create()
        {
            ViewBag.superviseur = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.produit = new SelectList(db.Produits, "id", "nom");
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
            ViewBag.tmpPro = 0;


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction))
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

        // POST: Fin_Inventaire/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_produit,qtite_laissee")] Fin_Inventaire fin_Inventaire)
        {
            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"]; ;
            }

            if (ModelState.IsValid)
            {
                fin_Inventaire.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                fin_Inventaire.date_fin_inventaire = DateTime.Now;
                Utilisateur info_superv = db.Utilisateurs.Find(fin_Inventaire.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }


                if (id_depot != 0)
                {

                    fin_Inventaire.id_depot_fg = id_depot;
                    var i = Request.Form.GetValues("idP");
                    var c = Request.Form.GetValues("caisse");
                    var bo = Request.Form.GetValues("bouteille");



                    string[] idProduit = i.ToArray();
                    string[] caisse = c.ToArray();
                    string[] bouteille = bo.ToArray();


                    for (int a = 0; a < caisse.Count(); a++)
                    {

                        if (!caisse[a].Equals(""))
                        {
                            int v = 0, w = 0;
                            List<Produit_dispo> dispo = Fonct.Stock_dispos(info_superv.depot, int.Parse(idProduit[a].ToString()));

                            if (dispo.Count() > 0)
                            {
                                v = dispo[0].bouteille;
                                w = dispo[0].caisse;
                            }
                            fin_Inventaire.qtite_caisse_theo = w;
                            fin_Inventaire.qtite_bout_theo = v;
                            fin_Inventaire.id_produit = int.Parse(idProduit[a].ToString());

                            fin_Inventaire.qtite_laissee = int.Parse(caisse[a].ToString());
                            fin_Inventaire.qtite_bout = int.Parse(bouteille[a].ToString()); ;

                            db.Fin_Inventaire.Add(fin_Inventaire);

                            db.SaveChanges();
                        }
                    }


                    //db.Fin_Inventaire.Add(fin_Inventaire);

                    //db.SaveChanges();
                    }
                return RedirectToAction("Index");
            }

            return View(fin_Inventaire);
        }

        // GET: Fin_Inventaire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fin_Inventaire fin_Inventaire = db.Fin_Inventaire.Find(id);
            TempData["datetempon"] = fin_Inventaire.date_fin_inventaire;
            ViewBag.superviseuredit = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.produitedit = new SelectList(db.Produits, "id", "nom");

            if (fin_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(fin_Inventaire);
        }

        // POST: Fin_Inventaire/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_produit,qtite_laissee")] Fin_Inventaire fin_Inventaire)
        {
            if (ModelState.IsValid)
            {
                fin_Inventaire.date_fin_inventaire = (DateTime)TempData["datetempon"];
                fin_Inventaire.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                Utilisateur info_superv = db.Utilisateurs.Find(fin_Inventaire.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {
                    fin_Inventaire.id_depot_fg = id_depot;
                    db.Entry(fin_Inventaire).State = EntityState.Modified;
                    db.SaveChanges();
            }
                return RedirectToAction("Index");
            }
            return View(fin_Inventaire);
        }

        // GET: Fin_Inventaire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fin_Inventaire fin_Inventaire = db.Fin_Inventaire.Find(id);
            if (fin_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(fin_Inventaire);
        }

        // POST: Fin_Inventaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fin_Inventaire fin_Inventaire = db.Fin_Inventaire.Find(id);
            db.Fin_Inventaire.Remove(fin_Inventaire);
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
