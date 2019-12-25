using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace BackUpper
{
    public class DialogManager
    {
        /// <summary>
        /// Selects a directory with FolderBrowserDialog, returns the path and size of the chosen folder in a list
        /// </summary>
        /// <returns></returns>
        public DirectorySpecs SelectFolderToCopy()
        {
            List<DirectorySpecs> directories = new List<DirectorySpecs>();
            string path = "";
            string size = "";
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder";
            ReadWrite fileHandler = new ReadWrite();

            DialogResult result = folderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = folderDialog.SelectedPath;
                size = fileHandler.GetDirectorySize(path);
                if (path != "")
                {
                    return new DirectorySpecs() {Path = path, Size = size};
                }
                Console.WriteLine("Folder dialog selection returned empty path!");
                return null;
            }

            else return null;
        }

        public string SelectTargetFolder(string originalValue)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder";
            DialogResult result = folderDialog.ShowDialog();

            if (result == DialogResult.OK && folderDialog.SelectedPath != null)
            {
                return folderDialog.SelectedPath;
            }
            else return originalValue;
        }

        public void OpenSaveDialog(string variablesToSave)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = "saved files",
                DefaultExt = ".xml",
                AddExtension = true,
                OverwritePrompt = true
            };
            saveDialog.ShowDialog();
            Console.WriteLine(saveDialog.FileName);
            ReadWrite rw = new ReadWrite();
            rw.SaveFile(saveDialog.FileName, variablesToSave);
        }

        public string OpenLoadDialog()
        {
            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.Filter = "xml files (*.xml)|*.xml";

            // ShowDialog "ok" returns true, "cancel" returns false
            bool? open = loadDialog.ShowDialog();
            if (open == true)
            {
                return loadDialog.FileName;
            }
            return null;
        }
    }
}

   