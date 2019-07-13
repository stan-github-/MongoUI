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
using ScintillaNET;

namespace DBUI.Queries {
    public partial class FormQuery : Form
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
                    Program.Config.Data.Miscellaneous.TempFolder
                    + "\\" + Guid.NewGuid() + ".js");
            }
        }

        public Mode mode { get; set; }

        public FormQuery(FormMainMDI parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel2.Hide();

            this.QueryBox.Dock = DockStyle.Fill;
            this.QueryOuput.Dock = DockStyle.Fill;

            FormatStyles(QueryBox);
            FormatStyles(QueryOuput);

            //brace highlighting
            QueryBox.UpdateUI += QueryBox_UpdateUI;
            
        }

        
        public void FormatStyles(ScintillaNET.Scintilla scintilla)
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.

            // For an explanation of this code visit:
            // https://github.com/jacobslusser/ScintillaNET/wiki/Automatic-Syntax-Highlighting

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.Styles[Style.Default].ForeColor = Color.Wheat;
            scintilla.Styles[Style.Default].BackColor = Color.Black;
            scintilla.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Orange;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.GhostWhite; // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.GhostWhite; // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.GhostWhite; // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.BlueViolet;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Orange;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.LimeGreen; // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.LimeGreen; // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.LimeGreen; // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            scintilla.Lexer = Lexer.Cpp;

            // Set the keywords
            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "var bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");

            scintilla.CaretForeColor = Color.Transparent;

            scintilla.Styles[Style.BraceLight].BackColor = Color.Black;
            scintilla.Styles[Style.BraceLight].ForeColor = Color.White;
            scintilla.Styles[Style.BraceBad].ForeColor = Color.Magenta;
           
        }

        #region brace highlight //could be refactored, copied directly from scintilla!!
        private static bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }

        int lastCaretPos = 0;

        void QueryBox_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            var caretPos = QueryBox.CurrentPosition;
            if (lastCaretPos != caretPos)
            {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(QueryBox.GetCharAt(caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace(QueryBox.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = QueryBox.BraceMatch(bracePos1);
                    if (bracePos2 == ScintillaNET.Scintilla.InvalidPosition)
                    {
                        QueryBox.BraceBadLight(bracePos1);
                        QueryBox.HighlightGuide = 0;
                    }
                    else
                    {
                        QueryBox.BraceHighlight(bracePos1, bracePos2);
                        QueryBox.HighlightGuide = QueryBox.GetColumn(bracePos1);
                    }
                }
                else
                {
                    // Turn off brace matching
                    QueryBox.BraceHighlight(ScintillaNET.Scintilla.InvalidPosition, ScintillaNET.Scintilla.InvalidPosition);
                    QueryBox.HighlightGuide = 0;
                }
            }
        }
        #endregion

        private void scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // Has the caret changed position?
            
        }

        public bool Init(Mode mode, String filePath = null)
        {
            this.QueryBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);
            this.QueryBox.KeyUp += QueryBox_KeyUp;
            QueryBox.Margins[0].Width = 20;
            //mode whether new or existing
            switch (mode)
            {
                case Mode.New:
                    break;
                case Mode.Existing:
                    this.QueryFilePath = filePath;
                    break;
                case Mode.Last:
                    this.QueryFilePath = Program.Config.LastOpenedFile();
                    break;
                case Mode.FileDialog:
                    this.QueryFilePath = this.OpenOpenFileDialog();
                    break;
            }
            EnsureQueryFilePathExists();

            //form tile
            this.Text = this.QueryFilePath;
            this.QueryBox.Text = FileManager.ReadFromFile(this.QueryFilePath);

            //resize window
            this.WindowState = FormWindowState.Maximized;
            this.Show();

            //set output type
            SetQueryOutputDisplayType();

            //instantiate child class
            _queryExecuter = new JavaScriptExecuter()
            {
                MessageManager = new MessageManager(this.QueryFilePath),
                QueryFileManager = new QueryFileManager() { 
                    QueryFilePath = this.QueryFilePath
                }
            };

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
            foreach (var t in Program.Config.Data.QueryOutputTypes)
            {
                this.OutputTypeComboBox.Items.Add(t.Name);
            }

            this.OutputTypeComboBox.Text = Program.Config.Data.QueryOutputTypes.First(o=>o.IsCurrent).Name;
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
            var l = Program.Config.Data.Miscellaneous.LastOpenedFilePaths;
            l.Add(this.QueryFilePath);
            
            if (l.Count > 10) {
                var f = l.First();
                l.Remove(f);
            }

            ((FormMainMDI)this.MdiParent).SetDropDownFileHistory();
        }

        private void QueryOutputType_Selected(object sender, EventArgs e)
        {
            Program.Config.Data.QueryOutputTypes.ForEach(q => q.IsCurrent = false);
            Program.Config.Data.QueryOutputTypes.Find
                (f => f.Name == OutputTypeComboBox.Text).IsCurrent = true;
        }

        #endregion

        #region "file/query handlers"
        private void ExecuteQueryAndSaveToFile()
        {
            var query = String.IsNullOrEmpty(QueryBox.SelectedText)
                                ? this.QueryBox.Text
                                : QueryBox.SelectedText;

            var output = _queryExecuter.Execute(query);
            DispalyQueryOutput(output);
            
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
            var outputType = Program.Config.CurrentQueryOutputType().Name;
            if (!outputType.Contains("MongoUI"))
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer1.Panel2.Visible = false;
                 _queryExecuter.DisplayQueryInExe(content, outputType);
            }
            else if (outputType == "MongoUI")
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

     

        /// <summary>
        /// Query (Mongo, SQL) Execution Handler
        /// </summary>
        
        //todo
        //mongo/sql/dos cmds should be executed in sequence, 
        //additionally should just split the queryies with special tokens
        
    }
}
