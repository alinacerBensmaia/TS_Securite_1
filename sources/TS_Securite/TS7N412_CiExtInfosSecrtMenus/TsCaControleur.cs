
using Rrq.InfrastructureLotPFI.ScenarioTransactionnel;
using System.Data;
using System;
using System.Collections.Generic;
using Rrq.InfrastructureCommune.Parametres;
using TS7N414_DdInfosSecrtMenus;
using Rrq.InfrastructureLotPFI.GestionFichier;

namespace TS7N412_CiExtInfosSecrtMenus
{
    ///---------------------------------------------------------------------------------------------------------
    /// Project		: TS7N412_CiExtInfosSecrtMenus
    /// Class		: xxCaControleur
    ///---------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Classe de point d'entrée pour la fonction. 
    /// </summary>
    /// <history>
    ///  Historique des modifications: 
    ///  -------------------------------------------------------------------------------------------------------
    ///  Demande    Date		   Nom			        Description
    ///  -------------------------------------------------------------------------------------------------------
    ///  [...]    2019-07-18       Nom                  Création initiale
    ///  </history>
    ///---------------------------------------------------------------------------------------------------------


    public class TsCaControleur : XdCaTravailNonEncadre
    {

        #region --- Variables ---

      
      
        #endregion


        #region --- Propriétés ---


        #endregion

        #region --- Méthodes protected ---


