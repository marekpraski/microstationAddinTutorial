
namespace csAddins
{
    partial class SegmentDrawForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbStart = new System.Windows.Forms.TextBox();
            this.labelKoniec = new System.Windows.Forms.Label();
            this.tbKoniec = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "metry start przerwy";
            // 
            // tbStart
            // 
            this.tbStart.Location = new System.Drawing.Point(128, 14);
            this.tbStart.Name = "tbStart";
            this.tbStart.Size = new System.Drawing.Size(100, 20);
            this.tbStart.TabIndex = 1;
            // 
            // labelKoniec
            // 
            this.labelKoniec.AutoSize = true;
            this.labelKoniec.Location = new System.Drawing.Point(12, 44);
            this.labelKoniec.Name = "labelKoniec";
            this.labelKoniec.Size = new System.Drawing.Size(106, 13);
            this.labelKoniec.TabIndex = 2;
            this.labelKoniec.Text = "metry koniec przerwy";
            // 
            // tbKoniec
            // 
            this.tbKoniec.Location = new System.Drawing.Point(128, 41);
            this.tbKoniec.Name = "tbKoniec";
            this.tbKoniec.Size = new System.Drawing.Size(100, 20);
            this.tbKoniec.TabIndex = 3;
            // 
            // SegmentDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 69);
            this.Controls.Add(this.tbKoniec);
            this.Controls.Add(this.labelKoniec);
            this.Controls.Add(this.tbStart);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SegmentDrawForm";
            this.Text = "SegmentDrawForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbStart;
        private System.Windows.Forms.Label labelKoniec;
        public System.Windows.Forms.TextBox tbKoniec;
    }
}