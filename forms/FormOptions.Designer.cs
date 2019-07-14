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
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxQueryFolder = new System.Windows.Forms.TextBox();
            this.tab_query = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_okay = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tab_control.SuspendLayout();
            this.tab_general.SuspendLayout();
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(618, 393);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tab_control
            // 
            this.tab_control.Controls.Add(this.tab_general);
            this.tab_control.Controls.Add(this.tab_query);
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Location = new System.Drawing.Point(3, 3);
            this.tab_control.Name = "tab_control";
            this.tab_control.SelectedIndex = 0;
            this.tab_control.Size = new System.Drawing.Size(612, 337);
            this.tab_control.TabIndex = 1;
            // 
            // tab_general
            // 
            this.tab_general.Controls.Add(this.label2);
            this.tab_general.Controls.Add(this.TextBoxQueryFolder);
            this.tab_general.Location = new System.Drawing.Point(4, 22);
            this.tab_general.Name = "tab_general";
            this.tab_general.Padding = new System.Windows.Forms.Padding(3);
            this.tab_general.Size = new System.Drawing.Size(604, 311);
            this.tab_general.TabIndex = 2;
            this.tab_general.Text = "General";
            this.tab_general.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temp Folder";
            // 
            // TextBoxQueryFolder
            // 
            this.TextBoxQueryFolder.Location = new System.Drawing.Point(92, 18);
            this.TextBoxQueryFolder.Name = "TextBoxQueryFolder";
            this.TextBoxQueryFolder.Size = new System.Drawing.Size(273, 20);
            this.TextBoxQueryFolder.TabIndex = 1;
            // 
            // tab_query
            // 
            this.tab_query.Location = new System.Drawing.Point(4, 22);
            this.tab_query.Name = "tab_query";
            this.tab_query.Padding = new System.Windows.Forms.Padding(3);
            this.tab_query.Size = new System.Drawing.Size(604, 311);
            this.tab_query.TabIndex = 1;
            this.tab_query.Text = "Query";
            this.tab_query.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_okay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 346);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(612, 44);
            this.panel1.TabIndex = 2;
            // 
            // button_okay
            // 
            this.button_okay.Location = new System.Drawing.Point(519, 12);
            this.button_okay.Name = "button_okay";
            this.button_okay.Size = new System.Drawing.Size(75, 23);
            this.button_okay.TabIndex = 0;
            this.button_okay.Text = "&Ok";
            this.button_okay.UseVisualStyleBackColor = true;
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 393);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormOptions";
            this.Text = "Options";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tab_control.ResumeLayout(false);
            this.tab_general.ResumeLayout(false);
            this.tab_general.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_okay;
        private System.Windows.Forms.TabControl tab_control;
        private System.Windows.Forms.TabPage tab_query;
        private System.Windows.Forms.TabPage tab_general;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxQueryFolder;


    }
}