        /// <summary>
        /// C'est ici que le traitement se fait entre chaque point de synchro
        /// </summary>
        /// <param name="ChaineContexte"></param>
        protected override void ExecuterTravailNonEncadre(ref string ChaineContexte)
        {

            string mEnvirRess = XuCuConfiguration.get_ValeurSysteme("General", "Environnement");

            string NomlogiqueFichier = "TS7EXTRMEN";
            // Initialisation de l'écrivain du fichier rapport (sortie)
            XdIEnregEcrivain<TsEngrMenus> mEcrivainInfoSecMenus = this.CreerEcrivain<TsEngrMenus>(ref ChaineContexte, ContexteTravail.NomChaine(ref ChaineContexte), NomlogiqueFichier, System.Text.Encoding.UTF8, false,false);


            //  Ces méthodes font l'obtention des postes de TCHOIX et TGECRAn nécessairement dans XP car ces tables sont aiguillées en phase 1.
            DataTable mTCHOIX = XuCaSpitab.ObtenirTousLesPostesRessource(ref ChaineContexte, "TCHOIX");
            DataTable mTGECRAN = XuCaSpitab.ObtenirTousLesPostesRessource(ref ChaineContexte, "TGECRAN");

            TsEngrMenus EnrgInfoSecMenus = new TsEngrMenus();
            foreach (DataRow row in mTCHOIX.Rows)
            {
                if (
                    (!(row["COD_ETAT_TRAIT_MENU_1"].ToString() == "") |
                     !(row["COD_ETAT_TRAIT_MENU_2"].ToString() == "") |
                     !(row["COD_ETAT_TRAIT_MENU_3"].ToString() == "") |
                     !(row["COD_ETAT_TRAIT_MENU_4"].ToString() == "") |
                     !(row["COD_ETAT_TRAIT_MENU_5"].ToString() == "")
                     ) &
                     !(row["NUM_CHOIX_MENU"].ToString() == "") &
                     !(row["DES_CHOIX_MENU"].ToString() == "")
                   )
                {

                    foreach (DataRow rowEcran in mTGECRAN.Rows)
                    {

                    if (row["NUM_CHOIX_MENU"].ToString() == rowEcran["NUM_CHOIX"].ToString())
                        {
                            EnrgInfoSecMenus.CoEnv = mEnvirRess.Substring(0,1) + ";";
                            EnrgInfoSecMenus.NomTsConsrPanrm = rowEcran["NOM_TS_CONSR_PANRM"].ToString() + ";" ;                                          
                            EnrgInfoSecMenus.NumChoix = rowEcran["NUM_CHOIX"].ToString() + ";";
                            EnrgInfoSecMenus.DesChoixMenu= row["DES_CHOIX_MENU"].ToString().Trim() + ";" + rowEcran["DES_TITRE_AFFCH"].ToString().TrimStart().TrimEnd() + ";" + row["COD_INDCT_APPLC"].ToString() + ";";
                            EnrgInfoSecMenus.DesTitreAffch = "";
                            EnrgInfoSecMenus.CodIndctApplc = "";

                            if (!(row["COD_ETAT_TRAIT_MENU_1"].ToString() == "") & rowEcran["COD_ACTN"].ToString() == "")
                            {
                                EnrgInfoSecMenus.CodActnEcran = rowEcran["COD_ACTN"].ToString() + ";";
                                EnrgInfoSecMenus.CoNivAccSec = row["VAL_TRAIT_AUTRS_MENU_1"].ToString().Substring(0, 1) + ";";
                                mEcrivainInfoSecMenus.EcrireEnregistrement(EnrgInfoSecMenus);
                            }

                            if (!(row["COD_ETAT_TRAIT_MENU_2"].ToString() == "") & rowEcran["COD_ACTN"].ToString() == "A")
                            {
                                EnrgInfoSecMenus.CodActnEcran = rowEcran["COD_ACTN"].ToString() + ";";
                                EnrgInfoSecMenus.CoNivAccSec = row["VAL_TRAIT_AUTRS_MENU_2"].ToString().Substring(0, 1) + ";";
                                mEcrivainInfoSecMenus.EcrireEnregistrement(EnrgInfoSecMenus);
                            }

                            if (!(row["COD_ETAT_TRAIT_MENU_3"].ToString() == "") & rowEcran["COD_ACTN"].ToString() == "D")
                            {
                                EnrgInfoSecMenus.CodActnEcran = rowEcran["COD_ACTN"].ToString() + ";";
                                EnrgInfoSecMenus.CoNivAccSec = row["VAL_TRAIT_AUTRS_MENU_3"].ToString().Substring(0, 1) + ";";
                                mEcrivainInfoSecMenus.EcrireEnregistrement(EnrgInfoSecMenus);
                            }

                            if (!(row["COD_ETAT_TRAIT_MENU_4"].ToString() == "") & rowEcran["COD_ACTN"].ToString() == "M")
                            {
                                EnrgInfoSecMenus.CodActnEcran = rowEcran["COD_ACTN"].ToString() + ";";
                                EnrgInfoSecMenus.CoNivAccSec = row["VAL_TRAIT_AUTRS_MENU_4"].ToString().Substring(0, 1) + ";";
                                mEcrivainInfoSecMenus.EcrireEnregistrement(EnrgInfoSecMenus);
                            }

                            if (!(row["COD_ETAT_TRAIT_MENU_5"].ToString() == "") & rowEcran["COD_ACTN"].ToString() == "X")
                            {
                                EnrgInfoSecMenus.CodActnEcran = rowEcran["COD_ACTN"].ToString() + ";";
                                EnrgInfoSecMenus.CoNivAccSec = row["VAL_TRAIT_AUTRS_MENU_5"].ToString().Substring(0, 1) + ";";
                                mEcrivainInfoSecMenus.EcrireEnregistrement(EnrgInfoSecMenus);
                            }

                        }
                    }

                }
            }
                                 
            
            mEcrivainInfoSecMenus.Fermer();

            /*   DataRow Drs = mTCHOIX.Select("NUM_CHOIX_MENU='" & txtMenuSousMenu.Text & "'");

           If Drs.Length > 0 Then
               ' Si trouvé, initialiser le nom de la fonction et en permettre la modification
               txtNom.Text = Drs(0).Item("DES_CHOIX_MENU").ToString()
               txtNom.ReadOnly = False
               txtNom.Focus()

           Else
               ' Si pas trouvé, message d'erreur
               Me.AfficheMessage.AfficherMsg("XY50030E", txtMenuSousMenu.Text)

               txtMenuSousMenu.Text = ""
               txtNom.Text = ""
               txtMenuSousMenu.Focus()
           End If

           */

        }



        #endregion

        #region --- Méthodes private ---

        #endregion
    }
   
}

