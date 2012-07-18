namespace DBUI {
    partial class FormOptions {
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tab_control = new System.Windows.Forms.TabControl();
            this.tab_general = new System.Windows.Forms.TabPage();
            this.checkBoxDeleteTempFolderContents = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxTempFolder = new System.Windows.Forms.TextBox();
            this.tab_query = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxQueryFolder = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tab_control.SuspendLayout();
            this.tab_general.SuspendLayout();
            this.tab_query.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tab_control, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(824, 484);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tab_control
            // 
            this.tab_control.Controls.Add(this.tab_general);
            this.tab_control.Controls.Add(this.tab_query);
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Location = new System.Drawing.Point(4, 4);
            this.tab_control.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_control.Name = "tab_control";
            this.tab_control.SelectedIndex = 0;
            this.tab_control.Size = new System.Drawing.Size(816, 414);
            this.tab_control.TabIndex = 1;
            // 
            // tab_general
            // 
            this.tab_general.Controls.Add(this.checkBoxDeleteTempFolderContents);
            this.tab_general.Controls.Add(this.label2);
            this.tab_general.Controls.Add(this.TextBoxTempFolder);
            this.tab_general.Location = new System.Drawing.Point(4, 25);
            this.tab_general.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_general.Name = "tab_general";
            this.tab_general.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_general.Size = new System.Drawing.Size(808, 385);
            this.tab_general.TabIndex = 2;
            this.tab_general.Text = "General";
            this.tab_general.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeleteTempFolderContents
            // 
            this.checkBoxDeleteTempFolderContents.AutoSize = true;
            this.checkBoxDeleteTempFolderContents.Location = new System.Drawing.Point(519, 26);
            this.checkBoxDeleteTempFolderContents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDeleteTempFolderContents.Name = "checkBoxDeleteTempFolderContents";
            this.checkBoxDeleteTempFolderContents.Size = new System.Drawing.Size(174, 21);
            this.checkBoxDeleteTempFolderContents.TabIndex = 3;
            this.checkBoxDeleteTempFolderContents.Text = "Delete contents on exit";
            this.checkBoxDeleteTempFolderContents.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temp Folder";
            // 
            // TextBoxTempFolder
            // 
            this.TextBoxTempFolder.Location = new System.Drawing.Point(123, 22);
            this.TextBoxTempFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextBoxTempFolder.Name = "TextBoxTempFolder";
            this.TextBoxTempFolder.Size = new System.Drawing.Size(363, 22);
            this.TextBoxTempFolder.TabIndex = 1;
            // 
            // tab_query
            // 
            this.tab_query.Controls.Add(this.label1);
            this.tab_query.Controls.Add(this.textBoxQueryFolder);
            this.tab_query.Location = new System.Drawing.Point(4, 25);
            this.tab_query.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_query.Name = "tab_query";
            this.tab_query.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_query.Size = new System.Drawing.Size(808, 385);
            this.tab_query.TabIndex = 1;
            this.tab_query.Text = "Query";
            this.tab_query.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Query Folder";
            // 
            // textBoxQueryFolder
            // 
            this.textBoxQueryFolder.Location = new System.Drawing.Point(124, 27);
            this.textBoxQueryFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxQueryFolder.Name = "textBoxQueryFolder";
            this.textBoxQueryFolder.Size = new System.Drawing.Size(363, 22);
            this.textBoxQueryFolder.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_apply);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 426);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(816, 54);
            this.panel1.TabIndex = 2;
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(704, 15);
            this.button_apply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(100, 28);
            this.button_apply.TabIndex = 2;
            this.button_apply.Text = "&Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(589, 15);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 28);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "&Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(481, 15);
            this.button_save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(100, 28);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "&Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 484);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormOptions";
            this.Text = "Options";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tab_control.ResumeLayout(false);
            this.tab_general.ResumeLayout(false);
            this.tab_general.PerformLayout();
            this.tab_query.ResumeLayout(false);
            this.tab_query.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.TabControl tab_control;
        private System.Windows.Forms.TabPage tab_query;
        private System.Windows.Forms.TabPage tab_general;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxTempFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxQueryFolder;
        private System.Windows.Forms.CheckBox checkBoxDeleteTempFolderContents;


    }
}