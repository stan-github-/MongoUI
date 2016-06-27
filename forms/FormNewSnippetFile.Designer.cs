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
            this.text_box_file_path = new System.Windows.Forms.TextBox();
            this.button_okay = new System.Windows.Forms.Button();
            this.combo_box_group_name = new System.Windows.Forms.ComboBox();
            this.text_box_snippet_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_file_path = new System.Windows.Forms.Button();
            this.open_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // text_box_file_path
            // 
            this.text_box_file_path.Location = new System.Drawing.Point(102, 26);
            this.text_box_file_path.Name = "text_box_file_path";
            this.text_box_file_path.Size = new System.Drawing.Size(309, 20);
            this.text_box_file_path.TabIndex = 0;
            // 
            // button_okay
            // 
            this.button_okay.Location = new System.Drawing.Point(322, 136);
            this.button_okay.Name = "button_okay";
            this.button_okay.Size = new System.Drawing.Size(75, 23);
            this.button_okay.TabIndex = 1;
            this.button_okay.Text = "&OK";
            this.button_okay.UseVisualStyleBackColor = true;
            // 
            // combo_box_group_name
            // 
            this.combo_box_group_name.FormattingEnabled = true;
            this.combo_box_group_name.Location = new System.Drawing.Point(103, 59);
            this.combo_box_group_name.Name = "combo_box_group_name";
            this.combo_box_group_name.Size = new System.Drawing.Size(308, 21);
            this.combo_box_group_name.TabIndex = 2;
            // 
            // text_box_snippet_name
            // 
            this.text_box_snippet_name.Location = new System.Drawing.Point(102, 100);
            this.text_box_snippet_name.Name = "text_box_snippet_name";
            this.text_box_snippet_name.Size = new System.Drawing.Size(309, 20);
            this.text_box_snippet_name.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Group Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Name";
            // 
            // button_file_path
            // 
            this.button_file_path.Location = new System.Drawing.Point(21, 26);
            this.button_file_path.Name = "button_file_path";
            this.button_file_path.Size = new System.Drawing.Size(75, 23);
            this.button_file_path.TabIndex = 7;
            this.button_file_path.Text = "File Path";
            this.button_file_path.UseVisualStyleBackColor = true;
            // 
            // open_file_dialog
            // 
            this.open_file_dialog.FileName = "open_file_dialog";
            // 
            // FormNewSnippetFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 178);
            this.Controls.Add(this.button_file_path);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.text_box_snippet_name);
            this.Controls.Add(this.combo_box_group_name);
            this.Controls.Add(this.button_okay);
            this.Controls.Add(this.text_box_file_path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormNewSnippetFile";
            this.Text = "New Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_box_file_path;
        private System.Windows.Forms.Button button_okay;
        private System.Windows.Forms.ComboBox combo_box_group_name;
        private System.Windows.Forms.TextBox text_box_snippet_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_file_path;
        private System.Windows.Forms.OpenFileDialog open_file_dialog;
    }
}