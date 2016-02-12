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
    public class CassesController : Controller
    {
        private BddContext db = new BddContext();
        private BddContext dbb = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();

        // GET: Casses
        public ActionResult Index()
        {

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "admin_FG"; var fonction1 = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction) || Session["fonction"].ToString().Equals(fonction1))
                {
                    Session["iddataclock"] = Session["IdUser"];
                    return View(db.Casses.ToList());
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }

        // GET: Casses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse casse = db.Casses.Find(id);
            if (casse == null)
            {
                return HttpNotFound();
            }
            return View(casse);
        }

        // GET: Casses/Create
        public ActionResult Create()
        {
            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.userNomPre = Fonct.ListSuperviseurs();


            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction1 = "super_FG";
                //   && Session["verify_Inv"] != null
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

        // POST: Casses/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_depot,motif,type,endroit,injured_person,chauffeur_forklift,description,safety_material,idP,caisse,bouteille")] Casse casse)
        {
            if (ModelState.IsValid)
            {


                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");



                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();


                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeP = Fonct.listproduit().Count();


                casse.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                casse.date_casse = DateTime.Now;

                var sup = int.Parse(Session["iddataclock"].ToString());
                int va = sup;
                ViewBag.tmpSup = va;
                Utilisateur info_superv = db.Utilisateurs.Find(casse.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {

                   
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



                            var reket = from lign_depot in db.Depot_Produit
                                        select lign_depot;

                            foreach (Depot_Produit lo in reket)
                            {

                                if (lo.id_produit == int.Parse(idProduit[a].ToString()) && lo.id_depot == info_depot.id)
                                {
                                    if (lo.qtite_produit_dispo <= int.Parse(caisse[a].ToString()) || lo.qtite_bouteille <= int.Parse(bouteille[a].ToString()))
                                    {
                                        ViewBag.inferieur = "La quantite de " + ViewBag.produitlist[a].nom.ToString() + " est superieure aux quantites disponibles!";
                                        ViewBag.verification = "Non enregistre";
                                        ViewBag.temp_caisse = caisse;
                                        ViewBag.temp_bout = bouteille;
                                        return View();


                                    }


                                }

                            }

                        }

                    }

                    casse.id_depot = id_depot;
                    db.Casses.Add(casse);
                    db.SaveChanges();


                    //////////////////////////////////////////////////

                    for (int a = 0; a < caisse.Count(); a++)
                    {
                        Casse_Produit load_p = new Casse_Produit();
                        if ((!caisse[a].Equals("") && bouteille[a].Equals("")) || (caisse[a].Equals("") && !bouteille[a].Equals("")) || (!caisse[a].Equals("") && !bouteille[a].Equals("")))
                        {
                            load_p.id_produit = int.Parse(idProduit[a].ToString());
                            load_p.id_casse = casse.id;
                            
                            load_p.qtite_caisse = int.Parse(caisse[a].ToString());
                            

                            load_p.qtite_bout = int.Parse(bouteille[a].ToString()); ;
                            


                            var reket = from lign_depot in db.Depot_Produit
                                        select lign_depot;

                            foreach (Depot_Produit lo in reket)
                            {

                                if (lo.id_produit == int.Parse(idProduit[a].ToString()) && lo.id_depot == info_depot.id)
                                {

                                    lo.qtite_produit_dispo -= int.Parse(caisse[a].ToString());
                                    lo.qtite_bouteille -= int.Parse(bouteille[a].ToString());

                                }

                            }

                            dbb.Casse_Produit.Add(load_p);
                            

                        }
                        dbb.SaveChanges();
                    }




                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeP = Fonct.listproduit().Count();


                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");




                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();


                ViewBag.verification = "Non enregistre";
                ViewBag.temp_caisse = caisse;
                ViewBag.temp_bout = bouteille;


                var emb = Request["type"];

                ViewBag.tmptype = emb;

            }


            return View(casse);
        }

        // GET: Casses/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            var info_Casse1 = db.Casses.Where(b => b.id == id).FirstOrDefault();
            //  ViewBag.savanb = info_Casse1.qtite_bout;
            // ViewBag.idd = info_Casse1.id_produit;
            if (info_Casse1.type.Equals("Perte"))
            {
                //  ViewBag.savan = info_Casse1.qtite_perte;
            }

            if (info_Casse1.type.Equals("Short fill"))
            {
                //  ViewBag.savan = info_Casse1.qtite_shortfill;
            }

            if (info_Casse1.type.Equals("Casse"))
            {
                // ViewBag.savan = info_Casse1.qtite_casse;
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse casse = db.Casses.Find(id);
            if (casse == null)
            {
                return HttpNotFound();
            }
            return View(casse);
        }

        // POST: Casses/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_produit,qtite_casse,id_depot,motif,type,qtite_bout,idP,caisse,bouteille")] Casse casse)
        {
            //if (ModelState.IsValid)
            //{
            //    casse.id_superviseur= int.Parse(Session["iddataclock"].ToString());
            //    casse.date_casse = DateTime.Now;

            //    var sup = int.Parse(Session["iddataclock"].ToString());
            //    int va = int.Parse(sup.ToString());
            //    ViewBag.tmpSup = va;
            //    Utilisateur info_superv = db.Utilisateurs.Find(casse.id_superviseur);

            //    var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
            //    var id_depot = 0;
            //    if (info_depot != null)
            //    {
            //        id_depot = info_depot.id;
            //    }

            //    if (id_depot != 0)
            //    {

            //        casse.id_depot = id_depot;
            //        db.Entry(casse).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //    return RedirectToAction("Index");
            //}


            if (ModelState.IsValid)
            {
                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");



                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();


                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeP = Fonct.listproduit().Count();


                casse.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                casse.date_casse = DateTime.Now;

                var sup = int.Parse(Session["iddataclock"].ToString());
                int va = sup;
                ViewBag.tmpSup = va;
                Utilisateur info_superv = db.Utilisateurs.Find(casse.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {
                    //casse.id_depot = id_depot;
                    //db.Casses.Add(casse);
                    //db.SaveChanges();


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



                            var reket = from lign_depot in db.Depot_Produit
                                        select lign_depot;

                            foreach (Depot_Produit lo in reket)
                            {

                                if (lo.id_produit == int.Parse(idProduit[a].ToString()) && lo.id_depot == info_depot.id)
                                {
                                    if (lo.qtite_produit_dispo <= int.Parse(caisse[a].ToString()) || lo.qtite_bouteille <= int.Parse(bouteille[a].ToString()))
                                    {
                                        ViewBag.inferieur = "La quantite de " + ViewBag.produitlist[a].nom.ToString() + " est superieure aux quantites disponibles!";
                                        ViewBag.verification = "Non enregistre";
                                        ViewBag.temp_caisse = caisse;
                                        ViewBag.temp_bout = bouteille;
                                        return View();


                                    }


                                }

                            }

                        }

                    }
                    //casse.id_depot = id_depot;                    
                    //////////////////////////////////////////////////

                    for (int a = 0; a < caisse.Count(); a++)
                    {
                        Casse load_p = new Casse();
                        if ((!caisse[a].Equals("") && bouteille[a].Equals("")) || (caisse[a].Equals("") && !bouteille[a].Equals("")) || (!caisse[a].Equals("") && !bouteille[a].Equals("")))
                        {
                            //   / load_p.id_produit = int.Parse(idProduit[a].ToString());

                            load_p.id_superviseur = casse.id_superviseur;
                            load_p.type = casse.type;

                            // if (load_p.type.Equals("Casse"))
                            //   load_p.qtite_casse = int.Parse(caisse[a].ToString());
                            //else if (load_p.type.Equals("Perte"))
                            //    load_p.qtite_perte = int.Parse(caisse[a].ToString());
                            //else if (load_p.type.Equals("Short fill"))
                            //   load_p.qtite_shortfill = int.Parse(caisse[a].ToString());

                            //  load_p.qtite_bout = int.Parse(bouteille[a].ToString()); ;
                            load_p.id_depot = id_depot;
                            load_p.motif = casse.motif;
                            load_p.date_casse = casse.date_casse;


                            var reket = from lign_depot in db.Depot_Produit
                                        select lign_depot;

                            foreach (Depot_Produit lo in reket)
                            {

                                if (lo.id_produit == int.Parse(idProduit[a].ToString()) && lo.id_depot == info_depot.id)
                                {
                                    var info_Casse = db.Casses.Where(b => b.id_depot.Equals(int.Parse(info_depot.id.ToString()))); //&& b.id_produit.Equals(int.Parse(idProduit[a].ToString()))).FirstOrDefault();
                                                                                                                                   //  if (info_Casse.type.Equals("Casse"))
                                    {
                                        //  if (info_Casse.qtite_casse != int.Parse(caisse[a].ToString()))
                                        {
                                            //     lo.qtite_produit_dispo += (int.Parse(caisse[a].ToString()) - info_Casse.qtite_casse);

                                        }

                                        //  if (info_Casse.qtite_bout != int.Parse(bouteille[a].ToString()))
                                        {

                                            //     lo.qtite_bouteille += (int.Parse(bouteille[a].ToString()) - info_Casse.qtite_bout);
                                        }
                                    }

                                    //   if (info_Casse.type.Equals("Perte"))
                                    {
                                        //  if (info_Casse.qtite_perte != int.Parse(caisse[a].ToString()))
                                        {
                                            // lo.qtite_produit_dispo += (int.Parse(caisse[a].ToString()) - info_Casse.qtite_perte);

                                        }

                                        // if (info_Casse.qtite_bout != int.Parse(bouteille[a].ToString()))
                                        {

                                            // lo.qtite_bouteille += (int.Parse(bouteille[a].ToString()) - info_Casse.qtite_bout);
                                        }
                                    }


                                    // if (info_Casse.type.Equals("Short fill"))
                                    {
                                        //  if (info_Casse.qtite_shortfill != int.Parse(caisse[a].ToString()))
                                        {
                                            // lo.qtite_produit_dispo += (int.Parse(caisse[a].ToString()) - info_Casse.qtite_shortfill);

                                        }

                                        // if (info_Casse.qtite_bout != int.Parse(bouteille[a].ToString()))
                                        {

                                            //   lo.qtite_bouteille += (int.Parse(bouteille[a].ToString()) - info_Casse.qtite_bout);
                                        }
                                    }

                                }

                            }


                            db.Entry(load_p).State = EntityState.Modified;

                        }
                        db.SaveChanges();
                    }




                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.produitlist = Fonct.listproduit();
                ViewBag.sizeP = Fonct.listproduit().Count();


                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");




                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();


                ViewBag.verification = "Non enregistre";
                ViewBag.temp_caisse = caisse;
                ViewBag.temp_bout = bouteille;


                var emb = Request["type"];

                ViewBag.tmptype = emb;

            }



            return View(casse);
        }

        // GET: Casses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Casse casse = db.Casses.Find(id);
            if (casse == null)
            {
                return HttpNotFound();
            }
            return View(casse);
        }

        // POST: Casses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Casse casse = db.Casses.Find(id);
            db.Casses.Remove(casse);
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
