using System;
using System.IO;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    class Utils
    {
        public static bool EnsurePathExists(string path)
        {
            /* Ensures the selected path exist. */
            if (!Directory.Exists(path))
            {
                try
                {
                    MessageBox.Show(String.Format("'{0}' does not exist.\n Create one?", path),
                        "Creating a Directory",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Directory.CreateDirectory(path);

                    return true;
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
