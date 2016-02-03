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
    public class TransfertsController : Controller
    {
        private BddContext db = new BddContext();
        private BddContext ddd = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();
        // GET: Transferts
        public ActionResult Index()
        {

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
                    return View(db.Transferts.ToList());
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
            //return View(db.Transferts.ToList());
        }

        // GET: Transferts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert transfert = db.Transferts.Find(id);
            if (transfert == null)
            {
                return HttpNotFound();
            }
            return View(transfert);
        }
        // GET: NUmero transfert
        public ActionResult NumTransfert()
        {
            if (TempData["msg1"] != null)
            {
                ViewBag.transf_alerte = "Numero Transfert non existant.";
            }

            if (TempData["msg2"] != null)
            {
                ViewBag.transf_alerte = "Ce Tranfert n'est pas destiné a votre dépot.";
            }
            if (TempData["msg3"] != null)
            {
                ViewBag.transf_alerte = "Ce Transfert a été deja enregistré.";
            }


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                 var fonction1 = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction1) && Session["verify_Inv"] != null)
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
        // GET: Tranferts/Reception

        public ActionResult Reception(string num_transfert)
        {

            var num_trans_verify = db.Transferts.Where(a => a.num_transfert.Equals(num_transfert)).FirstOrDefault();

            Utilisateur infosup = db.Utilisateurs.Find(Session["idUser"]);

            var infdepot = db.Depots.Where(b => b.nom.Equals(infosup.depot)).FirstOrDefault();


            //var depot_verify = db.Transferts.Where(b => b.id_depot_source.Equals(infdepot.id)).FirstOrDefault();

            //int ii = num_trans_verify.id_depot_dest;

            if (num_trans_verify == null)
            {
                TempData["msg1"] = 1;
                return RedirectToAction("NumTransfert");
            }
            else if (!(infdepot.id.Equals(num_trans_verify.id_depot_dest)))
            {
                TempData["msg2"] = 1;
                return RedirectToAction("NumTransfert");

            }
            else if(num_trans_verify.process != 0)
            {
                TempData["msg3"] = 1;
                return RedirectToAction("NumTransfert");
            }



            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.superviseur = Fonct.ListSuperviseurs();

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            var st = num_transfert;
            int id_transfert = Fonct.idtransfert_numTrans(st.ToString());

            ViewBag.sizeRetour = Fonct.ListRetour(id_transfert).Count();
            ViewBag.listretour = Fonct.ListRetour(id_transfert);


            

          
                ViewBag.idtrans = id_transfert;
                
              
     
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "super_FG"; 

                if (Session["fonction"].ToString().Equals(fonction) && Session["verify_Inv"] != null)
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

        // GET: Transferts/Create
        public ActionResult Create()
        {

            
            

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            ViewBag.chauffeurlist = Fonct.ListChauffeur();
            ViewBag.sizeC = Fonct.ListChauffeur().Count();

            ViewBag.vehiculelist = Fonct.ListVehicule();
            ViewBag.sizeV = Fonct.ListVehicule().Count();

            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();


            ViewBag.tmpPro= 0;
            ViewBag.tmpSup = 0;
            ViewBag.tmpChauf = 0;
            ViewBag.tmpVeh = 0;
            ViewBag.tmpdepot = 0;


            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
               var fonction1 = "super_FG";

                if (Session["fonction"].ToString().Equals(fonction1) && Session["verify_Inv"] != null)
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


        // POST: Transferts/Reception
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reception([Bind(Include = "numTrans,quantite_palette,idP,caisse,bouteille")]Transfert transfert)
        {
            var i = Request.Form.GetValues("idP");
            var c = Request.Form.GetValues("caisse");
            var bo = Request.Form.GetValues("bouteille");



            string[] idProduit = i.ToArray();
            string[] caisse = c.ToArray();
            string[] bouteille = bo.ToArray();

            var num = Request["numTrans"];
            int va = int.Parse(num.ToString());


            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.superviseur = Fonct.ListSuperviseurs();
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
           
            Utilisateur info_superv1 = db.Utilisateurs.Find(Session["idUser"]);

            var info_depot1 = db.Depots.Where(b => b.nom.Equals(info_superv1.depot)).FirstOrDefault();



            
            ViewBag.sizeRetour = Fonct.ListRetour(va).Count();
            ViewBag.listretour = Fonct.ListRetour(va);
            int compteur = 0;
            for (int a = 0; a < ViewBag.sizeRetour; a++)
            {
                for (int b = 0; b < ViewBag.sizeRetour; b++)
                if (ViewBag.listretour[a].id_produit == int.Parse(idProduit[b].ToString()) && ViewBag.listretour[a].qtite_caisse == int.Parse(caisse[b].ToString()) && ViewBag.listretour[a].qtite_bout == int.Parse(bouteille[b].ToString()))
                {
                        compteur++;
                }

            }

           if (compteur == ViewBag.sizeRetour)
            {
                var query = from retr in db.Transferts
                            where retr.id == va
                            select retr;

                foreach (Transfert lo in query)
                {
               
                    lo.process = 1;
                    lo.id_superviseur_recu = int.Parse(Session["idUser"].ToString());
            
               }
                //  db.SaveChanges();
                //  Depot_Produit value = new Depot_Produit();




                var reket = from lign_depot in db.Depot_Produit
                            select lign_depot;

                foreach (Depot_Produit lo in reket)
                {

                    for (int k = 0; k < ViewBag.sizeRetour; k++)
                    {
                        if (lo.id_produit == ViewBag.listretour[k].id_produit && lo.id_depot == info_depot1.id)
                        {
                            lo.qtite_produit_dispo += int.Parse(caisse[k].ToString());
                            lo.qtite_bouteille += int.Parse(bouteille[k].ToString());

                        }
                    }

                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.messagealerte = "VERIFIEZ VOS INFORMATOINS";
            }



            return View();
        }





        // POST: Transferts/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,num_transfert,id_depot_dest,quantite_palette,id_chauffeur,id_vehicule,idP,caisse,bouteille")] Transfert transfert)
        {

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            ViewBag.chauffeurlist = Fonct.ListChauffeur();
            ViewBag.sizeC = Fonct.ListChauffeur().Count();

            ViewBag.vehiculelist = Fonct.ListVehicule();
            ViewBag.sizeV = Fonct.ListVehicule().Count();

            ViewBag.sizeDP = Fonct.ListDepot().Count();
            ViewBag.listdep = Fonct.ListDepot();


            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"];
            }
            

            if (ModelState.IsValid)
            {

                var tansf_verify = ddd.Transferts.Where(a => a.num_transfert.Equals(transfert.num_transfert)).FirstOrDefault();
                if (tansf_verify != null)
                {
                    ViewBag.alert = "Le numero de transfert existe deja";
                    return View(transfert);
                }
                var sup = (int) Session["iddataclock"];
                int va = int.Parse(sup.ToString());

                Utilisateur info_superv = db.Utilisateurs.Find(va);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();

                transfert.id_depot_source = info_depot.id;



                
                transfert.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                transfert.date_transfert = DateTime.Now;
                db.Transferts.Add(transfert);
                db.SaveChanges();

              
                db.SaveChanges();



                //////////////////////////////////////////////////////////////////////////
                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");



                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();
                ////////////////////////////////////////////////


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
                                    ViewBag.inferieurT = "La quantite de " + ViewBag.produitlist[a].nom.ToString() + " est superieure aux quantites disponibles!";
                                    return View();


                                }


                            }

                        }

                    }

                }

                ///////////////////////////////////////////////

                for (int a = 0; a < caisse.Count(); a++)
                {
                    Transfert_Produit load_p = new Transfert_Produit();
                    if ((!caisse[a].Equals("") && bouteille[a].Equals("")) || (caisse[a].Equals("") && !bouteille[a].Equals("")) || (!caisse[a].Equals("") && !bouteille[a].Equals("")))
                    {
                        load_p.id_produit = int.Parse(idProduit[a].ToString());
                      
                        load_p.qtite_caisse = int.Parse(caisse[a].ToString());
                        load_p.qtite_bout = int.Parse(bouteille[a].ToString()); ;
                        
                        load_p.id_transfert = transfert.id;

                        ddd.Transfert_Produit.Add(load_p);




                        var reket = from lign_depot in ddd.Depot_Produit
                                    select lign_depot;

                        foreach (Depot_Produit lo in reket)
                        {

                          
                                if (lo.id_produit == int.Parse(idProduit[a].ToString()) && lo.id_depot == info_depot.id)
                                {
                                    lo.qtite_produit_dispo -= int.Parse(caisse[a].ToString());
                                    lo.qtite_bouteille -= int.Parse(bouteille[a].ToString());
                                
                                }

                        
                        }



                        ddd.SaveChanges();
                    }
                }

               

                /////////////////////////////////////////////////////////////////////////


                return RedirectToAction("Index");
            }

            return View(transfert);
        }

        // GET: Transferts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert transfert = db.Transferts.Find(id);
            TempData["datetempon"] = transfert.date_transfert;
            ViewBag.superviseuredit = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.produitedit = new SelectList(db.Produits, "id", "nom");
            ViewBag.depotedit = new SelectList(db.Depots, "id", "nom");
            ViewBag.chauffeuredit = new SelectList(db.Chauffeurs, "id", "nom");
            ViewBag.vehiculeedit = new SelectList(db.Vehicules, "id", "plaque");
            if (transfert == null)
            {
                return HttpNotFound();
            }

            //TempData["tempQtite"] = transfert.quantite_caisse;

            return View(transfert);
        }

        // POST: Transferts/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_depot_source,id_depot_dest,quantite_palette,id_chauffeur,id_vehicule,num_transfert")] Transfert transfert)
        {
            if (ModelState.IsValid)
            {
                transfert.date_transfert = (DateTime)TempData["datetempon"];
                transfert.id_superviseur = int.Parse(Session["iddataclock"].ToString());
                db.Entry(transfert).State = EntityState.Modified;
                db.SaveChanges();


                //var query = from lign_depot in db.Depot_Produit
                //            where lign_depot.id_produit == transfert.id_produit && (lign_depot.id_depot == transfert.id_depot_dest || lign_depot.id_depot == transfert.id_depot_source)
                //            select lign_depot;

                //foreach (Depot_Produit lo in query)
               // {
                    //if (transfert.id_depot_dest == lo.id_depot)
                    //{
                    //    lo.qtite_produit_dispo += transfert.quantite_caisse;
                    //}
                    //else if (transfert.id_depot_source == lo.id_depot)
                    //{
                    //    lo.qtite_produit_dispo -= transfert.quantite_caisse;
                    //}

               // }
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            return View(transfert);
        }

        // GET: Transferts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfert transfert = db.Transferts.Find(id);
            if (transfert == null)
            {
                return HttpNotFound();
            }
            return View(transfert);
        }

        // POST: Transferts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transfert transfert = db.Transferts.Find(id);
            db.Transferts.Remove(transfert);
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
