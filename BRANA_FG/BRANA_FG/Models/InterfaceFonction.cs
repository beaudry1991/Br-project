using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRANA_FG.Models
{
    interface InterfaceFonction
    {
        /*change Password*/
        void UpdatingPassword(string mail, string newpassword);
        String nomEmballage(int id);
        List<SuperviseurLIst> ListSuperviseurs();
        List<ProduitList> listproduit();
        List<ChauffeurList> ListChauffeur();
        List<VehiculeList> ListVehicule();
        List<LigneProductionList> ListLigneProduction();
        List<ListDepots> ListDepot();
        List<ListRetour> ListRetour(int idtransfert);
        List<ListUnload> ListUnload(string idloading);
        int id_loadingWrNumEmb(string numEmb);
        int idtransfert_numTrans(String numTrans);
        int qtite_dispo_ligne_depot(int produit, int depot);
        List<ListSomme> ListTotal(int id, DateTime date, int superv);
        List<ListSommeTransf> ListTotalTransf(int id, DateTime date, int superv);
        List<ListCasses> listCaPeSh(int id, DateTime date, int superv);
        List<Produit_dispo> ProduitDispos();
        List<Produit_dispo> Stock_dispos(string iddep, int idprod);
        List<Emballage> listEmballage();
        List<Categorie> listCategorie();
    }
}
