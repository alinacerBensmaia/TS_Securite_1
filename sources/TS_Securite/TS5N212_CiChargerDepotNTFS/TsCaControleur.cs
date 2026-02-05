
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureLotPFI.ScenarioTransactionnel;
using System.Transactions;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.UtilitairesCommuns;

using TS5N211_CbFonctionCommune.AccessBD;
using TS5N211_CbFonctionCommune.Entites;

namespace TS5N212_CiChargerDepotNTFS
{
    ///---------------------------------------------------------------------------------------------------------
    /// Project		: TS5N212_CiChargerDepotNTFS
    /// Class		: TsCaControleur
    ///---------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Travail permettant de récupérer l'information NTFS des dépôts de fichiers.
    /// Elle permet également de confronter cette information à une autre de Référence et d'enressortir 
    /// les différences.
    /// </summary>
    /// <history>
    ///  Historique des modifications: 
    ///  -------------------------------------------------------------------------------------------------------
    ///  Demande    Date		   Nom			        Description
    ///  -------------------------------------------------------------------------------------------------------
    ///  [...]   9999-09-99        Nom                  Création initiale
    ///  </history>
    ///---------------------------------------------------------------------------------------------------------
    public class TsCaControleur : XdCaTravailNonEncadre
    {
        #region --- Variables ---    

        private string _mCheminRacine;
        private TsCaEnregistrerRepertoire _mEnregistreur;
        //Indique s'il y a lieu d'engager la confrontation des situations Courante et de Référence
        private bool _mActiverMecaniqueConfrontation;
        //Indique s'il y a lieu de bloquer la réinitialisation de la situation de Référence (et pouvoir la conserver tel qu'elle)
        private bool _mBloquerReinitialisationReference;

        #endregion

        #region --- Propriétés ---



        #endregion

        #region --- Méthodes protected ---

        /// <summary>
        /// C'est ici que le traitement. Notez que ce traitement non enacadré n'est pas synchronisé et aucune transaction est mise en place. 
        /// Voir le fascicule EF20 pour en lire et comprendre plus sur les traitements lot non encadré.
        /// </summary>
        /// <param name="chaineContexte"></param>
        protected override void ExecuterTravailNonEncadre(ref string chaineContexte)
        {

            string[] listeRepertoiresSource = new string[] { };
            string[] listeRepertoiresASupprimer = new string[] { };

            _mEnregistreur = new TsCaEnregistrerRepertoire();

            //System.Diagnostics.Debugger.Launch();

            //Récupérer les paramètres de la chaine
            var ctx = new XdCuContexteTravail();

            var listeParams = ctx.ObtenirListeParametreSpecifique(ref chaineContexte);
            //Récupération du paramètre d'Analyse
            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("Analyser"))
                listeRepertoiresSource = ctx.ObtenirListeParametreSpecifique(ref chaineContexte)["Analyser"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            else
                Bavard.EcrireMessageTraceSysteme("Aucun répertoire à traiter passé en paramètre.");
            //Récupération du paramètre d'Analyse
            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("Supprimer"))
                listeRepertoiresASupprimer = ctx.ObtenirListeParametreSpecifique(ref chaineContexte)["Supprimer"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            else
                Bavard.EcrireMessageTraceSysteme("Aucun répertoire à SUPPRIMER passé en paramètre.");

            //Récupération du paramètre d'activation de la mécanique de confrontataion
            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("Confronter"))
            {
                if (bool.TryParse(ctx.ObtenirListeParametreSpecifique(ref chaineContexte)["Confronter"].ToLower(), out _mActiverMecaniqueConfrontation))
                {
                    Bavard.EcrireMessageTraceSysteme("Mécanique de confrontation activée.");
                }
                else
                {
                    Bavard.EcrireMessageTraceSysteme("Mécanique de confrontation désactivée (la valeur du paramètre n'est pas un booléen).");
                    _mActiverMecaniqueConfrontation = false;
                }
            }
            else
            {
                Bavard.EcrireMessageTraceSysteme("Mécanique de confrontation désactivée (le paramètre n'est pas défini dans le contexte).");
                _mActiverMecaniqueConfrontation = false;
            }

