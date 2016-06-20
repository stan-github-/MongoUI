﻿namespace DBUI {
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.database_split_container = new System.Windows.Forms.SplitContainer();
            this.button_database_add = new System.Windows.Forms.Button();
            this.button_database_delete = new System.Windows.Forms.Button();
            this.list_view_database = new System.Windows.Forms.ListView();
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
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layout_panel.Size = new System.Drawing.Size(1087, 538);
            this.layout_panel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_apply);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 485);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 50);
            this.panel1.TabIndex = 2;
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(528, 12);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 2;
            this.button_apply.Text = "&Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(442, 12);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "&Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(361, 12);
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
            this.main_split_conntainer.Size = new System.Drawing.Size(1081, 476);
            this.main_split_conntainer.SplitterDistance = 360;
            this.main_split_conntainer.TabIndex = 4;
            // 
            // list_view_server
            // 
            this.list_view_server.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_view_server.Location = new System.Drawing.Point(0, 0);
            this.list_view_server.Name = "list_view_server";
            this.list_view_server.Size = new System.Drawing.Size(360, 476);
            this.list_view_server.TabIndex = 4;
            this.list_view_server.UseCompatibleStateImageBehavior = false;
            this.list_view_server.View = System.Windows.Forms.View.List;
            // 
            // tab_control
            // 
            this.tab_control.Controls.Add(this.tab_page_databases);
            this.tab_control.Controls.Add(this.tabPage2);
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Location = new System.Drawing.Point(0, 0);
            this.tab_control.Name = "tab_control";
            this.tab_control.SelectedIndex = 0;
            this.tab_control.Size = new System.Drawing.Size(717, 476);
            this.tab_control.TabIndex = 1;
            // 
            // tab_page_databases
            // 
            this.tab_page_databases.Controls.Add(this.database_split_container);
            this.tab_page_databases.Location = new System.Drawing.Point(4, 22);
            this.tab_page_databases.Name = "tab_page_databases";
            this.tab_page_databases.Padding = new System.Windows.Forms.Padding(3);
            this.tab_page_databases.Size = new System.Drawing.Size(709, 450);
            this.tab_page_databases.TabIndex = 0;
            this.tab_page_databases.Text = "databases";
            this.tab_page_databases.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(709, 450);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.database_split_container.Size = new System.Drawing.Size(703, 444);
            this.database_split_container.SplitterDistance = 390;
            this.database_split_container.TabIndex = 0;
            // 
            // button_database_add
            // 
            this.button_database_add.Location = new System.Drawing.Point(520, 12);
            this.button_database_add.Name = "button_database_add";
            this.button_database_add.Size = new System.Drawing.Size(75, 23);
            this.button_database_add.TabIndex = 1;
            this.button_database_add.Text = "&Add";
            this.button_database_add.UseVisualStyleBackColor = true;
            // 
            // button_database_delete
            // 
            this.button_database_delete.Location = new System.Drawing.Point(614, 12);
            this.button_database_delete.Name = "button_database_delete";
            this.button_database_delete.Size = new System.Drawing.Size(75, 23);
            this.button_database_delete.TabIndex = 2;
            this.button_database_delete.Text = "&Delete";
            this.button_database_delete.UseVisualStyleBackColor = true;
            
            // 
            // list_view_database
            // 
            this.list_view_database.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_view_database.Location = new System.Drawing.Point(0, 0);
            this.list_view_database.Name = "list_view_database";
            this.list_view_database.Size = new System.Drawing.Size(703, 390);
            this.list_view_database.TabIndex = 0;
            this.list_view_database.UseCompatibleStateImageBehavior = false;
            this.list_view_database.View = System.Windows.Forms.View.List;
            // 
            // FormServerOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 538);
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
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView list_view_database;
        private System.Windows.Forms.Button button_database_delete;
        private System.Windows.Forms.Button button_database_add;


    }
}