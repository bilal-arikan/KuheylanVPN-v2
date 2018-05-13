namespace Kuheylan
{
    partial class VpnListViewItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelIp = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPass = new System.Windows.Forms.Label();
            this.linkLabelSite = new System.Windows.Forms.LinkLabel();
            this.labelProt = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelRegion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelIp
            // 
            this.labelIp.AutoSize = true;
            this.labelIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelIp.Location = new System.Drawing.Point(55, 17);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(96, 13);
            this.labelIp.TabIndex = 2;
            this.labelIp.Text = "178.62.149.124";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(56, 31);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(29, 13);
            this.labelUser.TabIndex = 2;
            this.labelUser.Text = "User";
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Location = new System.Drawing.Point(55, 44);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(30, 13);
            this.labelPass.TabIndex = 2;
            this.labelPass.Text = "Pass";
            // 
            // linkLabelSite
            // 
            this.linkLabelSite.AutoSize = true;
            this.linkLabelSite.Location = new System.Drawing.Point(55, 2);
            this.linkLabelSite.Name = "linkLabelSite";
            this.linkLabelSite.Size = new System.Drawing.Size(115, 13);
            this.linkLabelSite.TabIndex = 3;
            this.linkLabelSite.TabStop = true;
            this.linkLabelSite.Text = "http://dfgsfgsdfgs.com";
            // 
            // labelProt
            // 
            this.labelProt.AutoSize = true;
            this.labelProt.Location = new System.Drawing.Point(55, 57);
            this.labelProt.Name = "labelProt";
            this.labelProt.Size = new System.Drawing.Size(33, 13);
            this.labelProt.TabIndex = 2;
            this.labelProt.Text = "L2TP";
            // 
            // labelCountry
            // 
            this.labelCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(224, 9);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelCountry.Size = new System.Drawing.Size(35, 13);
            this.labelCountry.TabIndex = 4;
            this.labelCountry.Text = "label1";
            this.labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCity
            // 
            this.labelCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(224, 24);
            this.labelCity.Name = "labelCity";
            this.labelCity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelCity.Size = new System.Drawing.Size(35, 13);
            this.labelCity.TabIndex = 5;
            this.labelCity.Text = "label2";
            this.labelCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRegion
            // 
            this.labelRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRegion.AutoSize = true;
            this.labelRegion.Location = new System.Drawing.Point(224, 41);
            this.labelRegion.Name = "labelRegion";
            this.labelRegion.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelRegion.Size = new System.Drawing.Size(35, 13);
            this.labelRegion.TabIndex = 6;
            this.labelRegion.Text = "label3";
            this.labelRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(4, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // VpnListViewItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelRegion);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.linkLabelSite);
            this.Controls.Add(this.labelProt);
            this.Controls.Add(this.labelPass);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelIp);
            this.Name = "VpnListViewItem";
            this.Size = new System.Drawing.Size(316, 74);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.Label labelIp;
        protected System.Windows.Forms.Label labelUser;
        protected System.Windows.Forms.Label labelPass;
        protected System.Windows.Forms.LinkLabel linkLabelSite;
        protected System.Windows.Forms.Label labelProt;
        protected System.Windows.Forms.Label labelCountry;
        protected System.Windows.Forms.Label labelCity;
        protected System.Windows.Forms.Label labelRegion;
        protected System.Windows.Forms.PictureBox pictureBox1;
    }
}
