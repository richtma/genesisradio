namespace PowerSDR
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPowerSDR = new System.Windows.Forms.Label();
            this.lblGenesis = new System.Windows.Forms.Label();
            this.lblFIRMWARE = new System.Windows.Forms.Label();
            this.lblBOOT = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(161, 306);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(92, 37);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPowerSDR);
            this.groupBox1.Controls.Add(this.lblGenesis);
            this.groupBox1.Controls.Add(this.lblFIRMWARE);
            this.groupBox1.Controls.Add(this.lblBOOT);
            this.groupBox1.Location = new System.Drawing.Point(29, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 264);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // lblPowerSDR
            // 
            this.lblPowerSDR.AutoSize = true;
            this.lblPowerSDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerSDR.Location = new System.Drawing.Point(40, 88);
            this.lblPowerSDR.Name = "lblPowerSDR";
            this.lblPowerSDR.Size = new System.Drawing.Size(207, 20);
            this.lblPowerSDR.TabIndex = 3;
            this.lblPowerSDR.Text = "(based on PowerSDR 1.8.0)";
            // 
            // lblGenesis
            // 
            this.lblGenesis.AutoSize = true;
            this.lblGenesis.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenesis.Location = new System.Drawing.Point(40, 55);
            this.lblGenesis.Name = "lblGenesis";
            this.lblGenesis.Size = new System.Drawing.Size(210, 20);
            this.lblGenesis.TabIndex = 2;
            this.lblGenesis.Text = "Genesis  1.0 by YT7PWR";
            // 
            // lblFIRMWARE
            // 
            this.lblFIRMWARE.AutoSize = true;
            this.lblFIRMWARE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFIRMWARE.Location = new System.Drawing.Point(40, 197);
            this.lblFIRMWARE.Name = "lblFIRMWARE";
            this.lblFIRMWARE.Size = new System.Drawing.Size(149, 20);
            this.lblFIRMWARE.TabIndex = 1;
            this.lblFIRMWARE.Text = "Firmware version:";
            // 
            // lblBOOT
            // 
            this.lblBOOT.AutoSize = true;
            this.lblBOOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBOOT.Location = new System.Drawing.Point(40, 126);
            this.lblBOOT.Name = "lblBOOT";
            this.lblBOOT.Size = new System.Drawing.Size(124, 20);
            this.lblBOOT.TabIndex = 0;
            this.lblBOOT.Text = "BOOT version:";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 364);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Text = "About";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblGenesis;
        private System.Windows.Forms.Label lblFIRMWARE;
        private System.Windows.Forms.Label lblBOOT;
        private System.Windows.Forms.Label lblPowerSDR;
    }
}