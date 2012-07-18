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
            this.split_container_inner = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.text_box_query_status = new System.Windows.Forms.ToolStripTextBox();
            this.split_container_outer = new System.Windows.Forms.SplitContainer();
            this.text_box = new ScintillaNET.Scintilla();
            this.tool_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_container_inner)).BeginInit();
            this.split_container_inner.Panel2.SuspendLayout();
            this.split_container_inner.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_container_outer)).BeginInit();
            this.split_container_outer.Panel2.SuspendLayout();
            this.split_container_outer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.text_box)).BeginInit();
            this.SuspendLayout();
            // 
            // tool_strip
            // 
            this.tool_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_excecute,
            this.button_translate});
            this.tool_strip.Location = new System.Drawing.Point(0, 0);
            this.tool_strip.Name = "tool_strip";
            this.tool_strip.Size = new System.Drawing.Size(967, 27);
            this.tool_strip.TabIndex = 6;
            this.tool_strip.Text = "toolStrip1";
            // 
            // button_excecute
            // 
            this.button_excecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_excecute.Image = ((System.Drawing.Image)(resources.GetObject("button_excecute.Image")));
            this.button_excecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_excecute.Name = "button_excecute";
            this.button_excecute.Size = new System.Drawing.Size(67, 24);
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
            this.button_translate.Size = new System.Drawing.Size(73, 24);
            this.button_translate.Text = "Translate";
            this.button_translate.Visible = false;
            // 
            // split_container_inner
            // 
            this.split_container_inner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_container_inner.Location = new System.Drawing.Point(0, 0);
            this.split_container_inner.Margin = new System.Windows.Forms.Padding(4);
            this.split_container_inner.Name = "split_container_inner";
            // 
            // split_container_inner.Panel2
            // 
            this.split_container_inner.Panel2.Controls.Add(this.toolStrip1);
            this.split_container_inner.Size = new System.Drawing.Size(558, 382);
            this.split_container_inner.SplitterDistance = 273;
            this.split_container_inner.SplitterWidth = 5;
            this.split_container_inner.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.text_box_query_status});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(280, 29);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // text_box_query_status
            // 
            this.text_box_query_status.Name = "text_box_query_status";
            this.text_box_query_status.ReadOnly = true;
            this.text_box_query_status.Size = new System.Drawing.Size(300, 27);
            // 
            // split_container_outer
            // 
            this.split_container_outer.Location = new System.Drawing.Point(0, 0);
            this.split_container_outer.Margin = new System.Windows.Forms.Padding(4);
            this.split_container_outer.Name = "split_container_outer";
            // 
            // split_container_outer.Panel2
            // 
            this.split_container_outer.Panel2.Controls.Add(this.split_container_inner);
            this.split_container_outer.Size = new System.Drawing.Size(596, 382);
            this.split_container_outer.SplitterDistance = 33;
            this.split_container_outer.SplitterWidth = 5;
            this.split_container_outer.TabIndex = 3;
            // 
            // text_box
            // 
            this.text_box.Caret.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.text_box.ConfigurationManager.CustomLocation = "ScintillaNET.xml";
            this.text_box.ConfigurationManager.Language = "js";
            this.text_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text_box.Location = new System.Drawing.Point(0, 27);
            this.text_box.Name = "text_box";
            this.text_box.Size = new System.Drawing.Size(967, 350);
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
            // FormMongoQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 377);
            this.Controls.Add(this.text_box);
            this.Controls.Add(this.tool_strip);
            this.Controls.Add(this.split_container_outer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMongoQuery";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.tool_strip.ResumeLayout(false);
            this.tool_strip.PerformLayout();
            this.split_container_inner.Panel2.ResumeLayout(false);
            this.split_container_inner.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_container_inner)).EndInit();
            this.split_container_inner.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.split_container_outer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_container_outer)).EndInit();
            this.split_container_outer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.text_box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog open_file_dialog;
        private System.Windows.Forms.ToolStrip tool_strip;
        private System.Windows.Forms.ToolStripButton button_excecute;
        private System.Windows.Forms.ToolStripButton button_translate;
        private System.Windows.Forms.SplitContainer split_container_inner;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox text_box_query_status;
        private System.Windows.Forms.SplitContainer split_container_outer;
        private ScintillaNET.Scintilla text_box;
    }
}