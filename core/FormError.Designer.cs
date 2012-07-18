namespace DBUI {
    partial class FormError {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.textbox_error = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textbox_error
            // 
            this.textbox_error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textbox_error.Location = new System.Drawing.Point(0, 0);
            this.textbox_error.Multiline = true;
            this.textbox_error.Name = "textbox_error";
            this.textbox_error.Size = new System.Drawing.Size(292, 273);
            this.textbox_error.TabIndex = 1;
            // 
            // FormError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.textbox_error);
            this.Name = "FormError";
            this.Text = "FormError";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textbox_error;

    }
}