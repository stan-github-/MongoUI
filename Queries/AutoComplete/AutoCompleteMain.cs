using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Queries.AutoComplete
{
    

    public class AutoCompleteMain
    {

        public static void Run(ScintillaNET.Scintilla text_box, bool debug = false)
        {
            //if (MongoMethods.IsQueryEndingInDB(text_box.TextBeforeCursor()) == true)
            //{
            //    var collections = MongoMethods.GetCollectionNames();
            //    SetList(text_box, collections);
            //    return;
            //}
            
            //if (MongoMethods.IsQueryEndingInCollectionName(text_box.TextBeforeCursor()) == true)
            //{
            //    SetList(text_box, MongoMethods.CollectionObjectMethods);
            //    return;
            //}
            
            //todo not most elegant!!! need more refactoring
            //if (ObjectAutoCompleter.IsQueryEndingInClosingParenthesis(text_box.TextBeforeCursor()) == true)
            //{
            var properties = ObjectAutoCompleter
            .Main(text_box.TextBeforeCursor(), text_box.TextAfterCursor());

            SetList(text_box, properties);
            
            
            //if (ObjectAutoCompleter.IsQueryEndingInUnderScore(text_box.TextBeforeCursor())){
            //    var properties = ObjectAutoCompleter
            //        .Main(text_box.TextBeforeCursor(), text_box.TextAfterCursor());

            //    SetList(text_box, properties);
            //    return;
            //}

        }

        private static void SetList(ScintillaNET.Scintilla text_box, List<String> methods)
        {
            text_box.AutoComplete.MaxHeight = 10;
            text_box.AutoComplete.Show(0, methods);
        }
    }

}
