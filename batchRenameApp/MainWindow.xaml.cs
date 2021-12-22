using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Contract;

namespace batchRenameApp
{

    public partial class MainWindow : Window
    {

        int totalRule = 0;
        List<IRule> allRules = new List<IRule>();
        List<String> allRulesName = new List<String>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        List<UserControl> userControls = new List<UserControl>();
        MyPikaFile testingFile = new MyPikaFile();
        string backupName = "    Pikachu   .txt";
        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();


        private void addFile(string filedir) {
            if (isFileNotExist(filedir))
            {
                MyFile newfile = new MyFile(filedir);
                newfile.fileimage = "images/file.png";
                filelist.Add(newfile);
                FileList.ItemsSource = filelist;
            }
        }

        private void addFolder(string folderdir)
        {
            if (isFolderNotExist(folderdir))
            {
                Folder newfolder = new Folder(folderdir);
                newfolder.folderimage = "images/folder.png";
                folderlist.Add(newfolder);
                FolderList.ItemsSource = folderlist;
            }
        }

        private bool isFileNotExist(string filedir)
        {

            foreach (MyFile myfile in filelist)
            {
                if (myfile.filepath.Equals(filedir))
                {
                    return false;
                }
            }
            return true;
        }

        private bool isFolderNotExist(string folderdir)
        {

            foreach (Folder myfolder in folderlist)
            {
                if (myfolder.folderpath.Equals(folderdir))
                {
                    return false;
                }
            }
            return true;
        }


        private void DragOverFileList(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames =
                                 e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string filename in filenames)
                {
                    if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".CS")
                    {
                        dropEnabled = false;
                        break;
                    }
                }
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }


        private void DropFileList(object sender, DragEventArgs e)
        {
            string[] droppedFilenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string filename in droppedFilenames)
            {
                
                addFile(filename);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get all rule from DLL
            totalRule = RuleFactory.GetInstance().RuleAmount();
            for (int i = 0; i < totalRule; i++)
            {
                allRules.Add(RuleFactory.GetInstance().Create(i));
                userControls.Add(allRules[i].GetUI());
                userRules.Add(allRules[i]);
                allRulesName.Add(allRules[i].GetName());
            }
            RuleComboBox.ItemsSource = allRulesName;
            RuleList.ItemsSource = userRules;
            this.DataContext = testingFile;

        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void previewItem_Click(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
                addFile(openFileDialog.FileName);
        }

        private void unlockMenu_Click(object sender, RoutedEventArgs e)
        {
        }
        private void deleteMenu_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void lockMenu_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void changePasswordMenu_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void renameMenu_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void importMenu_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void unlockFileMenu_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void lockFileMenu_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void deleteFileMenu_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void renameFileMenu_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void changePasswordFileMenu_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void exportFileMenu_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void ExportVolume_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                addFolder(dialog.FileName);
        }

        private void ChangePasswordVolume_Click(object sender, RoutedEventArgs e)
        {

         
        }

        private void ChangeNameVolume_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void CreateNewVolume_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void DropFolderList(object sender, DragEventArgs e)
        {
            string[] droppedFoldernames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string foldername in droppedFoldernames)
            {
                if (System.IO.Path.GetExtension(foldername) == String.Empty)
                {
                    addFolder(foldername);
                }
            }
        }

        private void DragOverFolderList(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] foldernames =
                                 e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string foldername in foldernames)
                {
                    if (System.IO.Path.GetExtension(foldername) != String.Empty)
                    {
                        dropEnabled = false;
                        break;
                    }
                }
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void StartBatching_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RuleList_LayoutUpdated(object sender, EventArgs e)
        {
            List<string> listOfFileName = new List<string>();
           // List<string> listOfFolderName = new List<string>();

            for (int i=0; i < filelist.Count(); i++)
            {
                listOfFileName.Add(filelist[i].filename);
                filelist[i].newfilename = filelist[i].filename;
            }

           // for (int i = 0; i < folderlist.Count(); i++)
           // {
           //     listOfFolderName.Add(folderlist[i].foldername);
           //     folderlist[i].newfoldername = folderlist[i].foldername;
           // }

            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(listOfFileName);
                    //List<string> temp2 = userRules[i].Rename(listOfFolderName);
                    for (int j = 0; j < filelist.Count(); j++)
                    {
                        filelist[j].newfilename = temp[j];
                    }

                   // for (int j = 0; j < folderlist.Count(); j++)
                  //  {

                  //      folderlist[i].newfoldername = temp2[j];
                  //  }
                }
            }
           
        }

        private void RuleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListView)sender;
            if (e.AddedItems.Count > 0 && listView.SelectedItem != e.AddedItems[0])
                listView.SelectedItem = e.AddedItems[0];
        }


        private void Remove_Rule_Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            IRule rule = b.CommandParameter as IRule;
            userRules.Remove(rule);
        }

        private void Use_Rule_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;
            int index = userRules.IndexOf(rule);
            //code here
        }

        private void Use_Rule_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;
            int index = userRules.IndexOf(rule);
            //code here
        }
    }
}