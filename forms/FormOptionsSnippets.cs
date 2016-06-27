﻿using DBUI.Forms;
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
            //var server = list_view_files.SelectedItems[0].Text;            
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

        private void ConfigFileViewFiles() {

            //this.list_view_files.Columns.Add("GroupName");
            //this.list_view_files.Columns.Add("Name");
            //this.list_view_files.Columns.Add("File Path");

        }

        private void SetControls() {
            SetDisplaySnippetFiles();

            //this.list_view_files.Items[0].Selected = true;
            SetEventHandlers();
            
        }


        private void SetEventHandlers() {
            this.button_add_file.Click += new EventHandler(ButtonFileAdd_EventHandler);
            this.button_delete_file.Click += new System.EventHandler(this.buttonFileDelete_Click);
        }

        
        private void ButtonFileAdd_EventHandler(object sender, EventArgs e)
        {

            //var filePath = OpenOpenFileDialog();

            //var mongoRepo = GetMongoRepo();
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
            //mongoRepo.DeleteDatabase(serverName, this.list_view_files.SelectedItems[0].Text);
        }
        
        private void ButtonDatabaseAdd_EventHandler(object sender, EventArgs e){
            
            var form = new FormNewItem() {
                //callBack = ButtonFileAdd_SetNewItem
            };

            form.ShowDialog();
            
        }

        
        private MongoXMLRepository GetMongoRepo() {
            return Program.JsEngine.GetMongoRepo();
        }


        private void SetDisplaySnippetFiles() {
            var mongoRepo = GetMongoRepo();

            this.list_view_files.Columns.Add(new ColumnHeader() { 
                Text = "Group Name"
            });
            this.list_view_files.Columns.Add(new ColumnHeader() { 
                Text = "Name"
            });
            this.list_view_files.Columns.Add(new ColumnHeader() { 
                Text= "File Path"
            });

            foreach (var file in mongoRepo.GetSnippetFiles()) {
                var item = new ListViewItem(file.GroupName);
                
                item.SubItems.Add(file.Name);
                item.SubItems.Add(file.FilePath);
                this.list_view_files.Items.Add(item);
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
