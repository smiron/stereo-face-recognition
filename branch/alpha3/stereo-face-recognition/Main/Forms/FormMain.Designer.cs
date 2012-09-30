namespace FaceDetection.Forms
{
    partial class FormMain
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
            this.imageBoxCamera = new Emgu.CV.UI.ImageBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudRightRoll = new System.Windows.Forms.NumericUpDown();
            this.nudLeftRoll = new System.Windows.Forms.NumericUpDown();
            this.nudFacePitch = new System.Windows.Forms.NumericUpDown();
            this.nudFaceYaw = new System.Windows.Forms.NumericUpDown();
            this.nudFaceRoll = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.corespondenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCorrelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useGpuMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stereoCalibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findFacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imageBoxPoints = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxCamera)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFacePitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFaceYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFaceRoll)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBoxCamera
            // 
            this.imageBoxCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxCamera.Location = new System.Drawing.Point(3, 3);
            this.imageBoxCamera.Name = "imageBoxCamera";
            this.imageBoxCamera.Size = new System.Drawing.Size(778, 263);
            this.imageBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBoxCamera.TabIndex = 2;
            this.imageBoxCamera.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.imageBoxCamera, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 538);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 272);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 263);
            this.panel1.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Right Face Angle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Left Face Angle";
            // 
            // nudRightRoll
            // 
            this.nudRightRoll.DecimalPlaces = 2;
            this.nudRightRoll.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudRightRoll.Location = new System.Drawing.Point(224, 124);
            this.nudRightRoll.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRightRoll.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudRightRoll.Name = "nudRightRoll";
            this.nudRightRoll.ReadOnly = true;
            this.nudRightRoll.Size = new System.Drawing.Size(120, 20);
            this.nudRightRoll.TabIndex = 11;
            this.nudRightRoll.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudLeftRoll
            // 
            this.nudLeftRoll.DecimalPlaces = 2;
            this.nudLeftRoll.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudLeftRoll.Location = new System.Drawing.Point(224, 98);
            this.nudLeftRoll.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLeftRoll.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudLeftRoll.Name = "nudLeftRoll";
            this.nudLeftRoll.ReadOnly = true;
            this.nudLeftRoll.Size = new System.Drawing.Size(120, 20);
            this.nudLeftRoll.TabIndex = 10;
            this.nudLeftRoll.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudFacePitch
            // 
            this.nudFacePitch.DecimalPlaces = 2;
            this.nudFacePitch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudFacePitch.Location = new System.Drawing.Point(224, 60);
            this.nudFacePitch.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFacePitch.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudFacePitch.Name = "nudFacePitch";
            this.nudFacePitch.ReadOnly = true;
            this.nudFacePitch.Size = new System.Drawing.Size(120, 20);
            this.nudFacePitch.TabIndex = 9;
            this.nudFacePitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudFaceYaw
            // 
            this.nudFaceYaw.DecimalPlaces = 2;
            this.nudFaceYaw.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudFaceYaw.Location = new System.Drawing.Point(224, 34);
            this.nudFaceYaw.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFaceYaw.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudFaceYaw.Name = "nudFaceYaw";
            this.nudFaceYaw.ReadOnly = true;
            this.nudFaceYaw.Size = new System.Drawing.Size(120, 20);
            this.nudFaceYaw.TabIndex = 8;
            this.nudFaceYaw.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudFaceRoll
            // 
            this.nudFaceRoll.DecimalPlaces = 2;
            this.nudFaceRoll.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudFaceRoll.Location = new System.Drawing.Point(224, 8);
            this.nudFaceRoll.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFaceRoll.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudFaceRoll.Name = "nudFaceRoll";
            this.nudFaceRoll.ReadOnly = true;
            this.nudFaceRoll.Size = new System.Drawing.Size(120, 20);
            this.nudFaceRoll.TabIndex = 7;
            this.nudFaceRoll.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Face Pitch (Angle between face and mouth)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Face Yaw (Z Axis - Eyes Angle)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Face Roll (X Axis - Eyes Angle)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facesToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // facesToolStripMenuItem
            // 
            this.facesToolStripMenuItem.Name = "facesToolStripMenuItem";
            this.facesToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.facesToolStripMenuItem.Text = "Faces";
            this.facesToolStripMenuItem.Click += new System.EventHandler(this.OnFacesToolStripMenuItemClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnButtonSaveClick);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.OnButtonLoadClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitToolStripMenuItemClick);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugToolStripMenuItem,
            this.resetCorrelationToolStripMenuItem,
            this.useGpuMenuItem,
            this.resolutionToolStripMenuItem,
            this.stereoCalibrateToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.findFacesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotationToolStripMenuItem,
            this.corespondenceToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.rotationToolStripMenuItem.Text = "Rotation";
            this.rotationToolStripMenuItem.Click += new System.EventHandler(this.OnDebugRotationToolStripMenuItemClick);
            // 
            // corespondenceToolStripMenuItem
            // 
            this.corespondenceToolStripMenuItem.Name = "corespondenceToolStripMenuItem";
            this.corespondenceToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.corespondenceToolStripMenuItem.Text = "Corespondence";
            this.corespondenceToolStripMenuItem.Click += new System.EventHandler(this.OnDebugCorespondenceToolStripMenuItemClick);
            // 
            // resetCorrelationToolStripMenuItem
            // 
            this.resetCorrelationToolStripMenuItem.Name = "resetCorrelationToolStripMenuItem";
            this.resetCorrelationToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.resetCorrelationToolStripMenuItem.Text = "Reset Correlation";
            this.resetCorrelationToolStripMenuItem.Click += new System.EventHandler(this.OnResetCorrelationToolStripMenuItemClick);
            // 
            // useGpuMenuItem
            // 
            this.useGpuMenuItem.Name = "useGpuMenuItem";
            this.useGpuMenuItem.Size = new System.Drawing.Size(164, 22);
            this.useGpuMenuItem.Text = "Use GPU";
            this.useGpuMenuItem.Click += new System.EventHandler(this.OnUseGpuMenuItemClick);
            // 
            // resolutionToolStripMenuItem
            // 
            this.resolutionToolStripMenuItem.Name = "resolutionToolStripMenuItem";
            this.resolutionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.resolutionToolStripMenuItem.Text = "Resolution";
            // 
            // stereoCalibrateToolStripMenuItem
            // 
            this.stereoCalibrateToolStripMenuItem.Name = "stereoCalibrateToolStripMenuItem";
            this.stereoCalibrateToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.stereoCalibrateToolStripMenuItem.Text = "Stereo Calibrate";
            this.stereoCalibrateToolStripMenuItem.Click += new System.EventHandler(this.OnStereoCalibrateToolStripMenuItemClick);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // findFacesToolStripMenuItem
            // 
            this.findFacesToolStripMenuItem.Name = "findFacesToolStripMenuItem";
            this.findFacesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.findFacesToolStripMenuItem.Text = "Find Faces";
            this.findFacesToolStripMenuItem.Click += new System.EventHandler(this.OnFindFacesToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItemClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.imageBoxPoints, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(778, 263);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.nudRightRoll);
            this.panel2.Controls.Add(this.nudFaceRoll);
            this.panel2.Controls.Add(this.nudLeftRoll);
            this.panel2.Controls.Add(this.nudFaceYaw);
            this.panel2.Controls.Add(this.nudFacePitch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(383, 257);
            this.panel2.TabIndex = 0;
            // 
            // imageBoxPoints
            // 
            this.imageBoxPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxPoints.Location = new System.Drawing.Point(392, 3);
            this.imageBoxPoints.Name = "imageBoxPoints";
            this.imageBoxPoints.Size = new System.Drawing.Size(383, 257);
            this.imageBoxPoints.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBoxPoints.TabIndex = 2;
            this.imageBoxPoints.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormMainFormClosing);
            this.Load += new System.EventHandler(this.OnFormMainLoad);
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxCamera)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRightRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFacePitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFaceYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFaceRoll)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBoxCamera;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useGpuMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stereoCalibrateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCorrelationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem corespondenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findFacesToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudFacePitch;
        private System.Windows.Forms.NumericUpDown nudFaceYaw;
        private System.Windows.Forms.NumericUpDown nudFaceRoll;
        private System.Windows.Forms.NumericUpDown nudRightRoll;
        private System.Windows.Forms.NumericUpDown nudLeftRoll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private Emgu.CV.UI.ImageBox imageBoxPoints;
    }
}