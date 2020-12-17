namespace csAddins
{
    partial class ToolbarForm
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
            this.btnModal = new System.Windows.Forms.Button();
            this.btnOntop = new System.Windows.Forms.Button();
            this.btnTools = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnModal
            // 
            this.btnModal.Image = global::csAddins.Properties.Resources.modal;
            this.btnModal.Location = new System.Drawing.Point(3, 3);
            this.btnModal.Name = "btnModal";
            this.btnModal.Size = new System.Drawing.Size(28, 28);
            this.btnModal.TabIndex = 0;
            this.btnModal.UseVisualStyleBackColor = true;
            this.btnModal.Click += new System.EventHandler(this.btnModal_Click);
            // 
            // btnOntop
            // 
            this.btnOntop.Image = global::csAddins.Properties.Resources.onTop;
            this.btnOntop.Location = new System.Drawing.Point(37, 2);
            this.btnOntop.Name = "btnOntop";
            this.btnOntop.Size = new System.Drawing.Size(28, 28);
            this.btnOntop.TabIndex = 1;
            this.btnOntop.UseVisualStyleBackColor = true;
            this.btnOntop.Click += new System.EventHandler(this.btnOntop_Click);
            // 
            // btnTools
            // 
            this.btnTools.Image = global::csAddins.Properties.Resources.tools;
            this.btnTools.Location = new System.Drawing.Point(71, 2);
            this.btnTools.Name = "btnTools";
            this.btnTools.Size = new System.Drawing.Size(28, 28);
            this.btnTools.TabIndex = 2;
            this.btnTools.UseVisualStyleBackColor = true;
            this.btnTools.Click += new System.EventHandler(this.btnTools_Click);
            // 
            // ToolbarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(104, 36);
            this.Controls.Add(this.btnTools);
            this.Controls.Add(this.btnOntop);
            this.Controls.Add(this.btnModal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolbarForm";
            this.Text = "Toolbar";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnModal;
        private System.Windows.Forms.Button btnOntop;
        private System.Windows.Forms.Button btnTools;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}