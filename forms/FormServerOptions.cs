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

        public void finalize() {
            Program.JsEngine.Repository.SaveXml();
        }

        private void button_save_Click(object sender, EventArgs e) {
            SaveControls();
            this.finalize();
            this.Close();
        }

        private void button_apply_Click(object sender, EventArgs e) {
            SaveControls();
            this.finalize();
        }

        private void button_cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void SetControls() {
            SetMongoServers();

            SetEventHandlers();
            //this.textBoxQueryFolder.Text = Program.MainXMLManager.QueryFolderPath;
            //this.TextBoxTempFolder.Text = Program.MainXMLManager.TempFolderPath;
            //this.checkBoxDeleteTempFolderContents.Checked = Program.MainXMLManager.DeleteTempFolderContents;
        }

        private void SaveControls() {
            //Program.MainXMLManager.QueryFolderPath = this.textBoxQueryFolder.Text;
            //Program.MainXMLManager.TempFolderPath = this.TextBoxTempFolder.Text;
            //Program.MainXMLManager.DeleteTempFolderContents = this.checkBoxDeleteTempFolderContents.Checked;   
        }


        private void SetEventHandlers() {
            this.list_view_server.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler
                (ListViewServer_SelectionChangedEventHandler);
            this.button_database_add.Click += new EventHandler(ButtonDatabaseAdd_EventHandler);
        }

        private void ButtonDatabaseAdd_SetNewItem(String item)
        {
            if (this.list_view_server.SelectedItems.Count != 1)
            {
                return;
            }

            var serverName = this.list_view_server.SelectedItems[0].Text;
            var mongoRepo = GetMongoRepo();
            var databases = mongoRepo.Servers.First(s => s.Name == serverName).Databases;

            databases.Add(new DataModel.Database()
            {
                Name = item,
            });

            Program.JsEngine.Repository.SaveXml();
            Program.MainXMLManager.SaveXml();

            SetMongoDatabases(serverName);
        }

        private void ButtonDatabaseAdd_EventHandler(object sender, EventArgs e){
            
            var form = new FormNewItem() {
                callBack = ButtonDatabaseAdd_SetNewItem
            };

            form.ShowDialog();
            
        }

        private void ListViewServer_SelectionChangedEventHandler(object sender, ListViewItemSelectionChangedEventArgs e) 
        {
            var serverName = e.Item.Text;
            SetMongoDatabases(serverName);
        }

        private MongoXMLRepository GetMongoRepo() {
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB)
            {
                return null;
            }

            var mongoRepo = (MongoXMLRepository)Program.JsEngine.MongoEngine.Repository;

            return mongoRepo;
        }

        private void SetMongoDatabases(string serverName)
        {
            var mongoRepo = GetMongoRepo();

            this.list_view_database.Clear();

            var databases = mongoRepo.Servers.First(s=>s.Name == serverName).Databases;

            foreach (var database in databases)
            {
                this.list_view_database.Items.Add(new ListViewItem(database.Name));
            }
        }

        private void SetMongoServers() {
            var mongoRepo = GetMongoRepo();

            foreach (var server in mongoRepo.Servers) {
                this.list_view_server.Items.Add(new ListViewItem(server.Name));
            }
        }
    }
}
