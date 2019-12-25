using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BackUpper
{
    /// <summary>
    /// Interaction logic for ControlsWindow.xaml
    /// </summary>
    public partial class ControlsWindow : UserControl
    {
        private List<DirectorySpecs> _directoriesToCopy = new List<DirectorySpecs>();
        private string _targetDirectory = @"D:\BackupperTest\";

        public struct VariablesToSave
        {
            public List<DirectorySpecs> DirectoriesToCopy;
            public string TargetDirectory;

            public VariablesToSave(List<DirectorySpecs> directories, string target)
            {
                DirectoriesToCopy = directories;
                TargetDirectory = target;
            }
        }

        public ControlsWindow()
        {
            InitializeComponent();
            TargetPath.Content = _targetDirectory;
        }

        public ControlsWindow(List<DirectorySpecs> directories, string target)
        {
            InitializeComponent();
            TargetPath.Content = target;
            _directoriesToCopy = directories;
        }
        
        

        #region Button Clicks

        private void AddFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            AddFoldersToList();
        }
        private void RemoveFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            // This needs to be done BEFORE removing the item from the listview as removing naturally unselects it too
            _directoriesToCopy.Remove((DirectorySpecs)ListViewFolders.SelectedItem);     
            ListViewFolders.Items.Remove(ListViewFolders.SelectedItem);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "Copying...";
            ReadWrite readWrite = new ReadWrite();
            readWrite.CopyFiles(_directoriesToCopy, TargetPath.Content.ToString());
            StatusLabel.Content = "Done!";
            ListViewFolders.SelectAll();
        }

        private void ChangeTargetFolderButton_Click(object sender, RoutedEventArgs e)
        {
            DialogManager dialogManager = new DialogManager();
            _targetDirectory = dialogManager.SelectTargetFolder(_targetDirectory);
            TargetPath.Content = _targetDirectory;
        }

        #endregion

        #region Menu item clicks
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            DialogManager dialogManager = new DialogManager();
            string filePath = dialogManager.OpenLoadDialog();

            Serialization serialization = new Serialization();

            VariablesToSave varsToSave = serialization.DeserializeVariables(filePath);
            _directoriesToCopy = varsToSave.DirectoriesToCopy;
            _targetDirectory = varsToSave.TargetDirectory;

            UpdateControlWindow(_directoriesToCopy, _targetDirectory);
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            VariablesToSave varsToSave = new VariablesToSave(_directoriesToCopy, _targetDirectory);
            Serialization serialization = new Serialization();
            string variablesToSave = serialization.SerializeVariables(varsToSave);
            DialogManager dialogManager = new DialogManager();
            dialogManager.OpenSaveDialog(variablesToSave);
        }

        private void MenuItemHelp_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }

        #endregion
        private void AddFoldersToList()
        {
            DialogManager dialogManager = new DialogManager();
            DirectorySpecs directory = new DirectorySpecs();
            directory = dialogManager.SelectFolderToCopy();

            if (directory != null)
            {
                try
                {
                    _directoriesToCopy.Add(directory);
                    ListViewFolders.Items.Add(directory);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            } 
        }


        private void ClearAll()
        {
            while (_directoriesToCopy.Count != 0 && ListViewFolders.Items.Count != 0)
            {
                ListViewFolders.SelectAll();
                _directoriesToCopy.Remove((DirectorySpecs) ListViewFolders.SelectedItem);
                ListViewFolders.Items.Remove(ListViewFolders.SelectedItem);
            }
            _targetDirectory = @"D:\BackupperTest\";
        }

        private void UpdateControlWindow(List<DirectorySpecs> directories, string target)
        {
            foreach (var VARIABLE in directories)
            {
                ListViewFolders.Items.Add(VARIABLE);
            }
            TargetPath.Content = target;
        }
    }
}
