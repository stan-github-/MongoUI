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
        private FormServerOptions form_options_server;
        private FormOptionsSnippets form_options_snippet;

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
            
            this.setToolStripMenuItem();
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
            //var mongoChildForm = new FormQuery(this).Init(FormQuery.Mode.FileDialog);
            var filePath = this.OpenOpenFileDialog();
            var mongoChildForm = new FormQuery(this).Init(FormQuery.Mode.Existing, filePath);
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
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB) {
                return;
            }

            Program.JsEngine.MongoEngine.Repository.Servers.ForEach
                    (x =>{
                        if (!this.serverComboBox.Items.Contains(x.Alias)){
                            this.serverComboBox.Items.Add(x.Alias);
                        }
                    });

            this.serverComboBox.Text = Program.JsEngine.MongoEngine.Repository.CurrentServer.Alias;
            SetDatabaseComboBox();
        }

        private void SetDatabaseComboBox()
        {

            var server = 
                Program.JsEngine.MongoEngine.Repository.Servers.Where
                (x => x.Alias == serverComboBox.Text).FirstOrDefault();
                        
            if (server == null)
            {
                MessageBox.Show("Server not configured correctly; check configure file.");
                return;
            }

            try
            {
                databaseComboBox.Items.Clear();
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

            this.databaseComboBox.Text = Program.JsEngine.MongoEngine.Repository.CurrentServer.CurrentDatabase.Name;
            //_autoCompleter.RefreshCurrentDBCollectionNames();
        }

        #region "saving server and databae setting"
        private void serverComboBox_Select(object sender, EventArgs e)
        {
            SetDatabaseComboBox();
            SaveCurrentServerAndDatabase();        
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

            Program.JsEngine.MongoEngine.Repository.CurrentServer =
                new Server
                {
                    Alias = serverComboBox.Text,
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
                (f =>{
                    string s = String.Format("{0};{1}", f.Name, f.FilePath);
                    this.snippetsMenu.DropDownItems.Add(s);
                });
        }

        #endregion

        #region tools/options tools/servers
        private void setToolStripMenuItem(){
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            this.serversToolStripMenuItem.Click += new EventHandler(this.serversToolStripMenuItem_Click);
            this.snippetsToolStripMenuItem.Click += new EventHandler(this.snippetsToolStripMenuItem_Click);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.form_options == null || this.form_options.IsDisposed == true)
            {
                this.form_options = new FormOptions();
            }

            this.form_options.Show();

        }

        private void snippetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.form_options_snippet == null || this.form_options_snippet.IsDisposed == true)
            {
                this.form_options_snippet = new FormOptionsSnippets();
            }

            this.form_options_snippet.Show();

        }

        private void serversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.form_options_server == null || this.form_options_server.IsDisposed == true)
            {
                this.form_options_server = new FormServerOptions();
            }

            this.form_options_server.Show();
        }
    
        #endregion  

        #region "new features"
        private void SetMongoCollectionsOnDataImport()
        {
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB) {
                return;
            }
            
            return; 
            //todo
            //todo:code not working

            var currentDB = Program.JsEngine.MongoEngine.Repository.CurrentServer.Databases.FirstOrDefault(
                d => Name == Program.JsEngine.MongoEngine.Repository.CurrentServer.CurrentDatabase.Name);
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

            if (Program.MainXMLManager.CurrentEngine == JsEngineType.MongoDB) {
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
            Program.JsEngine.Repository.Init(Program.MainXMLManager.CurrentEngine);
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
            
            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }
            return this.open_file_dialog.FileName;
        }
        
        

        
    }
}
