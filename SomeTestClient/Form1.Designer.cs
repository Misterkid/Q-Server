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
            this.startGameButton = new System.Windows.Forms.Button();
            this.stopGameButton = new System.Windows.Forms.Button();
            this.spamButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(205, 24);
            this.loginButton.Margin = new System.Windows.Forms.Padding(2);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(56, 26);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(46, 24);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(76, 20);
            this.userTextBox.TabIndex = 2;
            // 
            // passTextBox
            // 
            this.passTextBox.Location = new System.Drawing.Point(125, 24);
            this.passTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.passTextBox.Name = "passTextBox";
            this.passTextBox.Size = new System.Drawing.Size(76, 20);
            this.passTextBox.TabIndex = 3;
            // 
            // chatRichTextBox
            // 
            this.chatRichTextBox.Location = new System.Drawing.Point(9, 111);
            this.chatRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.chatRichTextBox.Name = "chatRichTextBox";
            this.chatRichTextBox.ReadOnly = true;
            this.chatRichTextBox.Size = new System.Drawing.Size(359, 200);
            this.chatRichTextBox.TabIndex = 4;
            this.chatRichTextBox.Text = "";
            // 
            // labelLoggedIn
            // 
            this.labelLoggedIn.AutoSize = true;
            this.labelLoggedIn.Location = new System.Drawing.Point(9, 7);
            this.labelLoggedIn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLoggedIn.Name = "labelLoggedIn";
            this.labelLoggedIn.Size = new System.Drawing.Size(73, 13);
            this.labelLoggedIn.TabIndex = 5;
            this.labelLoggedIn.Text = "Not logged in.";
            // 
            // messageButton
            // 
            this.messageButton.Location = new System.Drawing.Point(310, 316);
            this.messageButton.Margin = new System.Windows.Forms.Padding(2);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(56, 26);
            this.messageButton.TabIndex = 6;
            this.messageButton.Text = "Send";
            this.messageButton.UseVisualStyleBackColor = true;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // chatTextBox
            // 
            this.chatTextBox.AcceptsReturn = true;
            this.chatTextBox.AcceptsTab = true;
            this.chatTextBox.Location = new System.Drawing.Point(9, 320);
            this.chatTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.Size = new System.Drawing.Size(297, 20);
            this.chatTextBox.TabIndex = 7;
            this.chatTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatTextBox_KeyDown);
            // 
            // unusedPacketsRichTextBox
            // 
            this.unusedPacketsRichTextBox.Location = new System.Drawing.Point(493, 111);
            this.unusedPacketsRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.unusedPacketsRichTextBox.Name = "unusedPacketsRichTextBox";
            this.unusedPacketsRichTextBox.ReadOnly = true;
            this.unusedPacketsRichTextBox.Size = new System.Drawing.Size(192, 200);
            this.unusedPacketsRichTextBox.TabIndex = 8;
            this.unusedPacketsRichTextBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(490, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Unhandled Packets";
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(205, 53);
            this.createButton.Margin = new System.Windows.Forms.Padding(2);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(56, 26);
            this.createButton.TabIndex = 10;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // roomTextBox
            // 
            this.roomTextBox.Location = new System.Drawing.Point(75, 57);
            this.roomTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.roomTextBox.Name = "roomTextBox";
            this.roomTextBox.Size = new System.Drawing.Size(126, 20);
            this.roomTextBox.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "CreateRoom";
            // 
            // roomListBox
            // 
            this.roomListBox.FormattingEnabled = true;
            this.roomListBox.Location = new System.Drawing.Point(266, 24);
            this.roomListBox.Margin = new System.Windows.Forms.Padding(2);
            this.roomListBox.Name = "roomListBox";
            this.roomListBox.Size = new System.Drawing.Size(91, 69);
            this.roomListBox.TabIndex = 13;
            this.roomListBox.SelectedIndexChanged += new System.EventHandler(this.roomListBox_SelectedIndexChanged);
            // 
            // startGameButton
            // 
            this.startGameButton.Location = new System.Drawing.Point(493, 316);
            this.startGameButton.Name = "startGameButton";
            this.startGameButton.Size = new System.Drawing.Size(75, 23);
            this.startGameButton.TabIndex = 14;
            this.startGameButton.Text = "Start Game";
            this.startGameButton.UseVisualStyleBackColor = true;
            this.startGameButton.Click += new System.EventHandler(this.startGameButton_Click);
            // 
            // stopGameButton
            // 
            this.stopGameButton.Location = new System.Drawing.Point(610, 317);
            this.stopGameButton.Name = "stopGameButton";
            this.stopGameButton.Size = new System.Drawing.Size(75, 23);
            this.stopGameButton.TabIndex = 15;
            this.stopGameButton.Text = "Stop Game";
            this.stopGameButton.UseVisualStyleBackColor = true;
            this.stopGameButton.Click += new System.EventHandler(this.stopGameButton_Click);
            // 
            // spamButton
            // 
            this.spamButton.Location = new System.Drawing.Point(362, 22);
            this.spamButton.Name = "spamButton";
            this.spamButton.Size = new System.Drawing.Size(93, 23);
            this.spamButton.TabIndex = 16;
            this.spamButton.Text = "Login Spam";
            this.spamButton.UseVisualStyleBackColor = true;
            this.spamButton.Click += new System.EventHandler(this.spamButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 344);
            this.Controls.Add(this.spamButton);
            this.Controls.Add(this.stopGameButton);
            this.Controls.Add(this.startGameButton);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Button startGameButton;
        private System.Windows.Forms.Button stopGameButton;
        private System.Windows.Forms.Button spamButton;
    }
}

