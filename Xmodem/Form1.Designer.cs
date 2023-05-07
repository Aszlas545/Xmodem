using System.IO.Ports;

namespace Xmodem
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
            this.PortSelectionBox = new System.Windows.Forms.ComboBox();
            this.PortInfo = new System.Windows.Forms.TextBox();
            this.FileToSendButton = new System.Windows.Forms.Button();
            this.SendXmodemButton = new System.Windows.Forms.Button();
            this.RecieveXmodemButton = new System.Windows.Forms.Button();
            this.CRCUsage = new System.Windows.Forms.CheckBox();
            this.Comms = new System.Windows.Forms.TextBox();
            this.ComsInfo = new System.Windows.Forms.TextBox();
            this.FilePathButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PortSelectionBox
            // 
            this.PortSelectionBox.AllowDrop = true;
            this.PortSelectionBox.DataSource = new string[0];
            this.PortSelectionBox.FormattingEnabled = true;
            this.PortSelectionBox.Location = new System.Drawing.Point(55, 11);
            this.PortSelectionBox.Name = "PortSelectionBox";
            this.PortSelectionBox.Size = new System.Drawing.Size(121, 21);
            this.PortSelectionBox.TabIndex = 0;
            // 
            // PortInfo
            // 
            this.PortInfo.BackColor = System.Drawing.Color.LightBlue;
            this.PortInfo.Enabled = false;
            this.PortInfo.Location = new System.Drawing.Point(13, 12);
            this.PortInfo.Name = "PortInfo";
            this.PortInfo.Size = new System.Drawing.Size(36, 20);
            this.PortInfo.TabIndex = 1;
            this.PortInfo.Text = "Port:";
            // 
            // FileToSendButton
            // 
            this.FileToSendButton.BackColor = System.Drawing.Color.Lavender;
            this.FileToSendButton.Location = new System.Drawing.Point(183, 11);
            this.FileToSendButton.Name = "FileToSendButton";
            this.FileToSendButton.Size = new System.Drawing.Size(171, 21);
            this.FileToSendButton.TabIndex = 8;
            this.FileToSendButton.Text = "Wybierz plik do wysłania";
            this.FileToSendButton.UseVisualStyleBackColor = false;
            this.FileToSendButton.Click += new System.EventHandler(this.FileToSendButton_Click);
            // 
            // SendXmodemButton
            // 
            this.SendXmodemButton.BackColor = System.Drawing.Color.Lavender;
            this.SendXmodemButton.Location = new System.Drawing.Point(13, 38);
            this.SendXmodemButton.Name = "SendXmodemButton";
            this.SendXmodemButton.Size = new System.Drawing.Size(341, 21);
            this.SendXmodemButton.TabIndex = 9;
            this.SendXmodemButton.Text = "Wyślij plik przez Xmodem";
            this.SendXmodemButton.UseVisualStyleBackColor = false;
            this.SendXmodemButton.Click += new System.EventHandler(this.SendXmodemButton_Click);
            // 
            // RecieveXmodemButton
            // 
            this.RecieveXmodemButton.BackColor = System.Drawing.Color.Lavender;
            this.RecieveXmodemButton.Location = new System.Drawing.Point(447, 37);
            this.RecieveXmodemButton.Name = "RecieveXmodemButton";
            this.RecieveXmodemButton.Size = new System.Drawing.Size(341, 21);
            this.RecieveXmodemButton.TabIndex = 10;
            this.RecieveXmodemButton.Text = "Odbierz plik przez Xmodem";
            this.RecieveXmodemButton.UseVisualStyleBackColor = false;
            this.RecieveXmodemButton.Click += new System.EventHandler(this.RecieveXmodemButton_Click);
            // 
            // CRCUsage
            // 
            this.CRCUsage.AutoSize = true;
            this.CRCUsage.BackColor = System.Drawing.Color.LightBlue;
            this.CRCUsage.Location = new System.Drawing.Point(360, 10);
            this.CRCUsage.Name = "CRCUsage";
            this.CRCUsage.Size = new System.Drawing.Size(81, 17);
            this.CRCUsage.TabIndex = 11;
            this.CRCUsage.Text = "użycie CRC";
            this.CRCUsage.UseVisualStyleBackColor = false;
            this.CRCUsage.CheckedChanged += new System.EventHandler(this.CRCUsage_CheckedChanged);
            // 
            // Comms
            // 
            this.Comms.Enabled = false;
            this.Comms.Location = new System.Drawing.Point(126, 69);
            this.Comms.Name = "Comms";
            this.Comms.Size = new System.Drawing.Size(661, 20);
            this.Comms.TabIndex = 12;
            // 
            // ComsInfo
            // 
            this.ComsInfo.BackColor = System.Drawing.Color.LightBlue;
            this.ComsInfo.Enabled = false;
            this.ComsInfo.Location = new System.Drawing.Point(12, 69);
            this.ComsInfo.Name = "ComsInfo";
            this.ComsInfo.Size = new System.Drawing.Size(108, 20);
            this.ComsInfo.TabIndex = 13;
            this.ComsInfo.Text = "Komunikat Zwrotny:";
            // 
            // FilePathButton
            // 
            this.FilePathButton.BackColor = System.Drawing.Color.Lavender;
            this.FilePathButton.Location = new System.Drawing.Point(447, 10);
            this.FilePathButton.Name = "FilePathButton";
            this.FilePathButton.Size = new System.Drawing.Size(340, 21);
            this.FilePathButton.TabIndex = 14;
            this.FilePathButton.Text = "Wybierz plik do którego zostanie zapisana zawartość";
            this.FilePathButton.UseVisualStyleBackColor = false;
            this.FilePathButton.Click += new System.EventHandler(this.PathToSaveBox_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(800, 97);
            this.Controls.Add(this.FilePathButton);
            this.Controls.Add(this.ComsInfo);
            this.Controls.Add(this.Comms);
            this.Controls.Add(this.CRCUsage);
            this.Controls.Add(this.RecieveXmodemButton);
            this.Controls.Add(this.SendXmodemButton);
            this.Controls.Add(this.FileToSendButton);
            this.Controls.Add(this.PortInfo);
            this.Controls.Add(this.PortSelectionBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PortSelectionBox;
        private System.Windows.Forms.TextBox PortInfo;
        private System.Windows.Forms.Button FileToSendButton;
        private System.Windows.Forms.Button SendXmodemButton;
        private System.Windows.Forms.Button RecieveXmodemButton;
        private System.Windows.Forms.CheckBox CRCUsage;
        private System.Windows.Forms.TextBox Comms;
        private System.Windows.Forms.TextBox ComsInfo;
        private System.Windows.Forms.Button FilePathButton;
    }
}

