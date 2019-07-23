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

        
        private void button_okay_Click(object sender, EventArgs e) {
            SaveControls();
            this.Close();
        }

        private void SetControls() {
            this.TextBoxQueryFolder.Text = Program.Config.Data.Miscellaneous.QueryFolder;
            this.TextBoxMongoExePath.Text = Program.Config.Data.MongoClientExePath;
            this.button_okay.Click +=button_okay_Click;
        }

        private void SaveControls() {
            Program.Config.Data.Miscellaneous.QueryFolder = this.TextBoxQueryFolder.Text;
            Program.Config.Data.MongoClientExePath = this.TextBoxMongoExePath.Text;
        }
    }
}
