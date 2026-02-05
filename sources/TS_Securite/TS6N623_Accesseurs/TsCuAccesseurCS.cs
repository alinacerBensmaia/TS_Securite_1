using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel.XU5N160_AppelAosExtern;
using TS6N627_IGererFichierChiffr;

namespace Rrq.Securite.ParametreChiffrement
{
    public class TsCuAccesseurCS : TsIAccesseur
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InscrireFichierChiffre(string strNomFichier, string strNomTypeDataSet, string strContenuFichier, bool blnChiffrementDechiffrementPermis)
        {
            if (blnChiffrementDechiffrementPermis)
            {
                string ChaineContexte = CreerContexte();
                using (XuCuAppelerAOS<TsIGererFichierChiffr> objAppelSerAOS = new XuCuAppelerAOS<TsIGererFichierChiffr>())
                {
                    // Obtenir la référence à l'inscription d'une demande de traitement
                    TsIGererFichierChiffr objetAOS = objAppelSerAOS.CreerComposantService(ref ChaineContexte);

                    // Ajouter notre demande
                    objetAOS.InscrireFichierChiffre(ref ChaineContexte, strNomFichier, strNomTypeDataSet, strContenuFichier);

                    // Vérifier le retour
                    objAppelSerAOS.AnalyserRetour(ref ChaineContexte);
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string ObtenirFichierChiffre(string strNomFichier, string strNomTypeDataSet, bool blnChiffrementDechiffrementPermis)
        {
            if (blnChiffrementDechiffrementPermis)
            {
                string ChaineContexte = CreerContexte();
                using (XuCuAppelerAOS<TsIGererFichierChiffr> objAppelSerAOS = new XuCuAppelerAOS<TsIGererFichierChiffr>())
                {
                    // Obtenir la référence à l'inscription d'une demande de traitement
                    TsIGererFichierChiffr objetAOS = objAppelSerAOS.CreerComposantService(ref ChaineContexte);

                    // Ajouter notre demande
                    string fichierChiffre = objetAOS.ObtenirFichierChiffre(ref ChaineContexte, strNomFichier, strNomTypeDataSet);

                    // Vérifier le retour
                    objAppelSerAOS.AnalyserRetour(ref ChaineContexte);

                    return fichierChiffre;
                }
            }
            else
            {
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string CreerContexte()
        {
            string contexte;
            if (XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement").Equals("PROD"))
                contexte = XuCaCreerContexte.CreerContexte(new XuDtContexte() { CellDonnees = "001", CellParametre = "001", EnvrnCics = XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement") });
            else
                contexte = XuCaCreerContexte.CreerContexte(new XuDtContexte() { PhaseCentral = "1", CellDonnees = "001", CellParametre = "001", EnvrnCics = XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement") });
            // Comme nous n'avons pas de contexte et nous voulons faire un appel à un service TS pour que la lecture du fichier de cle symgolique
            // se fasse sur le pool TS, nous devons donner un nom de composnat d'integration fictif. Par contre, il faut qu'il soit d'un système différent de TS
            // afin de provoquer le changement de pool.
            XuCaContexte.AssignerValeur(ref contexte, "CompsIntg", "XU5N152_CiLocalisateurDeService");

            XuCaContexte.AssignerValeur(ref contexte, "IdInstTrans", Guid.NewGuid().ToString());

            if (EstAppelWEBAPI())
            {
                XuCuApplicationHost.WCFHost = true;
                contexte = ObtenirContexteAppelExterne(ref contexte);
            }

            return contexte;
        }

        private bool EstAppelWEBAPI()
        {
            // Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur
            string[] strDomCodeUtil = WindowsIdentity.GetCurrent().Name.Split(@"\".ToCharArray());
            Regex regexComptePool = new Regex("SYS(INT|ACC|FOA|SIM|PRD)APAPI");
            if (regexComptePool.IsMatch(strDomCodeUtil[1]))
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string ObtenirContexteAppelExterne(ref string ChaineContexte)
        {
            return XuCuContextAosExtern.PreparerAppel(ref ChaineContexte, "XU5N152_CiLocalisateurDeService");
        }
    }
}
