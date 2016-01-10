using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Queries.AutoComplete
{
    
    public class AutoCompleteUI
    {
        public static void RunMongo(ScintillaNET.Scintilla text_box, bool debug = false)
        {
            var methods = Query.AutoComplete.QueryExecuter.Main
                (text_box.TextBeforeCursor(), text_box.TextAfterCursor());
            SetList(text_box, methods);
        }

        private static void SetList(ScintillaNET.Scintilla text_box, List<String> methods)
        {
            text_box.AutoCMaxHeight = 10;

            if (methods.Count == 1) {
                text_box.InsertText(text_box.CurrentPosition,  methods[0]);
                return;
            }
            
            text_box.AutoCShow(0, string.Join(" ", methods));
        }
    }

}
