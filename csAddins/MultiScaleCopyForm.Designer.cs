namespace csAddins
{
    partial class MultiScaleCopyForm
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
            this.tbScale = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbXOffset = new System.Windows.Forms.TextBox();
            this.tbYOffset = new System.Windows.Forms.TextBox();
            this.tbZOffset = new System.Windows.Forms.TextBox();
            this.tbCopies = new System.Windows.Forms.TextBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbScale
            // 
            this.tbScale.Location = new System.Drawing.Point(118, 13);
            this.tbScale.Name = "tbScale";
            this.tbScale.Size = new System.Drawing.Size(100, 20);
            this.tbScale.TabIndex = 0;
            this.tbScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbScale_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scale";
            // 
            // tbXOffset
            // 
            this.tbXOffset.Location = new System.Drawing.Point(118, 40);
            this.tbXOffset.Name = "tbXOffset";
            this.tbXOffset.Size = new System.Drawing.Size(100, 20);
            this.tbXOffset.TabIndex = 2;
            this.tbXOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbXOffset_KeyPress);
            // 
            // tbYOffset
            // 
            this.tbYOffset.Location = new System.Drawing.Point(118, 67);
            this.tbYOffset.Name = "tbYOffset";
            this.tbYOffset.Size = new System.Drawing.Size(100, 20);
            this.tbYOffset.TabIndex = 3;
            this.tbYOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbXOffset_KeyPress);
            // 
            // tbZOffset
            // 
            this.tbZOffset.Location = new System.Drawing.Point(118, 94);
            this.tbZOffset.Name = "tbZOffset";
            this.tbZOffset.Size = new System.Drawing.Size(100, 20);
            this.tbZOffset.TabIndex = 4;
            this.tbZOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbXOffset_KeyPress);
            // 
            // tbCopies
            // 
            this.tbCopies.Location = new System.Drawing.Point(118, 121);
            this.tbCopies.Name = "tbCopies";
            this.tbCopies.Size = new System.Drawing.Size(100, 20);
            this.tbCopies.TabIndex = 5;
            this.tbCopies.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCopies_KeyPress);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(54, 162);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(115, 23);
            this.btnDefault.TabIndex = 6;
            this.btnDefault.Text = "Load Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "X Offset";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Y Offset";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Z Offset";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Copies";
            // 
            // MultiScaleCopyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 196);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.tbCopies);
            this.Controls.Add(this.tbZOffset);
            this.Controls.Add(this.tbYOffset);
            this.Controls.Add(this.tbXOffset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbScale);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiScaleCopyForm";
            this.Text = "MultiScaleCopyForm";
            this.Load += new System.EventHandler(this.MultiScaleCopyForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MultiScaleCopyForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbXOffset;
        private System.Windows.Forms.TextBox tbYOffset;
        private System.Windows.Forms.TextBox tbZOffset;
        private System.Windows.Forms.TextBox tbCopies;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}