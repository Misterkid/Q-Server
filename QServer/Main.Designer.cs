namespace QServer
{
    partial class Main
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
            this.onlineCountLabel = new System.Windows.Forms.Label();
            this.exitBtn = new System.Windows.Forms.Button();
            this.creditsLabel = new System.Windows.Forms.Label();
            this.dcBtn = new System.Windows.Forms.Button();
            this.sendPackageBtn = new System.Windows.Forms.Button();
            this.packageText = new System.Windows.Forms.TextBox();
            this.splitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // onlineCountLabel
            // 
            this.onlineCountLabel.AutoSize = true;
            this.onlineCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlineCountLabel.Location = new System.Drawing.Point(12, 9);
            this.onlineCountLabel.Name = "onlineCountLabel";
            this.onlineCountLabel.Size = new System.Drawing.Size(187, 54);
            this.onlineCountLabel.TabIndex = 0;
            this.onlineCountLabel.Text = "Count:0";
            // 
            // exitBtn
            // 
            this.exitBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exitBtn.Location = new System.Drawing.Point(601, 267);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 1;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // creditsLabel
            // 
            this.creditsLabel.AutoSize = true;
            this.creditsLabel.Location = new System.Drawing.Point(12, 277);
            this.creditsLabel.Name = "creditsLabel";
            this.creditsLabel.Size = new System.Drawing.Size(115, 13);
            this.creditsLabel.TabIndex = 2;
            this.creditsLabel.Text = "Credits: Eddy Meivogel";
            // 
            // dcBtn
            // 
            this.dcBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dcBtn.Location = new System.Drawing.Point(587, 237);
            this.dcBtn.Name = "dcBtn";
            this.dcBtn.Size = new System.Drawing.Size(89, 23);
            this.dcBtn.TabIndex = 3;
            this.dcBtn.Text = "Dissconnect all";
            this.dcBtn.UseVisualStyleBackColor = true;
            this.dcBtn.Click += new System.EventHandler(this.dcBtn_Click);
            // 
            // sendPackageBtn
            // 
            this.sendPackageBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sendPackageBtn.Location = new System.Drawing.Point(520, 266);
            this.sendPackageBtn.Name = "sendPackageBtn";
            this.sendPackageBtn.Size = new System.Drawing.Size(75, 23);
            this.sendPackageBtn.TabIndex = 4;
            this.sendPackageBtn.Text = "Send";
            this.sendPackageBtn.UseVisualStyleBackColor = true;
            this.sendPackageBtn.Click += new System.EventHandler(this.sendPackageBtn_Click);
            // 
            // packageText
            // 
            this.packageText.Location = new System.Drawing.Point(174, 268);
            this.packageText.Name = "packageText";
            this.packageText.Size = new System.Drawing.Size(340, 20);
            this.packageText.TabIndex = 5;
            // 
            // splitBtn
            // 
            this.splitBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.splitBtn.Location = new System.Drawing.Point(520, 237);
            this.splitBtn.Name = "splitBtn";
            this.splitBtn.Size = new System.Drawing.Size(61, 23);
            this.splitBtn.TabIndex = 6;
            this.splitBtn.Text = "Ins Split";
            this.splitBtn.UseMnemonic = false;
            this.splitBtn.UseVisualStyleBackColor = true;
            this.splitBtn.Click += new System.EventHandler(this.splitBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(688, 302);
            this.Controls.Add(this.splitBtn);
            this.Controls.Add(this.packageText);
            this.Controls.Add(this.sendPackageBtn);
            this.Controls.Add(this.dcBtn);
            this.Controls.Add(this.creditsLabel);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.onlineCountLabel);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Main";
            this.Text = "Master Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label onlineCountLabel;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Label creditsLabel;
        private System.Windows.Forms.Button dcBtn;
        private System.Windows.Forms.Button sendPackageBtn;
        private System.Windows.Forms.TextBox packageText;
        private System.Windows.Forms.Button splitBtn;
    }
}