﻿using System;
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
using ScintillaNET;

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
            get { return this.QueryBox.Text; }
        }

        public String QueryFilePath { get; set; }

        public String TempJSFile
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(
                    Program.JsEngine.Repository.TempFolderPath
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
            this.QueryBox.Dock = DockStyle.Fill;
            this.QueryOuput.Dock = DockStyle.Fill;

            FormatStyles(QueryBox);
            FormatStyles(QueryOuput);
            
        }

        public void FormatStyles(ScintillaNET.Scintilla scintilla)
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            
            scintilla.StyleResetDefault();
            
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.Styles[Style.Default].BackColor = Color.Black;
            scintilla.Styles[Style.Default].ForeColor = Color.Maroon;
            
            scintilla.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            //scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.DarkGreen; // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.DarkGreen;
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.DarkGreen;

            scintilla.Styles[Style.Cpp.Comment].BackColor = Color.Black; // Green
            scintilla.Styles[Style.Cpp.CommentLine].BackColor = Color.Black; // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].BackColor = Color.Black; // Green
            
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            //scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            //scintilla.Styles[Style.Cpp.Default].BackColor = Color.Black;
            
        }

        /*
         <Language Name="default">
    <Styles>
      <Style Name="CHARACTER" ForeColor="Orange" BackColor="Black"/>
    </Styles>
  </Language>
  <Language Name="js">
    <Indentation TabWidth="4" UseTabs="true"/>
    <Lexer>
      <Keywords List="0">print sleep</Keywords>
    </Lexer>
    <Styles>
      <Style Name="COMMENT" ForeColor="DarkGreen" BackColor="Black"/>
      <Style Name="COMMENTLINE" ForeColor="DarkGreen" BackColor="Black"/>
      <Style Name="COMMENTDOC" ForeColor="DarkGreen" BackColor="Black"/>
      <Style Name="NUMBER" ForeColor="Red" BackColor="Black"/>
      <Style Name="WORD" ForeColor="White" BackColor="Black"/>
      <Style Name="STRING" ForeColor="LightGreen" BackColor="Black"/>
      <Style Name="CHARACTER" ForeColor="LightGreen" BackColor="Black"/>
      <Style Name="OPERATOR" ForeColor="Cyan" BackColor="Black"/>
      <Style Name="IDENTIFIER" ForeColor="Yellow" BackColor="Black"/>
      <Style Name="GLOBALCLASS" ForeColor="Magenta" BackColor="Black"/>
      <style Name="STANDARD" ForeColor="Orange" BackColor="Black" />
      <style Name="DEFAULT" ForeColor="Orange" BackColor="Black"  />
      <style Name="BRACELIGHT" ForeColor="Gray" BackColor="Black" />
      <style Name="BRACEBAD" ForeColor="Gray" BackColor="Black" />
    </Styles>
  </Language> 
         */

        public bool Init(Mode mode, String filePath = null)
        {
            this.QueryBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);
            this.QueryBox.KeyUp += QueryBox_KeyUp;
            QueryBox.Margins[0].Width = 20;
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
                    this.QueryFilePath = Program.JsEngine.Repository.LastFilePath;
                    EnsureQueryFilePathExists();
                    break;
                case Mode.FileDialog:
                    this.QueryFilePath = this.OpenOpenFileDialog();
                    EnsureQueryFilePathExists();
                    break;
            }


            //form tile
            this.Text = this.QueryFilePath;
            this.QueryBox.Text = FileManager.ReadFromFile(this.QueryFilePath);

            //resize window
            this.WindowState = FormWindowState.Maximized;
            this.Show();

            //set output type
            SetQueryOutputDisplayType();

            //instantiate child class
            _queryExecuter = new JavaScriptExecuter();

            return true;
        }

        private void QueryBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Program.ProgramMode != Program.Mode.Mongo)
            //{
            //    return;
            //}

            if (e.KeyCode != Keys.OemPeriod)
            {
                return;
            }

            Queries.AutoComplete.AutoCompleteUI.RunMongo(this.QueryBox);
        }

        #region "control init"
        private void SetQueryOutputDisplayType()
        {
            foreach (var t in Program.JsEngine.Repository.QueryOutputTypes.Types)
            {
                this.OutputTypeComboBox.Items.Add(t);
            }

            this.OutputTypeComboBox.Text = Program.JsEngine.Repository.QueryOutputTypes.CurrentOutputType;
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
            Program.JsEngine.Repository.SaveXml();
        }

        private void QueryOutputType_Selected(object sender, EventArgs e)
        {
            Program.JsEngine.Repository.QueryOutputTypes =
                new DBUI.Queries.JsEngineXMLRepository.QueryOutputType() 
                { CurrentOutputType = OutputTypeComboBox.Text };
        }

        #endregion

        #region "file/query handlers"
        private void ExecuteQueryAndSaveToFile()
        {
            var query = String.IsNullOrEmpty(QueryBox.SelectedText)
                                ? this.QueryBox.Text
                                : QueryBox.SelectedText;

            //DispalyQueryOutput(_queryExecuter.ExecuteMongo(query));
            DispalyQueryOutput(_queryExecuter.Execute(query));
            
            var javascriptError = _queryExecuter.
                    MessageManager.GetJavascriptQueryError();
            if (!String.IsNullOrEmpty(javascriptError))
            {
                ErrorManager.Write(javascriptError);
            }
            
            FileManager.SaveToFile(this.QueryFilePath, QueryBox.Text);
        }

        private void DispalyQueryOutput(String content)
        {
            if (!Program.JsEngine.Repository.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer1.Panel2.Visible = false;
                 _queryExecuter.DisplayQueryInExe(content, Program.JsEngine.Repository
                     .QueryOutputTypes.CurrentOutputType);
            }
            else if (Program.JsEngine.Repository.QueryOutputTypes.CurrentOutputType == "MongoUI")
            {
                this.splitContainer1.Panel2Collapsed = false;
                this.splitContainer1.Panel2.Show();
                this.QueryOuput.Text = content;
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

        private void QueryBox_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Query (Mongo, SQL) Execution Handler
        /// </summary>
        
        //todo
        //mongo/sql/dos cmds should be executed in sequence, 
        //additionally should just split the queryies with special tokens
        
    }
}
