namespace Rq.Infrastructure.Securite.SignatureDigital
{
    partial class FenetrePrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FenetrePrincipal));
            this.tabChoix = new System.Windows.Forms.TabControl();
            this.tabChoixUtilisateurs = new System.Windows.Forms.TabPage();
            this.grpProfilUtilisateur = new System.Windows.Forms.GroupBox();
            this.grpProfilUtilisateurProfil = new System.Windows.Forms.GroupBox();
            this.lstProfilSignatures = new System.Windows.Forms.ListView();
            this.chSignature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRepertoire = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnProfilSignaturesSupprimer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.profilCoba = new Rq.Infrastructure.Securite.SignatureDigital.Controls.ProfilInfo();
            this.profilUtilisateur = new Rq.Infrastructure.Securite.SignatureDigital.Controls.ProfilInfo();
            this.utilisateurProfil = new Rq.Infrastructure.Securite.SignatureDigital.Controls.UtilisateurInfo();
            this.btnProfilRechercher = new System.Windows.Forms.Button();
            this.txtProfilCompteUtilisateur = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabChoixSignatures = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cboSignatureEnvironnement = new System.Windows.Forms.ComboBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.grpSignatureUtilisateur = new System.Windows.Forms.GroupBox();
            this.txtSignature = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSignatureSupprimer = new System.Windows.Forms.Button();
            this.btnSignatureCopier = new System.Windows.Forms.Button();
            this.btnSignatureVisualiser = new System.Windows.Forms.Button();
            this.utilisateurSignature = new Rq.Infrastructure.Securite.SignatureDigital.Controls.UtilisateurInfo();
            this.btnSignatureRechercher = new System.Windows.Forms.Button();
            this.cboSignatureLoi = new System.Windows.Forms.ComboBox();
            this.txtSignatureCompteUtilisateur = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.ttFenetrePrincipal = new System.Windows.Forms.ToolTip(this.components);
            this.tabChoix.SuspendLayout();
            this.tabChoixUtilisateurs.SuspendLayout();
            this.grpProfilUtilisateur.SuspendLayout();
            this.grpProfilUtilisateurProfil.SuspendLayout();
            this.tabChoixSignatures.SuspendLayout();
            this.grpSignatureUtilisateur.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabChoix
            // 
            this.tabChoix.Controls.Add(this.tabChoixUtilisateurs);
            this.tabChoix.Controls.Add(this.tabChoixSignatures);
            this.tabChoix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChoix.Location = new System.Drawing.Point(0, 0);
            this.tabChoix.Name = "tabChoix";
            this.tabChoix.SelectedIndex = 0;
            this.tabChoix.Size = new System.Drawing.Size(691, 548);
            this.tabChoix.TabIndex = 0;
            // 
            // tabChoixUtilisateurs
            // 
            this.tabChoixUtilisateurs.Controls.Add(this.grpProfilUtilisateur);
            this.tabChoixUtilisateurs.Controls.Add(this.btnProfilRechercher);
            this.tabChoixUtilisateurs.Controls.Add(this.txtProfilCompteUtilisateur);
            this.tabChoixUtilisateurs.Controls.Add(this.label9);
            this.tabChoixUtilisateurs.Location = new System.Drawing.Point(4, 22);
            this.tabChoixUtilisateurs.Name = "tabChoixUtilisateurs";
            this.tabChoixUtilisateurs.Padding = new System.Windows.Forms.Padding(3);
            this.tabChoixUtilisateurs.Size = new System.Drawing.Size(683, 522);
            this.tabChoixUtilisateurs.TabIndex = 0;
            this.tabChoixUtilisateurs.Text = "Gérer les utilisateurs";
            this.tabChoixUtilisateurs.UseVisualStyleBackColor = true;
            // 
            // grpProfilUtilisateur
            // 
            this.grpProfilUtilisateur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProfilUtilisateur.Controls.Add(this.grpProfilUtilisateurProfil);
            this.grpProfilUtilisateur.Controls.Add(this.utilisateurProfil);
            this.grpProfilUtilisateur.Location = new System.Drawing.Point(11, 98);
            this.grpProfilUtilisateur.Name = "grpProfilUtilisateur";
            this.grpProfilUtilisateur.Size = new System.Drawing.Size(661, 417);
            this.grpProfilUtilisateur.TabIndex = 9;
            this.grpProfilUtilisateur.TabStop = false;
            this.grpProfilUtilisateur.Text = "Utilisateur";
            // 
            // grpProfilUtilisateurProfil
            // 
            this.grpProfilUtilisateurProfil.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProfilUtilisateurProfil.Controls.Add(this.lstProfilSignatures);
            this.grpProfilUtilisateurProfil.Controls.Add(this.btnProfilSignaturesSupprimer);
            this.grpProfilUtilisateurProfil.Controls.Add(this.label2);
            this.grpProfilUtilisateurProfil.Controls.Add(this.profilCoba);
            this.grpProfilUtilisateurProfil.Controls.Add(this.profilUtilisateur);
            this.grpProfilUtilisateurProfil.Location = new System.Drawing.Point(7, 164);
            this.grpProfilUtilisateurProfil.Name = "grpProfilUtilisateurProfil";
            this.grpProfilUtilisateurProfil.Size = new System.Drawing.Size(648, 247);
            this.grpProfilUtilisateurProfil.TabIndex = 1;
            this.grpProfilUtilisateurProfil.TabStop = false;
            this.grpProfilUtilisateurProfil.Text = "Profil";
            // 
            // lstProfilSignatures
            // 
            this.lstProfilSignatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstProfilSignatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSignature,
            this.chRepertoire});
            this.lstProfilSignatures.FullRowSelect = true;
            this.lstProfilSignatures.HideSelection = false;
            this.lstProfilSignatures.Location = new System.Drawing.Point(14, 125);
            this.lstProfilSignatures.MultiSelect = false;
            this.lstProfilSignatures.Name = "lstProfilSignatures";
            this.lstProfilSignatures.Size = new System.Drawing.Size(599, 116);
            this.lstProfilSignatures.TabIndex = 23;
            this.lstProfilSignatures.UseCompatibleStateImageBehavior = false;
            this.lstProfilSignatures.View = System.Windows.Forms.View.Details;
            // 
            // chSignature
            // 
            this.chSignature.Text = "Nom signature";
            this.chSignature.Width = 120;
            // 
            // chRepertoire
            // 
            this.chRepertoire.Text = "Répertoire";
            this.chRepertoire.Width = 400;
            // 
            // btnProfilSignaturesSupprimer
            // 
            this.btnProfilSignaturesSupprimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfilSignaturesSupprimer.Enabled = false;
            this.btnProfilSignaturesSupprimer.Image = ((System.Drawing.Image)(resources.GetObject("btnProfilSignaturesSupprimer.Image")));
            this.btnProfilSignaturesSupprimer.Location = new System.Drawing.Point(619, 218);
            this.btnProfilSignaturesSupprimer.Name = "btnProfilSignaturesSupprimer";
            this.btnProfilSignaturesSupprimer.Size = new System.Drawing.Size(23, 23);
            this.btnProfilSignaturesSupprimer.TabIndex = 22;
            this.ttFenetrePrincipal.SetToolTip(this.btnProfilSignaturesSupprimer, "Supprimer les signatures");
            this.btnProfilSignaturesSupprimer.UseVisualStyleBackColor = true;
            this.btnProfilSignaturesSupprimer.Click += new System.EventHandler(this.btnProfilSignaturesSupprimer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Signatures";
            // 
            // profilCoba
            // 
            this.profilCoba.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profilCoba.BackColor = System.Drawing.Color.Transparent;
            this.profilCoba.Location = new System.Drawing.Point(14, 61);
            this.profilCoba.MessageCreation = "Voulez-vous créer le profil Coba en production?";
            this.profilCoba.MessageSuppression = "Voulez-vous supprimer le profil Coba en production?";
            this.profilCoba.Name = "profilCoba";
            this.profilCoba.Size = new System.Drawing.Size(628, 36);
            this.profilCoba.TabIndex = 19;
            this.profilCoba.Text = "Profil Coba";
            // 
            // profilUtilisateur
            // 
            this.profilUtilisateur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profilUtilisateur.BackColor = System.Drawing.Color.Transparent;
            this.profilUtilisateur.Location = new System.Drawing.Point(14, 19);
            this.profilUtilisateur.MessageCreation = "Voulez-vous créer le profil utilisateur?";
            this.profilUtilisateur.MessageSuppression = "Voulez-vous supprimer le profil utilisateur?";
            this.profilUtilisateur.Name = "profilUtilisateur";
            this.profilUtilisateur.Size = new System.Drawing.Size(628, 36);
            this.profilUtilisateur.TabIndex = 18;
            this.profilUtilisateur.Text = "Profil utilisateur";
            // 
            // utilisateurProfil
            // 
            this.utilisateurProfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.utilisateurProfil.BackColor = System.Drawing.Color.Transparent;
            this.utilisateurProfil.Location = new System.Drawing.Point(11, 19);
            this.utilisateurProfil.Name = "utilisateurProfil";
            this.utilisateurProfil.Size = new System.Drawing.Size(644, 139);
            this.utilisateurProfil.TabIndex = 0;
            // 
            // btnProfilRechercher
            // 
            this.btnProfilRechercher.Location = new System.Drawing.Point(18, 69);
            this.btnProfilRechercher.Name = "btnProfilRechercher";
            this.btnProfilRechercher.Size = new System.Drawing.Size(118, 23);
            this.btnProfilRechercher.TabIndex = 8;
            this.btnProfilRechercher.Text = "Rechercher";
            this.btnProfilRechercher.UseVisualStyleBackColor = true;
            this.btnProfilRechercher.Click += new System.EventHandler(this.btnProfilRechercher_Click);
            // 
            // txtProfilCompteUtilisateur
            // 
            this.txtProfilCompteUtilisateur.Location = new System.Drawing.Point(18, 32);
            this.txtProfilCompteUtilisateur.Name = "txtProfilCompteUtilisateur";
            this.txtProfilCompteUtilisateur.Size = new System.Drawing.Size(118, 20);
            this.txtProfilCompteUtilisateur.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Compte utilisateur";
            // 
            // tabChoixSignatures
            // 
            this.tabChoixSignatures.Controls.Add(this.label7);
            this.tabChoixSignatures.Controls.Add(this.cboSignatureEnvironnement);
            this.tabChoixSignatures.Controls.Add(this.Label6);
            this.tabChoixSignatures.Controls.Add(this.grpSignatureUtilisateur);
            this.tabChoixSignatures.Controls.Add(this.btnSignatureRechercher);
            this.tabChoixSignatures.Controls.Add(this.cboSignatureLoi);
            this.tabChoixSignatures.Controls.Add(this.txtSignatureCompteUtilisateur);
            this.tabChoixSignatures.Controls.Add(this.Label1);
            this.tabChoixSignatures.Location = new System.Drawing.Point(4, 22);
            this.tabChoixSignatures.Name = "tabChoixSignatures";
            this.tabChoixSignatures.Padding = new System.Windows.Forms.Padding(3);
            this.tabChoixSignatures.Size = new System.Drawing.Size(683, 522);
            this.tabChoixSignatures.TabIndex = 1;
            this.tabChoixSignatures.Text = "Gérer les signatures";
            this.tabChoixSignatures.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Environnement";
            // 
            // cboSignatureEnvironnement
            // 
            this.cboSignatureEnvironnement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSignatureEnvironnement.FormattingEnabled = true;
            this.cboSignatureEnvironnement.Location = new System.Drawing.Point(154, 32);
            this.cboSignatureEnvironnement.Name = "cboSignatureEnvironnement";
            this.cboSignatureEnvironnement.Size = new System.Drawing.Size(249, 21);
            this.cboSignatureEnvironnement.TabIndex = 9;
            this.cboSignatureEnvironnement.SelectedIndexChanged += new System.EventHandler(this.cboSignatureEnvironnement_SelectedIndexChanged);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(415, 13);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(21, 13);
            this.Label6.TabIndex = 8;
            this.Label6.Text = "Loi";
            // 
            // grpSignatureUtilisateur
            // 
            this.grpSignatureUtilisateur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSignatureUtilisateur.Controls.Add(this.txtSignature);
            this.grpSignatureUtilisateur.Controls.Add(this.label8);
            this.grpSignatureUtilisateur.Controls.Add(this.btnSignatureSupprimer);
            this.grpSignatureUtilisateur.Controls.Add(this.btnSignatureCopier);
            this.grpSignatureUtilisateur.Controls.Add(this.btnSignatureVisualiser);
            this.grpSignatureUtilisateur.Controls.Add(this.utilisateurSignature);
            this.grpSignatureUtilisateur.Location = new System.Drawing.Point(11, 98);
            this.grpSignatureUtilisateur.Name = "grpSignatureUtilisateur";
            this.grpSignatureUtilisateur.Size = new System.Drawing.Size(661, 251);
            this.grpSignatureUtilisateur.TabIndex = 6;
            this.grpSignatureUtilisateur.TabStop = false;
            this.grpSignatureUtilisateur.Text = "Utilisateur";
            // 
            // txtSignature
            // 
            this.txtSignature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSignature.BackColor = System.Drawing.SystemColors.Control;
            this.txtSignature.Location = new System.Drawing.Point(115, 186);
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.ReadOnly = true;
            this.txtSignature.Size = new System.Drawing.Size(540, 20);
            this.txtSignature.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Signature";
            // 
            // btnSignatureSupprimer
            // 
            this.btnSignatureSupprimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignatureSupprimer.Location = new System.Drawing.Point(295, 219);
            this.btnSignatureSupprimer.Name = "btnSignatureSupprimer";
            this.btnSignatureSupprimer.Size = new System.Drawing.Size(178, 23);
            this.btnSignatureSupprimer.TabIndex = 18;
            this.btnSignatureSupprimer.Text = "Supprimer";
            this.btnSignatureSupprimer.UseVisualStyleBackColor = true;
            this.btnSignatureSupprimer.Click += new System.EventHandler(this.btnSignatureSupprimer_Click);
            // 
            // btnSignatureCopier
            // 
            this.btnSignatureCopier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignatureCopier.Location = new System.Drawing.Point(477, 219);
            this.btnSignatureCopier.Name = "btnSignatureCopier";
            this.btnSignatureCopier.Size = new System.Drawing.Size(178, 23);
            this.btnSignatureCopier.TabIndex = 17;
            this.btnSignatureCopier.Text = "Transférer vers production";
            this.btnSignatureCopier.UseVisualStyleBackColor = true;
            this.btnSignatureCopier.Click += new System.EventHandler(this.btnSignatureCopier_Click);
            // 
            // btnSignatureVisualiser
            // 
            this.btnSignatureVisualiser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignatureVisualiser.Location = new System.Drawing.Point(112, 219);
            this.btnSignatureVisualiser.Name = "btnSignatureVisualiser";
            this.btnSignatureVisualiser.Size = new System.Drawing.Size(178, 23);
            this.btnSignatureVisualiser.TabIndex = 16;
            this.btnSignatureVisualiser.Text = "Visualiser";
            this.btnSignatureVisualiser.UseVisualStyleBackColor = true;
            this.btnSignatureVisualiser.Click += new System.EventHandler(this.btnSignatureVisualiser_Click);
            // 
            // utilisateurSignature
            // 
            this.utilisateurSignature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.utilisateurSignature.BackColor = System.Drawing.Color.Transparent;
            this.utilisateurSignature.Location = new System.Drawing.Point(11, 19);
            this.utilisateurSignature.Name = "utilisateurSignature";
            this.utilisateurSignature.Size = new System.Drawing.Size(644, 128);
            this.utilisateurSignature.TabIndex = 24;
            // 
            // btnSignatureRechercher
            // 
            this.btnSignatureRechercher.Location = new System.Drawing.Point(18, 69);
            this.btnSignatureRechercher.Name = "btnSignatureRechercher";
            this.btnSignatureRechercher.Size = new System.Drawing.Size(118, 23);
            this.btnSignatureRechercher.TabIndex = 5;
            this.btnSignatureRechercher.Text = "Rechercher";
            this.btnSignatureRechercher.UseVisualStyleBackColor = true;
            this.btnSignatureRechercher.Click += new System.EventHandler(this.btnSignatureRechercher_Click);
            // 
            // cboSignatureLoi
            // 
            this.cboSignatureLoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSignatureLoi.FormattingEnabled = true;
            this.cboSignatureLoi.Location = new System.Drawing.Point(423, 32);
            this.cboSignatureLoi.Name = "cboSignatureLoi";
            this.cboSignatureLoi.Size = new System.Drawing.Size(249, 21);
            this.cboSignatureLoi.TabIndex = 7;
            this.cboSignatureLoi.SelectedIndexChanged += new System.EventHandler(this.cboSignatureLoi_SelectedIndexChanged);
            // 
            // txtSignatureCompteUtilisateur
            // 
            this.txtSignatureCompteUtilisateur.Location = new System.Drawing.Point(18, 32);
            this.txtSignatureCompteUtilisateur.Name = "txtSignatureCompteUtilisateur";
            this.txtSignatureCompteUtilisateur.Size = new System.Drawing.Size(118, 20);
            this.txtSignatureCompteUtilisateur.TabIndex = 4;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(8, 12);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(90, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Compte utilisateur";
            // 
            // FenetrePrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 548);
            this.Controls.Add(this.tabChoix);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FenetrePrincipal";
            this.Text = "Gestion de l\'opérationnel";
            this.tabChoix.ResumeLayout(false);
            this.tabChoixUtilisateurs.ResumeLayout(false);
            this.tabChoixUtilisateurs.PerformLayout();
            this.grpProfilUtilisateur.ResumeLayout(false);
            this.grpProfilUtilisateurProfil.ResumeLayout(false);
            this.grpProfilUtilisateurProfil.PerformLayout();
            this.tabChoixSignatures.ResumeLayout(false);
            this.tabChoixSignatures.PerformLayout();
            this.grpSignatureUtilisateur.ResumeLayout(false);
            this.grpSignatureUtilisateur.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabChoix;
        private System.Windows.Forms.TabPage tabChoixUtilisateurs;
        private System.Windows.Forms.TabPage tabChoixSignatures;
        internal System.Windows.Forms.Button btnSignatureRechercher;
        internal System.Windows.Forms.TextBox txtSignatureCompteUtilisateur;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.GroupBox grpSignatureUtilisateur;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox cboSignatureLoi;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ComboBox cboSignatureEnvironnement;
        internal System.Windows.Forms.Button btnSignatureSupprimer;
        internal System.Windows.Forms.Button btnSignatureCopier;
        internal System.Windows.Forms.Button btnSignatureVisualiser;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtSignature;
        internal System.Windows.Forms.Button btnProfilRechercher;
        internal System.Windows.Forms.TextBox txtProfilCompteUtilisateur;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grpProfilUtilisateur;
        private Controls.UtilisateurInfo utilisateurProfil;
        private Controls.UtilisateurInfo utilisateurSignature;
        private System.Windows.Forms.GroupBox grpProfilUtilisateurProfil;
        private Controls.ProfilInfo profilCoba;
        private Controls.ProfilInfo profilUtilisateur;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProfilSignaturesSupprimer;
        private System.Windows.Forms.ToolTip ttFenetrePrincipal;
        private System.Windows.Forms.ListView lstProfilSignatures;
        private System.Windows.Forms.ColumnHeader chSignature;
        private System.Windows.Forms.ColumnHeader chRepertoire;
    }
}