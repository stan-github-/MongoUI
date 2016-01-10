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
using DBUI.Queries;
using DBUI.DataModel;

//using ScintillaNET.Configuration;
//delete output files
//handle F5 at the topmost level and pass down to the child form.

namespace DBUI {
    public partial class FormMainMDI : Form {
        
        private FormOptions form_options;

        public String ServerName
        {
            get { return serverComboBox.Text; }
        }

        public String DatabaeName
        {
            get { return databaseComboBox.Text; }
        }

        public FormMainMDI() {
            InitializeComponent();
            Init();
        }

        private CollectionAutoCompleter _autoCompleter = new CollectionAutoCompleter();

        private bool Init() {
            //if (Program.MongoXMLManager.Init() == false) { return false; }

            this.SetJsEngineComboBox();

            this.SetDropDownFileHistory();
            this.SetDropDownCodeSnippet();
            this.SetMongoCollectionsOnDataImport();
            this.OpenLastOpendedFiles();

            this.Closed += new EventHandler(FormMainMDI_Closed);
            this.historyMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(OpenExistingFile);
            this.snippetsMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(OpenExistingFile);
            
            //new file button
            this.newToolStripButton.Click += OpenNewQueryFile;
            this.newToolStripMenuItem.Click += OpenNewQueryFile;

            //save file button functions
            this.saveToolStripButton.Click += SaveQueryFile;
            this.saveToolStripMenuItem.Click += SaveQueryFile;

            this.saveToolStripButton.Visible = true;
            this.saveFileDialog1.FileOk += SaveQueryWithFileDialog;

            _autoCompleter.RefreshCurrentDBCollectionNames();
        
            //future features
            this.historyMenu.Enabled = false;
            this.historyMenu.Visible = false;

            return true;
        }

        #region "query file open/save"

        private void SaveQueryFile(object sender, EventArgs eventArgs)
        {
            var activeChild = (FormQuery)this.ActiveMdiChild;
            if (activeChild != null)
            {
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(activeChild.Text);
            }
            
            this.saveFileDialog1.ShowDialog();
        }

        private void SaveQueryWithFileDialog(object sender, CancelEventArgs cancelEventArgs)
        {
            var activeChild = (FormQuery)this.ActiveMdiChild;
            if (activeChild == null)
            {
                return;
            }
            //saveFileDialog1.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";
            FileManager.SaveToFile(saveFileDialog1.FileName, activeChild.Title);
            new FormQuery(this).Init(FormQuery.Mode.Existing, saveFileDialog1.FileName);
        }

        private void OpenExistingFile(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (FormQuery c in this.MdiChildren)
            {
                if (c.QueryFilePath == e.ClickedItem.Text)
                {
                    return;
                }
            }
            new FormQuery(this).Init(FormQuery.Mode.Existing, e.ClickedItem.Text);
        }

        private void OpenLastOpendedFiles()
        {
            foreach (var path in Program.JsEngine.Repository.LastOpenedFilePaths)
            {
                new Queries.FormQuery(this).Init(FormQuery.Mode.Existing, path);
            }
        }

        private void OpenFileWithFileDialog(object sender, EventArgs e)
        {
            var mongoChildForm = new FormQuery(this).Init(FormQuery.Mode.FileDialog);
        }

        private void OpenNewQueryFile(object sender, EventArgs e)
        {
            new FormQuery(this).Init(FormQuery.Mode.New);
        }

        #endregion

