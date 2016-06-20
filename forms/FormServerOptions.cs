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
            //this.textBoxQueryFolder.Text = Program.MainXMLManager.QueryFolderPath;
            //this.TextBoxTempFolder.Text = Program.MainXMLManager.TempFolderPath;
            //this.checkBoxDeleteTempFolderContents.Checked = Program.MainXMLManager.DeleteTempFolderContents;
        }

        private void SaveControls() {
            //Program.MainXMLManager.QueryFolderPath = this.textBoxQueryFolder.Text;
            //Program.MainXMLManager.TempFolderPath = this.TextBoxTempFolder.Text;
            //Program.MainXMLManager.DeleteTempFolderContents = this.checkBoxDeleteTempFolderContents.Checked;   
        }

        private void SetMongoServers() { 
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB){
                return;
            }

            var mongoEngine = Program.JsEngine.MongoEngine;

            var mongoRepo = (MongoXMLRepository)mongoEngine.Repository;

            foreach (var server in mongoRepo.Servers) {
                this.server_list_view.Items.Add(new ListViewItem(server.Name));
            }
        }
    }
}
