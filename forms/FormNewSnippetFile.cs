using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI.Forms
{
    public partial class FormNewSnippetFile : Form
    {
        public delegate void CallBack(String value, string groupName, string name);

        public CallBack callBack { get; set; }
        public FormNewSnippetFile()
        {
            InitializeComponent();

            this.button_okay.Click += button_okay_Click;
            this.button_file_path.Click += button_file_path_Click;
            
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
            else
            {
                throw new NotImplementedException();
            }


            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }

            return this.open_file_dialog.FileName;
        }

        private void button_file_path_Click(object sender, EventArgs e)
        {
            
            var mongoRepo = Program.JsEngine.GetMongoRepo();

            if (mongoRepo == null) {
                throw new NotImplementedException();
            }

            var filePath = OpenOpenFileDialog();
            this.text_box_file_path.Text = filePath;
            
        }

        void button_okay_Click(object sender, EventArgs e)
        {
            this.callBack(this.text_box_file_path.Text, this.combo_box_group_name.SelectedText, this.text_box_snippet_name.Text);

            try
            {
                this.Dispose();
                this.Close();
            }
            catch (Exception ex) {
                ErrorManager.Write(ex);
            }
            
        }

    }
}