            //Récupération du paramètre d'activation de la mécanique de confrontataion
            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("ConserverReference"))
            {
                if (bool.TryParse(ctx.ObtenirListeParametreSpecifique(ref chaineContexte)["ConserverReference"].ToLower(), out _mBloquerReinitialisationReference))
                {
                    Bavard.EcrireMessageTraceSysteme("Blocage de la réinitialisation de la situation de Référence.");
                }
                else
                {
                    Bavard.EcrireMessageTraceSysteme("Réinitialisation de la situation de Référence confirmé.");
                    _mBloquerReinitialisationReference = false;
                }
            }
            else
            {
                Bavard.EcrireMessageTraceSysteme("Réinitialisation de la situation de Référence confirmé (choix par défaut).");
                _mBloquerReinitialisationReference = false;
            }

            DateTime dateAnalyse = DateTime.Now;
            double totalMinutes = 0;

            var situationReference = new TsCaSituationDeReference();

            //Toilettage des chemins à analyser
            ToilettageDesCheminsAAnalyser(listeRepertoiresSource);

            //Si la mécanique de confrontation est activée => constituer la situation de référence
            if (_mActiverMecaniqueConfrontation && !_mBloquerReinitialisationReference)
            {
                Bavard.EcrireMessageTraceSysteme("Reconstitution de la Situation de Référence à " + DateTime.Now);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                situationReference.ReconstituerSituationDeReference(listeRepertoiresSource.ToList());
                watch.Stop();
                Bavard.EcrireMessageTraceSysteme("FIN Reconstitution  --> à " + DateTime.Now + "(" + Math.Round(watch.Elapsed.TotalMinutes, 2) + " minutes)");
            }

            //Traiter les répertoires à ajouter à la BD
            for (int i = 0; i < listeRepertoiresSource.Length; i++)
            {

                var watch = System.Diagnostics.Stopwatch.StartNew();

                Bavard.EcrireMessageTraceSysteme("Traiter les répertoires à ajouter à la BD");

                Bavard.EcrireMessageTraceSysteme("DÉBUT --> " + listeRepertoiresSource[i] + " à " + DateTime.Now);

                TraiterRepertoire(listeRepertoiresSource[i], dateAnalyse);

                watch.Stop();

                Bavard.EcrireMessageTraceSysteme("FIN   --> " + listeRepertoiresSource[i] + " à " + DateTime.Now + "("+ Math.Round(watch.Elapsed.TotalMinutes,2) + " minutes)");

                totalMinutes = totalMinutes + Math.Round(watch.Elapsed.TotalMinutes, 2);

            }

            Bavard.EcrireMessageTraceSysteme("Durée total du traitement : " + totalMinutes + " minutes)");

            //Si la mécanique de confrontation est activée => confronter les Situations Courante et Référence
            if (_mActiverMecaniqueConfrontation)
            {
                Bavard.EcrireMessageTraceSysteme("Lancement de la confrontation entre situation Courante et situation de Référence " + DateTime.Now);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                situationReference.CollecterLesDonneesDesRapportsDeConfrontation(listeRepertoiresSource);
                watch.Stop();
                Bavard.EcrireMessageTraceSysteme("FIN Confrontation  --> à " + DateTime.Now + "(" + Math.Round(watch.Elapsed.TotalMinutes, 2) + " minutes)");
            }

