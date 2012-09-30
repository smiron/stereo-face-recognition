namespace FaceDetection
{
    partial class FormFaces
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
            this.dgvFaces = new System.Windows.Forms.DataGridView();
            this.ColumnLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNumImages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonStartRecord = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.tboxRecordNewLabel = new System.Windows.Forms.TextBox();
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonEndRecord = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaces)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFaces
            // 
            this.dgvFaces.AllowUserToAddRows = false;
            this.dgvFaces.AllowUserToDeleteRows = false;
            this.dgvFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFaces.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaces.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLabel,
            this.ColumnNumImages});
            this.dgvFaces.Location = new System.Drawing.Point(12, 41);
            this.dgvFaces.Name = "dgvFaces";
            this.dgvFaces.ReadOnly = true;
            this.dgvFaces.Size = new System.Drawing.Size(486, 225);
            this.dgvFaces.TabIndex = 0;
            // 
            // ColumnLabel
            // 
            this.ColumnLabel.DataPropertyName = "Label";
            this.ColumnLabel.HeaderText = "Label";
            this.ColumnLabel.Name = "ColumnLabel";
            this.ColumnLabel.ReadOnly = true;
            this.ColumnLabel.Width = 343;
            // 
            // ColumnNumImages
            // 
            this.ColumnNumImages.DataPropertyName = "ImagesCount";
            this.ColumnNumImages.HeaderText = "Images";
            this.ColumnNumImages.Name = "ColumnNumImages";
            this.ColumnNumImages.ReadOnly = true;
            // 
            // buttonStartRecord
            // 
            this.buttonStartRecord.Location = new System.Drawing.Point(12, 12);
            this.buttonStartRecord.Name = "buttonStartRecord";
            this.buttonStartRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonStartRecord.TabIndex = 1;
            this.buttonStartRecord.Text = "Start Record";
            this.buttonStartRecord.UseVisualStyleBackColor = true;
            this.buttonStartRecord.Click += new System.EventHandler(this.OnButtonStartRecordClick);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(93, 272);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            // 
            // tboxRecordNewLabel
            // 
            this.tboxRecordNewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxRecordNewLabel.Location = new System.Drawing.Point(174, 12);
            this.tboxRecordNewLabel.Name = "tboxRecordNewLabel";
            this.tboxRecordNewLabel.Size = new System.Drawing.Size(324, 20);
            this.tboxRecordNewLabel.TabIndex = 3;
            // 
            // buttonView
            // 
            this.buttonView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonView.Location = new System.Drawing.Point(12, 272);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 4;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(423, 272);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnButtonCloseClick);
            // 
            // buttonEndRecord
            // 
            this.buttonEndRecord.Location = new System.Drawing.Point(93, 12);
            this.buttonEndRecord.Name = "buttonEndRecord";
            this.buttonEndRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonEndRecord.TabIndex = 6;
            this.buttonEndRecord.Text = "End Record";
            this.buttonEndRecord.UseVisualStyleBackColor = true;
            this.buttonEndRecord.Click += new System.EventHandler(this.OnButtonEndRecordClick);
            // 
            // FormFaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 307);
            this.ControlBox = false;
            this.Controls.Add(this.buttonEndRecord);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.tboxRecordNewLabel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonStartRecord);
            this.Controls.Add(this.dgvFaces);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFaces";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Faces";
            this.Load += new System.EventHandler(this.OnFormFacesLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaces)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFaces;
        private System.Windows.Forms.Button buttonStartRecord;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.TextBox tboxRecordNewLabel;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonEndRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumImages;

    }
}