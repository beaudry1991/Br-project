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
    public class Produit_manquantController : Controller
    {
        private BddContext db = new BddContext();
        private BddContext ddd = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();


        // GET: Produit_manquant
        public ActionResult Index()
        {
            return View(db.Produit_manquant.ToList());
        }

        // GET: Produit_manquant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_manquant produit_manquant = db.Produit_manquant.Find(id);
            if (produit_manquant == null)
            {
                return HttpNotFound();
            }
            return View(produit_manquant);
        }
        //
        public ActionResult NumEmb()
        {
            if (TempData["msg1"] != null)
            {
                ViewBag.emb_alerte = "Numero d'Embarquement non existant.";
            }
            
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "admin_FG"; var fonction1 = "data_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    // Session["iddataclock"] = Session["IdUser"];

                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }

        }

        // GET: Produit_manquant/Create
        public ActionResult Create(string num_emb)
        {
            var num_emb_verify = db.Loadings.Where(b => b.numero_emb.Equals(num_emb)).FirstOrDefault();


            if (num_emb_verify == null)
            {
                TempData["msg1"] = 1;
                return RedirectToAction("NumEmb");

            }
            else
            {




                var st = num_emb;
                string eo = num_emb;
                Session["val_num_emb"] = eo;

                TempData["id_load"] = num_emb;

                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeP = Fonct.listproduit().Count();
                ViewBag.tmpPro = 0;

                ViewBag.sizeUnload = Fonct.ListUnload(eo).Count();
                ViewBag.listUnload = Fonct.ListUnload(eo);

            }


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "admin_FG"; var fonction1 = "data_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    // Session["iddataclock"] = Session["IdUser"];
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
            ///  return View();
        }

        // POST: Produit_manquant/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numEmb,id_user,idP,caisse,bouteille")] Produit_manquant produit_manquant)
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
            ViewBag.sizeUnload = Fonct.ListUnload((string)Session["val_num_emb"]).Count();
            ViewBag.listUnload = Fonct.ListUnload((string)Session["val_num_emb"]);

            if (ModelState.IsValid)
            {
                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");


                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();

                var num = Request["numEmb"];
                //int va = int.Parse(num.ToString());
                produit_manquant.num_emb = num;

                var infoload = db.Loadings.Where(l => l.numero_emb.Equals(produit_manquant.num_emb)).FirstOrDefault();

                var query = from load_pro in ddd.Prod_manq_relation
                            select load_pro;

                foreach (Prod_manq_relation lo in query)
                {
                    for (int d = 0; d < ViewBag.sizeUnload; d++)
                    {
                        lo.id_produit = int.Parse(idProduit[d].ToString());
                        lo.qtite_caisse = int.Parse(caisse[d].ToString());
                        lo.qtite_bout = int.Parse(bouteille[d].ToString());
                        lo.id_prod_manq = produit_manquant.id;
                      //  ddd.SaveChanges();


                    }
                }


                produit_manquant.id_depot = infoload.id_depot;
                produit_manquant.id_chauffeur = infoload.id_chauffeur;
                produit_manquant.id_user = int.Parse(Session["IdUser"].ToString());

                db.Produit_manquant.Add(produit_manquant);
                db.SaveChanges();


               

                
                return RedirectToAction("Index");
            }

            return View(produit_manquant);
        }

        // GET: Produit_manquant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_manquant produit_manquant = db.Produit_manquant.Find(id);
            if (produit_manquant == null)
            {
                return HttpNotFound();
            }
            return View(produit_manquant);
        }

        // POST: Produit_manquant/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_depot,id_user,id_chauffeur,num_emb")] Produit_manquant produit_manquant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produit_manquant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produit_manquant);
        }

        // GET: Produit_manquant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit_manquant produit_manquant = db.Produit_manquant.Find(id);
            if (produit_manquant == null)
            {
                return HttpNotFound();
            }
            return View(produit_manquant);
        }

        // POST: Produit_manquant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produit_manquant produit_manquant = db.Produit_manquant.Find(id);
            db.Produit_manquant.Remove(produit_manquant);
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
