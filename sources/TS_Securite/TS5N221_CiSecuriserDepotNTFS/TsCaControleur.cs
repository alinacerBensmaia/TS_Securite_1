
using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.UtilitairesCommuns;
using Rrq.InfrastructureLotPFI.ScenarioTransactionnel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TS5N211_CbFonctionCommune;
using TS5N211_CbFonctionCommune.AccessBD;


namespace TS5N221_CiSecuriserDepotNTFS
{
    public class TsCaControleur : XdCaTravailNonEncadre
    {

        #region --- Variables ---    


        private TsCuGererSecurite mInfoEmprunt;
        public static readonly string MCleSymbolique = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"CleSymboliqueNTFS");
        private string[] mListeRepertoiresRegroupe;
        public TsCaControleur()
        {
            mInfoEmprunt = new TsCuGererSecurite(MCleSymbolique);
        }

        #endregion

        /// <summary>
        /// Produire les rapport.
        /// </summary>
        /// <param name="chaineContexte">Chaîne de contexte</param>
        protected override void ExecuterTravailNonEncadre(ref string chaineContexte)
        {
            try
            {
                //Récupérer les paramètres de la chaine
                var ctx = new XdCuContexteTravail();

                //Récupérer les propriété de la chaine de contexte pour obtenir le demandeur
                TsCuUtilisateur utilisateur = new TsCuUtilisateur();
                var propriete = ctx.PreparerVariablesHeritage(ref chaineContexte);

                if (propriete.ContainsKey("CodeUsager"))
                {
                    utilisateur.CodeUtilisateur = propriete["CodeUsager"];
                }

                string[] listeRepertoiresSource = ObtenirListeRepertoiresParametres(ref chaineContexte, ctx);

                double totalMinutes = 0;

                foreach (string path in listeRepertoiresSource)
                {
                    if (path == string.Empty)
                    {
                        continue;
                    }

                    List<TsCaRegleAAppliquer> listeReglesAAppliquer = ObtenirReglesAAppliquer(path);

                    var watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    totalMinutes = totalMinutes + Math.Round(watch.Elapsed.TotalMinutes, 2);
                    //Diviser le traitement en plusieurs listes(threads), selon le nombre de chemin voulu
                    if (listeReglesAAppliquer.Count > 0)
                    {
                        if (mListeRepertoiresRegroupe.Length > 1)
                        {
                            IEnumerable<int> listeInsertionParametre = CreerListeInsertionParametre(listeReglesAAppliquer, mListeRepertoiresRegroupe);

                            //Il faut s'assurer qu'un répertoire ne se retrouva pas dans deux thread différentes
                            List<int> listeInsertionFinaleTest = RegrouperRepertoire(listeInsertionParametre, listeReglesAAppliquer);

                            //Créer une liste de tâches
                            List<Task> lstTraitements = new List<Task>();

                            foreach (var i in listeInsertionFinaleTest)
                            {
                                var range = listeReglesAAppliquer.GetRange(0, i);

                                using (XuCuEmpruntIdent.DebuterEmpruntCompte(mInfoEmprunt.NomUsager, mInfoEmprunt.MotDePasse))
                                {
                                    lstTraitements.Add(Task.Factory.StartNew(action: () => Traitement(range, utilisateur)));
                                }

                                //Task.WaitAll(lstTraitements.ToArray()); //à activer pour faire du pas à pas dans le code

                                listeReglesAAppliquer.RemoveRange(0, i);
                            }

                            Task.WaitAll(lstTraitements.ToArray());
                        }
                        //Si nous avons pas de paramètre pour découper en plusieurs liste, on en fait qu'une seule
                        else
                        {
                            using (XuCuEmpruntIdent.DebuterEmpruntCompte(mInfoEmprunt.NomUsager, mInfoEmprunt.MotDePasse))
                            {
                                Traitement(listeReglesAAppliquer, utilisateur);
                            }
                        }

                    }
                    else
                    {
                        Bavard.EcrireMessageTraceSysteme("Le fichier csv ne contient pas de règles.");
                    }
                    watch.Stop();
                    totalMinutes = totalMinutes + Math.Round(watch.Elapsed.TotalMinutes, 2);
                    Console.WriteLine($"Temps d'exécution: {watch.ElapsedMilliseconds} ms");


                    //Envoie du courriel de traitement complété
                    string destinataire = XuCaContexte.get_CodeUsagerEssai(chaineContexte) + XuCuConfiguration.get_ValeurSysteme("TS5", "TS5\\SuffixeCourrielUtilisateur");
                    string environnement = XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement");
                    string titre = "Fin de traitement de la chaine TS5DBS Sécuriser les accès NTFS";
                    string contenu = "Répertoires analysés: " + "<br/>" + listeRepertoiresSource.Aggregate("  ", (current, s) => current + s + "<br/>") + "<br/>" +
                                     "Durée total du traitement: " + totalMinutes + " minutes";


                    TS5N211_CbFonctionCommune.TsCaOutils.TransmettreCourrielChargement(ref chaineContexte, titre, contenu, destinataire, environnement);
                }
            }
            catch (Exception ex)
            {
                //Envoie du courriel de traitement en erreur
                string destinataire = XuCaContexte.get_CodeUsagerEssai(chaineContexte) + XuCuConfiguration.get_ValeurSysteme("TS5", "TS5\\SuffixeCourrielUtilisateur");
                string environnement = XuCuConfiguration.get_ValeurSysteme("General", @"General\Environnement");
                string titre = "ERREUR --> Fin de traitement anomale --> TS5DBS Sécuriser les accès NTFS";
                string contenu = "ERREUR --> Fin de traitement anomale --> Exception" + ex.Message;

                TS5N211_CbFonctionCommune.TsCaOutils.TransmettreCourrielChargement(ref chaineContexte, titre, contenu, destinataire, environnement);
            }
        }

