namespace Rq.Infrastructure.Securite.SignatureDigital
{
    partial class FenetreChoixSignature
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FenetreChoixSignature));
            this.lstSignatures = new System.Windows.Forms.ListView();
            this.chSignature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRepertoire = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnVisualiser = new System.Windows.Forms.Button();
            this.btnSelectionner = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstSignatures
            // 
            this.lstSignatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSignatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSignature,
            this.chRepertoire});
            this.lstSignatures.FullRowSelect = true;
            this.lstSignatures.HideSelection = false;
            this.lstSignatures.LabelWrap = false;
            this.lstSignatures.Location = new System.Drawing.Point(12, 12);
            this.lstSignatures.MultiSelect = false;
            this.lstSignatures.Name = "lstSignatures";
            this.lstSignatures.Size = new System.Drawing.Size(529, 165);
            this.lstSignatures.TabIndex = 0;
            this.lstSignatures.UseCompatibleStateImageBehavior = false;
            this.lstSignatures.View = System.Windows.Forms.View.Details;
            this.lstSignatures.SelectedIndexChanged += new System.EventHandler(this.lstSignatures_SelectedIndexChanged);
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
            // btnVisualiser
            // 
            this.btnVisualiser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVisualiser.Location = new System.Drawing.Point(304, 198);
            this.btnVisualiser.Name = "btnVisualiser";
            this.btnVisualiser.Size = new System.Drawing.Size(75, 23);
            this.btnVisualiser.TabIndex = 1;
            this.btnVisualiser.Text = "Visualiser";
            this.btnVisualiser.UseVisualStyleBackColor = true;
            this.btnVisualiser.Click += new System.EventHandler(this.btnVisualiser_Click);
            // 
            // btnSelectionner
            // 
            this.btnSelectionner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectionner.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelectionner.Location = new System.Drawing.Point(385, 198);
            this.btnSelectionner.Name = "btnSelectionner";
            this.btnSelectionner.Size = new System.Drawing.Size(75, 23);
            this.btnSelectionner.TabIndex = 2;
            this.btnSelectionner.Text = "Sélectionner";
            this.btnSelectionner.UseVisualStyleBackColor = true;
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.Location = new System.Drawing.Point(466, 198);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(75, 23);
            this.btnAnnuler.TabIndex = 3;
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.UseVisualStyleBackColor = true;
            // 
            // FenetreChoixSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 233);
            this.ControlBox = false;
            this.Controls.Add(this.btnAnnuler);
            this.Controls.Add(this.btnSelectionner);
            this.Controls.Add(this.btnVisualiser);
            this.Controls.Add(this.lstSignatures);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FenetreChoixSignature";
            this.ShowInTaskbar = false;
            this.Text = "Liste des choix de signature";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstSignatures;
        private System.Windows.Forms.Button btnVisualiser;
        private System.Windows.Forms.Button btnSelectionner;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.ColumnHeader chSignature;
        private System.Windows.Forms.ColumnHeader chRepertoire;
    }
}