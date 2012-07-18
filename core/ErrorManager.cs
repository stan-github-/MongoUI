using System;
using System.Collections.Generic;
using System.Text;

namespace DBUI {
    class ErrorManager {
        private static FormError _errorForm;
        
        private static void OpenForm(){
            if (_errorForm == null){
                _errorForm = new FormError();
            }
            if (_errorForm.IsDisposed == true) {
                _errorForm = new FormError();
            }
            _errorForm.Show();
        }

        private static void appendToErrorForm(String s)
        {
            _errorForm.Append
       (String.Format("{0} {1}{2}",
    DateTime.Now.ToString(), s, Environment.NewLine));
        }
    

        public static void Write(string s) {
             OpenForm();
             appendToErrorForm(s);
        }

        public static void Debug(string s) {
            Write(s);
        }
        
        public static void Write(Exception ex) {
            Write(ex.Message);
        }
    }
}
