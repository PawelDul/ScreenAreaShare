namespace ScreenAreaCapture
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer VDCheckTimer;

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
            this.pbScreenDisplay = new System.Windows.Forms.PictureBox();
            this.tPicture = new System.Windows.Forms.Timer(this.components);
            this.startCapture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // pbScreenDisplay
            // 
            this.pbScreenDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScreenDisplay.Location = new System.Drawing.Point(0, 0);
            this.pbScreenDisplay.Name = "pbScreenDisplay";
            this.pbScreenDisplay.Size = new System.Drawing.Size(1342, 978);
            this.pbScreenDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbScreenDisplay.TabIndex = 0;
            this.pbScreenDisplay.TabStop = false;
            // 
            // tPicture
            // 
            this.tPicture.Interval = 40;
            this.tPicture.Tick += new System.EventHandler(this.tPicture_Tick);
            // 
            // startCapture
            // 
            this.startCapture.Location = new System.Drawing.Point(493, 334);
            this.startCapture.Name = "startCapture";
            this.startCapture.Size = new System.Drawing.Size(359, 182);
            this.startCapture.TabIndex = 1;
            this.startCapture.Text = "Select capture area";
            this.startCapture.UseVisualStyleBackColor = true;
            this.startCapture.Click += new System.EventHandler(this.startCapture_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 978);
            this.Controls.Add(this.startCapture);
            this.Controls.Add(this.pbScreenDisplay);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Screen area share";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenDisplay)).EndInit();
            this.ResumeLayout(false);

            this.components = new System.ComponentModel.Container();
            this.VDCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.VDCheckTimer.Enabled = true;
            this.VDCheckTimer.Interval = 1000;
            this.VDCheckTimer.Tick += new System.EventHandler(this.VDCheckTimer_Tick);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbScreenDisplay;
        private System.Windows.Forms.Timer tPicture;
        private System.Windows.Forms.Button startCapture;
    }
}

