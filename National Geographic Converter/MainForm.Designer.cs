namespace National_Geographic_Converter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._pickInputButton = new System.Windows.Forms.Button();
            this._inputPathTextBox = new System.Windows.Forms.TextBox();
            this._outputPathTextBox = new System.Windows.Forms.TextBox();
            this._pickOutputButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._numTotalLabel = new System.Windows.Forms.Label();
            this._numDoneLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pickInputButton
            // 
            this._pickInputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._pickInputButton.Location = new System.Drawing.Point(303, 12);
            this._pickInputButton.Name = "_pickInputButton";
            this._pickInputButton.Size = new System.Drawing.Size(103, 23);
            this._pickInputButton.TabIndex = 0;
            this._pickInputButton.Text = "Pick Input...";
            this._pickInputButton.UseVisualStyleBackColor = true;
            this._pickInputButton.Click += new System.EventHandler(this.PickInputButton_Click);
            // 
            // _inputPathTextBox
            // 
            this._inputPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._inputPathTextBox.Location = new System.Drawing.Point(67, 12);
            this._inputPathTextBox.Name = "_inputPathTextBox";
            this._inputPathTextBox.ReadOnly = true;
            this._inputPathTextBox.Size = new System.Drawing.Size(230, 22);
            this._inputPathTextBox.TabIndex = 1;
            // 
            // _outputPathTextBox
            // 
            this._outputPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._outputPathTextBox.Location = new System.Drawing.Point(67, 40);
            this._outputPathTextBox.Name = "_outputPathTextBox";
            this._outputPathTextBox.ReadOnly = true;
            this._outputPathTextBox.Size = new System.Drawing.Size(230, 22);
            this._outputPathTextBox.TabIndex = 2;
            // 
            // _pickOutputButton
            // 
            this._pickOutputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._pickOutputButton.Location = new System.Drawing.Point(303, 41);
            this._pickOutputButton.Name = "_pickOutputButton";
            this._pickOutputButton.Size = new System.Drawing.Size(103, 23);
            this._pickOutputButton.TabIndex = 3;
            this._pickOutputButton.Text = "Pick Output...";
            this._pickOutputButton.UseVisualStyleBackColor = true;
            this._pickOutputButton.Click += new System.EventHandler(this.PickOutputButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Input:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._numTotalLabel);
            this.groupBox1.Controls.Add(this._numDoneLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._progressBar);
            this.groupBox1.Location = new System.Drawing.Point(13, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 109);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // _numTotalLabel
            // 
            this._numTotalLabel.Location = new System.Drawing.Point(77, 52);
            this._numTotalLabel.Name = "_numTotalLabel";
            this._numTotalLabel.Size = new System.Drawing.Size(87, 13);
            this._numTotalLabel.TabIndex = 4;
            this._numTotalLabel.Text = "##TOTAL##";
            this._numTotalLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _numDoneLabel
            // 
            this._numDoneLabel.Location = new System.Drawing.Point(77, 28);
            this._numDoneLabel.Name = "_numDoneLabel";
            this._numDoneLabel.Size = new System.Drawing.Size(87, 13);
            this._numDoneLabel.TabIndex = 3;
            this._numDoneLabel.Text = "##DONE##";
            this._numDoneLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Total:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Files Done:";
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(6, 80);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(390, 23);
            this._progressBar.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 191);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._pickOutputButton);
            this.Controls.Add(this._outputPathTextBox);
            this.Controls.Add(this._inputPathTextBox);
            this.Controls.Add(this._pickInputButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "National Geographic Converter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _pickInputButton;
        private System.Windows.Forms.TextBox _inputPathTextBox;
        private System.Windows.Forms.TextBox _outputPathTextBox;
        private System.Windows.Forms.Button _pickOutputButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label _numTotalLabel;
        private System.Windows.Forms.Label _numDoneLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar _progressBar;
    }
}

