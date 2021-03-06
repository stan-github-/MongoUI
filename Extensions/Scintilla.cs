﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Scintilla
{
    public static String TextBeforeCursor(this ScintillaNET.Scintilla text_box) {
        var s = text_box.Text.Substring(0, text_box.CurrentPosition - 1);
        return s;
    }

    public static String TextAfterCursor(this ScintillaNET.Scintilla text_box){
        var currentPos = text_box.CurrentPosition;
        var text = text_box.Text;
                
        var e = currentPos == text.Length ? "" : text_box.Text.Substring(
                    text_box.CurrentPosition + 1,  //skipping the dot
                    text_box.Text.Length - text_box.CurrentPosition - 1);
        return e;
    }
}
