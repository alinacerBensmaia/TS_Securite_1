namespace Rq.Infrastructure.Securite.SignatureDigital
{
    partial class FenetreSelectionTypeSignature
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboTypes = new System.Windows.Forms.ComboBox();
            this.btnTransferer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Précisez le type de signature";
            // 
            // cboTypes
            // 
            this.cboTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypes.FormattingEnabled = true;
            this.cboTypes.Location = new System.Drawing.Point(24, 27);
            this.cboTypes.Name = "cboTypes";
            this.cboTypes.Size = new System.Drawing.Size(275, 21);
            this.cboTypes.TabIndex = 1;
            // 
            // btnTransferer
            // 
            this.btnTransferer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransferer.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTransferer.Location = new System.Drawing.Point(143, 70);
            this.btnTransferer.Name = "btnTransferer";
            this.btnTransferer.Size = new System.Drawing.Size(75, 23);
            this.btnTransferer.TabIndex = 2;
            this.btnTransferer.Text = "Transférer";
            this.btnTransferer.UseVisualStyleBackColor = true;
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.Location = new System.Drawing.Point(224, 70);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(75, 23);
            this.btnAnnuler.TabIndex = 3;
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.UseVisualStyleBackColor = true;
            // 
            // FenetreSelectionTypeSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 105);
            this.ControlBox = false;
            this.Controls.Add(this.btnAnnuler);
            this.Controls.Add(this.btnTransferer);
            this.Controls.Add(this.cboTypes);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FenetreSelectionTypeSignature";
            this.ShowInTaskbar = false;
            this.Text = "Sélection type de signature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTypes;
        private System.Windows.Forms.Button btnTransferer;
        private System.Windows.Forms.Button btnAnnuler;
    }
}