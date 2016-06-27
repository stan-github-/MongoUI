using DBUI.Forms;
using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI {
    public partial class FormOptionsSnippets : Form {
        
        public FormOptionsSnippets() {
            InitializeComponent();
            init();
        }

        public bool init() 
        {
            SetControls();
            return true;
        }

        public void finalize() {

            //save user name and password
            var server = list_view_files.SelectedItems[0].Text;            
            Program.JsEngine.Repository.SaveXml();
        }

        private void button_save_Click(object sender, EventArgs e) {
            this.finalize();
            this.Close();
        }

        private void button_apply_Click(object sender, EventArgs e) {
            this.finalize();
        }

        private void button_cancel_Click(object sender, EventArgs e) {
            //todo:
            //reload everything from saved xml file
            //may not be the best, since file history is lost
            //could make this just reload the servers and not any other
            Program.JsEngine.Repository.Init(JsEngineType.MongoDB);
            this.Close();
        }

        private void SetControls() {
            SetDisplayMongoServers();

            this.list_view_files.Items[0].Selected = true;
            SetEventHandlers();
            
        }


        private void SetEventHandlers() {
            this.list_view_files.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler
                (ListViewServer_SelectionChangedEventHandler);
            this.button_add_file.Click += new EventHandler(ButtonFileAdd_EventHandler);
            this.button_delete_file.Click += new System.EventHandler(this.buttonFileDelete_Click);
        }

        private string OpenOpenFileDialog()
        {
            this.open_file_dialog.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.open_file_dialog.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";

            //minimize window, can't hide
            //todo should be better way to do this
            if (Program.MainXMLManager.CurrentEngine == JsEngineType.MongoDB)
            {
                this.open_file_dialog.InitialDirectory =
                Path.GetDirectoryName(Program.JsEngine.MongoEngine.Repository.GetQueryFolder());
            }
            else {
                throw new NotImplementedException();
            }
            

            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }

            return this.open_file_dialog.FileName;
        }

        private void ButtonFileAdd_EventHandler(object sender, EventArgs e)
        {

            var filePath = OpenOpenFileDialog();

            var mongoRepo = GetMongoRepo();
            //mongoRepo.AddDatabase(serverName, item);
         
            //SetDisplayMongoDatabases(serverName);
        }

        private void buttonFileDelete_Click(object sender, EventArgs e)
        {
            return;
            if (this.list_view_files.SelectedItems.Count != 1)
            {
                return;
            }
            
            var serverName = this.list_view_files.SelectedItems[0].Text;
            var mongoRepo = GetMongoRepo();
            mongoRepo.DeleteDatabase(serverName, this.list_view_files.SelectedItems[0].Text);
            SetDisplayMongoDatabases(serverName);
        }
        
        private void ButtonDatabaseAdd_EventHandler(object sender, EventArgs e){
            
            var form = new FormNewItem() {
                //callBack = ButtonFileAdd_SetNewItem
            };

            form.ShowDialog();
            
        }

        private void ListViewServer_SelectionChangedEventHandler(object sender, ListViewItemSelectionChangedEventArgs e) 
        {
            var serverName = e.Item.Text;
            SetDisplayMongoDatabases(serverName);
            SetDisplayUserAndPassword(serverName);
        }

        private MongoXMLRepository GetMongoRepo() {
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB)
            {
                return null;
            }

            var mongoRepo = (MongoXMLRepository)Program.JsEngine.MongoEngine.Repository;

            return mongoRepo;
        }

        private void SetDisplayMongoDatabases(string serverName)
        {
            var mongoRepo = GetMongoRepo();

            //this.list_view_database.Clear();

            var databases = mongoRepo.Servers.First(s=>s.Name == serverName).Databases;

            foreach (var database in databases)
            {
                //this.list_view_database.Items.Add(new ListViewItem(database.Name));
            }
        }

        private void SetDisplayMongoServers() {
            var mongoRepo = GetMongoRepo();

            foreach (var server in mongoRepo.Servers) {
                //this.list_view_server.Items.Add(new ListViewItem(server.Name));
            }
        }

        private void SetDisplayUserAndPassword(string serverName) {
            var mongoRepo = GetMongoRepo();

           // this.textbox_user.Text = mongoRepo.GetServerAttribute(serverName, MongoXMLRepository.ServerAttribute.user);
            //this.tex_box_password.Text = mongoRepo.GetServerAttribute(serverName, MongoXMLRepository.ServerAttribute.password);
            
        }

        private void SaveUserAndPasswordToXMLCache(string serverName) {
            var mongoRepo = GetMongoRepo();

            //mongoRepo.SetServerAttribute(serverName, MongoXMLRepository.ServerAttribute.user, this.textbox_user.Text);
            //mongoRepo.SetServerAttribute(serverName, MongoXMLRepository.ServerAttribute.password, this.tex_box_password.Text);
        }
    }
}
