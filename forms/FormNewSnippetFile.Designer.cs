namespace DBUI.Forms
{
    partial class FormNewSnippetFile
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
            this.text_box = new System.Windows.Forms.TextBox();
            this.button_okay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // text_box
            // 
            this.text_box.Location = new System.Drawing.Point(12, 22);
            this.text_box.Name = "text_box";
            this.text_box.Size = new System.Drawing.Size(201, 20);
            this.text_box.TabIndex = 0;
            // 
            // button_okay
            // 
            this.button_okay.Location = new System.Drawing.Point(230, 22);
            this.button_okay.Name = "button_okay";
            this.button_okay.Size = new System.Drawing.Size(75, 23);
            this.button_okay.TabIndex = 1;
            this.button_okay.Text = "&OK";
            this.button_okay.UseVisualStyleBackColor = true;
            // 
            // FormNewItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 69);
            this.Controls.Add(this.button_okay);
            this.Controls.Add(this.text_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormNewItem";
            this.Text = "New Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_box;
        private System.Windows.Forms.Button button_okay;
    }
}