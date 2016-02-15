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
    public class LoadingsController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();

        // GET: Loadings
        public ActionResult Index()
        {
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizePro = Fonct.listproduit().Count();
            ViewBag.tmpPro = 0;


            if (Request["date1"] != null || Request["date1"] == "")
            {
                ViewBag.date1 = Request["date1"];
                ViewBag.date2 = Request["date2"];


                string a = Request["date1"];
                string b = Request["date2"];
       

                ViewBag.Loading_count = Loading(a, b).Count();
                ViewBag.Loading = new Func<string, string, List<ListLoading>>(Loading);

            }
            else
            {
                ViewBag.Loading_count = 0;
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
                    Session["iddataclock"] = Session["IdUser"];
                    return View(db.Loadings.ToList());
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
            //return View(db.Loadings.ToList());
        }
       

        // GET: Loadings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading loading = db.Loadings.Find(id);
            if (loading == null)
            {
                return HttpNotFound();
            }
            return View(loading);
        }

        // GET: Loadings/Create
        public ActionResult Create()
        {
            
            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.userNomPre = Fonct.ListSuperviseurs();

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            ViewBag.chauffeurlist = Fonct.ListChauffeur();
            ViewBag.sizeC = Fonct.ListChauffeur().Count();

            ViewBag.vehiculelist = Fonct.ListVehicule();
            ViewBag.sizeV= Fonct.ListVehicule().Count();

            ViewBag.tmpSup = 0;
            ViewBag.tmpChauf = 0;
            ViewBag.tmpVeh = 0;


            ViewBag.produitsLoad = null;
            
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

        // POST: Loadings/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_superviseur,id_chauffeur,client,id_vehicule,numero_sp,numero_emb,nbre_palette,type_emb,heure_debut,heure_fin,remarque,destination,idP,caisse,bouteille")] Loading loading)
        {

            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.userNomPre = Fonct.ListSuperviseurs();
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
            ViewBag.chauffeurlist = Fonct.ListChauffeur();
            ViewBag.sizeC = Fonct.ListChauffeur().Count();
            ViewBag.vehiculelist = Fonct.ListVehicule();
            ViewBag.sizeV = Fonct.ListVehicule().Count();
            
          

            if (Session["iddataclock"] == null)
            {
                Session["iddataclock"] = Session["IdUser"];
            }

            if (ModelState.IsValid)
            {
                var i = Request.Form.GetValues("idP");
                var c = Request.Form.GetValues("caisse");
                var bo = Request.Form.GetValues("bouteille");

                

                string[] idProduit = i.ToArray();
                string[] caisse = c.ToArray();
                string[] bouteille = bo.ToArray();
                var sp_verify = db.Loadings.Where(a => a.numero_sp.Equals(loading.numero_sp)).FirstOrDefault();
                var emb_verify = db.Loadings.Where(a => a.numero_emb.Equals(loading.numero_emb)).FirstOrDefault();
                if (sp_verify != null && emb_verify != null)
                {
                    //reste a ajoute un message d'erreur
                    ViewBag.errorsp = "Le #SP et le #Embarquement existent deja!";
                    
                    return View(loading);
                }else if (sp_verify != null)
                {
                    ViewBag.errorsp = "Le #SP existe deja!";

                    return View(loading);
                } else if (emb_verify != null)
                {
                    //reste a ajoute un message d'erreur
                    ViewBag.errorsp = "Le # Embarquement existe deja!";

                    return View(loading);
                }



                loading.id_data_clock = int.Parse(Session["iddataclock"].ToString());
                
                loading.date_loading = DateTime.Now;
                loading.retour = 0;
                var sup = Request["id_superviseur"];
                int va = int.Parse(sup.ToString());
                loading.id_superviseur = va;
                ViewBag.tmpSup = va;
                
                Utilisateur info_superv = db.Utilisateurs.Find(loading.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                for (int a = 0; a < caisse.Count(); a++)
                {
                    Loading_produit load_p = new Loading_produit();
                    if (!caisse[a].Equals(""))
                    {
                        load_p.id_produit = int.Parse(idProduit[a].ToString());
                        load_p.id_loading = loading.numero_emb;
                        load_p.qte_caisse_delivre = int.Parse(caisse[a].ToString());
                        load_p.qte_bouteille_delivre = int.Parse(bouteille[a].ToString()); ;
                        load_p.qte_caisse_retourne = 0;
                        load_p.qte_bout_retourne = 0;

                        db.Loading_produit.Add(load_p);

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
                    }

                }


                if (id_depot != 0)
                {
                    
                    loading.id_depot = id_depot;
                    db.Loadings.Add(loading);
                    
                    db.SaveChanges();
                   


                }
                
                return RedirectToAction("Index");
                
            }
            else
            {
                var sup = Request["id_superviseur"];
                int va = int.Parse(sup.ToString());
                ViewBag.tmpSup = va;

                var chauf = Request["id_chauffeur"];
                int vaC = int.Parse(chauf.ToString());
                ViewBag.tmpChauf = vaC;


                var veh = Request["id_chauffeur"];
                int vaV = int.Parse(veh.ToString());
                ViewBag.tmpVeh = vaV;
            }

            return View(loading);
        }

        // GET: Loadings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading loading = db.Loadings.Find(id);
            ///////////////////////
            TempData["datetempon"] = loading.date_loading;
            ViewBag.superviseuredit = new SelectList(db.Utilisateurs, "id", "nom");
            ViewBag.chauffeuredit = new SelectList(db.Chauffeurs, "id", "nom");
            ViewBag.vehiculeedit = new SelectList(db.Vehicules, "id", "plaque");


            ViewBag.size = Fonct.ListSuperviseurs().Count();
            ViewBag.userNomPre = Fonct.ListSuperviseurs();

            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();

            ViewBag.chauffeurlist = Fonct.ListChauffeur();
            ViewBag.sizeC = Fonct.ListChauffeur().Count();

            ViewBag.vehiculelist = Fonct.ListVehicule();
            ViewBag.sizeV = Fonct.ListVehicule().Count();

            /////////////////////////////////////////
            if (loading == null)
            {
                return HttpNotFound();
            }
            return View(loading);
        }

        // POST: Loadings/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_superviseur,id_chauffeur,client,id_vehicule,numero_sp,numero_emb,nbre_palette,type_emb,heure_debut,heure_fin,qtite_casse,remarque,destination")] Loading loading)
        {
            if (ModelState.IsValid)
            {
                loading.date_loading= (DateTime)TempData["datetempon"];
                loading.id_data_clock = int.Parse(Session["iddataclock"].ToString());
                loading.retour = 0;

                Utilisateur info_superv = db.Utilisateurs.Find(loading.id_superviseur);

                var info_depot = db.Depots.Where(b => b.nom.Equals(info_superv.depot)).FirstOrDefault();
                var id_depot = 0;
                if (info_depot != null)
                {
                    id_depot = info_depot.id;
                }

                if (id_depot != 0)
                {
                    loading.id_depot= id_depot;
                    db.Entry(loading).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(loading);
        }

        // GET: Loadings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loading loading = db.Loadings.Find(id);
            if (loading == null)
            {
                return HttpNotFound();
            }
            return View(loading);
        }

        // POST: Loadings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loading loading = db.Loadings.Find(id);
            db.Loadings.Remove(loading);
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


        public List<ListLoading> Loading(string c, string b)
        {
            List<ListLoading> PC = new List<ListLoading>();

            ListLoading l_d = new ListLoading();
            DateTime date1 = Convert.ToDateTime(c);
            DateTime date2 = Convert.ToDateTime(b);

            var load = from ld in db.Loadings
                            where (ld.heure_debut.Day >= date1.Day && ld.heure_debut.Month >= date1.Month && ld.heure_debut.Year >= date1.Year && ld.heure_debut.Day <= date2.Day && ld.heure_debut.Month <= date2.Month && ld.heure_debut.Year <= date2.Year)
                               join chauff in db.Chauffeurs on ld.id_chauffeur equals chauff.id
                               join dep in db.Depots on ld.id_depot equals dep.id
                               join veh in db.Vehicules on ld.id_vehicule equals veh.id
                               join sup in db.Utilisateurs on ld.id_superviseur equals sup.id
                            select new
                            {
                               ld, chauff, dep, veh, sup
                                

                            };



            if (load.Count() != 0)
            { 
                foreach (var a in load)
            {

                PC.Add(new ListLoading()
                {
                    id = a.ld.id,
                    heure_debut  = a.ld.heure_debut,
                    heure_fin    = a.ld.heure_fin,
                    numero_sp    = a.ld.numero_sp,
                    numero_emb   = a.ld.numero_emb,
                    nbre_palette = a.ld.nbre_palette,
                    chauffeur    = a.chauff.nom + " " + a.chauff.prenom,
                    vehicule     = a.veh.numero,
                    superviseur  = a.sup.nom + "" + a.sup.prenom,
                    client = "",
                    destination = "",
                    data_clock ="",
                    depot =a.dep.nom,
                    date_loading =date1,
                    remarque ="",
                    retour =0,
                    type_emb = ""
                    
                }  
                    );

            }
            }
            //if (PC.Count() == 0)
            //{
            //    PC.Add(new ListLoading()
            //    {
            //        id = 0,
            //        heure_debut = date1,
            //        heure_fin = date2,
            //        chauffeur = "",
            //        vehicule = "",
            //        numero_sp = "",
            //        numero_emb = "",
            //        superviseur = "",
            //        nbre_palette = 0
            //    });
            //}
            return PC;
        }

    }

}
