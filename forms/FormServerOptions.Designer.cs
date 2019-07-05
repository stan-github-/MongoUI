namespace DBUI {
    partial class FormServerOptions {
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
            this.layout_panel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.main_split_conntainer = new System.Windows.Forms.SplitContainer();
            this.list_view_server = new System.Windows.Forms.ListView();
            this.tab_control = new System.Windows.Forms.TabControl();
            this.tab_page_databases = new System.Windows.Forms.TabPage();
            this.database_split_container = new System.Windows.Forms.SplitContainer();
            this.list_view_database = new System.Windows.Forms.ListView();
            this.button_database_delete = new System.Windows.Forms.Button();
            this.button_database_add = new System.Windows.Forms.Button();
            this.tab_page_user_password = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tex_box_password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textbox_user = new System.Windows.Forms.TextBox();
            this.layout_panel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.main_split_conntainer)).BeginInit();
            this.main_split_conntainer.Panel1.SuspendLayout();
            this.main_split_conntainer.Panel2.SuspendLayout();
            this.main_split_conntainer.SuspendLayout();
            this.tab_control.SuspendLayout();
            this.tab_page_databases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.database_split_container)).BeginInit();
            this.database_split_container.Panel1.SuspendLayout();
            this.database_split_container.Panel2.SuspendLayout();
            this.database_split_container.SuspendLayout();
            this.tab_page_user_password.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout_panel
            // 
            this.layout_panel.ColumnCount = 1;
            this.layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout_panel.Controls.Add(this.panel1, 0, 1);
            this.layout_panel.Controls.Add(this.main_split_conntainer, 0, 0);
            this.layout_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout_panel.Location = new System.Drawing.Point(0, 0);
            this.layout_panel.Name = "layout_panel";
            this.layout_panel.RowCount = 2;
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layout_panel.Size = new System.Drawing.Size(701, 344);
            this.layout_panel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_apply);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Location = new System.Drawing.Point(3, 306);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(695, 35);
            this.panel1.TabIndex = 2;
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(611, 9);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 2;
            this.button_apply.Text = "&Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(523, 9);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "&Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(433, 9);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "&Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // main_split_conntainer
            // 
            this.main_split_conntainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_split_conntainer.Location = new System.Drawing.Point(3, 3);
            this.main_split_conntainer.Name = "main_split_conntainer";
            // 
            // main_split_conntainer.Panel1
            // 
            this.main_split_conntainer.Panel1.Controls.Add(this.list_view_server);
            // 
            // main_split_conntainer.Panel2
            // 
            this.main_split_conntainer.Panel2.Controls.Add(this.tab_control);
            this.main_split_conntainer.Size = new System.Drawing.Size(695, 297);
            this.main_split_conntainer.SplitterDistance = 141;
            this.main_split_conntainer.TabIndex = 4;
            // 
            // list_view_server
            // 
            this.list_view_server.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_view_server.HideSelection = false;
            this.list_view_server.Location = new System.Drawing.Point(0, 0);
            this.list_view_server.MultiSelect = false;
            this.list_view_server.Name = "list_view_server";
            this.list_view_server.Size = new System.Drawing.Size(141, 297);
            this.list_view_server.TabIndex = 4;
            this.list_view_server.UseCompatibleStateImageBehavior = false;
            this.list_view_server.View = System.Windows.Forms.View.List;
            // 
            // tab_control
            // 
            this.tab_control.Controls.Add(this.tab_page_databases);
            this.tab_control.Controls.Add(this.tab_page_user_password);
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Location = new System.Drawing.Point(0, 0);
            this.tab_control.Name = "tab_control";
            this.tab_control.SelectedIndex = 0;
            this.tab_control.Size = new System.Drawing.Size(550, 297);
            this.tab_control.TabIndex = 1;
            // 
            // tab_page_databases
            // 
            this.tab_page_databases.Controls.Add(this.database_split_container);
            this.tab_page_databases.Location = new System.Drawing.Point(4, 22);
            this.tab_page_databases.Name = "tab_page_databases";
            this.tab_page_databases.Padding = new System.Windows.Forms.Padding(3);
            this.tab_page_databases.Size = new System.Drawing.Size(542, 271);
            this.tab_page_databases.TabIndex = 0;
            this.tab_page_databases.Text = "databases";
            this.tab_page_databases.UseVisualStyleBackColor = true;
            // 
            // database_split_container
            // 
            this.database_split_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.database_split_container.Location = new System.Drawing.Point(3, 3);
            this.database_split_container.Name = "database_split_container";
            this.database_split_container.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // database_split_container.Panel1
            // 
            this.database_split_container.Panel1.Controls.Add(this.list_view_database);
            // 
            // database_split_container.Panel2
            // 
            this.database_split_container.Panel2.Controls.Add(this.button_database_delete);
            this.database_split_container.Panel2.Controls.Add(this.button_database_add);
            this.database_split_container.Size = new System.Drawing.Size(536, 265);
            this.database_split_container.SplitterDistance = 232;
            this.database_split_container.TabIndex = 0;
            // 
            // list_view_database
            // 
            this.list_view_database.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_view_database.Location = new System.Drawing.Point(0, 0);
            this.list_view_database.Name = "list_view_database";
            this.list_view_database.Size = new System.Drawing.Size(536, 232);
            this.list_view_database.TabIndex = 0;
            this.list_view_database.UseCompatibleStateImageBehavior = false;
            this.list_view_database.View = System.Windows.Forms.View.List;
            // 
            // button_database_delete
            // 
            this.button_database_delete.Location = new System.Drawing.Point(455, 8);
            this.button_database_delete.Name = "button_database_delete";
            this.button_database_delete.Size = new System.Drawing.Size(75, 23);
            this.button_database_delete.TabIndex = 2;
            this.button_database_delete.Text = "&Delete";
            this.button_database_delete.UseVisualStyleBackColor = true;
            // 
            // button_database_add
            // 
            this.button_database_add.Location = new System.Drawing.Point(370, 8);
            this.button_database_add.Name = "button_database_add";
            this.button_database_add.Size = new System.Drawing.Size(75, 23);
            this.button_database_add.TabIndex = 1;
            this.button_database_add.Text = "&Add";
            this.button_database_add.UseVisualStyleBackColor = true;
            // 
            // tab_page_user_password
            // 
            this.tab_page_user_password.Controls.Add(this.label2);
            this.tab_page_user_password.Controls.Add(this.tex_box_password);
            this.tab_page_user_password.Controls.Add(this.label1);
            this.tab_page_user_password.Controls.Add(this.textbox_user);
            this.tab_page_user_password.Location = new System.Drawing.Point(4, 22);
            this.tab_page_user_password.Name = "tab_page_user_password";
            this.tab_page_user_password.Padding = new System.Windows.Forms.Padding(3);
            this.tab_page_user_password.Size = new System.Drawing.Size(709, 450);
            this.tab_page_user_password.TabIndex = 1;
            this.tab_page_user_password.Text = "user password";
            this.tab_page_user_password.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // tex_box_password
            // 
            this.tex_box_password.Location = new System.Drawing.Point(104, 72);
            this.tex_box_password.Name = "tex_box_password";
            this.tex_box_password.Size = new System.Drawing.Size(226, 20);
            this.tex_box_password.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User";
            // 
            // textbox_user
            // 
            this.textbox_user.Location = new System.Drawing.Point(102, 22);
            this.textbox_user.Name = "textbox_user";
            this.textbox_user.Size = new System.Drawing.Size(226, 20);
            this.textbox_user.TabIndex = 0;
            // 
            // FormServerOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 344);
            this.Controls.Add(this.layout_panel);
            this.Name = "FormServerOptions";
            this.Text = "Server Options";
            this.layout_panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.main_split_conntainer.Panel1.ResumeLayout(false);
            this.main_split_conntainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.main_split_conntainer)).EndInit();
            this.main_split_conntainer.ResumeLayout(false);
            this.tab_control.ResumeLayout(false);
            this.tab_page_databases.ResumeLayout(false);
            this.database_split_container.Panel1.ResumeLayout(false);
            this.database_split_container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.database_split_container)).EndInit();
            this.database_split_container.ResumeLayout(false);
            this.tab_page_user_password.ResumeLayout(false);
            this.tab_page_user_password.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layout_panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.SplitContainer main_split_conntainer;
        private System.Windows.Forms.ListView list_view_server;
        private System.Windows.Forms.TabControl tab_control;
        private System.Windows.Forms.TabPage tab_page_databases;
        private System.Windows.Forms.SplitContainer database_split_container;
        private System.Windows.Forms.TabPage tab_page_user_password;
        private System.Windows.Forms.ListView list_view_database;
        private System.Windows.Forms.Button button_database_delete;
        private System.Windows.Forms.Button button_database_add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tex_box_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textbox_user;


    }
}