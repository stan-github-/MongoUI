using DBUI.Forms;
using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI {
    public partial class FormServerOptions : Form {
        public FormServerOptions() {
            InitializeComponent();
            init();
        }

        public bool init() 
        {
            SetControls();
            return true;
        }

        private void SetControls() {
            SetMongoServers();
            SetEventHandlers();

            if (this.list_view_server.Items.Count == 0) {
                return;
            }
            this.list_view_server.Items[0].Selected = true;
        }

        private void SetMongoServers()
        {
            foreach (var server in Program.Config.Data.Servers)
            {
                this.list_view_server.Items.Add(new ListViewItem(server.Name));
            }
        }

        private void SetEventHandlers() {
            this.list_view_server.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler
                (ListViewServer_SelectionChangedEventHandler);
            this.button_database_add.Click += new EventHandler(ButtonDatabaseAdd_EventHandler);
            this.button_database_delete.Click += new System.EventHandler(this.button_database_delete_Click);
            this.button_ok.Click += button_ok_Click;
            this.text_box_password.Leave += tex_box_password_Leave;
            this.textbox_user.Leave += textbox_user_Leave;
        }

        void textbox_user_Leave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void tex_box_password_Leave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void button_ok_Click(object sender, EventArgs e)
        {
            //server.User = this.textbox_user.Text;
            //server.Password = this.tex_box_password.Text;
        }

        #region database add delete
        private void ButtonDatabaseAdd_SetNewItem(String item)
        {
            if (this.list_view_server.SelectedItems.Count != 1)
            {
                return;
            }

            var serverName = this.list_view_server.SelectedItems[0].Text;

            var server = Program.Config.Data.Servers.Find(s => s.Name == serverName);
            server.Databases.Add(new DataModel.Database() { 
                Name=item
            });
         
            SetMongoDatabases(serverName);
        }

        private void button_database_delete_Click(object sender, EventArgs e)
        {
            if (this.list_view_server.SelectedItems.Count != 1 || 
                this.list_view_database.SelectedItems.Count != 1)
            {
                return;
            }
            
            var serverName = this.list_view_server.SelectedItems[0].Text;
            var server = Program.Config.Data.Servers.FirstOrDefault(s => s.Name == serverName);
            var database = server.Databases.Find(d=>d.Name == this.list_view_database.SelectedItems[0].Text);
            server.Databases.Remove(database);
            Program.Config.Save();
        }
        
        private void ButtonDatabaseAdd_EventHandler(object sender, EventArgs e){
            
            var form = new FormNewItem() {
                callBack = ButtonDatabaseAdd_SetNewItem
            };

            form.ShowDialog();
        }
        #endregion

        #region "server selected"
        private void ListViewServer_SelectionChangedEventHandler(object sender, ListViewItemSelectionChangedEventArgs e) 
        {
            var serverName = e.Item.Text;
            SetMongoDatabases(serverName);
            SetUserAndPassword(serverName);
        }

        private void SetMongoDatabases(string serverName)
        {
            this.list_view_database.Clear();

            var databases = Program.Config.Data.Servers.Find(s => s.Name == serverName).Databases;

            foreach (var database in databases)
            {
                this.list_view_database.Items.Add(new ListViewItem(database.Name));
            }
        }

        private void SetUserAndPassword(string serverName) {
            var server = Program.Config.Data.Servers.Find(s => s.Name == serverName);
            this.textbox_user.Text = server.User;
            this.text_box_password.Text = server.Password;
        }

        private void SaveUserAndPasswordToXMLCache(string serverName) {
            var server = Program.Config.Data.Servers.Find(s => s.Name == serverName);

            server.User =  this.textbox_user.Text;
            server.Password =  this.text_box_password.Text;
        }
        #endregion

    }
}
