using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI {
    public partial class FormOptions : Form {
        public FormOptions() {
            InitializeComponent();
            init();
        }

        public bool init() 
        {
            SetControls();
            return true;
        }

        public void finalize() {
            Program.MongoXMLManager.SaveXml();
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
            this.textBoxQueryFolder.Text = Program.MongoXMLManager.QueryFolderPath;
            this.TextBoxTempFolder.Text = Program.MongoXMLManager.TempFolderPath;
            this.checkBoxDeleteTempFolderContents.Checked = Program.MongoXMLManager.DeleteTempFolderContents;
        }

        private void SaveControls() {
            Program.MongoXMLManager.QueryFolderPath = this.textBoxQueryFolder.Text;
            Program.MongoXMLManager.TempFolderPath = this.TextBoxTempFolder.Text;
            Program.MongoXMLManager.DeleteTempFolderContents = this.checkBoxDeleteTempFolderContents.Checked;
            
        }
    }
}
