namespace DBUI {
    partial class FormOptionsSnippets {
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_add_file = new System.Windows.Forms.Button();
            this.button_delete_file = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.list_view_files = new System.Windows.Forms.ListView();
            this.layout_panel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.layout_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.list_view_files);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button_cancel);
            this.splitContainer1.Panel2.Controls.Add(this.button_delete_file);
            this.splitContainer1.Panel2.Controls.Add(this.button_add_file);
            this.splitContainer1.Size = new System.Drawing.Size(686, 508);
            this.splitContainer1.SplitterDistance = 429;
            this.splitContainer1.TabIndex = 6;
            // 
            // button_add_file
            // 
            this.button_add_file.Location = new System.Drawing.Point(323, 20);
            this.button_add_file.Name = "button_add_file";
            this.button_add_file.Size = new System.Drawing.Size(75, 23);
            this.button_add_file.TabIndex = 0;
            this.button_add_file.Text = "&Add";
            this.button_add_file.UseVisualStyleBackColor = true;
            // 
            // button_delete_file
            // 
            this.button_delete_file.Location = new System.Drawing.Point(443, 20);
            this.button_delete_file.Name = "button_delete_file";
            this.button_delete_file.Size = new System.Drawing.Size(75, 23);
            this.button_delete_file.TabIndex = 1;
            this.button_delete_file.Text = "&Delete";
            this.button_delete_file.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(566, 20);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "&Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // list_view_files
            // 
            this.list_view_files.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.list_view_files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_view_files.HoverSelection = true;
            this.list_view_files.Location = new System.Drawing.Point(0, 0);
            this.list_view_files.Name = "list_view_files";
            this.list_view_files.Size = new System.Drawing.Size(686, 429);
            this.list_view_files.TabIndex = 0;
            this.list_view_files.UseCompatibleStateImageBehavior = false;
            this.list_view_files.View = System.Windows.Forms.View.Details;
            // 
            // layout_panel
            // 
            this.layout_panel.ColumnCount = 1;
            this.layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout_panel.Controls.Add(this.splitContainer1, 0, 0);
            this.layout_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout_panel.Location = new System.Drawing.Point(0, 0);
            this.layout_panel.Name = "layout_panel";
            this.layout_panel.RowCount = 1;
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 346F));
            this.layout_panel.Size = new System.Drawing.Size(692, 514);
            this.layout_panel.TabIndex = 1;
            // 
            // FormOptionsSnippets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 514);
            this.Controls.Add(this.layout_panel);
            this.Name = "FormOptionsSnippets";
            this.Text = "Server Options";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.layout_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView list_view_files;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_delete_file;
        private System.Windows.Forms.Button button_add_file;
        private System.Windows.Forms.TableLayoutPanel layout_panel;



    }
}