        private string[] ObtenirListeRepertoiresParametres(ref string chaineContexte, XdCuContexteTravail ctx)
        {
            string[] listeRepertoiresSource = new string[] { };

            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("Chemin"))
            {
                listeRepertoiresSource = ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ElementAt(0).Value.Split(';');
            }
            else
            {
                Bavard.EcrireMessageTraceSysteme("Aucun répertoire passé en paramètre.");
            }
            mListeRepertoiresRegroupe = new string[] { };

            if (ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ContainsKey("Decoupage"))
            {
                mListeRepertoiresRegroupe = ctx.ObtenirListeParametreSpecifique(ref chaineContexte).ElementAt(1).Value.Split(';');
            }
            else
            {
                Bavard.EcrireMessageTraceSysteme("Aucun répertoire à découper passé en paramètre.");
            }

            return listeRepertoiresSource;
        }

        private List<int> RegrouperRepertoire(IEnumerable<int> pListeInsertion, List<TsCaRegleAAppliquer> pListeReglesAAppliquer)
        {
            int i = 1;
            int index = 0;

            int[] listeInsertion = pListeInsertion as int[] ?? pListeInsertion.ToArray();

            List<int> nouvelleListe = new List<int>();

            int nbrParListe = listeInsertion[0];

            //Parcourir l'ensemble des règles et les séparer dans des listes sans séparer un répertoire dans deux listes
            foreach (var regle in pListeReglesAAppliquer.Where(regle => pListeReglesAAppliquer.Count != index + 1))
            {
                //Si nous avons atteint le nbr de règle par traitement ET que la règle suivante n'a pas le même chemin
                if ((i >= nbrParListe && regle.Chemin != pListeReglesAAppliquer[index + 1].Chemin))
                {
                    nouvelleListe.Add(i);
                    i = 1;
                }
                else
                {
                    i += 1;
                }

                index += 1;
            }

            //Nous sommes arrivé à la fin, on ajoute les regles restantes 
            nouvelleListe.Add(pListeReglesAAppliquer.Count - nouvelleListe.Sum());

            return nouvelleListe;
        }

        private static IEnumerable<int> CreerListeInsertionParametre(List<TsCaRegleAAppliquer> listeReglesAAppliquer, string[] ListeRepertoiresRegroupe)
        {
            List<int> listeInsertion = new List<Int32>();
            int compteurLigne = 0;
            foreach (var path in ListeRepertoiresRegroupe)
            {
                int nombreLigne = Convert.ToInt32(listeReglesAAppliquer.Count(regle => regle.Chemin.StartsWith(path)));
                if (nombreLigne != 0)
                {
                    listeInsertion.Add(nombreLigne);
                    compteurLigne += nombreLigne;
                }
            }

            if (listeReglesAAppliquer.Count() > compteurLigne)
            {
                listeInsertion.Add(listeReglesAAppliquer.Count - compteurLigne);
            }
            return listeInsertion;
        }

        private void Traitement(List<TsCaRegleAAppliquer> listeReglesAAppliquer, TsCuUtilisateur utilisateur)
        {
            string repEncours = "";
            string message = null;
            List<TsCuSecuriteRepertoire> listeSecuriteRepertoires = new List<TsCuSecuriteRepertoire>();

            foreach (TsCaRegleAAppliquer regle in listeReglesAAppliquer)
            {

                //Si on change de répertoire, on applique la sécurité pour ne pas avoir à faire toute les règles à la fin
                if (regle.Chemin.Equals(repEncours, StringComparison.OrdinalIgnoreCase) == false && listeSecuriteRepertoires.Count > 0)
                {
                    foreach (var securite in listeSecuriteRepertoires.ToList())
                    {
                        Bavard.EcrireMessageTraceSysteme(securite.AppliquerRegles());
                    }

                    listeSecuriteRepertoires.Clear();
                }

                repEncours = regle.Chemin;

                if (!Directory.Exists(regle.Chemin))
                {
                    Directory.CreateDirectory(regle.Chemin);
                }

                message = TraiterRegles(regle, message, listeSecuriteRepertoires);

                //Copier la règle dans la table de journalisation avec la date et heure de l’ajout. 
                JournaliserRegleAppliquee(regle, utilisateur.CodeUtilisateur, message);

            }

            //Appliquer les règles du dernier répertoire
            foreach (var securite in listeSecuriteRepertoires.ToList())
            {
                Bavard.EcrireMessageTraceSysteme(securite.AppliquerRegles());
            }
        }

        private string TraiterRegles(TsCaRegleAAppliquer regle, string message, List<TsCuSecuriteRepertoire> listeSecuriteRepertoires)
        {
            if (regle.CodeTypeApplication.Equals("AJO"))
            {
                message = AjouterRegleAjout(message,
                    listeSecuriteRepertoires,
                    regle,
                    regle.DroitAccesHerite.Equals("O"));
            }
            else if (regle.CodeTypeApplication.Equals("SUP"))
            {
                message = AjoutRegleSuppression(message,
                    listeSecuriteRepertoires,
                    regle,
                    regle.DroitAccesHerite.Equals("O"));
            }
            else
            {
                Bavard.EcrireMessageTraceSysteme("ERREUR --> Le code d'application doit être AJO ou SUP seulement");
            }

            return message;
        }

        private string AjoutRegleSuppression(string message, List<TsCuSecuriteRepertoire> listeSecuriteRepertoires, TsCaRegleAAppliquer regle, bool heritage)
        {
            if (regle.CodeTypeApplication.Equals("SUP"))
            {
                //s'il y a déjà des éléments dans la liste on vérifie si l'on peut ajouter la régle à une TsCuSecuriteRepertoire
                if (listeSecuriteRepertoires.Count > 0)
                {
                    bool trouve = false;
                    foreach (var securite in listeSecuriteRepertoires)
                    {
                        //Si ce répertoire a déjà des règles a appliquer, on ajoute la régle à l'objet TsCuSecuriteRepertoire
                        if (securite.Repertoire.Equals(regle.Chemin))
                        {
                            trouve = true;
                            if (regle.CodeTypeRegle.ToUpper().Equals("REG"))
                            {
                                message = heritage ? securite.SupprimerRegleAccesHerite(regle) : securite.SupprimerRegleAcces(regle);
                            }
                            else if (regle.CodeTypeRegle.ToUpper().Equals("SID"))
                            {
                                message = securite.SupprimerRegleAccesSID(regle);
                            }
                            else
                            {
                                message = "ERREUR --> cible: " + regle.Chemin + "Le type de règle doit être REG ou SID. regle.CodeTypeRegle";
                            }
                            if (message.Contains("ERREUR"))
                            {
                                Bavard.EcrireMessageTraceSysteme(message);
                            }

                            break;
                        }
                    }

                    if (trouve)
                    {
                        return message;
                    }

                    if (!trouve)
                    {
                        message = ValiderCodeTypeDeRegleAjout(listeSecuriteRepertoires, regle);
                    }
                }
                //La liste listeSecuriteRepertoires est vide, on ajoute la première TsCuSecuriteRepertoire
                else
                {
                    message = ValiderCodeTypeRegleSuppression(listeSecuriteRepertoires, regle);

                }
            }
            else
            {
                message = "ERREUR --> cible: " + regle.Chemin + "groupe: " + regle.NomIdentite + "Le code d'application doit être AJO ou SUP seulement";
            }

            return message;
        }

        private string ValiderCodeTypeRegleSuppression(List<TsCuSecuriteRepertoire> listeSecuriteRepertoires, TsCaRegleAAppliquer regle)
        {
            string message;
            if (regle.CodeTypeRegle.ToUpper().Equals("REG"))
            {
                TsCuSecuriteRepertoire securiteRepertoireAAppliquer = new TsCuSecuriteRepertoire(regle.Chemin, true);
                message = securiteRepertoireAAppliquer.SupprimerRegleAcces(regle);
                AjouterSecuriteRepertoireATraiter(message, listeSecuriteRepertoires, securiteRepertoireAAppliquer);
            }
            else if (regle.CodeTypeRegle.ToUpper().Equals("SID"))
            {
                TsCuSecuriteRepertoire securiteRepertoireAAppliquer = new TsCuSecuriteRepertoire(regle.Chemin, true);
                message = securiteRepertoireAAppliquer.SupprimerRegleAccesSID(regle);
                AjouterSecuriteRepertoireATraiter(message, listeSecuriteRepertoires, securiteRepertoireAAppliquer);
            }
            else
            {
                message = "ERREUR --> cible: " + regle.Chemin + "Le type de règle doit être REG ou SID. regle.CodeTypeRegle";
            }
            if (message.Contains("ERREUR"))
            {
                Bavard.EcrireMessageTraceSysteme(message);
            }

            return message;
        }

        private string AjouterRegleAjout(string message, List<TsCuSecuriteRepertoire> listeSecuriteRepertoires, TsCaRegleAAppliquer regle, bool heritage)
        {
            //Si la liste est contient déjà des objets TsCuSecuriteRepertoires
            if (listeSecuriteRepertoires.Count > 0)
            {
                bool trouve = false;
                foreach (var securite in listeSecuriteRepertoires.ToList())
                {
                    //Si ce répertoire a déjà des règles a appliquer, on ajoute la régle à l'objet TsCuSecuriteRepertoire
                    if (securite.Repertoire.Equals(regle.Chemin))
                    {
                        trouve = true;
                        if (regle.CodeTypeRegle.ToUpper().Equals("REG"))
                        {
                            message = heritage ? securite.AjouterRegleAccesHerite(regle) : securite.AjouterRegleAcces(regle);
                        }
                        else if (regle.CodeTypeRegle.ToUpper().Equals("SID"))
                        {
                            message = message = "ERREUR --> cible: " + regle.Chemin + " L'ajout de droits avec un SID n'est pas supporté. Le type de règle doit être REG. regle.CodeTypeRegle";
                        }
                        else
                        {
                            message = "ERREUR --> cible: " + regle.Chemin + "Le type de règle doit être REG ou SID. regle.CodeTypeRegle";
                        }
                        if (message.Contains("ERREUR"))
                        {
                            Bavard.EcrireMessageTraceSysteme(message);
                        }

                        break;
                    }
                }
                //si on a pas trouvé le répertoire, on ajoute la règle à la liste
                if (!trouve)
                {
                    message = ValiderCodeTypeDeRegleAjout(listeSecuriteRepertoires, regle);
                }
            }
            //La liste listeSecuriteRepertoires est vide, on ajoute la première TsCuSecuriteRepertoire
            else
            {
                message = ValiderCodeTypeDeRegleAjout(listeSecuriteRepertoires, regle);
            }

            return message;
        }

        private string ValiderCodeTypeDeRegleAjout(List<TsCuSecuriteRepertoire> listeSecuriteRepertoires, TsCaRegleAAppliquer regle)
        {
            string message;
            if (regle.CodeTypeRegle.ToUpper().Equals("REG"))
            {
                TsCuSecuriteRepertoire securiteRepertoireAAppliquer = new TsCuSecuriteRepertoire(regle.Chemin, true);
                message = securiteRepertoireAAppliquer.AjouterRegleAcces(regle);
                AjouterSecuriteRepertoireATraiter(message, listeSecuriteRepertoires, securiteRepertoireAAppliquer);
            }
            else if (regle.CodeTypeRegle.ToUpper().Equals("SID"))
            {
                message = message = "ERREUR --> cible: " + regle.Chemin + " L'ajout de droits avec un SID n'est pas supporté. Le type de règle doit être REG. regle.CodeTypeRegle";
            }
            else
            {
                message = "ERREUR --> cible: " + regle.Chemin + "Le type de règle doit être REG ou SID. regle.CodeTypeRegle";
            }
            if (message.Contains("ERREUR"))
            {
                Bavard.EcrireMessageTraceSysteme(message);
            }

            return message;
        }

        private void AjouterSecuriteRepertoireATraiter(string message, List<TsCuSecuriteRepertoire> listeSecuriteRepertoires, TsCuSecuriteRepertoire securiteRepertoireAAppliquer)
        {
            if (message.Contains("ERREUR"))
            {
                Bavard.EcrireMessageTraceSysteme(message);
            }
            else
            {
                listeSecuriteRepertoires.Add(securiteRepertoireAAppliquer);
            }
        }


        private void JournaliserRegleAppliquee(TsCaRegleAAppliquer regle, string codeDemandeur, string commentaire)
        {
            TsCaEnregistrerJournalisation journalisation = new TsCaEnregistrerJournalisation();
            string retour = journalisation.Executer(regle, codeDemandeur, commentaire);

            if (retour != "")
            {
                Bavard.EcrireMessageTraceSysteme(retour);
            }
        }

        private List<TsCaRegleAAppliquer> ObtenirReglesAAppliquer(string path)
        {
            //Valider que le fichier d'entrée existe. S'il n'existe pas, on ajoute une trace
            if (!File.Exists(path))
            {
                Bavard.EcrireMessageTraceSysteme(string.Format("Le fichier '{0}' est introuvable", path));
            }

            //récupérer les données dans le fichier CVS
            var donneesFichierCsv = new DataTable();

            if (File.Exists(path))
            {
                //On convertit les données du fichier en entrée pour faciliter l'insertion dans la table TS5.REGNTFS
                donneesFichierCsv = ConvertirCVSDataTable(path);
            }

            //Insérer les règles extraite du fichier csv entré en paramètre dans la table TS5.REGNTFS
            EnregistrerReglesAAppliquer donnees = new EnregistrerReglesAAppliquer();

            //Vider la table TS5.REGNTFS avant d'insérer les règles
            donnees.ViderTable();

            bool succes = donnees.ExecuterRequeteInsert(donneesFichierCsv);
            if (!succes)
            {
                Bavard.EcrireMessageTraceSysteme(donnees.Message);
            }
            
            List<TsCaRegleAAppliquer> listeReglesAAppliquer = new List<TsCaRegleAAppliquer>();


            foreach (DataRow dr in donneesFichierCsv.Rows)
            {
                listeReglesAAppliquer.Add(new TsCaRegleAAppliquer(dr));
            }

            return listeReglesAAppliquer;
        }

        private DataTable ConvertirCVSDataTable(string pPath)
        {
            var donneesFichierCsv = new DataTable();

            using (StreamReader sr = new StreamReader(pPath, System.Text.Encoding.UTF8))
            {
                string[] colonnes = sr.ReadLine()?.Split(';');

                //Obtenir le nom des colonnes                  
                if (colonnes != null)
                {
                    foreach (string colonne in colonnes)
                    {
                        donneesFichierCsv.Columns.Add(colonne);
                    }

                    //Obtenir les lignes
                    while (!sr.EndOfStream)
                    {
                        DataRow dr = donneesFichierCsv.NewRow();
                        string[] rows = sr.ReadLine()?.Split(';');
                        for (int champs = 0; champs < colonnes.Length; champs++)
                        {
                            if (rows != null)
                            {
                                dr[champs] = rows[champs];
                            }
                        }
                        if (rows.Length < 1)
                        {
                            Bavard.EcrireMessageTraceSysteme(string.Format("Le fichier csv ne contient pas de règles. " + pPath));
                        }
                        else if (!LigneExiste(donneesFichierCsv, dr))
                        {
                            donneesFichierCsv.Rows.Add(dr);
                        }
                    }

                }
                else
                {
                    Bavard.EcrireMessageTraceSysteme(string.Format("Le fichier csv est vide. Chemin: " + pPath));
                }
            }
            return donneesFichierCsv;
        }
        static bool LigneExiste(DataTable table, DataRow ligneEntree)
        {
            bool retour = false;
            for (int ligne = 0; ligne < table.Rows.Count; ligne++)
            {
                IEqualityComparer<DataRow> comparer = DataRowComparer.Default;
                var li = table.Rows[ligne];
                retour = comparer.Equals(li, ligneEntree);
                if (retour)
                {
                    break;
                }
            }
            return retour;
        }

    }

}