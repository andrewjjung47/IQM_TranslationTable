using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Parse a text of a list of position pairs into a list of position pairs.
        /// </summary>
        /// <param name="text">A text of a list of position pairs, 
        /// formatted as (pos1, pos2), (pos3, pos4), and so on</param>
        /// <returns></returns>
        public static List<Tuple<int, int>> parsePairList(string text)
        {
            // Split the text into a string array of pairs.
            string[] pairListText = Regex.Split(text, @"\),\s*");
            List<Tuple<int, int>> pairList= new List<Tuple<int,int>>();
            foreach (string pairText in pairListText) {
                // Replace "(" and ")" in a pair, and split the pair.
                string[] pair = Regex.Split(pairText.Replace("(", "").Replace(")", ""), @",\s*");
                pairList.Add(new Tuple<int, int> (int.Parse(pair[0]), int.Parse(pair[1])));
            }
            return pairList;
        }
    }
}
