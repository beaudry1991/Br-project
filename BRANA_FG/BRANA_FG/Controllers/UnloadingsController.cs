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
    public class UnloadingsController : Controller
    {
        private BddContext db = new BddContext();
            private BddContext ddd = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();
       
        // GET: Unloadings
        
        public ActionResult Index()
        {
            

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
                    return View(db.Unloadings.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }
       
        // GET: Unloadings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unloading unloading = db.Unloadings.Find(id);
            if (unloading == null)
            {
                return HttpNotFound();
            }
            return View(unloading);
        }


        // GET: Unloadings/Num embarq
        public ActionResult NumEmb()
        {
            if (TempData["msg1"] !=null)
              {
            ViewBag.emb_alerte = "Numero d'Embarquement non existant.";
           }
            if (TempData["msg2"] != null)
            {
                ViewBag.emb_alerte = "Numero d'Embarquement deja Enregistré.";
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



        // GET: Unloadings/Create
        public ActionResult Create(string num_emb)
        {
            var num_emb_verify = ddd.Loadings.Where(b => b.numero_emb.Equals(num_emb)).FirstOrDefault();
            

            if (num_emb_verify == null )
            {
               TempData["msg1"] = 1 ;
               return RedirectToAction("NumEmb");
                
            }
            else
            {
                var num = ddd.Unloadings.Where(b => b.id_loading.Equals(num_emb_verify.id)).FirstOrDefault();

                if (num!= null)
                {
                    TempData["msg2"] = 1;
                    return RedirectToAction("NumEmb");
                }
                else
                {
                    var st = num_emb;
                    string eo = num_emb;
                    Session["kiki"] = eo;

                    TempData["id_load"] = Fonct.id_loadingWrNumEmb(st.ToString());

                    ViewBag.produitlist = Fonct.listproduit();
                    ViewBag.sizeP = Fonct.listproduit().Count();
                    ViewBag.tmpPro = 0;

                    ViewBag.sizeUnload = Fonct.ListUnload(eo).Count();
                    ViewBag.listUnload = Fonct.ListUnload(eo);

                }
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


       

        // POST: Unloadings/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numEmb,qtite_palette_retr,time_unloading_debut,time_unloading_fin,remarque,idP,caisse,bouteille")] Unloading unloading)
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
            ViewBag.sizeUnload = Fonct.ListUnload((string)Session["kiki"]).Count();
            ViewBag.listUnload = Fonct.ListUnload((string)Session["kiki"]);


            if (ModelState.IsValid)
            {
                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");


                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();

                var num = Request["numEmb"];
                int va = int.Parse(num.ToString());
                unloading.id_loading = va;

                

                var mimi = db.Loadings.Where(l => l.id.Equals(unloading.id_loading)).FirstOrDefault();


               



             //   for (int a = 0; a < caisse.Count(); a++)
              //  {
                   // Loading_produit load_p = new Loading_produit();
                    //if (!caisse[a].Equals(""))
                  //  {
                        
                        
                        //load_p.qte_caisse_retourne= int.Parse(caisse[a].ToString());
                        //load_p.qte_bout_retourne = int.Parse(bouteille[a].ToString());

                        //int id_p = int.Parse(idProduit[a].ToString());
                        

                        var query = from load_pro in db.Loading_produit
                                    select load_pro;

                        foreach (Loading_produit lo in query)
                        {
                        for (int d = 0; d < ViewBag.sizeUnload; d++)
                        {
                            if (lo.id_produit == ViewBag.listUnload[d].id_produit && lo.id_loading == mimi.numero_emb)
                            {
                                lo.qte_caisse_retourne = int.Parse(caisse[d].ToString());
                                lo.qte_bout_retourne = int.Parse(bouteille[d].ToString());
                            }
                        }
                        }
                      //  db.SaveChanges();



                        var query1 = from lign_depot in db.Depot_Produit                             
                                     select lign_depot;

                        foreach (Depot_Produit lo in query1)
                        {
                            for (int k = 0; k < ViewBag.sizeUnload; k++)
                            {
                                if (lo.id_produit == ViewBag.listUnload[k].id_produit && lo.id_depot == mimi.id_depot)
                                {
                                    lo.qtite_produit_dispo += int.Parse(caisse[k].ToString());
                                    lo.qtite_bouteille += int.Parse(bouteille[k].ToString());
                                }
                            }
                        }
                       // db.SaveChanges();



                   // }
             //   }
               
                
                db.Unloadings.Add(unloading);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");



               
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();


                ViewBag.verification = "Non enregistre";
                ViewBag.temp_caisse = caisse;
                ViewBag.temp_bout = bouteille;
            }

            return View(unloading);
        }

        // GET: Unloadings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unloading unloading = db.Unloadings.Find(id);

            if (unloading == null)
            {
                return HttpNotFound();
            }
            return View(unloading);
        }

        // POST: Unloadings/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_loading,qtite_palette_retr,time_unloading_debut,time_unloading_fin,remarque,qtite_casse")] Unloading unloading)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unloading).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unloading);
        }

        // GET: Unloadings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unloading unloading = db.Unloadings.Find(id);
            if (unloading == null)
            {
                return HttpNotFound();
            }
            return View(unloading);
        }

        // POST: Unloadings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unloading unloading = db.Unloadings.Find(id);
            db.Unloadings.Remove(unloading);
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
