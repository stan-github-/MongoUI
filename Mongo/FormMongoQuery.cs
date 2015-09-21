using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DBUI;
using System.Diagnostics;

namespace DBUI.Queries {
    public partial class FormMongoQuery : Form
    {
        private JavaScriptExecuter _queryExecuter;

        public enum Mode
        {
            New,
            FileDialog,
            Existing,
            Last
        }

        public String Title {
            get { return this.text_box.Text; }
        }

        public String QueryFilePath { get; set; }

        public String TempJSFile
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(
                    Program.MainXMLManager.TempFolderPath
                    + "\\" + Guid.NewGuid() + ".js");
            }
        }

        public Mode mode { get; set; }

        public FormMongoQuery(FormMainMDI parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel2.Hide();
        }

        public bool Init(Mode mode, String filePath = null)
        {
            this.text_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);
            this.text_box.KeyUp += text_box_KeyUp;
            text_box.Margins[0].Width = 20;
            //mode whether new or existing
            switch (mode)
            {
                case Mode.New:
                    EnsureQueryFilePathExists();
                    break;
                case Mode.Existing:
                    this.QueryFilePath = filePath;
                    EnsureQueryFilePathExists();
                    break;
                case Mode.Last:
                    this.QueryFilePath = Program.MainXMLManager.LastFilePath;
                    EnsureQueryFilePathExists();
                    break;
                case Mode.FileDialog:
                    this.QueryFilePath = this.OpenOpenFileDialog();
                    EnsureQueryFilePathExists();
                    break;
            }


            //form tile
            this.Text = this.QueryFilePath;
            this.text_box.Text = FileManager.ReadFromFile(this.QueryFilePath);

            //resize window
            this.WindowState = FormWindowState.Maximized;
            this.Show();

            //set output type
            SetQueryOutputDisplayType();

            //instantiate child class
            _queryExecuter = new JavaScriptExecuter();

            return true;
        }

        private void text_box_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Program.ProgramMode != Program.Mode.Mongo)
            //{
            //    return;
            //}

            if (e.KeyCode != Keys.OemPeriod)
            {
                return;
            }

            Queries.AutoComplete.AutoCompleteMain.RunMongo(this.text_box);
        }

        #region "control init"
        private void SetQueryOutputDisplayType()
        {
            foreach (var t in Program.MainXMLManager.QueryOutputTypes.Types)
            {
                this.OutputTypeComboBox.Items.Add(t);
            }

            this.OutputTypeComboBox.Text = Program.MainXMLManager.QueryOutputTypes.CurrentOutputType;
        }

        #endregion

        #region "control event handlers"

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ExecuteQueryAndSaveToFile();
            }
        }

        private void button_excecute_Click(object sender, EventArgs e)
        {
            ExecuteQueryAndSaveToFile();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Program.MongoXMLManager.SaveXml();
        }

        private void QueryOutputType_Selected(object sender, EventArgs e)
        {
            Program.MainXMLManager.QueryOutputTypes =
                new MainXMLRepository.QueryOutputType() { CurrentOutputType = OutputTypeComboBox.Text };
        }

        #endregion

        #region "file/query handlers"
        private void ExecuteQueryAndSaveToFile()
        {
            var query = String.IsNullOrEmpty(text_box.Selection.Text)
                                ? this.text_box.Text
                                : text_box.Selection.Text;

            DispalyQueryOutput(_queryExecuter.ExecuteMongo(query));
            
            var javascriptError = _queryExecuter.QueryHelper.JavascriptQueryError;
            if (!String.IsNullOrEmpty(javascriptError))
            {
                ErrorManager.Write(javascriptError);
            }
            
            FileManager.SaveToFile(this.QueryFilePath, text_box.Text);
        }

        private void DispalyQueryOutput(String content)
        {
            if (!Program.MainXMLManager.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer1.Panel2.Visible = false;
                 _queryExecuter.DisplayQueryInExe(content, Program.MainXMLManager.QueryOutputTypes.CurrentOutputType);
            }
            else if (Program.MainXMLManager.QueryOutputTypes.CurrentOutputType == "MongoUI")
            {
                this.splitContainer1.Panel2Collapsed = false;
                this.splitContainer1.Panel2.Show();
                this.scintillaOutput.Text = content;
            }
        }

        private void EnsureQueryFilePathExists()
        {
            if (!File.Exists(QueryFilePath))
            {
                QueryFilePath = TempJSFile;
                FileManager.SaveToFile(QueryFilePath, "//new query");
            }
        }
        
        private string OpenOpenFileDialog()
        {
            this.open_file_dialog.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.open_file_dialog.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";
            
            //minimize window, can't hide
            if (this.MdiParent != null && this.MdiParent.ActiveMdiChild != null)
            {
                this.open_file_dialog.InitialDirectory = 
                    Path.GetDirectoryName(this.MdiParent.ActiveMdiChild.Text);
            }
            this.WindowState = FormWindowState.Minimized;
            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }
            return this.open_file_dialog.FileName;
        }
        #endregion

        /// <summary>
        /// Query (Mongo, SQL) Execution Handler
        /// </summary>
        
        //todo
        //mongo/sql/dos cmds should be executed in sequence, 
        //additionally should just split the queryies with special tokens
        
    }
}
