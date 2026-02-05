namespace RRQ.Infrastructure.Securite
{
    partial class FenetrePrincipal
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FenetrePrincipal));
            this.btnChargerSrce = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton();
            this.lblSource = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel();
            this.txtRepSrce = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox();
            this.xzCrLabel1 = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel();
            this.txtRepDest = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox();
            this.btnChargerDest = new Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton();
            this.SuspendLayout();
            // 
            // btnChargerSrce
            // 
            this.btnChargerSrce.Location = new System.Drawing.Point(377, 65);
            this.btnChargerSrce.Name = "btnChargerSrce";
            this.btnChargerSrce.Size = new System.Drawing.Size(75, 23);
            this.btnChargerSrce.TabIndex = 0;
            this.btnChargerSrce.Text = "Charger";
            this.btnChargerSrce.UseVisualStyleBackColor = true;
            this.btnChargerSrce.Click += new System.EventHandler(this.btnChargerSrce_Click);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(6, 14);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(94, 13);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Répertoire source:";
            // 
            // txtRepSrce
            // 
            this.txtRepSrce.Location = new System.Drawing.Point(9, 39);
            this.txtRepSrce.Name = "txtRepSrce";
            this.txtRepSrce.NomChampsDonnee = null;
            this.txtRepSrce.NomSourceDonnee = null;
            this.txtRepSrce.Size = new System.Drawing.Size(443, 20);
            this.txtRepSrce.TabIndex = 2;
            // 
            // xzCrLabel1
            // 
            this.xzCrLabel1.AutoSize = true;
            this.xzCrLabel1.Location = new System.Drawing.Point(499, 14);
            this.xzCrLabel1.Name = "xzCrLabel1";
            this.xzCrLabel1.Size = new System.Drawing.Size(113, 13);
            this.xzCrLabel1.TabIndex = 3;
            this.xzCrLabel1.Text = "Répertoire destination:";
            // 
            // txtRepDest
            // 
            this.txtRepDest.Location = new System.Drawing.Point(502, 39);
            this.txtRepDest.Name = "txtRepDest";
            this.txtRepDest.NomChampsDonnee = null;
            this.txtRepDest.NomSourceDonnee = null;
            this.txtRepDest.Size = new System.Drawing.Size(443, 20);
            this.txtRepDest.TabIndex = 4;
            // 
            // btnChargerDest
            // 
            this.btnChargerDest.Location = new System.Drawing.Point(870, 65);
            this.btnChargerDest.Name = "btnChargerDest";
            this.btnChargerDest.Size = new System.Drawing.Size(75, 23);
            this.btnChargerDest.TabIndex = 5;
            this.btnChargerDest.Text = "Charger";
            this.btnChargerDest.UseVisualStyleBackColor = true;
            // 
            // FenetrePrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 535);
            this.Controls.Add(this.btnChargerDest);
            this.Controls.Add(this.txtRepDest);
            this.Controls.Add(this.xzCrLabel1);
            this.Controls.Add(this.txtRepSrce);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.btnChargerSrce);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FenetrePrincipal";
            this.Text = "Sécurisation dépôts NTFS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton btnChargerSrce;
        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel lblSource;
        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox txtRepSrce;
        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel xzCrLabel1;
        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox txtRepDest;
        private Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton btnChargerDest;
    }
}

