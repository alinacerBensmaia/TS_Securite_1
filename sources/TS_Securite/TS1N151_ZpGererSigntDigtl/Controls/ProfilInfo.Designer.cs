namespace Rq.Infrastructure.Securite.SignatureDigital.Controls
{
    partial class ProfilInfo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfilInfo));
            this.lblProfil = new System.Windows.Forms.Label();
            this.txtProfil = new System.Windows.Forms.TextBox();
            this.btnCreer = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.ttProfil = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblProfil
            // 
            this.lblProfil.AutoSize = true;
            this.lblProfil.Location = new System.Drawing.Point(-3, 0);
            this.lblProfil.Name = "lblProfil";
            this.lblProfil.Size = new System.Drawing.Size(66, 13);
            this.lblProfil.TabIndex = 0;
            this.lblProfil.Text = "<undefined>";
            // 
            // txtProfil
            // 
            this.txtProfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProfil.Location = new System.Drawing.Point(0, 16);
            this.txtProfil.Name = "txtProfil";
            this.txtProfil.ReadOnly = true;
            this.txtProfil.Size = new System.Drawing.Size(417, 20);
            this.txtProfil.TabIndex = 1;
            this.txtProfil.TabStop = false;
            // 
            // btnCreer
            // 
            this.btnCreer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreer.Enabled = false;
            this.btnCreer.Image = ((System.Drawing.Image)(resources.GetObject("btnCreer.Image")));
            this.btnCreer.Location = new System.Drawing.Point(423, 14);
            this.btnCreer.Name = "btnCreer";
            this.btnCreer.Size = new System.Drawing.Size(23, 23);
            this.btnCreer.TabIndex = 2;
            this.ttProfil.SetToolTip(this.btnCreer, "Créer");
            this.btnCreer.UseVisualStyleBackColor = true;
            this.btnCreer.Click += new System.EventHandler(this.btnCreer_Click);
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSupprimer.Enabled = false;
            this.btnSupprimer.Image = ((System.Drawing.Image)(resources.GetObject("btnSupprimer.Image")));
            this.btnSupprimer.Location = new System.Drawing.Point(452, 14);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Size = new System.Drawing.Size(23, 23);
            this.btnSupprimer.TabIndex = 3;
            this.ttProfil.SetToolTip(this.btnSupprimer, "Supprimer");
            this.btnSupprimer.UseVisualStyleBackColor = true;
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // ProfilInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnSupprimer);
            this.Controls.Add(this.btnCreer);
            this.Controls.Add(this.txtProfil);
            this.Controls.Add(this.lblProfil);
            this.Name = "ProfilInfo";
            this.Size = new System.Drawing.Size(474, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProfil;
        private System.Windows.Forms.TextBox txtProfil;
        private System.Windows.Forms.Button btnCreer;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.ToolTip ttProfil;
    }
}