            //Traiter les répertoires à supprimer à la BD
            foreach (string d in listeRepertoiresASupprimer)
            {
                if (d != string.Empty)
                { 
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    Bavard.EcrireMessageTraceSysteme(Environment.NewLine + "Traiter les répertoires à supprimer à la BD");
                    Bavard.EcrireMessageTraceSysteme("DÉBUT --> " + d.Trim() + " à " + DateTime.Now);

                    //TraiterRepertoire(d.Trim(), dateAnalyse,true);
                    _mEnregistreur.ViderTableEmplacement(d);

                    watch.Stop();

                    Bavard.EcrireMessageTraceSysteme("FIN   --> " + d.Trim() + " à " + DateTime.Now + "(" + Math.Round(watch.Elapsed.TotalMinutes, 2) + " minutes)");

                    totalMinutes = totalMinutes + Math.Round(watch.Elapsed.TotalMinutes, 2);
                }
            }

            //Envoie du courriel de traitement complété
            string destinataire = XuCaContexte.get_CodeUsagerEssai(chaineContexte) + XuCuConfiguration.get_ValeurSysteme("TS5", "TS5\\SuffixeCourrielUtilisateur");
            string environnement = XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement");
            string titre = "Fin de traitement de la chaine TS5HA1 chargement des accès NTFS";
            string contenu = "Répertoires analysés: " + "<br/>" +  listeRepertoiresSource.Aggregate("  ", (current, s) => current + (s + "<br/>")) + "<br/>" +
                             "Répertoires supprimés: " + "<br/>" + listeRepertoiresASupprimer.Aggregate("  ", (current, s) => current + (s + "<br/>")) + "<br/>" +
                             "Durée total du traitement: " + totalMinutes + " minutes";

