﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI.Forms
{
    public partial class FormNewSnippetFile : Form
    {
        public delegate void CallBack(string groupName, string name, String filePath);

        public CallBack callBack { get; set; }

        public FormNewSnippetFile()
        {
            InitializeComponent();

            this.button_okay.Click += button_okay_Click;
            this.button_file_path.Click += button_file_path_Click;

            //SetGroupNameComboBox();    
        }

        /*private void SetGroupNameComboBox() {
            var groupNames = Program.Config.Data.Miscellaneous.CodeSnippets
                .Select(f => f.GroupName).Distinct();

            groupNames.ToList().ForEach(g => this.combo_box_group_name.Items.Add(g));
        }*/

        private string OpenOpenFileDialog()
        {
            this.open_file_dialog.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.open_file_dialog.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";

            this.open_file_dialog.InitialDirectory =
            Path.GetDirectoryName(Program.Config.Data.Miscellaneous.QueryFolder);
            
            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }

            return this.open_file_dialog.FileName;
        }

        private void button_file_path_Click(object sender, EventArgs e)
        {
            
            var filePath = OpenOpenFileDialog();
            this.text_box_file_path.Text = filePath;
            
        }

        void button_okay_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.combo_box_group_name.Text) ||
                string.IsNullOrEmpty(this.text_box_snippet_name.Text) ||
                string.IsNullOrEmpty(this.text_box_file_path.Text)) {
                    ErrorManager.Write("Must fill all fields!");
                    return;
            }

            this.callBack(this.combo_box_group_name.Text, this.text_box_snippet_name.Text, this.text_box_file_path.Text);

            try
            {
                this.Dispose();
                this.Close();
            }
            catch (Exception ex) {
                ErrorManager.Write(ex);
            }
            
        }

    }
}
