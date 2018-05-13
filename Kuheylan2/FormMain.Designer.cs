namespace Kuheylan
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonX = new System.Windows.Forms.Button();
            this.buttonS = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.paneProgress = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonLoadJson = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.paneProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // buttonX
            // 
            this.buttonX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX.FlatAppearance.BorderSize = 2;
            this.buttonX.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonX.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkRed;
            this.buttonX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.buttonX.ForeColor = System.Drawing.Color.Black;
            this.buttonX.Location = new System.Drawing.Point(397, 12);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(29, 25);
            this.buttonX.TabIndex = 35;
            this.buttonX.Text = "X";
            this.buttonX.UseVisualStyleBackColor = false;
            this.buttonX.Click += new System.EventHandler(this.buttonX_Click);
            // 
            // buttonS
            // 
            this.buttonS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SandyBrown;
            this.buttonS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.buttonS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonS.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.buttonS.Location = new System.Drawing.Point(362, 12);
            this.buttonS.Name = "buttonS";
            this.buttonS.Size = new System.Drawing.Size(29, 25);
            this.buttonS.TabIndex = 34;
            this.buttonS.Text = "---";
            this.buttonS.UseVisualStyleBackColor = false;
            this.buttonS.Click += new System.EventHandler(this.buttonS_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SandyBrown;
            this.buttonAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.buttonAbout.Location = new System.Drawing.Point(309, 12);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(47, 25);
            this.buttonAbout.TabIndex = 3;
            this.buttonAbout.Text = "?";
            this.buttonAbout.UseVisualStyleBackColor = false;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.textBoxInfo.ForeColor = System.Drawing.Color.DarkOrange;
            this.textBoxInfo.Location = new System.Drawing.Point(40, 469);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(364, 30);
            this.textBoxInfo.TabIndex = 32;
            this.textBoxInfo.Text = ". . . . . . .";
            this.textBoxInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // disconnectButton
            // 
            this.disconnectButton.BackColor = System.Drawing.SystemColors.Control;
            this.disconnectButton.FlatAppearance.BorderSize = 0;
            this.disconnectButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SandyBrown;
            this.disconnectButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.disconnectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.disconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.disconnectButton.Location = new System.Drawing.Point(11, 12);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(197, 50);
            this.disconnectButton.TabIndex = 2;
            this.disconnectButton.Text = "Bağlantıyı Kes";
            this.disconnectButton.UseVisualStyleBackColor = false;
            // 
            // paneProgress
            // 
            this.paneProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paneProgress.BackColor = System.Drawing.Color.SandyBrown;
            this.paneProgress.Controls.Add(this.progressBar1);
            this.paneProgress.Location = new System.Drawing.Point(11, 502);
            this.paneProgress.Name = "paneProgress";
            this.paneProgress.Size = new System.Drawing.Size(417, 30);
            this.paneProgress.TabIndex = 37;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progressBar1.ForeColor = System.Drawing.Color.DarkOrange;
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(411, 24);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 32;
            // 
            // buttonLoadJson
            // 
            this.buttonLoadJson.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoadJson.FlatAppearance.BorderSize = 0;
            this.buttonLoadJson.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SandyBrown;
            this.buttonLoadJson.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.buttonLoadJson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonLoadJson.Location = new System.Drawing.Point(214, 12);
            this.buttonLoadJson.Name = "buttonLoadJson";
            this.buttonLoadJson.Size = new System.Drawing.Size(89, 50);
            this.buttonLoadJson.TabIndex = 39;
            this.buttonLoadJson.Text = "Bilgileri Tekrar Al";
            this.buttonLoadJson.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(11, 68);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(417, 395);
            this.flowLayoutPanel1.TabIndex = 43;
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(310, 49);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(16, 13);
            this.labelInfo.TabIndex = 45;
            this.labelInfo.Text = "...";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(440, 544);
            this.ControlBox = false;
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.buttonLoadJson);
            this.Controls.Add(this.paneProgress);
            this.Controls.Add(this.buttonX);
            this.Controls.Add(this.buttonS);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.disconnectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KÜHEYLAN";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.paneProgress.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button buttonX;
        private System.Windows.Forms.Button buttonS;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Panel paneProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonLoadJson;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelInfo;
    }
}

