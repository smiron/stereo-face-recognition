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
            this.numericUpDownSquaresX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSquaresY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownSquareSize = new System.Windows.Forms.NumericUpDown();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquaresX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquaresY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquareSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(194, 168);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnButtonCloseClick);
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Location = new System.Drawing.Point(194, 12);
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
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Images";
            // 
            // labelAvailableImages
            // 
            this.labelAvailableImages.AutoSize = true;
            this.labelAvailableImages.Location = new System.Drawing.Point(102, 17);
            this.labelAvailableImages.Name = "labelAvailableImages";
            this.labelAvailableImages.Size = new System.Drawing.Size(22, 13);
            this.labelAvailableImages.TabIndex = 3;
            this.labelAvailableImages.Text = "NA";
            // 
            // buttonStartCapture
            // 
            this.buttonStartCapture.Location = new System.Drawing.Point(12, 139);
            this.buttonStartCapture.Name = "buttonStartCapture";
            this.buttonStartCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonStartCapture.TabIndex = 4;
            this.buttonStartCapture.Text = "Start";
            this.buttonStartCapture.UseVisualStyleBackColor = true;
            this.buttonStartCapture.Click += new System.EventHandler(this.OnButtonStartCaptureClick);
            // 
            // buttonEndCapture
            // 
            this.buttonEndCapture.Location = new System.Drawing.Point(93, 139);
            this.buttonEndCapture.Name = "buttonEndCapture";
            this.buttonEndCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonEndCapture.TabIndex = 5;
            this.buttonEndCapture.Text = "End";
            this.buttonEndCapture.UseVisualStyleBackColor = true;
            this.buttonEndCapture.Click += new System.EventHandler(this.OnButtonEndCaptureClick);
            // 
            // numericUpDownSquaresX
            // 
            this.numericUpDownSquaresX.Location = new System.Drawing.Point(149, 41);
            this.numericUpDownSquaresX.Name = "numericUpDownSquaresX";
            this.numericUpDownSquaresX.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSquaresX.TabIndex = 6;
            this.numericUpDownSquaresX.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // numericUpDownSquaresY
            // 
            this.numericUpDownSquaresY.Location = new System.Drawing.Point(149, 67);
            this.numericUpDownSquaresY.Name = "numericUpDownSquaresY";
            this.numericUpDownSquaresY.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSquaresY.TabIndex = 6;
            this.numericUpDownSquaresY.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Squares X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Squares Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Squares Size";
            // 
            // numericUpDownSquareSize
            // 
            this.numericUpDownSquareSize.DecimalPlaces = 1;
            this.numericUpDownSquareSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownSquareSize.Location = new System.Drawing.Point(149, 93);
            this.numericUpDownSquareSize.Name = "numericUpDownSquareSize";
            this.numericUpDownSquareSize.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSquareSize.TabIndex = 10;
            this.numericUpDownSquareSize.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 168);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.OnButtonSaveClick);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(93, 168);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 13;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.OnButtonLoadClick);
            // 
            // FormCalibrateCameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 203);
            this.ControlBox = false;
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownSquareSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownSquaresY);
            this.Controls.Add(this.numericUpDownSquaresX);
            this.Controls.Add(this.buttonEndCapture);
            this.Controls.Add(this.buttonStartCapture);
            this.Controls.Add(this.labelAvailableImages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCalibrate);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCalibrateCameras";
            this.ShowInTaskbar = false;
            this.Text = "Calibrate Cameras";
            this.Load += new System.EventHandler(this.OnFormCalibrateCamerasLoad);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquaresX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquaresY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSquareSize)).EndInit();
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
        private System.Windows.Forms.NumericUpDown numericUpDownSquaresX;
        private System.Windows.Forms.NumericUpDown numericUpDownSquaresY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownSquareSize;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
    }
}