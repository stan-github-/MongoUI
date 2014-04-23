﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBUI;
using DBUI.Mongo;
using DBUI.DataModel;
//using ScintillaNET.Configuration;

//delete output files
//switch between databases, okay for now, the .mongo file handles that
//implement options form
//handle F5 at the topmost level and pass down to the child form.
//
namespace DBUI {
    public partial class FormMainMDI : Form {
        private int childFormNumber = 0;
        private DBType dbType = DBType.MongoDB;

        //public static XMLManager ini_xml;
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
            init();
        }

        private bool init() {
            if (Program.MongoXMLManager.Init() == false) { return false; }

            this.SetServerComboBox();
            this.SetDropDownFileHistory();
            this.SetDropDownCodeSnippet();
            this.SetMongoCollectionsOnDataImport();
            this.OpenLastOpendedFile();

            this.Closed += new EventHandler(FormMainMDI_Closed);
            this.historyMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(javascriptpathClicked);
            this.snippetsMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(javascriptpathClicked);
            this.saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            return true;
        }

        void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void javascriptpathClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (FormMongoQuery c in this.MdiChildren)
            {
                if (c.QueryFilePath == e.ClickedItem.Text)
                {
                    return;
                }
            }
            new FormMongoQuery(this).Init(FormMongoQuery.Mode.Existing, e.ClickedItem.Text);
        }

        void FormMainMDI_Closed(object sender, EventArgs e)
        {
            var l = new List<String>();
            MdiChildren.ToList().ForEach(c=> l.Add(((FormMongoQuery)c).QueryFilePath));
            Program.MongoXMLManager.FileHistory = l;

            Program.MongoXMLManager.SaveXml();
        }

        #region "server database combo boxes"

        private void SetServerComboBox()
        {
            Program.MongoXMLManager.Servers.ForEach
                    (x =>this.serverComboBox.Items.Add(x.Name));

            this.serverComboBox.Text = Program.MongoXMLManager.CurrentServer.Name;
            SetDatabaseComboBox();
        }

        private void SetDatabaseComboBox()
        {
            var server = 
                Program.MongoXMLManager.Servers.Where(x => x.Name == serverComboBox.Text).FirstOrDefault();
                        
            if (server == null)
            {
                MessageBox.Show("Server not configured correctly; check configure file.");
                return;
            }

            try
            {
                databaseComboBox.Items.Clear();
                server.Databases.ForEach
                    (x => this.databaseComboBox.Items.Add(x.Name));
            }catch{
                MessageBox.Show("Server not configured correctly; check configure file.");
                return;
            }

            this.databaseComboBox.Text = Program.MongoXMLManager.CurrentServer.CurrentDatabase;
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
        }

        private void SaveCurrentServerAndDatabase()
        {
            if(String.IsNullOrEmpty(serverComboBox.Text) || 
                String.IsNullOrEmpty(databaseComboBox.Text))
            {
                return;
            }

            Program.MongoXMLManager.CurrentServer =
                new Server
                {
                    Name = serverComboBox.Text,
                    CurrentDatabase = databaseComboBox.Text
                };
        }
        #endregion

        #endregion

        private void SetDropDownFileHistory()
        {
            Program.MongoXMLManager.FileHistory.ForEach
                (f=> this.historyMenu.DropDownItems.Add(f)
            );
        }

        private void SetDropDownCodeSnippet()
        {
            Program.MongoXMLManager.CodeSnippets.ForEach
                (f => this.snippetsMenu.DropDownItems.Add(f)
            );
        }

        private void SetMongoCollectionsOnDataImport()
        {

            return; //code not working
            var currentDB = Program.MongoXMLManager.CurrentServer.Databases.FirstOrDefault(
                d => Name == Program.MongoXMLManager.CurrentServer.CurrentDatabase);
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

        private void OpenLastOpendedFile()
        {
            foreach (var path in Program.MongoXMLManager.LastOpenedFilePaths)
            {
                new Mongo.FormMongoQuery(this).Init(FormMongoQuery.Mode.Existing, path);        
            }
        }

        private void OpenFile(object sender, EventArgs e) {
            var mongoChildForm = new FormMongoQuery(this).Init(FormMongoQuery.Mode.FileDialog);
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            //string s = Program.MongoXMLManager.QueryFolderPath + "\\" + Guid.NewGuid() + ".js";
            //
            new FormMongoQuery(this).Init(FormMongoQuery.Mode.New);
        }

        
        #region "miscellaneous UI"
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            StringBuilder b = new StringBuilder();
            b.Append("Configure MongoXML.xml\n");
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
    }
}
