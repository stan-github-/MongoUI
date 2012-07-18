using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBUI {
    public partial class FormError : Form {
        public FormError() {
            InitializeComponent();
        }

        public void Append(string s) {
            textbox_error.AppendText(s);
        }

        public void Write(string s) {
            textbox_error.Text = s;
        }
    }
}