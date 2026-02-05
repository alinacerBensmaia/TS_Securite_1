using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureLotPFI.GestionFichier;
using Rrq.InfrastructureLotPFI.ScenarioTransactionnel;
using System;
using System.Collections.Generic;
using TS7N411_CiExtInfosSecrtNavgt;
using TS7N413_DdInfosSecrtNavgt;

namespace TS7N411_CiExtAccesNavEtMenus
{
    ///-----------------------------------------------------------------------------
    /// Project		: TS7N411_CiExtInfosSecrtNavgt
    /// Class		: xxCaControleur
    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Classe de point d'entrée pour la fonction. 
    /// </summary>
    ///-----------------------------------------------------------------------------
    public class TsCaControleur : XdCaTravail
    {

        #region --- Variables ---

        private TsCdAccesRessourcesInfosNavgt mUADNavigateurs;
        private TsDtNavigateurs mNavigateurCourant;
        private bool mFinNavigateur = false;
        private XdIEnregEcrivain<TsEngrNavgt> mEcrivainInfoSecNavgt;
        private string mEnvirRess;

        #endregion

        #region --- Propriétés ---

        /// <summary>
        /// Indique si le traitement est en lecture seul avec la BD
        /// </summary>
        protected override bool EstLectureSeul => true;

        /// <summary>
        /// Indique si le traitement effectue de la synchronisation
        /// </summary>
        protected override bool EstSynchronise => false;

        #endregion

        #region --- Méthodes protected ---

        /// <summary>
        /// Permet d'exécuter les travaux préparatoire du traitement 
        /// tel que l'ouverture des fichiers
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void DebuterTravail(ref string ChaineContexte)
        {

            DemarrageCommun(ref ChaineContexte, false);

        }

        /// <summary>
        /// C'est ici que le traitement se fait entre chaque point de synchro
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void ExecuterBlocTravail(ref string ChaineContexte)
        {

            while (!(mFinNavigateur))
            {

                // Mettre à jour l'identifiant courant.
                IdentifiantCourant =    mNavigateurCourant.Envir + "/" + mNavigateurCourant.ModlSecrt + "/" + mNavigateurCourant.CodProfl 
                                + "/" + mNavigateurCourant.CodFonct + "/" + mNavigateurCourant.CodFonct + "/" + mNavigateurCourant.CodNivSecrt;
                EcrireNavigateur();
                mNavigateurCourant = ObtenirProchainNavigateur();

            }

            SignalerFinTravail();
     
        }

        /// <summary>
        /// Permet la reprise du traitement après une fin anormale
        /// faire le traitement de récupération des données de reprise et repositionnement du traitement
        /// </summary>
        /// <param name="ChaineContexte"></param>
        /// <param name="DonnsReprs"></param>
        protected override void RepriseTravail(ref string ChaineContexte, System.Collections.Generic.Dictionary<String, String> DonnsReprs)
        {
           DemarrageCommun(ref ChaineContexte,true);
        }

        /// <summary>
        /// Permet d'effectuer les opérations de terminaison du travail
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void TerminerTravail(ref string ChaineContexte)
        {
            mEcrivainInfoSecNavgt.Fermer();
        }

        /// <summary>
        /// Pour Créer un curseur 
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void CreerCurseur(ref string ChaineContexte)
        {
            mUADNavigateurs.ObtenirInfosNavigateurs(ref ChaineContexte, mEnvirRess, "1");
            mNavigateurCourant = ObtenirProchainNavigateur();
        }

        /// <summary>
        /// Fermer un curseur 
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void FermerCurseur(ref string ChaineContexte)
        {
            mUADNavigateurs.FermerCurseur();
        }

        /// <summary>
        /// Obtenir les informations du prochain navigateur 
        /// </summary>
        /// <param name="ChaineContexte"></param>
       private TsDtNavigateurs ObtenirProchainNavigateur()
        {
            TsDtNavigateurs dtretour = new TsDtNavigateurs();

            if (mUADNavigateurs.LireSuivant())
            {
                dtretour = mUADNavigateurs.ObtenirNavigateurCourant();
            }
            else
            {
                mFinNavigateur = true;
            }

            return dtretour;
        }

        /// <summary>
        /// Ecrire les informations du prochain navigateur sur le fichier
        /// </summary>
        /// <param name="ChaineContexte"></param>
        private void EcrireNavigateur()
        {

            TsEngrNavgt EnrgInfoSecNavgt = new TsEngrNavgt();
            EnrgInfoSecNavgt.CoEnv = mNavigateurCourant.Envir;
            EnrgInfoSecNavgt.CoModSecNav = mNavigateurCourant.ModlSecrt;
            EnrgInfoSecNavgt.CoProAccSec = mNavigateurCourant.CodProfl;
            EnrgInfoSecNavgt.CoFonSec = mNavigateurCourant.CodFonct;
            EnrgInfoSecNavgt.NmFonSec = mNavigateurCourant.NomFonct + mNavigateurCourant.CodNivSecrt;
            EnrgInfoSecNavgt.CoNivAccSec = "";

            mEcrivainInfoSecNavgt.EcrireEnregistrement(EnrgInfoSecNavgt);
        
        }

        /// <summary>
        /// Permets d'exécuter les travaux préparatoires du traitement commun que l'on soit en initiale ou en reprise telle que la
        /// déclaration des lecteurs et écrivains.
        /// </summary>
        /// <param name="chaineContexte">Contexte courant</param>
        /// <param name="estEnReprise">Indique si on est en reprise.</param>
        private void DemarrageCommun(ref string chaineContexte, bool estEnReprise)
        {
            mEnvirRess = XuCuConfiguration.get_ValeurSysteme("General", "Environnement");

            mUADNavigateurs = new TsCdAccesRessourcesInfosNavgt();
            string NomlogiqueFichier = "TS7EXTRNAV";
            // Initialisation de l'écrivain du fichier rapport (sortie)
            mEcrivainInfoSecNavgt = this.CreerEcrivain<TsEngrNavgt>(ref chaineContexte, ContexteTravail.NomChaine(ref chaineContexte), NomlogiqueFichier, estEnReprise);

        }
               
        #endregion

        #region --- Méthodes private ---

        #endregion

    }
}
