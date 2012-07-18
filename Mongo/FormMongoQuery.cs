using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBUI;
using System.Diagnostics;

namespace DBUI.Mongo {
    public partial class FormMongoQuery : Form {
        public enum Mode { 
            New,Existing,Last
        }
                
        public string QueryFilePath
        {
            get;
            set;
        }

        public Mode mode { get; set; }

        public FormMongoQuery(FormMainMDI parent) {
            InitializeComponent();
            this.MdiParent = parent;
        }

        private void this_handle_keydown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                this.execute_query();
            }
        }

        public bool refresh() {
            return true;
        }

        private void ensureQueryFilePathExists()
        {
            if (!File.Exists(QueryFilePath))
            {
                QueryFilePath = Program.MongoXMLManager.TempFolderPath 
                    + "\\" + Guid.NewGuid() + ".js";
                FileManager.SaveToFile(QueryFilePath, "//new query");
            }
        }

        public bool init() {
            this.text_box.KeyDown += new System.Windows.Forms.KeyEventHandler
                (this.this_handle_keydown);
            
            if (this.mode == Mode.New) {
                ensureQueryFilePathExists();
            }else if (this.mode == Mode.Existing){
                this.QueryFilePath = this.ask_for_file_path();
                ensureQueryFilePathExists();
            }else if (this.mode == Mode.Last){
                this.QueryFilePath = Program.MongoXMLManager.LastFilePath;
                ensureQueryFilePathExists();
            }
            
            //form tile
            this.Text = this.QueryFilePath;
            this.text_box.Text = DBUI.FileManager.ReadFromFile(this.QueryFilePath);
            
            //resize window
            this.WindowState = FormWindowState.Maximized;
            this.Show();
            
            //this.scintilla_box.
            return true; 
        }

        private string ask_for_file_path() {
            this.open_file_dialog.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.open_file_dialog.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";
            //minimize window, can't hide
            this.WindowState = FormWindowState.Minimized;
            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK) {
                return "";
            }
            return this.open_file_dialog.FileName;
        }

        private void execute_query() {
            //update status
            string query = ""; // this.text_box.SelectedText;
            if (query == "") {
                query = this.text_box.Text;
            } 
            this.executeConsoleApp(query);
            this.text_box_query_status.Text = DateTime.Now.ToString() ;
            FileManager.SaveToFile(this.QueryFilePath, this.text_box.Text);
        }

        private void executeConsoleApp(String javascript) {
            //mongo.exe must be in path variable, 
            //mongod must be started as service or console app
            
            Process compiler = new Process();
            compiler.StartInfo.FileName = "mongo.exe ";
            
            //sets server from combo box
            //sets database from combo box
            //prepends custom javascript from files

            String arguments = String.Format(
                "{0} --host {1} --eval \"{2}\" ",
                ((FormMainMDI) this.ParentForm).DatabaeName,
                ((FormMainMDI) this.ParentForm).ServerName,
                PrependCustomJSCode(javascript));
            
            compiler.StartInfo.Arguments = arguments;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            //ErrorManager.write(compiler.StandardOutput.ReadToEnd());
            this.executeNotePad(compiler.StandardOutput.ReadToEnd());
            compiler.WaitForExit();
        }

        private String PrependCustomJSCode(String script)
        {
            var b =  new StringBuilder();
            foreach (var path in Program.MongoXMLManager.CustomJSFilePaths)
            {
                b.Append(FileManager.ReadFromFile(path)).Append("\n");
            }

            return b.Append(script).ToString();
        }

        private void executeNotePad(String content) {
            string tempPath = Program.MongoXMLManager.TempFolderPath + "\\"
                + Guid.NewGuid() + ".json";
            if (FileManager.SaveToFile(tempPath, content) 
                == false) { return; }

            Process compiler = new Process();
            compiler.StartInfo.FileName = "notepad.exe";
            compiler.StartInfo.Arguments = tempPath;
            compiler.Start();
        }

        private void button_excecute_Click(object sender, EventArgs e) {
            this.execute_query();
        }

        private void button_execute_highlighted_section_Click(object sender, EventArgs e) {
            this.execute_query();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Program.MongoXMLManager.SaveXml();
        }

    }
}
