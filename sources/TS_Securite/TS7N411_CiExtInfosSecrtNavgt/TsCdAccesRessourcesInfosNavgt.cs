using System;
using System.Collections.Generic;
using Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseService;
using TS7N411_CiExtInfosSecrtNavgt;

namespace TS7N411_CiExtAccesNavEtMenus
{
    internal class TsCdAccesRessourcesInfosNavgt :   XuCdAccesDonneesAvecCurseurSupport 
    {

        private const string NOM_CONNEXION = "SQL_SecuritePreventive_X01";
        internal TsCdAccesRessourcesInfosNavgt() : base(NOM_CONNEXION, null)
        {
        }
               
        /// <summary>
        ///Obtenir les informations de sécurité des navigateurs de mission et de supports
        /// </summary>
        ///  <param name="chaineContexte">Contexte courant</param>

        internal void ObtenirInfosNavigateurs(ref string ChaineContexte, string Envir, string Phase)
        {

            // Construction de la requête
            string requeteSQL = @"  
                                    SELECT DISTINCT
                                            @ENVIRONNEMENT AS ENVIRONNEMENT
	                                        ,CASE
                                                WHEN FONCTION_SECURISEE.CO_TYP_PRO_FON_SEC IN('CU','SU','SN','PU') 
			                                        THEN 'NavigSU'

                                                ELSE --AN, CR, CU, DN, EX, PA, SP, UT, VB, XX

                                                    'NavigCS'

                                            END AS MODELE_SECURITE
	                                        ,FONCTION_AUTORISEE.CO_PRO_ACC_SEC AS CODE_PROFIL
	                                        ,FONCTION_SECURISEE.CO_FON_SEC AS CODE_FONCTION
	                                        ,FONCTION_SECURISEE.NM_FON_SEC AS NOM_FONCTION
	                                        ,ISNULL(ASSIGNATION_NIVEAU.CO_NIV_ACC_SEC, '*') AS CODE_NIVEAU_SECURITE
                                    
                                    FROM

                                            XZ1.FONAUTO AS FONCTION_AUTORISEE
                                            JOIN XZ1.FONSECU AS FONCTION_SECURISEE
                                                ON(FONCTION_SECURISEE.CO_FON_SEC = FONCTION_AUTORISEE.CO_FON_SEC

                                                AND FONCTION_SECURISEE.NO_PHA_FON_SEC = FONCTION_AUTORISEE.NO_PHA_FON_SEC)
                                            --RIGHT-- seulement avec assignation_niveau
                                            LEFT  --avec les '*' pour assignation_niveau
                                            JOIN XZ1.ASNIVAC AS ASSIGNATION_NIVEAU
                                                    ON(ASSIGNATION_NIVEAU.CO_PRO_ACC_SEC = FONCTION_AUTORISEE.CO_PRO_ACC_SEC
                                                    AND ASSIGNATION_NIVEAU.CO_FON_SEC = FONCTION_AUTORISEE.CO_FON_SEC
                                                    AND ASSIGNATION_NIVEAU.NO_PHA_FON_SEC = FONCTION_AUTORISEE.NO_PHA_FON_SEC)
                                    WHERE
                                            FONCTION_AUTORISEE.NO_PHA_FON_SEC = @PHASE
                                            AND FONCTION_SECURISEE.CO_TYP_PRO_FON_SEC NOT IN('70', 'NL', 'WD', 'GR')
                                    ORDER BY
                                            MODELE_SECURITE,
	                                        FONCTION_AUTORISEE.CO_PRO_ACC_SEC,
	                                        FONCTION_SECURISEE.CO_FON_SEC";
                                   
            // Création d'une liste de critères à joindre à la requête (ici elle est vide)
            List<XuDtCritere> Criteres = new List<XuDtCritere>();

            requeteSQL = requeteSQL.Replace("@PHASE", Phase);
            requeteSQL = requeteSQL.Replace("@ENVIRONNEMENT", (Convert.ToChar(39) + (Envir.Substring(0,1) + Convert.ToChar(39))));

            base.ExecuterLecture(ref ChaineContexte, requeteSQL, Criteres);
        }

        /// <summary>
        ///Obtenir la prochaine row d'info navigateur
        ///</summary>

        internal TsDtNavigateurs ObtenirNavigateurCourant()
        {
            TsDtNavigateurs navigateurs = new TsDtNavigateurs();

            navigateurs.Envir = Convert.ToString(base.GetObject("ENVIRONNEMENT")).Trim() + ";";
            navigateurs.ModlSecrt = Convert.ToString(base.GetObject("MODELE_SECURITE")).Trim() + ";";
            navigateurs.CodProfl = Convert.ToString(base.GetObject("CODE_PROFIL")).Trim() + ";";
            navigateurs.CodFonct = Convert.ToString(base.GetObject("CODE_FONCTION")).Trim() + ";";
            navigateurs.NomFonct = Convert.ToString(base.GetObject("NOM_FONCTION")).Trim() + ";";
            navigateurs.CodNivSecrt = Convert.ToString(base.GetObject("CODE_NIVEAU_SECURITE")).Trim() + ";";
                        
            return navigateurs;
        }

    internal new bool LireSuivant()
        {
            return base.LireSuivant();
        }
        internal void FermerCurseur()
        {
            this.Dispose();
        }
           

    }
}
