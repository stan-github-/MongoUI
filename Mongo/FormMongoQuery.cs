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
        private QueryExecuter _queryExecuter;

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
                    Program.MongoXMLManager.TempFolderPath
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
                    this.QueryFilePath = Program.MongoXMLManager.LastFilePath;
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
            _queryExecuter = new QueryExecuter();

            return true;
        }

        private void text_box_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.OemPeriod)
            {
                return;
            }

            //this.text_box.AutoComplete.List = new List<String>() { "asf", "badf", "sdfc" };
            //this.text_box.AutoComplete.Show(0, new List<String>() { "asf", "badf", "sdfc" });

            MethodFinder.Run(this.text_box);
        }
        
        #region "autocomplete"

        //public class Method
        //{
        //    public String Name { get; set; }
        //    public List<String> ChildMethods { get; set; }
        //}

        public class MongoMethods {
            //            function test(){
            //    var x = db.c1;
            //    var a = [];
            //    for ( p in db.c1){
            //        a.push(p);
            //    }
            //    var z = _.sortBy(a, function(n){return n;});
	
            //    printz(z);
            //}
            public readonly static List<String> CollectionObjectMethods =
                new List<String> { 
                    //"_db",
                    //"_dbCommand",
                    //"_distinct",
                    //"_fullName",
                    //"_genIndexName",
                    //"_indexSpec",
                    //"_massageObject",
                    //"_mongo",
                    //"_printExtraInfo",
                    //"_shortName",
                    //"_validateForStorage",
                    //"_validateObject",
                    "aggregate",
                    "clean",
                    "convertToCapped",
                    "convertToSingleObject",
                    "copyTo",
                    "count",
                    "createIndex",
                    "dataSize",
                    "diskStorageStats",
                    "distinct",
                    "drop",
                    "dropIndex",
                    "dropIndexes",
                    "ensureIndex",
                    "exists",
                    "find",
                    "findAndModify",
                    "findOne",
                    "getCollection",
                    "getDB",
                    "getDiskStorageStats",
                    "getFullName",
                    "getIndexKeys",
                    "getIndexSpecs",
                    "getIndexStats",
                    "getIndexes",
                    "getIndices",
                    "getMongo",
                    "getName",
                    "getPagesInRAM",
                    "getQueryOptions",
                    "getShardDistribution",
                    "getShardVersion",
                    "getSlaveOk",
                    "getSplitKeysForChunks",
                    "group",
                    "groupcmd",
                    "help",
                    "indexStats",
                    "insert",
                    "isCapped",
                    "mapReduce",
                    "pagesInRAM",
                    "reIndex",
                    "remove",
                    "renameCollection",
                    "runCommand",
                    "save",
                    "setSlaveOk",
                    "shellPrint",
                    "stats",
                    "storageSize",
                    "toString",
                    "tojson",
                    "totalIndexSize",
                    "totalSize",
                    "update",
                    "validate",
                    "verify"
                };
        }
        
        //dot after a name, dot after a close parenthesis
        //build collection of method name, mongo and underscore
        //build collection of db collections

        public class MethodFinder {

            public static void Run(ScintillaNET.Scintilla text_box, bool debug = false) {

                if (IsQueryEndingInClosingParenthesis(text_box.TextBeforeCursor()) == true)
                {
                    var properties = QueryAutoCompleter
                    .GetMethodArray(text_box.TextBeforeCursor(), text_box.TextAfterCursor());

                    SetList(text_box, properties);
                }

                if (IsQueryEndingInCollectionName(text_box.TextBeforeCursor()) == true) {
                    SetList(text_box, MongoMethods.CollectionObjectMethods);
                }

                if (IsQueryEndingInDB(text_box.TextBeforeCursor()) == true) {
                    var currentServer = Program.MongoXMLManager.CurrentServer;
                    List<String> collections = null;
                    try
                    {
                        collections = Program.MongoXMLManager.Servers
                            .First(z => z.Name == currentServer.Name)
                            .Databases.First(z => z.Name == currentServer.CurrentDatabase.Name)
                            .Collections;
                    }
                    catch (Exception ex) {
                        ErrorManager.Write(ex);
                    }
                    SetList(text_box, collections);
                }
            }

            private static void SetList(ScintillaNET.Scintilla text_box, List<String> methods)
            {
                text_box.AutoComplete.MaxHeight = 10;
                text_box.AutoComplete.Show(0, methods);
            }


            private static bool IsQueryEndingInDB(String s)
            {
                //catched "db .   temp", "db.temp", "db. temp", "db .temp"
                //preceded by " ", "=", "(", or "{" or ";"
                //or at the begining of line

                string regex = @"(^|(\s)+|\=|\(|\{|\;)(db)(\s)*$";
                var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var reg = new Regex(regex, options);
                try
                {
                    var c = reg.Matches(s);
                    return c.Count > 0;
                }
                catch (Exception ex)
                {
                    ErrorManager.Write(ex);
                }
                return false;
            }

            private static bool IsQueryEndingInCollectionName(String s)
            {
                //catched "db .   temp", "db.temp", "db. temp", "db .temp"
                //preceded by " ", "=", "(", or "{" or ";"
                //or at the begining of line

                //use [] instead of | to group characters.
                string regex = @"(^|(\s)+|\=|\(|\{|\;)(db)(\s)*\.(\s)*([a-zA-Z0-9]*)$";
                var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var reg = new Regex(regex, options);
                try
                {
                    var c = reg.Matches(s);
                    return c.Count > 0;
                }
                catch (Exception ex)
                {
                    ErrorManager.Write(ex);
                }
                return false;
            }

            //#region "functions for handling general methods: find, etc"
            private static bool IsQueryEndingInClosingParenthesis(String s)
            {
                //var s = text_box.Text.Substring(0, text_box.CurrentPos - 1);
                //catch "db .   temp", "db.temp", "db. temp", "db .temp"
                string regex = @"\)([\s, \t, \r, \n]*)$";
                var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var reg = new Regex(regex, options);
                try
                {
                    var c = reg.Matches(s);
                    return c.Count > 0;
                }
                catch (Exception ex)
                {
                    ErrorManager.Write(ex);
                }
                return false;
            }
        

        }

        #endregion

        #region "control init"
        private void SetQueryOutputDisplayType()
        {
            foreach (var t in Program.MongoXMLManager.QueryOutputTypes.Types)
            {
                this.OutputTypeComboBox.Items.Add(t);
            }

            this.OutputTypeComboBox.Text = Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType;
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
            Program.MongoXMLManager.QueryOutputTypes =
                new MongoXMLRepository.QueryOutputType() { CurrentOutputType = OutputTypeComboBox.Text };
        }

        #endregion

        #region "file/query handlers"
        private void ExecuteQueryAndSaveToFile()
        {
            var query = String.IsNullOrEmpty(text_box.Selection.Text)
                                ? this.text_box.Text
                                : text_box.Selection.Text;

            DispalyQueryOutput(_queryExecuter.Execute(query));

            //save file
            FileManager.SaveToFile(this.QueryFilePath, text_box.Text);
        }

        private void DispalyQueryOutput(String content)
        {
            if (!Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer1.Panel2.Visible = false;
                 _queryExecuter.DisplayQueryInExe(content, Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType);
            }
            else if (Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType == "MongoUI")
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
