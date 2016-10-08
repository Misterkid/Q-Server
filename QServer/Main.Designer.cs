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
            this.SuspendLayout();
            // 
            // onlineCountLabel
            // 
            this.onlineCountLabel.AutoSize = true;
            this.onlineCountLabel.Font = new System.Drawing.Font("Dotum", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlineCountLabel.Location = new System.Drawing.Point(12, 9);
            this.onlineCountLabel.Name = "onlineCountLabel";
            this.onlineCountLabel.Size = new System.Drawing.Size(193, 47);
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
            this.dcBtn.Location = new System.Drawing.Point(506, 266);
            this.dcBtn.Name = "dcBtn";
            this.dcBtn.Size = new System.Drawing.Size(89, 23);
            this.dcBtn.TabIndex = 3;
            this.dcBtn.Text = "Dissconnect all";
            this.dcBtn.UseVisualStyleBackColor = true;
            this.dcBtn.Click += new System.EventHandler(this.dcBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(688, 302);
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
    }
}