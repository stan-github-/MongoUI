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
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel2.Hide();
        }

        private void this_handle_keydown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                this.ExecuteQuery();
            }
        }

        public bool refresh() {
            return true;
        }

        private void ensureQueryFilePathExists()
        {
            if (!File.Exists(QueryFilePath))
            {
                QueryFilePath = Environment.ExpandEnvironmentVariables(Program.MongoXMLManager.TempFolderPath 
                    + "\\" + Guid.NewGuid() + ".js");
                FileManager.SaveToFile(QueryFilePath, "//new query");
            }
        }

        public bool init() {
            this.text_box.KeyDown += new System.Windows.Forms.KeyEventHandler
                (this.this_handle_keydown);
            
            if (this.mode == Mode.New) {
                ensureQueryFilePathExists();
            }else if (this.mode == Mode.Existing){
                this.QueryFilePath = this.OpenFileDialog();
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
            
            SetQueryOutputDisplayType();

            return true; 
        }
        
        private void SetQueryOutputDisplayType()
        {
            foreach(var t in Program.MongoXMLManager.QueryOutputTypes.Types)
            {
                this.OutputTypeComboBox.Items.Add(t);    
            }

            this.OutputTypeComboBox.Text = Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType;
        }

        private string OpenFileDialog() {
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

        private void ExecuteQuery()
        {
            String query = string.IsNullOrEmpty(this.text_box.Selection.Text)? 
                this.text_box.Text :this.text_box.Selection.Text;
            
            if(String.IsNullOrEmpty(query))
            {
                return;
            }
            FileManager.SaveToFile(this.QueryFilePath, this.text_box.Text);
            ExecuteConsoleApp(query);
        }

        private void ExecuteConsoleApp(String javascript) {
            //mongo.exe must be in path variable, 
            //mongod must be started as service or console app
            
            Process process = new Process();
            process.StartInfo.FileName = "mongo.exe ";
            
            //sets server from combo box
            //sets database from combo box
            //prepends custom javascript from files

            String arguments = String.Format(
                "{0} --host {1} --eval \"{2}\" ",
                ((FormMainMDI) this.ParentForm).DatabaeName,
                ((FormMainMDI) this.ParentForm).ServerName,
                PrependCustomJSCode(javascript));
            
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            DispalyQueryOutput(process.StandardOutput.ReadToEnd());
            
            process.WaitForExit();
        }

        private void DispalyQueryOutput(String content)
        {
            if (!Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Visible = false;
                DisplayQueryInExe(content, Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType);
            }
            else if (Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType == "MongoUI")
            {
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();
                scintillaOutput.Text = content;
            }
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

        private void DisplayQueryInExe(String content, String exe) {
            string tempPath = Environment.ExpandEnvironmentVariables
                (Program.MongoXMLManager.TempFolderPath + "\\"
                + Guid.NewGuid() + ".json"); ;
            if (FileManager.SaveToFile(tempPath, content) 
                == false) { return; }

            Process compiler = new Process();
            compiler.StartInfo.FileName = exe;
            compiler.StartInfo.Arguments = tempPath;
            compiler.Start();
        }

        private void button_excecute_Click(object sender, EventArgs e) {
            this.ExecuteQuery();
        }

        private void button_execute_highlighted_section_Click(object sender, EventArgs e) {
            this.ExecuteQuery();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Program.MongoXMLManager.SaveXml();
        }

        private void QueryOutputType_Selected(object sender, EventArgs e)
        {
            Program.MongoXMLManager.QueryOutputTypes = 
                new MongoXMLManager.QueryOutputType()
                {CurrentOutputType = OutputTypeComboBox.Text};
        }

    }
}
