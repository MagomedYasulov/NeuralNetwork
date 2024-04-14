namespace MedicalSystem
{
    partial class EnterData
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
            this.Predict = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Predict
            // 
            this.Predict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Predict.Location = new System.Drawing.Point(328, 395);
            this.Predict.Name = "Predict";
            this.Predict.Size = new System.Drawing.Size(75, 23);
            this.Predict.TabIndex = 0;
            this.Predict.Text = "Прогноз";
            this.Predict.UseVisualStyleBackColor = true;
            this.Predict.Click += new System.EventHandler(this.Predict_Click);
            // 
            // EnterData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 443);
            this.Controls.Add(this.Predict);
            this.Name = "EnterData";
            this.Text = "EnterData";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Predict;
    }
}