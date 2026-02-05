using Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures
{
    internal class Signatures
    {
        public Signature[] ObtenirToutesSignaturesProduction(Utilisateur utilisateur)
        {
            return rechercherSignatures(utilisateur, Lois.Toutes, AppConfiguration.RepertoireSignatureProduction).ToArray();
        }

        public Signature Rechercher(Environnements environnement, Utilisateur utilisateur, Lois loi)
        {
            if (environnement.Est(Environnements.Depot))
                return rechercherDepot(utilisateur, loi);
            else if (environnement.Est(Environnements.Production))
                return rechercherProduction(utilisateur, loi);
            throw new ArgumentException("environnement");
        }
        
        public void Visualiser(Signature signature)
        {
            if (!File.Exists(signature.FullPath)) return;

            Process.Start(signature.FullPath);
        }

        public Signature SupprimerAvecMessage(Signature signature)
        {
            if (!File.Exists(signature.FullPath)) return Signature.Invalide;

            var message = string.Format("Voulez-vous supprimer la signature {0} du dépot ?", signature.Fichier);
            var title = "Suppresion de signature";

            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                return Supprimer(signature);
            return signature;
        }
        public Signature Supprimer(Signature signature)
        {
            if (!File.Exists(signature.FullPath)) return Signature.Invalide;

            File.Delete(signature.FullPath);
            return Signature.Invalide;
        }

        public bool CopierVersProduction(Signature signature, Utilisateur utilisateur, SignatureTypes type)
        {
            var source = signature.FullPath;
            if (!File.Exists(source)) return false;

            try
            {
                foreach (var formatter in type.Formatters)
                {
                    var nouveauNom = formatter.Format(utilisateur);
                    var cibleDir = Path.Combine(AppConfiguration.RepertoireSignatureProduction, signature.Loi.Dossier);
                    var cible = Path.Combine(cibleDir, nouveauNom);

                    if (File.Exists(cible))
                    {
                        var message = string.Format("{0}{1}{2}", "Copie annuler, la signature cible existe déjà.", System.Environment.NewLine, cible);
                        MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //copier
                    var copieur = new Xcopy(source, cible);
                    copieur.Executer();

                    //assigner securité au fichier
                    var securite = new SecuriteFichier(cible);
                    securite.AjouterAccesLectureEtExecution(utilisateur.IdentifiantAvecDomaine);
                }
                //supprimer l'original
                File.Delete(source);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private Signature rechercherProduction(Utilisateur utilisateur, Lois loi)
        {
            var trouvailles = rechercherSignatures(utilisateur, loi, AppConfiguration.RepertoireSignatureProduction);

            if (trouvailles.Count == 1)
                return trouvailles[0];
            if (trouvailles.Count > 1)
                return selectionnerUneSeuleSignature(trouvailles);

            return Signature.Invalide;
        }

        private Signature rechercherDepot(Utilisateur utilisateur, Lois loi)
        {
            var trouvailles = rechercherSignatures(utilisateur, loi, AppConfiguration.RepertoireSignatureDepot);

            //si on ne trouve rien pour l'utilisateur retourner toutes les signatures
            if (trouvailles.Count == 0)
                trouvailles = obtenirToutesSignatures(loi, AppConfiguration.RepertoireSignatureDepot);

            if (trouvailles.Count == 1)
                return trouvailles[0];
            if (trouvailles.Count > 1)
                return selectionnerUneSeuleSignature(trouvailles);

            return Signature.Invalide;
        }

        private List<Signature> rechercherSignatures(Utilisateur utilisateur, Lois loi, string repertoire)
        {
            var fichiersSignature = obtenirFichiersSignaturePossible(utilisateur);

            //determiner les repertoires a rechercher (nom des lois)
            var chercherPourLois = new List<Lois>();
            if (loi.Est(Lois.Toutes))
                chercherPourLois.AddRange(Lois.All);
            else
                chercherPourLois.Add(loi);

            var trouvailles = new List<Signature>();
            foreach (var fichier in fichiersSignature)
            {
                foreach (var loiSpecifique in chercherPourLois)
                {
                    var dir = Path.Combine(repertoire, loiSpecifique.Dossier);
                    var fullPath = Path.Combine(dir, fichier);

                    if (File.Exists(fullPath))
                        trouvailles.Add(new Signature(loiSpecifique, dir, fichier));
                }
            }

            return trouvailles;
        }

        private List<Signature> obtenirToutesSignatures(Lois loi, string repertoire)
        {
            var fichier = "*.tif";

            //determiner les repertoires a rechercher (nom des lois)
            var chercherPourLois = new List<Lois>();
            if (loi.Est(Lois.Toutes))
                chercherPourLois.AddRange(Lois.All);
            else
                chercherPourLois.Add(loi);

            var trouvailles = new List<Signature>();
            foreach (var loiSpecifique in chercherPourLois)
            {
                var dir = Path.Combine(repertoire, loiSpecifique.Dossier);
                var info = new DirectoryInfo(dir);
                var files = info.GetFiles(fichier);
                foreach (var file in files)
                    trouvailles.Add(new Signature(loiSpecifique, dir, file.Name));
            }

            return trouvailles;
        }

        private List<string> obtenirFichiersSignaturePossible(Utilisateur utilisateur)
        {
            var retour = new List<string>();
            retour.Add(new EmployeeNumberSignatureFormatter().Format(utilisateur));
            retour.Add(new EmployeeNameSignatureFormatter().Format(utilisateur));
            return retour;
        }

        private Signature selectionnerUneSeuleSignature(List<Signature> signatures)
        {
            using (var f = new FenetreChoixSignature())
            {
                f.Populer(signatures);

                var result = f.ShowDialog();
                if (result == DialogResult.OK)
                    return f.Selection;
            }
            return Signature.Invalide;
        }
    }
}
