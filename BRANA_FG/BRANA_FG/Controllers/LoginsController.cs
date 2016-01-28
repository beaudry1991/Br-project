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
    public class LoginsController : Controller
    {
        private BddContext db = new BddContext();
        public FonctionRequete Fonct = new FonctionRequete();

        // GET: Logins
        public ActionResult Index()
        {
            return View();
           // return View(db.Logins.ToList());
        }


        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Logins");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "mail,password")] Login log)
        {

            if (ModelState.IsValid)
            {
                /* compare the provide mail and password with the database info*/
                var pasword = log.password;
                var logged = db.Logins.Where(lg => lg.mail.Equals(log.mail) && lg.password.Equals(pasword)).FirstOrDefault();
                if (logged != null)
                {
                    Utilisateur user = db.Utilisateurs.Find(logged.idUser);
                    if (user != null)
                    {
                        Session["nom"] = user.nom.ToString();
                        Session["prenom"] = user.prenom.ToString();
                        Session["idUser"] = logged.idUser;
                        Session["fonction"] = user.fonction.ToString();
                        
                        /*Admin Loggin test*/
                        var ad = "admin_FG";
                        var sup = "super_FG";
                        var data = "data_FG";
                        var audit = "audit_FG";
                       // var empty = "Superviseur Empty";
                        if (user.fonction.ToString().Equals(ad))
                        {  /*Change initial Password*/
                            var initialPassword = "Password";
                            if (logged.password.Equals(initialPassword))
                            {
                                return RedirectToAction("ModifyPasswrd", "Logins");
                            }
                            return RedirectToAction("Acceuil", "Logins");
                        }
                        else if (user.fonction.ToString().Equals(sup))
                        {
                            var initialPassword = "Password";
                            if (logged.password.Equals(initialPassword))
                            {
                                return RedirectToAction("ModifyPasswrd", "Logins");
                            }
                            return RedirectToAction("AcceuilSup", "Logins");

                        }
                        else if (user.fonction.ToString().Equals(data))
                        {
                            var initialPassword = "Password";
                            if (logged.password.Equals(initialPassword))
                            {
                                return RedirectToAction("ModifyPasswrd", "Logins");
                            }
                            return RedirectToAction("AcceuilData", "Logins");

                        }
                        else if (user.fonction.ToString().Equals(audit))
                        {
                            var initialPassword = "Password";
                            if (logged.password.Equals(initialPassword))
                            {
                                return RedirectToAction("ModifyPasswrd", "Logins");
                            }
                            return RedirectToAction("AcceuilAud", "Logins");

                        }

                    }
                    else
                    {
                        ViewBag.message = "Cet utilisateur n'existe pas!";
                        return View(log);
                    }

                }
                else
                {
                    ViewBag.message = "E-mail ou Mot de passe est incorrect, verifiez-les!";
                    return View(log);
                }

            }
            return View();
        }



        public ActionResult ModifyPasswrd()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyPasswrd(string ancienpassword, string nouveaupassword, string confirmpassword)
        {   /*User Modifying pass word*/
            var pass = ancienpassword;
            var newpass = nouveaupassword;
            var user = db.Logins.Where(lg => lg.password.Equals(pass)).FirstOrDefault();
            if (user == null)
            {
                ViewBag.message = "L'ancien mot de passe est incorect!" + pass;
                return View();
            }
            if (nouveaupassword.Equals(confirmpassword))
            {
                if (newpass.Equals(pass))
                {
                    ViewBag.message = "Veuillez saisir un mot de passe different a celui d'anterieur!";
                    return View();
                }
                // modifiying password
                InterfaceFonction dal = new FonctionRequete(); // Instance of DAL
                dal.UpdatingPassword(user.mail, newpass);
            }
            return View();
        }

        public ActionResult Acceuil()
        {   /*Connection and Authorization required*/
            //gestion des messages

            //List<Produit_dispo> dispo = new List<Produit_dispo>();

            //for(int i =0; i< Fonct.listproduit().Count(); i++)
            //{

            //}
            
            ViewBag.produitlist = Fonct.listproduit();
            ViewBag.sizeP = Fonct.listproduit().Count();
            ViewBag.listproddispo = Fonct.ProduitDispos();

            //ViewBag.listprod_tot = Fonct.ProduitDispos().Count();

            if (Session.Keys.Count == 0)
            {
            //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "admin_FG";
              
                if (Session["fonction"].ToString().Equals(fonction) )
                {
                    //var i = Request.Form.GetValues("idP");
                    // string[] idProduit = i.ToArray();
                    // for (int a = 0; a < ViewBag.sizeP; a++)
                    // {
                    //  int w;
                    
                    // }

                    return View();

                }
                else
                {
                   /*No ADMIN connection*/
                   return Redirect("~/Logins/Index");
                }

           }
        }
        
        public ActionResult AcceuilData()
        {   /*Connection and Authorization required*/
            //gestion des messages
            // ViewBag.archivre = db.Archivres.SqlQuery("SELECT * FROM Archivres").ToList();
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "data_FG";

                if (Session["fonction"].ToString().Equals(fonction))
                {
                   
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }


        public ActionResult AcceuilSup()
        {   /*Connection and Authorization required*/
            //gestion des messages
            // ViewBag.archivre = db.Archivres.SqlQuery("SELECT * FROM Archivres").ToList();
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
                    int id_user =(int) Session["idUser"];
                   
                    Debut_Inventaire debut = db.Debut_Inventaire.Where(v => ( v.date_debut_inventaire.Year.Equals(DateTime.Now.Year)&&  v.date_debut_inventaire.Month.Equals(DateTime.Now.Month)&& v.date_debut_inventaire.Day.Equals(DateTime.Now.Day)) && v.id_superviseur.Equals(id_user) ).FirstOrDefault();

                    if ( debut != null ) {
                        ViewBag.produitlist = Fonct.listproduit();
                        ViewBag.sizeP = Fonct.listproduit().Count();
                        ViewBag.listproddispo = Fonct.ProduitDispos();
                      






                        return View();
                    }
                    else
                    {
                        return Redirect("~/Debut_Inventaire/Create");
                    }
                   
                    
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }




        public ActionResult AcceuilAud()
        {   /*Connection and Authorization required*/
            //gestion des messages
            // ViewBag.archivre = db.Archivres.SqlQuery("SELECT * FROM Archivres").ToList();
            if (Session.Keys.Count == 0)
            {
                //    /*No connection*/
                return Redirect("~/Logins/Index");
            }
            else
            {
                var fonction = "audit_FG";

                if (Session["fonction"].ToString().Equals(fonction))
                {
                   
                    return View();
                }
                else
                {
                    /*No ADMIN connection*/
                    return Redirect("~/Logins/Index");
                }

            }
        }


    }
}
