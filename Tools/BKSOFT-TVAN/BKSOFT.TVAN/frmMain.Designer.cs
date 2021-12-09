namespace BKSOFT.TVAN
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbResult = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbClock = new System.Windows.Forms.Label();
            this.txtMainPort = new System.Windows.Forms.TextBox();
            this.lblGPSConnecting = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMainServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbResult);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbClock);
            this.groupBox1.Controls.Add(this.txtMainPort);
            this.groupBox1.Controls.Add(this.lblGPSConnecting);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtMainServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(394, 125);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server settings";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(318, 96);
            this.lbResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(16, 17);
            this.lbResult.TabIndex = 15;
            this.lbResult.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 96);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "# HttpPost:";
            // 
            // lbClock
            // 
            this.lbClock.AutoSize = true;
            this.lbClock.Location = new System.Drawing.Point(81, 96);
            this.lbClock.Name = "lbClock";
            this.lbClock.Size = new System.Drawing.Size(64, 17);
            this.lbClock.TabIndex = 13;
            this.lbClock.Text = "00:00:00";
            // 
            // txtMainPort
            // 
            this.txtMainPort.Location = new System.Drawing.Point(84, 61);
            this.txtMainPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtMainPort.Name = "txtMainPort";
            this.txtMainPort.Size = new System.Drawing.Size(84, 22);
            this.txtMainPort.TabIndex = 3;
            // 
            // lblGPSConnecting
            // 
            this.lblGPSConnecting.AutoSize = true;
            this.lblGPSConnecting.Location = new System.Drawing.Point(318, 66);
            this.lblGPSConnecting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGPSConnecting.Name = "lblGPSConnecting";
            this.lblGPSConnecting.Size = new System.Drawing.Size(16, 17);
            this.lblGPSConnecting.TabIndex = 3;
            this.lblGPSConnecting.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(222, 64);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "# Connecting:";
            // 
            // txtMainServer
            // 
            this.txtMainServer.Location = new System.Drawing.Point(84, 29);
            this.txtMainServer.Margin = new System.Windows.Forms.Padding(4);
            this.txtMainServer.Name = "txtMainServer";
            this.txtMainServer.Size = new System.Drawing.Size(257, 22);
            this.txtMainServer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server IP:";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "BKSOFT TVAN";
            this.notifyIcon.BalloonTipTitle = "BKSOFT.TVAN";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "BKSOFT.TVAN";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnNotifyIconMouseDoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(406, 136);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Queue TVAN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMainPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMainServer;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblGPSConnecting;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbClock;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

