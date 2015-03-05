using System;
using System.IO;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    class Utils
    {
        /// <summary>
        /// Ensures the selected path exist. 
        /// </summary>
        /// <param name="path">Full path to the directory of interest. 
        /// If the path specified does not exist already, create a new one.
        /// If path is empty string, logging is disabled 
        /// and no new directory is created.</param>
        /// <returns></returns>
        public static bool EnsurePathExists(string path)
        {
            if (path == "")
            {
                DialogResult result = MessageBox.Show("Disable logging?",
                        "Perform Without a Log",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);
                if (result == DialogResult.OK) return true;
                else return false;
            }
            else if (path != "" && !Directory.Exists(path))
            {
                try
                {
                    DialogResult result = MessageBox.Show(
                        String.Format("'{0}' does not exist.\n Create one?", path),
                        "Creating a Directory",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        Directory.CreateDirectory(path);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while creating the selected directory.", 
                        "Unable to Create a Directory",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }
            else return true;
        }
    }
}
