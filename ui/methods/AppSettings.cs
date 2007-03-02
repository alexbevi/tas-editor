using System;
using System.Windows.Forms;
using System.Diagnostics;

using AMS.Profile;

namespace MovieSplicer.UI.Methods
{
    class AppSettings
    {
        const string SETTINGS_FILE = "tas-editor.ini";
        const string SECTION_FILES = "Recent Files";
        const byte MAX_FILES = 9;

        /// <summary>
        /// Save a new filename as the first recent file
        /// Only MAX_FILES are stored, so bump the last entry off the list
        /// </summary>        
        public static void Save(string filename)
        {
            
            Ini settingsFile = new Ini();
            string[] files = Load();

            settingsFile.SetValue(SECTION_FILES, "File 1", filename);            
            
            for (int i = 0; i < MAX_FILES - 1; i++)
                if (files[i] != null) settingsFile.SetValue(SECTION_FILES, "File " + (i + 2), files[i]);                                                
        }

        /// <summary>
        /// Load the recent files from the ini file
        /// </summary>        
        public static string[] Load()
        {
            Ini settingsFile = new Ini();
            string[] files = new string[MAX_FILES];

            for (int i = 0; i < MAX_FILES; i++)
            {
                if (settingsFile.HasEntry(SECTION_FILES, "File " + (i + 1)))
                    files[i] = settingsFile.GetValue(SECTION_FILES, "File " + (i + 1)).ToString();
            }

            return files;
        }
    }
}