            TS5N211_CbFonctionCommune.TsCaOutils.TransmettreCourrielChargement(ref chaineContexte,titre,contenu, destinataire, environnement);

        }
        /// <summary>
        /// Toiletter les divers chemin reçus en paramètre de façon à en enlever les espaces
        /// ainsi que les "\" et les "/" s'ils s'y trouvent à la fin
        /// </summary>
        /// <param name="listeRepertoiresSource"></param>
        private void ToilettageDesCheminsAAnalyser(string[] listeRepertoiresSource)
        {
            string temp;
            for (int i = 0; i < listeRepertoiresSource.Length; i++)
            {
                temp = listeRepertoiresSource[i].Trim();
                listeRepertoiresSource[i] = (temp.EndsWith("/") || temp.EndsWith("\\")) ? temp.Substring(0, (temp.Length - 1)) : temp;
            }
        }

        private  void TraiterRepertoire(string d, DateTime dateAnalyse)
        {
            var listeRpp = new List<TsCaRepertoire>();
            try
            {
                if (Directory.Exists(d.Trim())){

                    _mCheminRacine = d;
                    var listeRppASupprimer = new List<string>();

                    //Utiliser le compte ayant accès à tout les dépôts NTFS
                    //using (XuCuEmpruntIdent.DebuterEmprunt(TS5N211_CbFonctionCommune.TsCaOutils.CleSymbolique, "Récupération Information NTFS"))
                    //{
                    //    // Récupérer les informations NTFS du répertoire racine
                    //    var rep = RecupererInfoNtfs(new DirectoryInfo(d), dateAnalyse);
                    //    if (rep != null)
                    //    {
                    //        listeRpp.Add(rep);
                    //    }

                    //    //Récupération des répertoires à supprimer de la BD avant de faire les nouvelles insertions.
                    //    listeRppASupprimer.Add(d);
                    //    //On découpe la supression en plusieurs répertoires afin de ne pas avoir de timeout à l'exécution du delete sur la BD.
                    //    TS5N211_CbFonctionCommune.TsCaOutils.RecupererNiveauRep(d, 1, 2, listeRppASupprimer);
                    //    listeRppASupprimer.Reverse();
                    //}

                    //Vider les emplacements dans la BD avant d'insérer les nouveaux
                    _mEnregistreur.ViderTableEmplacement(d);
                    //_mEnregistreur.ViderTableEmplacement(listeRppASupprimer);

                    //if (pSupprimerSeulement == false)
                    //{
                        using (XuCuEmpruntIdent.DebuterEmprunt(TS5N211_CbFonctionCommune.TsCaOutils.CleSymbolique, "Récupération Information NTFS"))
                        {
                            // Récupérer les informations NTFS du répertoire racine
                            var rep = RecupererInfoNtfs(new DirectoryInfo(d), dateAnalyse);
                            if (rep != null)
                            {
                                listeRpp.Add(rep);
                            }
                            // Récupérer les informations NTFS des sous répertoires
                            ParcourirRepertoire(new DirectoryInfo(d), ref listeRpp, dateAnalyse);
                        }

                        //Incrire les répertoire trouvées dans la BD
                        if (listeRpp.Count > 0){

                            Bavard.EcrireMessageTraceSysteme("  DÉBUT --> Insertion des " + listeRpp.Count + " répertoires sous " + d);
                            SauvegarderRepertoires(listeRpp);
                            Bavard.EcrireMessageTraceSysteme("   FIN   --> Insertion de " + d + " terminée.");
                        }
                    //}

                }
                else{
                    Bavard.EcrireMessageTraceSysteme("ERREUR ---> Le répertoire " + d + " n'existe pas ou n'est pas accessible.");
                }
            }
            catch (Exception e)
            {
                Bavard.EcrireMessageTraceSysteme(e.Message);
            }
        }



        #endregion

        #region --- Méthodes private ---

        /// <summary>
        /// Parcourir de façon récursive chacun des répertoires de l'emplacement racine 
        /// </summary>
        /// <param name="racine"></param>
        /// <param name="listeRpp"></param>
        /// <param name="dateAnalyse"></param>
        private void ParcourirRepertoire(DirectoryInfo racine, ref List<TsCaRepertoire> listeRpp, DateTime dateAnalyse)
        {
            try
            {
                // Récupérer tous les sous répertoires
                var subDirs = racine.GetDirectories();

                foreach (var dirInfo in subDirs)
                {
                    var rppEnCours = dirInfo.FullName;

                    if (TS5N211_CbFonctionCommune.TsCaOutils.EstBavard()){
                        Bavard.EcrireMessageTraceSysteme(" En traitement --> " + dirInfo.FullName);
                    }

                    if (rppEnCours.Contains("$RECYCLE.BIN") ||
                        rppEnCours.Contains(@"\System Volume Information") ||
                        rppEnCours.Contains(@"\DfsrPrivate")) continue;

                    // Récupérer les informations NTFS du répertoire courant
                    var rpp = RecupererInfoNtfs(dirInfo, dateAnalyse);
                    if (rpp != null) listeRpp.Add(rpp);

                    // Parcourir de façon récursive les sous répertoires.
                    ParcourirRepertoire(dirInfo, ref listeRpp, dateAnalyse);
                }
            }
            catch (Exception e)
            {
                Bavard.EcrireMessageTraceSysteme("ERREUR --> " + e.StackTrace + Environment.NewLine);
            }
        }

        /// <summary>
        /// Récupérer les informations NTFS du répertoire spécifié
        /// </summary>
        /// <param name="repertoire"></param>
        /// <param name="dateAnalyse"></param>
        private TsCaRepertoire RecupererInfoNtfs(DirectoryInfo repertoire, DateTime dateAnalyse)
        {
            try
            {
                TsCaRepertoire rpp = new TsCaRepertoire(_mCheminRacine, repertoire.FullName, dateAnalyse);
                return rpp;
            }
            catch (Exception e)
            {
                Bavard.EcrireMessageTraceSysteme("ERREUR --> RecupererInfoNtfs " + repertoire.FullName + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Vider la table des emplacements avant de recevoir les nouvelles valeurs
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        //private void ViderTableEmplacement()
        //{
        //    ViderTableEmplacement("");
        //}

        private void SauvegarderRepertoires(List<TsCaRepertoire> listeRpp)
        {
            TsCaEnregistrerRepertoire enr = new TsCaEnregistrerRepertoire();

            enr.Executer(listeRpp);

        }


        #endregion
    }


}
