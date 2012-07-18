namespace DBUI.Mongo {
    partial class FormMongoQuery {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMongoQuery));
            this.open_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.tool_strip = new System.Windows.Forms.ToolStrip();
            this.button_excecute = new System.Windows.Forms.ToolStripButton();
            this.button_translate = new System.Windows.Forms.ToolStripButton();
            this.text_box = new ScintillaNET.Scintilla();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scintillaOutput = new ScintillaNET.Scintilla();
            this.tool_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.text_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintillaOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // tool_strip
            // 
            this.tool_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_excecute,
            this.button_translate});
            this.tool_strip.Location = new System.Drawing.Point(0, 0);
            this.tool_strip.Name = "tool_strip";
            this.tool_strip.Size = new System.Drawing.Size(1192, 25);
            this.tool_strip.TabIndex = 6;
            this.tool_strip.Text = "toolStrip1";
            // 
            // button_excecute
            // 
            this.button_excecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_excecute.Image = ((System.Drawing.Image)(resources.GetObject("button_excecute.Image")));
            this.button_excecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_excecute.Name = "button_excecute";
            this.button_excecute.Size = new System.Drawing.Size(51, 22);
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
            this.button_translate.Size = new System.Drawing.Size(59, 22);
            this.button_translate.Text = "Translate";
            this.button_translate.Visible = false;
            // 
            // text_box
            // 
            this.text_box.Caret.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.text_box.ConfigurationManager.CustomLocation = "ScintillaNET.xml";
            this.text_box.ConfigurationManager.Language = "js";
            this.text_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text_box.Location = new System.Drawing.Point(0, 0);
            this.text_box.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.text_box.Name = "text_box";
            this.text_box.Size = new System.Drawing.Size(1192, 225);
            this.text_box.Styles.BraceBad.Size = 7F;
            this.text_box.Styles.BraceLight.Size = 7F;
            this.text_box.Styles.ControlChar.Size = 7F;
            this.text_box.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.text_box.Styles.Default.Size = 7F;
            this.text_box.Styles.IndentGuide.Size = 7F;
            this.text_box.Styles.LastPredefined.Size = 7F;
            this.text_box.Styles.LineNumber.Size = 7F;
            this.text_box.Styles.Max.Size = 7F;
            this.text_box.TabIndex = 7;
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
            this.splitContainer1.Panel1.Controls.Add(this.text_box);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.scintillaOutput);
            this.splitContainer1.Size = new System.Drawing.Size(1192, 450);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 8;
            // 
            // scintillaOutput
            // 
            this.scintillaOutput.Caret.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scintillaOutput.ConfigurationManager.CustomLocation = "ScintillaNET.xml";
            this.scintillaOutput.ConfigurationManager.Language = "js";
            this.scintillaOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaOutput.Location = new System.Drawing.Point(0, 0);
            this.scintillaOutput.Margin = new System.Windows.Forms.Padding(2);
            this.scintillaOutput.Name = "scintillaOutput";
            this.scintillaOutput.Size = new System.Drawing.Size(1192, 221);
            this.scintillaOutput.Styles.BraceBad.Size = 7F;
            this.scintillaOutput.Styles.BraceLight.Size = 7F;
            this.scintillaOutput.Styles.ControlChar.Size = 7F;
            this.scintillaOutput.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintillaOutput.Styles.Default.Size = 7F;
            this.scintillaOutput.Styles.IndentGuide.Size = 7F;
            this.scintillaOutput.Styles.LastPredefined.Size = 7F;
            this.scintillaOutput.Styles.LineNumber.Size = 7F;
            this.scintillaOutput.Styles.Max.Size = 7F;
            this.scintillaOutput.TabIndex = 8;
            // 
            // FormMongoQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 475);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tool_strip);
            this.Name = "FormMongoQuery";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.tool_strip.ResumeLayout(false);
            this.tool_strip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.text_box)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scintillaOutput)).EndInit();
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
    }
}