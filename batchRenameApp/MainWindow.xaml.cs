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

namespace batchRenameApp
{

    public partial class MainWindow : Window
    {


        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();


        private void addFile(string filedir) {
            MyFile newfile = new MyFile(filedir);
            newfile.fileimage = "images/file.png";
         filelist.Add(newfile);
         FileList.ItemsSource = filelist;
        }

        private void addFolder(string folderdir)
        {
            Folder newfolder = new Folder(folderdir);
            newfolder.folderimage = "images/folder.png";
            folderlist.Add(newfolder);
            FolderList.ItemsSource = folderlist;
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
        private void SaveVolume_Click(object sender, RoutedEventArgs e)
        {
          
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
            dialog.ShowDialog();
            if (dialog.ShowDialog() != null)
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
    }
}
