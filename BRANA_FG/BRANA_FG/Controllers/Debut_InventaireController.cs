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

    public class Debut_InventaireController : Controller
    {
        private BddContext db = new BddContext();
        FonctionRequete Fonct = new FonctionRequete();
        // GET: Debut_Inventaire
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
                    return View(db.Debut_Inventaire.ToList());
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
            //return View(db.Debut_Inventaire.ToList());
        }

        // GET: Debut_Inventaire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debut_Inventaire debut_Inventaire = db.Debut_Inventaire.Find(id);
            if (debut_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(debut_Inventaire);
        }

        // GET: Debut_Inventaire/Create
        public ActionResult Create()
        {
            //ViewBag.superviseur = new SelectList(db.Utilisateurs, "id", "nom");
            //ViewBag.produit = new SelectList(db.Produits, "id", "nom");
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
               var fonction1 = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction1))
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

        // POST: Debut_Inventaire/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idP,caisse,bouteille")] Debut_Inventaire debut_Inventaire)
        {
            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"]; ;
            }
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            if (ModelState.IsValid)
              {

               

                debut_Inventaire.id_superviseur = int.Parse(Session["IdUser"].ToString());
                debut_Inventaire.date_debut_inventaire = DateTime.Now;
                Utilisateur info_superv = db.Utilisateurs.Find(debut_Inventaire.id_superviseur);


               



                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                    
                }


                if (id_depot != 0)
                {
                    debut_Inventaire.id_depot_fg = id_depot;

                    //db.Debut_Inventaire.Add(debut_Inventaire);
                    //////////////////////////////////////////////////////////////////////////
                    var i = Request.Form.GetValues("idP");
                    var c = Request.Form.GetValues("caisse");
                    var bo = Request.Form.GetValues("bouteille");



                    string[] idProduit = i.ToArray();
                    string[] caisse = c.ToArray();
                    string[] bouteille = bo.ToArray();


                    for (int a = 0; a < caisse.Count(); a++)
                    {

                        if ((!caisse[a].Equals("") && bouteille[a].Equals("")) || (caisse[a].Equals("") && !bouteille[a].Equals("")) || (!caisse[a].Equals("") && !bouteille[a].Equals("")))
                        {

                            if (caisse[a].Equals("") && !bouteille[a].Equals(""))
                            {
                                caisse[a] = "0";
                            }

                            if (!caisse[a].Equals("") && bouteille[a].Equals(""))
                            {
                                bouteille[a] = "0";
                            }


                            int v = 0, w = 0;
                            List<Produit_dispo> dispo = Fonct.Stock_dispos(info_superv.depot, int.Parse(idProduit[a].ToString()));

                            if (dispo.Count() > 0)
                            {
                                v = dispo[0].bouteille;
                                w = dispo[0].caisse;
                            }
                            
                            if( w != int.Parse(caisse[a].ToString()) &&  v != int.Parse(bouteille[a].ToString()))
                            {
                                ViewBag.InventaireError = "VEUILLEZ VERIFIER VOS DONNEES OU CONTACTEZ VOTRE ADMINISTRATEUR!";
                                return View();
                            }

                            
                        }
                    }

                    for (int a = 0; a < caisse.Count(); a++)
                    {
                        if ((!caisse[a].Equals("") && bouteille[a].Equals("")) || (caisse[a].Equals("") && !bouteille[a].Equals("")) || (!caisse[a].Equals("") && !bouteille[a].Equals("")))
                        {
                            int v = 0, w = 0;
                            List<Produit_dispo> dispo = Fonct.Stock_dispos(info_superv.depot, int.Parse(idProduit[a].ToString()));

                            if (dispo.Count() > 0)
                            {
                                v = dispo[0].bouteille;
                                w = dispo[0].caisse;
                            }
                            debut_Inventaire.caisse_theor = w;
                            debut_Inventaire.bout_theor = v;
                            debut_Inventaire.id_produit = int.Parse(idProduit[a].ToString());

                            debut_Inventaire.qtite_trouvee = int.Parse(caisse[a].ToString());
                            debut_Inventaire.qtite_bouteille = int.Parse(bouteille[a].ToString()); ;

                            db.Debut_Inventaire.Add(debut_Inventaire);

                            db.SaveChanges();

                            

                        }
                    }

                    Session["verify_Inv"] = "Valider";

                }

                return Redirect("~/Logins/AcceuilSup");
                    }
               
           
            return View(debut_Inventaire);
        }

        // GET: Debut_Inventaire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debut_Inventaire debut_Inventaire = db.Debut_Inventaire.Find(id);
            TempData["datetempon"] = debut_Inventaire.date_debut_inventaire;
            ViewBag.superviseuredit = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.produitedit = new SelectList(db.Produits, "id", "nom");
            if (debut_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(debut_Inventaire);
        }

        // POST: Debut_Inventaire/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_superviseur,id_produit,qtite_trouvee")] Debut_Inventaire debut_Inventaire)
        {
            if (ModelState.IsValid)
            {
                debut_Inventaire.date_debut_inventaire =(DateTime) TempData["datetempon"];
                debut_Inventaire.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                Utilisateur info_superv = db.Utilisateurs.Find(debut_Inventaire.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {
                    debut_Inventaire.id_depot_fg = id_depot;
                    db.Entry(debut_Inventaire).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(debut_Inventaire);
        }

        // GET: Debut_Inventaire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debut_Inventaire debut_Inventaire = db.Debut_Inventaire.Find(id);
            if (debut_Inventaire == null)
            {
                return HttpNotFound();
            }
            return View(debut_Inventaire);
        }

        // POST: Debut_Inventaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debut_Inventaire debut_Inventaire = db.Debut_Inventaire.Find(id);
            db.Debut_Inventaire.Remove(debut_Inventaire);
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
