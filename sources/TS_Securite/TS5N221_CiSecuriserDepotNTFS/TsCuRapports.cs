using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using Rrq.InfrastructureLotPFI.ScenarioTransactionnel;
using Rrq.InfrastructureLotPFI.GestionFichier;
using Rrq.InfrastructureCommune.Parametres;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;

namespace TS5N221_CiSecuriserDepotNTFS
{
    ///---------------------------------------------------------------------------------------------------------
    /// Project		: TS5N221_CiSecuriserDepotNTFS
    /// Class		: TsCuRapports
    ///---------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Classe de point d'entrée pour la fonction. 
    /// </summary>
    /// <history>
    ///  Historique des modifications: 
    ///  -------------------------------------------------------------------------------------------------------
    ///  Demande    Date		   Nom			        Description
    ///  -------------------------------------------------------------------------------------------------------
    ///  [...]   9999-09-99        Nom                  Création initiale
    ///  </history>
    ///---------------------------------------------------------------------------------------------------------
	internal class TsCuRapports
{
    /*
    private const string FICHIER_DONNEES_RAPPORT = "CE1X121";
    private XdCuContexteTravail mContexteTravail;

    // Tables de pilotage (SPITAB) utilisées dans les rapports.
    private DataTable mDtDomaineCodeLangue;
    private DataTable mDtDomaineCodeUniqueComm;
    private XdCuFichier mStockageFichier;  
    */

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="ChaineContexte">Chaîne de contexte</param>
    public TsCuRapports(ref string ChaineContexte, XdCuFichier Stockagefichier)
    {
        /*
        // Obtenir les données des tables de pilotage.
        mDtDomaineCodeLangue = XuCaSpitab.ObtenirListePostes(ref ChaineContexte, "FCDOMVA", "CO-LAN-COR", "CO-LAN-COR".PadRight(22, '9'));
        mDtDomaineCodeUniqueComm = XuCaSpitab.ObtenirTousLesPostes(ref ChaineContexte, "CEPOSCO");

        mContexteTravail = new XdCuContexteTravail();
        mStockageFichier = Stockagefichier;      
        */
    }

    /// <summary>
    /// Produire un rapport format lettre, paysage.
    /// </summary>
    /// <param name="ChaineContexte">Chaîne de contexte</param>
    public void ProduireRapportLettrePaysage(ref string ChaineContexte)
    {
        /*
        using (XdRpRapportLotLettrePaysage rapportLettrePaysage = new XdRpRapportLotLettrePaysage())
        {
            rapportLettrePaysage.Database.Tables["XdXlCommunication"].Location = ObtenirCheminFichier(ref ChaineContexte, FICHIER_DONNEES_RAPPORT);

            rapportLettrePaysage.Database.Tables["CodeUniqueComm"].SetDataSource(mDtDomaineCodeUniqueComm);
            rapportLettrePaysage.Database.Tables["domainevaleurlangue"].SetDataSource(mDtDomaineCodeLangue);

            rapportLettrePaysage.ParameterFields["NomApplication"].CurrentValues.AddValue("Nom de l'Application");
            rapportLettrePaysage.ParameterFields["NoRapport"].CurrentValues.AddValue("FC.I14R");
            rapportLettrePaysage.ParameterFields["TitreRapport_1"].CurrentValues.AddValue("Titre rapport 1");
            // rapportLettrePaysage.ParameterFields["TitreRapport_2"].CurrentValues.AddValue("")
            // rapportLettrePaysage.ParameterFields["TitreRapport_3"].CurrentValues.AddValue("")
            // rapportLettrePaysage.ParameterFields["TitreRapport_4"].CurrentValues.AddValue("")
            rapportLettrePaysage.ParameterFields["TitreRapport_2"].CurrentValues.AddValue("Titre rapport 2");
            rapportLettrePaysage.ParameterFields["TitreRapport_3"].CurrentValues.AddValue("Titre rapport 3");
            rapportLettrePaysage.ParameterFields["TitreRapport_4"].CurrentValues.AddValue("Titre rapport 4");
            rapportLettrePaysage.ParameterFields["NomChaine"].CurrentValues.AddValue("FC1HE");
            rapportLettrePaysage.ParameterFields["DateProduction"].CurrentValues.AddValue(mContexteTravail.DateProduction(ref ChaineContexte));
            rapportLettrePaysage.ParameterFields["NoSeqPasse"].CurrentValues.AddValue(mContexteTravail.NoPasseFormatChaine(ref ChaineContexte).PadLeft(2, '0'));

            rapportLettrePaysage.ExportToDisk(ExportFormatType.PortableDocFormat, ObtenirCheminFichier(ref ChaineContexte, "CE1R1213"));
        }
        */
    }

    /*
    /// <summary>
    /// Obtenir le chemin pysique d'un fichier logique.
    /// </summary>
    /// <param name="ChaineContexte">Chaîne de contexte</param>
    /// <param name="pNomLogiqueFichier">Nom logique du fichier.</param>
    /// <returns>Chemin physique du fichier.</returns>
    private string ObtenirCheminFichier(ref string ChaineContexte, string NomLogiqueFichier)
    {
        XdCuFichier accesFichier = mStockageFichier;
        XdDtInfoFichier dtInfoFichier = accesFichier.ObtenirInformationFichier(ref ChaineContexte, mContexteTravail.NomChaine(ref ChaineContexte), NomLogiqueFichier);

        return dtInfoFichier.VlEmpPhyFicPfi;
    }
    */
}
}