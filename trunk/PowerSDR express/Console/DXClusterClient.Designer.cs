//=================================================================
// DXClusterSetup.cs
//=================================================================
// Copyright (C) 2011 YT7PWR
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//=================================================================

namespace PowerSDR
{
    partial class DXClusterClient
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
            closing = true;
            SaveOptions();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DXClusterClient));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnBye = new System.Windows.Forms.Button();
            this.btnNoDX = new System.Windows.Forms.Button();
            this.rtbDXClusterText = new System.Windows.Forms.RichTextBox();
            this.btnShowDX = new System.Windows.Forms.Button();
            this.btnNoVHF = new System.Windows.Forms.Button();
            this.comboDXCluster = new System.Windows.Forms.ComboBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnClearTxt = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnConnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnect.Location = new System.Drawing.Point(19, 404);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(65, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnBye
            // 
            this.btnBye.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBye.ForeColor = System.Drawing.Color.Black;
            this.btnBye.Location = new System.Drawing.Point(91, 404);
            this.btnBye.Name = "btnBye";
            this.btnBye.Size = new System.Drawing.Size(65, 23);
            this.btnBye.TabIndex = 1;
            this.btnBye.Text = "Bye";
            this.btnBye.UseVisualStyleBackColor = false;
            this.btnBye.Click += new System.EventHandler(this.btnBye_Click);
            // 
            // btnNoDX
            // 
            this.btnNoDX.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNoDX.ForeColor = System.Drawing.Color.Black;
            this.btnNoDX.Location = new System.Drawing.Point(163, 404);
            this.btnNoDX.Name = "btnNoDX";
            this.btnNoDX.Size = new System.Drawing.Size(65, 23);
            this.btnNoDX.TabIndex = 2;
            this.btnNoDX.Text = "No DX";
            this.btnNoDX.UseVisualStyleBackColor = false;
            this.btnNoDX.Click += new System.EventHandler(this.btnNoDX_Click);
            // 
            // rtbDXClusterText
            // 
            this.rtbDXClusterText.BackColor = System.Drawing.Color.Black;
            this.rtbDXClusterText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbDXClusterText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtbDXClusterText.ForeColor = System.Drawing.Color.Green;
            this.rtbDXClusterText.Location = new System.Drawing.Point(25, 56);
            this.rtbDXClusterText.Name = "rtbDXClusterText";
            this.rtbDXClusterText.Size = new System.Drawing.Size(485, 324);
            this.rtbDXClusterText.TabIndex = 3;
            this.rtbDXClusterText.Text = "";
            this.rtbDXClusterText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbDXClusterText_LinkClicked);
            // 
            // btnShowDX
            // 
            this.btnShowDX.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowDX.ForeColor = System.Drawing.Color.Black;
            this.btnShowDX.Location = new System.Drawing.Point(235, 404);
            this.btnShowDX.Name = "btnShowDX";
            this.btnShowDX.Size = new System.Drawing.Size(65, 23);
            this.btnShowDX.TabIndex = 4;
            this.btnShowDX.Text = "Show DX";
            this.btnShowDX.UseVisualStyleBackColor = false;
            this.btnShowDX.Click += new System.EventHandler(this.btnShowDX_Click);
            // 
            // btnNoVHF
            // 
            this.btnNoVHF.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNoVHF.ForeColor = System.Drawing.Color.Black;
            this.btnNoVHF.Location = new System.Drawing.Point(307, 404);
            this.btnNoVHF.Name = "btnNoVHF";
            this.btnNoVHF.Size = new System.Drawing.Size(65, 23);
            this.btnNoVHF.TabIndex = 5;
            this.btnNoVHF.Text = "No VHF";
            this.btnNoVHF.UseVisualStyleBackColor = false;
            this.btnNoVHF.Click += new System.EventHandler(this.btnNoVHF_Click);
            // 
            // comboDXCluster
            // 
            this.comboDXCluster.BackColor = System.Drawing.Color.White;
            this.comboDXCluster.ForeColor = System.Drawing.Color.Black;
            this.comboDXCluster.FormattingEnabled = true;
            this.comboDXCluster.Items.AddRange(new object[] {
            "9A0DXC.HAMRADIO.HR:8000",
            "CAMPELO.CDF.UDC.ES:8000",
            "CS0RCL.HOPTO.ORG:9000",
            "DXC.KB1H.COM:7300",
            "F5LEN.HOMELINUX.ORG:7373",
            "SKCC.MATRIXLIST.COM:7300"});
            this.comboDXCluster.Location = new System.Drawing.Point(344, 19);
            this.comboDXCluster.Name = "comboDXCluster";
            this.comboDXCluster.Size = new System.Drawing.Size(167, 21);
            this.comboDXCluster.TabIndex = 6;
            this.comboDXCluster.SelectedIndexChanged += new System.EventHandler(this.comboDXCluster_SelectedIndexChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSettings.ForeColor = System.Drawing.Color.Black;
            this.btnSettings.Location = new System.Drawing.Point(450, 404);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(65, 23);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnClearTxt
            // 
            this.btnClearTxt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClearTxt.ForeColor = System.Drawing.Color.Black;
            this.btnClearTxt.Location = new System.Drawing.Point(379, 404);
            this.btnClearTxt.Name = "btnClearTxt";
            this.btnClearTxt.Size = new System.Drawing.Size(65, 23);
            this.btnClearTxt.TabIndex = 9;
            this.btnClearTxt.Text = "Clear";
            this.btnClearTxt.UseVisualStyleBackColor = false;
            this.btnClearTxt.Click += new System.EventHandler(this.btnClearTxt_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.Black;
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(23, 22);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "Message";
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(79, 19);
            this.txtMessage.MaxLength = 64;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(259, 20);
            this.txtMessage.TabIndex = 12;
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUP);
            // 
            // DXClusterClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(534, 444);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnClearTxt);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.comboDXCluster);
            this.Controls.Add(this.btnNoVHF);
            this.Controls.Add(this.btnShowDX);
            this.Controls.Add(this.rtbDXClusterText);
            this.Controls.Add(this.btnNoDX);
            this.Controls.Add(this.btnBye);
            this.Controls.Add(this.btnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 482);
            this.MinimumSize = new System.Drawing.Size(550, 482);
            this.Name = "DXClusterClient";
            this.Text = "DXClusterClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnBye;
        private System.Windows.Forms.Button btnNoDX;
        private System.Windows.Forms.Button btnShowDX;
        private System.Windows.Forms.Button btnNoVHF;
        private System.Windows.Forms.ComboBox comboDXCluster;
        private System.Windows.Forms.Button btnSettings;
        public System.Windows.Forms.RichTextBox rtbDXClusterText;
        private System.Windows.Forms.Button btnClearTxt;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtMessage;
    }
}