namespace SomeTestClient
{
    partial class Form1
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
            this.loginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.passTextBox = new System.Windows.Forms.TextBox();
            this.chatRichTextBox = new System.Windows.Forms.RichTextBox();
            this.labelLoggedIn = new System.Windows.Forms.Label();
            this.messageButton = new System.Windows.Forms.Button();
            this.chatTextBox = new System.Windows.Forms.TextBox();
            this.unusedPacketsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.roomTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.roomListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(273, 29);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 32);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(61, 29);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(100, 22);
            this.userTextBox.TabIndex = 2;
            // 
            // passTextBox
            // 
            this.passTextBox.Location = new System.Drawing.Point(167, 29);
            this.passTextBox.Name = "passTextBox";
            this.passTextBox.Size = new System.Drawing.Size(100, 22);
            this.passTextBox.TabIndex = 3;
            // 
            // chatRichTextBox
            // 
            this.chatRichTextBox.Location = new System.Drawing.Point(12, 137);
            this.chatRichTextBox.Name = "chatRichTextBox";
            this.chatRichTextBox.ReadOnly = true;
            this.chatRichTextBox.Size = new System.Drawing.Size(477, 245);
            this.chatRichTextBox.TabIndex = 4;
            this.chatRichTextBox.Text = "";
            // 
            // labelLoggedIn
            // 
            this.labelLoggedIn.AutoSize = true;
            this.labelLoggedIn.Location = new System.Drawing.Point(12, 9);
            this.labelLoggedIn.Name = "labelLoggedIn";
            this.labelLoggedIn.Size = new System.Drawing.Size(96, 17);
            this.labelLoggedIn.TabIndex = 5;
            this.labelLoggedIn.Text = "Not logged in.";
            // 
            // messageButton
            // 
            this.messageButton.Location = new System.Drawing.Point(413, 389);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(75, 32);
            this.messageButton.TabIndex = 6;
            this.messageButton.Text = "Send";
            this.messageButton.UseVisualStyleBackColor = true;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // chatTextBox
            // 
            this.chatTextBox.AcceptsReturn = true;
            this.chatTextBox.AcceptsTab = true;
            this.chatTextBox.Location = new System.Drawing.Point(12, 394);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.Size = new System.Drawing.Size(395, 22);
            this.chatTextBox.TabIndex = 7;
            this.chatTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatTextBox_KeyDown);
            // 
            // unusedPacketsRichTextBox
            // 
            this.unusedPacketsRichTextBox.Location = new System.Drawing.Point(657, 137);
            this.unusedPacketsRichTextBox.Name = "unusedPacketsRichTextBox";
            this.unusedPacketsRichTextBox.ReadOnly = true;
            this.unusedPacketsRichTextBox.Size = new System.Drawing.Size(255, 245);
            this.unusedPacketsRichTextBox.TabIndex = 8;
            this.unusedPacketsRichTextBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(654, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Unhandled Packets";
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(273, 65);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 32);
            this.createButton.TabIndex = 10;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // roomTextBox
            // 
            this.roomTextBox.Location = new System.Drawing.Point(100, 70);
            this.roomTextBox.Name = "roomTextBox";
            this.roomTextBox.Size = new System.Drawing.Size(167, 22);
            this.roomTextBox.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "CreateRoom";
            // 
            // roomListBox
            // 
            this.roomListBox.FormattingEnabled = true;
            this.roomListBox.ItemHeight = 16;
            this.roomListBox.Location = new System.Drawing.Point(354, 29);
            this.roomListBox.Name = "roomListBox";
            this.roomListBox.Size = new System.Drawing.Size(120, 84);
            this.roomListBox.TabIndex = 13;
            this.roomListBox.SelectedIndexChanged += new System.EventHandler(this.roomListBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 423);
            this.Controls.Add(this.roomListBox);
            this.Controls.Add(this.roomTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.unusedPacketsRichTextBox);
            this.Controls.Add(this.chatTextBox);
            this.Controls.Add(this.messageButton);
            this.Controls.Add(this.labelLoggedIn);
            this.Controls.Add(this.chatRichTextBox);
            this.Controls.Add(this.passTextBox);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loginButton);
            this.Name = "Form1";
            this.Text = "Master server test client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.TextBox passTextBox;
        private System.Windows.Forms.RichTextBox chatRichTextBox;
        private System.Windows.Forms.Label labelLoggedIn;
        private System.Windows.Forms.Button messageButton;
        private System.Windows.Forms.TextBox chatTextBox;
        private System.Windows.Forms.RichTextBox unusedPacketsRichTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.TextBox roomTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox roomListBox;
    }
}

