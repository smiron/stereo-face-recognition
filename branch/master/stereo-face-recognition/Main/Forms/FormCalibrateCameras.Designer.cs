namespace FaceDetection.Forms
{
    partial class FormCalibrateCameras
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCalibrate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAvailableImages = new System.Windows.Forms.Label();
            this.buttonStartCapture = new System.Windows.Forms.Button();
            this.buttonEndCapture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(191, 38);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnButtonCloseClick);
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Location = new System.Drawing.Point(191, 4);
            this.buttonCalibrate.Name = "buttonCalibrate";
            this.buttonCalibrate.Size = new System.Drawing.Size(75, 23);
            this.buttonCalibrate.TabIndex = 1;
            this.buttonCalibrate.Text = "Calibrate";
            this.buttonCalibrate.UseVisualStyleBackColor = true;
            this.buttonCalibrate.Click += new System.EventHandler(this.OnButtonCalibrateClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Images";
            // 
            // labelAvailableImages
            // 
            this.labelAvailableImages.AutoSize = true;
            this.labelAvailableImages.Location = new System.Drawing.Point(105, 9);
            this.labelAvailableImages.Name = "labelAvailableImages";
            this.labelAvailableImages.Size = new System.Drawing.Size(22, 13);
            this.labelAvailableImages.TabIndex = 3;
            this.labelAvailableImages.Text = "NA";
            // 
            // buttonStartCapture
            // 
            this.buttonStartCapture.Location = new System.Drawing.Point(12, 38);
            this.buttonStartCapture.Name = "buttonStartCapture";
            this.buttonStartCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonStartCapture.TabIndex = 4;
            this.buttonStartCapture.Text = "Start";
            this.buttonStartCapture.UseVisualStyleBackColor = true;
            this.buttonStartCapture.Click += new System.EventHandler(this.OnButtonStartCaptureClick);
            // 
            // buttonEndCapture
            // 
            this.buttonEndCapture.Location = new System.Drawing.Point(93, 38);
            this.buttonEndCapture.Name = "buttonEndCapture";
            this.buttonEndCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonEndCapture.TabIndex = 5;
            this.buttonEndCapture.Text = "End";
            this.buttonEndCapture.UseVisualStyleBackColor = true;
            this.buttonEndCapture.Click += new System.EventHandler(this.OnButtonEndCaptureClick);
            // 
            // FormCalibrateCameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 73);
            this.ControlBox = false;
            this.Controls.Add(this.buttonEndCapture);
            this.Controls.Add(this.buttonStartCapture);
            this.Controls.Add(this.labelAvailableImages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCalibrate);
            this.Controls.Add(this.buttonClose);
            this.Name = "FormCalibrateCameras";
            this.Text = "Calibrate Cameras";
            this.Load += new System.EventHandler(this.OnFormCalibrateCamerasLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCalibrate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelAvailableImages;
        private System.Windows.Forms.Button buttonStartCapture;
        private System.Windows.Forms.Button buttonEndCapture;
    }
}