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

       
        private void SetControls() {
            SetDisplaySnippetFiles();

            //this.list_view_files.Items[0].Selected = true;
            SetEventHandlers();     
        }


        private void SetEventHandlers() {
            this.button_add_file.Click += new EventHandler(ButtonFileAdd_EventHandler);
            this.button_delete_file.Click += new System.EventHandler(this.buttonFileDelete_Click);
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
        }


        public void AddNewFile(string groupName, string name, String filePath) {
            var listItem = new ListViewItem(groupName);
            listItem.SubItems.Add(name);
            listItem.SubItems.Add(filePath);
            list_view_files.Items.Add(listItem);

            Program.JsEngine.MongoEngine.Repository.AddSnippetFile(groupName, name, filePath);

        }
        
        private void ButtonFileAdd_EventHandler(object sender, EventArgs e)
        {
            var form = new FormNewSnippetFile() { 
                callBack = AddNewFile
            };
            form.Show();
        }

        private void buttonFileDelete_Click(object sender, EventArgs e)
        {
            if (this.list_view_files.SelectedItems.Count != 1)
            {
                return;
            }

            var groupName = this.list_view_files.SelectedItems[0].SubItems[0].Text;
            var name = this.list_view_files.SelectedItems[0].SubItems[1].Text;

            this.list_view_files.Items.Remove(this.list_view_files.SelectedItems[0]);

            Program.JsEngine.MongoEngine.Repository.DeleteSnippetFile(groupName, name);
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

            foreach (var file in mongoRepo.CodeSnippets) {
                var item = new ListViewItem(file.GroupName);
                
                item.SubItems.Add(file.Name);
                item.SubItems.Add(file.FilePath);
                this.list_view_files.Items.Add(item);
            }
        }
       
    }
}
