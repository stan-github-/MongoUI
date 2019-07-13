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

            this.SetServerComboBox();
            this.SetDropDownFileHistory();
            //this.SetDropDownCodeSnippet();
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

            //_autoCompleter.RefreshCurrentDBCollectionNames();
        
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
            var l = Program.Config.Data.Miscellaneous.LastOpenedFilePaths;
            if (l.Count == 0) {
                return;
            }

            var file = l.Last();
            new Queries.FormQuery(this).Init(FormQuery.Mode.Existing, file);
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
            Program.Config.Save();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region "server database combo boxes"

        private void SetServerComboBox()
        {
            Program.Config.Data.Servers.ForEach
                    (x =>{
                        if (!this.serverComboBox.Items.Contains(x.Alias)){
                            this.serverComboBox.Items.Add(x.Alias);
                        }
                    });

            this.serverComboBox.Text = Program.Config.CurrentServer().Alias;
            SetDatabaseComboBox();
        }

        private void SetDatabaseComboBox()
        {

            var server = 
                Program.Config.Data.Servers.Where
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

            this.databaseComboBox.Text = Program.Config.CurrentServer().CurrentDatabase.Name;
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
            //_autoCompleter.RefreshCurrentDBCollectionNames();
        }

        private void SaveCurrentServerAndDatabase()
        {
            if(String.IsNullOrEmpty(serverComboBox.Text) || 
                String.IsNullOrEmpty(databaseComboBox.Text))
            {
                return;
            }

            Program.Config.Data.Servers.ForEach(s=>s.IsCurrent = false);
            var server = Program.Config.Data.Servers.First
                (s => s.Name == serverComboBox.Text);
            server.IsCurrent = true;
            server.Databases.ForEach(s => s.IsCurrent = false);
            var database = server.Databases.First(d => d.Name == databaseComboBox.Text);
            database.IsCurrent = true;
            
        }
        #endregion

        #endregion

        #region "drop down snippet code files etc"
        public void SetDropDownFileHistory()
        {
            this.historyMenu.DropDownItems.Clear();
            Program.Config.Data.Miscellaneous.LastOpenedFilePaths.ForEach
                (f=> this.historyMenu.DropDownItems.Add(f)
            );
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
            return; 
            //todo
            //todo:code not working
            var currentDB = Program.Config.CurrentServer().CurrentDatabase;
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
