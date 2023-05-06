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
            this.OutcomingText = new System.Windows.Forms.TextBox();
            this.IncomingText = new System.Windows.Forms.TextBox();
            this.IncomingInfo = new System.Windows.Forms.TextBox();
            this.OutcomingInfo = new System.Windows.Forms.TextBox();
            this.SavePathInfo = new System.Windows.Forms.TextBox();
            this.PathToSaveBox = new System.Windows.Forms.TextBox();
            this.FileToSendButton = new System.Windows.Forms.Button();
            this.SendXmodemButton = new System.Windows.Forms.Button();
            this.RecieveXmodemButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PortSelectionBox
            // 
            this.PortSelectionBox.AllowDrop = true;
            this.PortSelectionBox.FormattingEnabled = true;
            this.PortSelectionBox.Location = new System.Drawing.Point(55, 11);
            this.PortSelectionBox.Name = "PortSelectionBox";
            this.PortSelectionBox.Size = new System.Drawing.Size(121, 21);
            this.PortSelectionBox.TabIndex = 0;
            this.PortSelectionBox.SelectedIndexChanged += new System.EventHandler(this.PortSelectionBox_SelectedIndexChanged);
            this.PortSelectionBox.DataSource = SerialPort.GetPortNames();
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
            // OutcomingText
            // 
            this.OutcomingText.Location = new System.Drawing.Point(13, 86);
            this.OutcomingText.Multiline = true;
            this.OutcomingText.Name = "OutcomingText";
            this.OutcomingText.ReadOnly = true;
            this.OutcomingText.Size = new System.Drawing.Size(341, 352);
            this.OutcomingText.TabIndex = 2;
            // 
            // IncomingText
            // 
            this.IncomingText.Location = new System.Drawing.Point(447, 86);
            this.IncomingText.Multiline = true;
            this.IncomingText.Name = "IncomingText";
            this.IncomingText.ReadOnly = true;
            this.IncomingText.Size = new System.Drawing.Size(341, 352);
            this.IncomingText.TabIndex = 3;
            // 
            // IncomingInfo
            // 
            this.IncomingInfo.BackColor = System.Drawing.Color.LightBlue;
            this.IncomingInfo.Enabled = false;
            this.IncomingInfo.Location = new System.Drawing.Point(13, 60);
            this.IncomingInfo.Name = "IncomingInfo";
            this.IncomingInfo.Size = new System.Drawing.Size(133, 20);
            this.IncomingInfo.TabIndex = 4;
            this.IncomingInfo.Text = "Komunikaty wychodzące:";
            // 
            // OutcomingInfo
            // 
            this.OutcomingInfo.BackColor = System.Drawing.Color.LightBlue;
            this.OutcomingInfo.Enabled = false;
            this.OutcomingInfo.Location = new System.Drawing.Point(447, 60);
            this.OutcomingInfo.Name = "OutcomingInfo";
            this.OutcomingInfo.Size = new System.Drawing.Size(133, 20);
            this.OutcomingInfo.TabIndex = 5;
            this.OutcomingInfo.Text = "Komunikaty przychodzące:";
            // 
            // SavePathInfo
            // 
            this.SavePathInfo.BackColor = System.Drawing.Color.LightBlue;
            this.SavePathInfo.Enabled = false;
            this.SavePathInfo.Location = new System.Drawing.Point(447, 11);
            this.SavePathInfo.Name = "SavePathInfo";
            this.SavePathInfo.Size = new System.Drawing.Size(96, 20);
            this.SavePathInfo.TabIndex = 6;
            this.SavePathInfo.Text = "Scieżka do zapisu:";
            // 
            // PathToSaveBox
            // 
            this.PathToSaveBox.Location = new System.Drawing.Point(550, 11);
            this.PathToSaveBox.Name = "PathToSaveBox";
            this.PathToSaveBox.Size = new System.Drawing.Size(238, 20);
            this.PathToSaveBox.TabIndex = 7;
            this.PathToSaveBox.TextChanged += new System.EventHandler(this.PathToSaveBox_TextChanged);
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
            this.SendXmodemButton.Location = new System.Drawing.Point(152, 59);
            this.SendXmodemButton.Name = "SendXmodemButton";
            this.SendXmodemButton.Size = new System.Drawing.Size(202, 21);
            this.SendXmodemButton.TabIndex = 9;
            this.SendXmodemButton.Text = "Wyślij plik przez Xmodem";
            this.SendXmodemButton.UseVisualStyleBackColor = false;
            this.SendXmodemButton.Click += new System.EventHandler(this.SendXmodemButton_Click);
            // 
            // RecieveXmodemButton
            // 
            this.RecieveXmodemButton.BackColor = System.Drawing.Color.Lavender;
            this.RecieveXmodemButton.Location = new System.Drawing.Point(586, 59);
            this.RecieveXmodemButton.Name = "RecieveXmodemButton";
            this.RecieveXmodemButton.Size = new System.Drawing.Size(202, 21);
            this.RecieveXmodemButton.TabIndex = 10;
            this.RecieveXmodemButton.Text = "Odbierz plik przez Xmodem";
            this.RecieveXmodemButton.UseVisualStyleBackColor = false;
            this.RecieveXmodemButton.Click += new System.EventHandler(this.RecieveXmodemButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RecieveXmodemButton);
            this.Controls.Add(this.SendXmodemButton);
            this.Controls.Add(this.FileToSendButton);
            this.Controls.Add(this.PathToSaveBox);
            this.Controls.Add(this.SavePathInfo);
            this.Controls.Add(this.OutcomingInfo);
            this.Controls.Add(this.IncomingInfo);
            this.Controls.Add(this.IncomingText);
            this.Controls.Add(this.OutcomingText);
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
        private System.Windows.Forms.TextBox OutcomingText;
        private System.Windows.Forms.TextBox IncomingText;
        private System.Windows.Forms.TextBox IncomingInfo;
        private System.Windows.Forms.TextBox OutcomingInfo;
        private System.Windows.Forms.TextBox SavePathInfo;
        private System.Windows.Forms.TextBox PathToSaveBox;
        private System.Windows.Forms.Button FileToSendButton;
        private System.Windows.Forms.Button SendXmodemButton;
        private System.Windows.Forms.Button RecieveXmodemButton;
    }
}

