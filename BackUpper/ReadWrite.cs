using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using WPFCustomMessageBox;

namespace BackUpper
{
    public class ReadWrite
    {
        public string GetDirectorySize(string path)
        {
            string[] files = Directory.GetFiles(path, "*.*" , SearchOption.AllDirectories);
            long totalBytes = 0;
            string size = "";
            foreach (var name in files)
            {
                FileInfo info = new FileInfo(name);
                totalBytes += info.Length;
            }
            // Convert size to a sensible unit
            if (totalBytes > Math.Pow(2, 10))
            {
                size = Math.Round(totalBytes / Math.Pow(2, 10), 2) + "kB";
            }
            if (totalBytes > Math.Pow(2, 20))
            {
                size = Math.Round(totalBytes / Math.Pow(2, 20), 2) + "MB";
            }
            if (totalBytes > Math.Pow(2, 30))
            {
                size = Math.Round(totalBytes / Math.Pow(2, 30), 2) + "GB";
            }
            return size;
        }

        public void CopyFiles(List<DirectorySpecs> directoriesToCopy, string targetDirectory)
        {
            // Used values 1: Overwrite one, 2: Do not overwrite, 3: Yes to all (i.e. always overwrite)
            int overwrite = 1;

            foreach (var folder in directoriesToCopy)
            {
                // Create a new directory tree "target folder + folders to copy" 
                Directory.CreateDirectory(targetDirectory + folder.Path.Substring(2, folder.Path.Length - 2));
                string[] dirPaths = Directory.GetDirectories(folder.Path, "*", SearchOption.AllDirectories);
                foreach (var path in dirPaths)
                {
                    string newDirectory = targetDirectory + path.Substring(2, path.Length - 2);
                    if (!Directory.Exists(newDirectory))
                    {
                        Directory.CreateDirectory(newDirectory);
                    }
                }

                // Copy each file to new directory
                string[] filePaths = Directory.GetFiles(folder.Path, "*.*", SearchOption.AllDirectories);
                foreach (var path in filePaths)
                {
                    // Check if the file exists and if we want to overwrite
                    if (overwrite != 3 && File.Exists(targetDirectory + path.Substring(2, path.Length - 2)))
                    {
                        overwrite = OverwritePromt(path.Substring(2, path.Length - 2));
                    }

                    if (overwrite == 2)
                    {
                        // Do not re-write the file
                        continue;
                    }

                    try
                    {
                        Stream readStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        Stream writeStream = new FileStream(targetDirectory + path.Substring(2, path.Length - 2), FileMode.OpenOrCreate, FileAccess.Write);

                        byte[] readBuffer = new byte[readStream.Length];
                        readStream.Read(readBuffer, 0, (int)readStream.Length);

                        writeStream.Seek(0, SeekOrigin.End);
                        writeStream.Write(readBuffer, 0, readBuffer.Length);

                    }
                    catch (IOException)
                    {
                        MessageBoxResult mbResult =
                            MessageBox.Show(
                                "An error occured while copying the file " + path +
                                "\nMost likely the file is being used by another process.\nThe file will not be copied to the directory.",
                                "Error copying file", MessageBoxButton.OK);
                        //throw;
                    }
                }
            }
        }

        private int OverwritePromt(string file)
        {
            MessageBoxResult mbResult = CustomMessageBox.ShowYesNoCancel("The file " + file + " already exists, do you want to overwrite?", "Do you want to overwrite?",
                "Overwrite this file", "Do not overwrite", "Overwrite all");

            if (mbResult == MessageBoxResult.Yes)
            {
                // Return yes to overwrite one file
                return 1;
            }
            if (mbResult == MessageBoxResult.No)
            {
                // Return no overwriting
                return 2;
            }
            if (mbResult == MessageBoxResult.Cancel)
            {
                // Return yes to overwrite all files
                return 3;
            }
            else
            {
                return 2;
            }
        }

        public void SaveFile(string filename, string contents)
        {
            StreamWriter writer = new StreamWriter(filename);
            try
            {
                writer.Write(contents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            writer.Flush();
            writer.Close();
        }

        public void LoadFile(string file)
        {
            StreamReader reader = new StreamReader(file);
            try
            {
                string fileContents = reader.ReadToEnd();
                Serialization serializer = new Serialization();
                //serializer.DeserializeVariables(fileContents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}