        #region "on exit"
        void FormMainMDI_Closed(object sender, EventArgs e)
        {
            var l = new List<String>();
            MdiChildren.ToList().ForEach(c=> l.Add(((FormQuery)c).QueryFilePath));
            Program.JsEngine.Repository.FileHistory = l;

            Program.JsEngine.Repository.SaveXml();
            Program.MainXMLManager.SaveXml();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region "server database combo boxes"

        private void SetServerComboBox()
        {
            if (Program.JsEngine.CurrentType != JsEngineType.MongoDB) {
                return;
            }

            Program.JsEngine.MongoXMLRepository.Servers.ForEach
                    (x =>{
                        if (!this.serverComboBox.Items.Contains(x.Name)){
                            this.serverComboBox.Items.Add(x.Name);
                        }
                    });

            this.serverComboBox.Text = Program.JsEngine.MongoXMLRepository.CurrentServer.Name;
            SetDatabaseComboBox();
        }

        private void SetDatabaseComboBox()
        {
            var server = 
                Program.JsEngine.MongoXMLRepository.Servers.Where
                (x => x.Name == serverComboBox.Text).FirstOrDefault();
                        
            if (server == null)
            {
                MessageBox.Show("Server not configured correctly; check configure file.");
                return;
            }

            try
            {
                //databaseComboBox.Items.Clear();
                server.Databases.ForEach
                    (x => {
                        if (!databaseComboBox.Items.Contains(x.Name)){
                            this.databaseComboBox.Items.Add(x.Name);
                        }
                    });
            }catch{
                MessageBox.Show("Server not configured correctly; check configure file.");
                return;
            }

            this.databaseComboBox.Text = Program.JsEngine.MongoXMLRepository.CurrentServer.CurrentDatabase.Name;
            //_autoCompleter.RefreshCurrentDBCollectionNames();
        }

        #region "saving server and databae setting"
        private void serverComboBox_Select(object sender, EventArgs e)
        {
            SetDatabaseComboBox();
            SaveCurrentServerAndDatabase();
            //_autoCompleter.RefreshCurrentDBCollectionNames();
        
        }

        private void databaseComboBox_Select(object sender, EventArgs e)
        {
            SaveCurrentServerAndDatabase();
            _autoCompleter.RefreshCurrentDBCollectionNames();
        }

        private void SaveCurrentServerAndDatabase()
        {
            if(String.IsNullOrEmpty(serverComboBox.Text) || 
                String.IsNullOrEmpty(databaseComboBox.Text))
            {
                return;
            }

            Program.JsEngine.MongoXMLRepository.CurrentServer =
                new Server
                {
                    Name = serverComboBox.Text,
                    CurrentDatabase = new Database { Name = databaseComboBox.Text }
                };
        }
        #endregion

        #endregion

        #region "drop down snippet code files etc"
        private void SetDropDownFileHistory()
        {
            Program.JsEngine.Repository.FileHistory.ForEach
                (f=> this.historyMenu.DropDownItems.Add(f)
            );
        }

        private void SetDropDownCodeSnippet()
        {
            Program.JsEngine.Repository.CodeSnippets.ForEach
                (f => this.snippetsMenu.DropDownItems.Add(f)
            );
        }

        #endregion

        #region "new features"
        private void SetMongoCollectionsOnDataImport()
        {
            if (Program.JsEngine.CurrentType != JsEngineType.MongoDB) {
                return;
            }
            
            return; //code not working
            var currentDB = Program.JsEngine.MongoXMLRepository.CurrentServer.Databases.FirstOrDefault(
                d => Name == Program.JsEngine.MongoXMLRepository.CurrentServer.CurrentDatabase.Name);
            if (currentDB == null)
            {
                return;
            }

            foreach (var collection in currentDB.Collections)
            {
                importSubMenu.DropDownItems.Add(collection);
                exportSubMenu.DropDownItems.Add(collection);
            }
            
        }
        #endregion

        #region "miscellaneous UI"
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Configure Mongo.xml\n");
            b.Append("Configure ScintillaNET.xml\n");

            MessageBox.Show(b.ToString());
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            status_strip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not Implemented");
            //return;
            if (this.form_options == null)
            {
                this.form_options = new FormOptions();
            }
            if (this.form_options.IsDisposed == true)
            {
                this.form_options = new FormOptions();
            }
            this.form_options.Show();

        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        #endregion

        #region "not used"
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ScintillaInit()
        {
            //var c = new Configuration("js");

        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void loadSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormExcelQuery childForm = new FormExcelQuery(this);
            //childFormNumber++;
            //childForm.init(Environment.CurrentDirectory + "\\sample.xls");
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutMongoUI MongoUI = new AboutMongoUI();
            //MongoUI.Show();
        }

        #endregion

        private void SetJsEngineComboBox()
        {
            Program.MainXMLManager.Engines.ForEach
                    (x =>
                    {
                        if (!this.jsEngineComboBox.Items.Contains(x.Type.ToString()))
                        {
                            this.jsEngineComboBox.Items.Add(x.Type.ToString());
                        }
                    });

            this.jsEngineComboBox.Text = Program.MainXMLManager.Engines
                .First(e => e.IsCurrent == true).Type.ToString();

            if (Program.JsEngine.CurrentType == JsEngineType.MongoDB) {
                this.SetServerComboBox();
            }

            this.jsEngineComboBox.SelectedIndexChanged += jsEngineComboBox_SelectedIndexChanged;

            this.updateMongoServerAndDatabaseUI();
        }

        private void updateMongoServerAndDatabaseUI()
        {
            var isMongo = Program.MainXMLManager.CurrentEngine == JsEngineType.MongoDB;
            this.serverComboBox.Visible = isMongo;
            this.databaseComboBox.Visible = isMongo;
        }


        void jsEngineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveJsEngineType();
            Program.JsEngine.InitRepository();
            SetJsEngineComboBox();
        }

        
        private void SaveJsEngineType()
        {
            if (String.IsNullOrEmpty(this.jsEngineComboBox.Text))
            {
                return;
            }

            var node = Program.MainXMLManager.Engines.First(e => e.Type == (JsEngineType)
                Enum.Parse(typeof(JsEngineType), this.jsEngineComboBox.Text));
            
            if (node == null){
                return;
            }

            Program.MainXMLManager.CurrentEngine = 
                (JsEngineType)Enum.Parse(typeof(JsEngineType), this.jsEngineComboBox.Text);
            
        }
        
        
        

        
    }
}
