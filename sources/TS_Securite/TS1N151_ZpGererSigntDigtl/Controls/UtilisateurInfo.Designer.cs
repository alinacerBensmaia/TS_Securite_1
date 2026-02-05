namespace Rq.Infrastructure.Securite.SignatureDigital.Controls
{
    partial class UtilisateurInfo
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboVille = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtIdentifiant = new System.Windows.Forms.TextBox();
            this.txtNumeroEmploye = new System.Windows.Forms.TextBox();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboVille
            // 
            this.cboVille.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboVille.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVille.Enabled = false;
            this.cboVille.FormattingEnabled = true;
            this.cboVille.Location = new System.Drawing.Point(92, 102);
            this.cboVille.Name = "cboVille";
            this.cboVille.Size = new System.Drawing.Size(293, 21);
            this.cboVille.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-3, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Ville";
            // 
            // txtIdentifiant
            // 
            this.txtIdentifiant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIdentifiant.Location = new System.Drawing.Point(92, 0);
            this.txtIdentifiant.Name = "txtIdentifiant";
            this.txtIdentifiant.ReadOnly = true;
            this.txtIdentifiant.Size = new System.Drawing.Size(293, 20);
            this.txtIdentifiant.TabIndex = 25;
            // 
            // txtNumeroEmploye
            // 
            this.txtNumeroEmploye.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNumeroEmploye.Location = new System.Drawing.Point(92, 76);
            this.txtNumeroEmploye.Name = "txtNumeroEmploye";
            this.txtNumeroEmploye.ReadOnly = true;
            this.txtNumeroEmploye.Size = new System.Drawing.Size(293, 20);
            this.txtNumeroEmploye.TabIndex = 24;
            // 
            // txtNom
            // 
            this.txtNom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNom.Location = new System.Drawing.Point(92, 50);
            this.txtNom.Name = "txtNom";
            this.txtNom.ReadOnly = true;
            this.txtNom.Size = new System.Drawing.Size(293, 20);
            this.txtNom.TabIndex = 23;
            // 
            // txtPrenom
            // 
            this.txtPrenom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrenom.Location = new System.Drawing.Point(92, 24);
            this.txtPrenom.Name = "txtPrenom";
            this.txtPrenom.ReadOnly = true;
            this.txtPrenom.Size = new System.Drawing.Size(293, 20);
            this.txtPrenom.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(-4, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Identifiant";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(-4, 79);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Numéro d\'employé";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(-3, 53);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Nom";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(-4, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Prénom";
            // 
            // UtilisateurInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cboVille);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtIdentifiant);
            this.Controls.Add(this.txtNumeroEmploye);
            this.Controls.Add(this.txtNom);
            this.Controls.Add(this.txtPrenom);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Name = "UtilisateurInfo";
            this.Size = new System.Drawing.Size(385, 123);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboVille;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.TextBox txtIdentifiant;
        internal System.Windows.Forms.TextBox txtNumeroEmploye;
        internal System.Windows.Forms.TextBox txtNom;
        internal System.Windows.Forms.TextBox txtPrenom;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label14;
    }
}
