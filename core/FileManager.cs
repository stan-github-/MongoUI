using System;
using System.IO;

namespace DBUI {
    public static class FileManager {

        public static string ReadFromFile(string filePath) {
            String s = String.Empty;
            if (string.IsNullOrEmpty(filePath))
            {
                return s;
            }
            if (File.Exists(filePath) == false)
            {
                return s;
            }

            try {
                s = File.ReadAllText(filePath);
            } catch (Exception e) {
                ErrorManager.Write(e);
            }
            return s;
        }

        public static bool AppendToFile(string filePath, String s)
        {
            try
            {
                File.AppendAllText(filePath, s);
            }
            catch (Exception e)
            {
                ErrorManager.Write(e);
            }
            return true;
        }

        public static bool SaveToFile(string filePath, string s) {
            try {
                File.WriteAllText(filePath, s);
            } catch (Exception e) {
                ErrorManager.Write(e);
            }
            return true;
        }

        public static void DeleteFile(string filePath) {
            if (File.Exists(filePath) == false) { return; }

            try {
                File.Delete(filePath);
            } catch (Exception e) {
                ErrorManager.Write(e);
            }
            return;
        }
    }
}