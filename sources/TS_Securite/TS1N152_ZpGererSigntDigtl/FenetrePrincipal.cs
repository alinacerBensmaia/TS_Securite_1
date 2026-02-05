using System;
using System.Windows.Forms;
using Rq.Infrastructure.Securite.SignatureDigital.Internes;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures;

namespace Rq.Infrastructure.Securite.SignatureDigital
{
    public partial class FenetrePrincipal : Form
    {
        public FenetrePrincipal()
        {
            InitializeComponent();
            cboSignatureLoi.PopulerLois();
            cboSignatureEnvironnement.PopulerEnvironnements();
        }


        private void afficherExceptionInnatendu(Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        #region " Gérer signatures - événements "

        private bool _enSignatureRecherche = false;

        private void btnSignatureRechercher_Click(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                {
                    viderChampsSignatures();
                    chercherSignature();
                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }

        }

        private void cboSignatureEnvironnement_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                    viderChampsSignatures();
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }

        private void cboSignatureLoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                    viderChampsSignatures();
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }

        private void btnSignatureVisualiser_Click(object sender, EventArgs e)
        {
            try
            {
                var signature = SignatureCourante;
                new Signatures().Visualiser(signature);
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }                   
        }

        private void btnSignatureSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                {
                    var signatures = new Signatures();
                    var signature = signatures.SupprimerAvecMessage(SignatureCourante);
                    afficherSignature(signature, null, null);
                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }

        private void btnSignatureCopier_Click(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                using (var f = new FenetreSelectionTypeSignature())
                {
                    var result = f.ShowDialog();
                    if (result != DialogResult.OK) return;

                    var type = f.Selection;
                    var utilisateur = utilisateurSignature.UtilisateurCourant;
                    var signature = SignatureCourante;

                    var signatures = new Signatures();
                    if (signatures.CopierVersProduction(signature, utilisateur, type))
                    {
                        viderChampsSignatures(true);
                        cboSignatureEnvironnement.Selectionner(Environnements.Production);
                        chercherSignature();
                    }
                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }

        #endregion

        #region " Gérer signatures "

        private Signature SignatureCourante
        {
            get { return (Signature)txtSignature.Tag; }
            set { txtSignature.Tag = value; }
        }

        private void viderChampsSignatures()
        {
            viderChampsSignatures(false);
        }

        private void viderChampsSignatures(bool signatureSeulement)
        {
            //ne pas vider les champs si nous sommes en affichage des resultats de recherche
            if (_enSignatureRecherche) return;

            txtSignature.Clear();
            SignatureCourante = null;

            if (!signatureSeulement)
                utilisateurSignature.Nettoyer();

            btnSignatureVisualiser.Enabled = false;
            btnSignatureSupprimer.Enabled = false;
            btnSignatureCopier.Enabled = false;
        }

        private void chercherSignature()
        {
            try
            {
                _enSignatureRecherche = true;

                var ad = new ActiveDirectory();
                var utilisateur = ad.ObtenirUtilisateur(txtSignatureCompteUtilisateur.Text);

                if (!utilisateur.Valide)
                    MessageBox.Show("Compte utilisateur non valide", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    utilisateurSignature.Afficher(utilisateur);

                    var env = cboSignatureEnvironnement.Selection<Environnements>();
                    var loi = cboSignatureLoi.Selection<Lois>();

                    var signatures = new Signatures();
                    var signature = signatures.Rechercher(env, utilisateur, loi);
                    afficherSignature(signature, env, loi);
                }
            }
            finally
            {
                _enSignatureRecherche = false;
            }
        }
        
        private void afficherSignature(Signature signature, Environnements env, Lois loi)
        {
            if (signature.Valide)
            {
                if (loi.Est(Lois.Toutes))
                    cboSignatureLoi.Selectionner(signature.Loi);

                txtSignature.Text = signature.FullPath;
                SignatureCourante = signature;

                btnSignatureVisualiser.Enabled = true;
                btnSignatureSupprimer.Enabled = true;
                btnSignatureCopier.Enabled = env.Est(Environnements.Depot);
            }
            else
            {
                txtSignature.Text = "Aucune";
                SignatureCourante = null;

                btnSignatureVisualiser.Enabled = false;
                btnSignatureSupprimer.Enabled = false;
                btnSignatureCopier.Enabled = false;
            }
        }

        #endregion

        #region " Gérer utilisateurs - événements "

        private void btnProfilRechercher_Click(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                {
                    viderChampsProfil();
                    rechercherPourProfil();
                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }       
        }

        private void btnProfilSignaturesSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                var message = "Voulez-vous supprimer la ou les signatures?";
                var title = "Suppresion de signatures";

                var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (this.CurseurWait())
                    {
                        var journal = new JournalEvenements();
                        var signatures = new Signatures();

                        foreach (ListViewItem item in lstProfilSignatures.Items)
                        {
                            var signature = (Signature)item.Tag;
                            var copie = signature;

                            signatures.Supprimer(signature);
                            journal.JournaliserSuppressionSignature(copie);
                        }
                    }
                    rafraichirProfilSignature();
                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }

        #endregion  

        #region " Gérer utilisateurs "

        private void rechercherPourProfil()
        { 
            var ad = new ActiveDirectory();
            var utilisateur = ad.ObtenirUtilisateur(txtProfilCompteUtilisateur.Text);

            if (!utilisateur.Valide)
                MessageBox.Show("Compte utilisateur non valide", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                utilisateurProfil.Afficher(utilisateur);

                profilUtilisateur.Afficher(new ProfilUtilisateur(utilisateur));
                profilCoba.Afficher(new ProfilCoba(utilisateur));

                rafraichirProfilSignature();
            }
        }

        private void rafraichirProfilSignature()
        {
            var utilisateur = utilisateurProfil.UtilisateurCourant;

            var signatures = new Signatures();
            var siganturesUtilisateur = signatures.ObtenirToutesSignaturesProduction(utilisateur);
            afficherProfilSignatures(siganturesUtilisateur);
        }

        private void afficherProfilSignatures(Signature[] signatures)
        {
            lstProfilSignatures.Items.Clear();
            btnProfilSignaturesSupprimer.Enabled = false;

            if (signatures.Length == 0) return;

            foreach (var signature in signatures)
            {
                var item = new ListViewItem();
                item.Tag = signature;
                item.Text = signature.Fichier;
                item.SubItems.Add(signature.Emplacement);

                lstProfilSignatures.Items.Add(item);
            }
            lstProfilSignatures.SelectedIndices.Clear();
            btnProfilSignaturesSupprimer.Enabled = true;
        }

        private void viderChampsProfil()
        {
            utilisateurProfil.Nettoyer();

            profilUtilisateur.Nettoyer();
            profilCoba.Nettoyer();
            lstProfilSignatures.Items.Clear();
            btnProfilSignaturesSupprimer.Enabled = false;
        }

        #endregion
    }
}
