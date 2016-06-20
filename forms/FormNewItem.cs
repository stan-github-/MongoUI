using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBUI.Forms
{
    public partial class FormNewItem : Form
    {
        public delegate void CallBack(String value);

        public CallBack callBack { get; set; }
        public FormNewItem()
        {
            InitializeComponent();

            this.button_okay.Click += button_okay_Click;
            
        }

        void button_okay_Click(object sender, EventArgs e)
        {
            this.callBack(this.text_box.Text);

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
