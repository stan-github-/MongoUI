using DBUI.DataModel;
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

        public Server SelectedServer {
            get {
                //assume localhost is always set, and we will always have one server!
                var server = Program.Config.Data.Miscellaneous.ServerOptions.SelectedServer;
                return Program.Config.Data.Servers.FirstOrDefault(s => s.Name == server);
            }            
        }

        private void SetControls() {
            SetMongoServers();
            SetEventHandlers();

            foreach (ListViewItem i in list_view_server.Items) {
                if (i.Text == SelectedServer.Name) {
                    i.Selected = true;
                    break;
                }
            }
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


        #region database add delete
        private void ButtonDatabaseAdd_SetNewItem(String item)
        {
            if (this.list_view_server.SelectedItems.Count != 1)
            {
                return;
            }

            SelectedServer.Databases.Add(new DataModel.Database() { 
                Name=item
            });
         
            SetMongoDatabases();
        }

        private void button_database_delete_Click(object sender, EventArgs e)
        {
            if (this.list_view_server.SelectedItems.Count != 1 || 
                this.list_view_database.SelectedItems.Count != 1)
            {
                return;
            }

            var server = SelectedServer;
            var database = server.Databases.Find(d=>d.Name == this.list_view_database.SelectedItems[0].Text);
            server.Databases.Remove(database);
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
            Program.Config.Data.Miscellaneous.ServerOptions.SelectedServer = e.Item.Text;

            SetMongoDatabases();
            SetUserAndPassword();
        }

        private void SetMongoDatabases()
        {
            this.list_view_database.Clear();

            var databases = SelectedServer.Databases;
            foreach (var database in databases)
            {
                this.list_view_database.Items.Add(new ListViewItem(database.Name));
            }
        }

        private void SetUserAndPassword() {
            var server = SelectedServer;
            this.textbox_user.Text = server.User;
            this.text_box_password.Text = server.Password;
        }

        void textbox_user_Leave(object sender, EventArgs e)
        {
            SelectedServer.User = this.textbox_user.Text;
        }

        void tex_box_password_Leave(object sender, EventArgs e)
        {
            SelectedServer.Password = this.text_box_password.Text;
        }

        void button_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
