namespace DBUI.Queries {
    partial class FormQuery {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuery));
            this.open_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.tool_strip = new System.Windows.Forms.ToolStrip();
            this.button_excecute = new System.Windows.Forms.ToolStripButton();
            this.button_translate = new System.Windows.Forms.ToolStripButton();
            this.OutputTypeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.QueryBox = new ScintillaNET.Scintilla();
            this.QueryOuput = new ScintillaNET.Scintilla();
            this.tool_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tool_strip
            // 
            this.tool_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_excecute,
            this.button_translate,
            this.OutputTypeComboBox});
            this.tool_strip.Location = new System.Drawing.Point(0, 0);
            this.tool_strip.Name = "tool_strip";
            this.tool_strip.Size = new System.Drawing.Size(771, 25);
            this.tool_strip.TabIndex = 6;
            this.tool_strip.Text = "toolStrip1";
            // 
            // button_excecute
            // 
            this.button_excecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_excecute.Image = ((System.Drawing.Image)(resources.GetObject("button_excecute.Image")));
            this.button_excecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_excecute.Name = "button_excecute";
            this.button_excecute.Size = new System.Drawing.Size(59, 22);
            this.button_excecute.Text = "E&xecute";
            this.button_excecute.ToolTipText = "Execute (F5)";
            this.button_excecute.Click += new System.EventHandler(this.button_excecute_Click);
            // 
            // button_translate
            // 
            this.button_translate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_translate.Enabled = false;
            this.button_translate.Image = ((System.Drawing.Image)(resources.GetObject("button_translate.Image")));
            this.button_translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_translate.Name = "button_translate";
            this.button_translate.Size = new System.Drawing.Size(67, 22);
            this.button_translate.Text = "Translate";
            this.button_translate.Visible = false;
            // 
            // OutputTypeComboBox
            // 
            this.OutputTypeComboBox.Name = "OutputTypeComboBox";
            this.OutputTypeComboBox.Size = new System.Drawing.Size(92, 25);
            this.OutputTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.QueryOutputType_Selected);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(210, 25);
            this.miniToolStrip.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.QueryBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.QueryOuput);
            this.splitContainer1.Size = new System.Drawing.Size(771, 450);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 8;
            // 
            // QueryBox
            // 
            this.QueryBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QueryBox.Location = new System.Drawing.Point(0, 0);
            this.QueryBox.Name = "QueryBox";
            this.QueryBox.Size = new System.Drawing.Size(771, 223);
            this.QueryBox.TabIndex = 0;
            this.QueryBox.UseTabs = false;
            this.QueryBox.Click += new System.EventHandler(this.QueryBox_Click);
            // 
            // QueryOuput
            // 
            this.QueryOuput.Location = new System.Drawing.Point(3, 3);
            this.QueryOuput.Name = "QueryOuput";
            this.QueryOuput.Size = new System.Drawing.Size(768, 217);
            this.QueryOuput.TabIndex = 0;
            this.QueryOuput.UseTabs = false;
            // 
            // FormMongoQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 475);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tool_strip);
            this.Name = "FormMongoQuery";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.tool_strip.ResumeLayout(false);
            this.tool_strip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog open_file_dialog;
        private System.Windows.Forms.ToolStrip tool_strip;
        private System.Windows.Forms.ToolStripButton button_excecute;
        private System.Windows.Forms.ToolStripButton button_translate;
        private ScintillaNET.Scintilla text_box;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ScintillaNET.Scintilla scintillaOutput;
        private System.Windows.Forms.ToolStripComboBox OutputTypeComboBox;
        private ScintillaNET.Scintilla QueryBox;
        private ScintillaNET.Scintilla QueryOuput;
    }
}