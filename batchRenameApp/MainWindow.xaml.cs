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
using System.Text.Json;
using System.Globalization;
using HandyControl.Data;

namespace batchRenameApp
{
    
    public partial class MainWindow : Window
    {

        int totalRule = 0;
        List<IRule> allRules = new List<IRule>();
        List<String> allRulesName = new List<String>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();
        String BatchingSuccessStatus = "Batching successfully";
        String BatchingUnsuccessErrorStatus = "Error: Batching unsuccessfully";
        String FileDulicateErrorStatus = "Error: File duplicate";
        String FileNotExistErrorStatus = "Error: File not exist";
        String FileNameNotChangeErrorStatus = "Error: File name not change";
        String LastProjectAddress = @"LastProject\LastProject.json";
        String AppTitle = "Batch Rename";
        RenameProject currentProject = null;
        LastProject lastProject = null;

        private void StoreToProject()
        {
            double height = this.ActualHeight;
            double width = this.ActualWidth;
            double left = this.Left;
            double top = this.Top;
            List<RuleContainer> ruleContainers = new List<RuleContainer>();
            for (int i = 0; i < userRules.Count(); i++)
            {
                ruleContainers.Add(new RuleContainer()
                {
                    Name = userRules[i].GetName(),
                    Data = userRules[i].ToJson()
                });
            }
            currentProject.WindowHeight = height;
            currentProject.WindowWidth = width;
            currentProject.WindowLeft = left;
            currentProject.WindowTop = top;
            currentProject.Rules = ruleContainers;
            currentProject.Files = filelist.ToList();
            currentProject.Folders = folderlist.ToList();
        }

        private void StoreRules(List<IRule> rules, string path)
        {

            List<RuleContainer> ruleContainers = new List<RuleContainer>();
            for (int i = 0; i < rules.Count(); i++)
            {
                ruleContainers.Add(new RuleContainer()
                {
                    Name = rules[i].GetName(),
                    Data = rules[i].ToJson()
                });
            }

            string json = JsonSerializer.Serialize(ruleContainers);
            File.WriteAllText(path, json);

        }
        private List<IRule> ReadRules(string path)
        {

            string outJson = File.ReadAllText(path);
            List<RuleContainer> outRuleContainers = (List<RuleContainer>)JsonSerializer.Deserialize(outJson, typeof(List<RuleContainer>));
            List<IRule> resultRules = new List<IRule>();
            for (int i = 0; i < outRuleContainers.Count(); i++)
            {
                resultRules.Add(RuleFactory.GetInstance().Create(outRuleContainers[i]));
            }

            return resultRules;
        }
        private void addFile(string filedir)
        {
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

        }


        private void DropFileList(object sender, DragEventArgs e)
        {
            string[] droppedFilenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string filename in droppedFilenames)
            {

                addFile(filename);
            }

            //bool dropEnabled = true;
            //  if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            // {
            //     string[] filenames =
            //                     e.Data.GetData(DataFormats.FileDrop, true) as string[];

            //    foreach (string filename in filenames)
            //    {
            //        if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".CS")
            //       {
            //           dropEnabled = false;
            //           break;
            //      }
            //  }
            //  }
            //  else
            // {
            //     dropEnabled = false;
            // }

            // if (!dropEnabled)
            // {
            //     e.Effects = DragDropEffects.None;
            //    e.Handled = true;
            // }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get all rule from DLL
            totalRule = RuleFactory.GetInstance().RuleAmount();
            for (int i = 0; i < totalRule; i++)
            {
                allRules.Add(RuleFactory.GetInstance().Create(i));
                allRulesName.Add(allRules[i].GetName());
            }
            RuleComboBox.ItemsSource = allRulesName;
            lastProject = LastProject.Parse(LastProjectAddress);
            if (lastProject != null)
            {
                currentProject = RenameProject.Parser(lastProject.Address);
            }
            else
            {
                currentProject = new RenameProject();
            }
            if (currentProject != null)
            {
                this.Width = currentProject.WindowWidth;
                this.Height = currentProject.WindowHeight;
                this.Top = currentProject.WindowTop;
                this.Left = currentProject.WindowLeft;
                if (currentProject.Rules == null)
                {
                    currentProject.Rules = new List<RuleContainer>();
                }
                if (currentProject.Files == null)
                {
                    currentProject.Files = new List<MyFile>();
                }
                if (currentProject.Folders == null)
                {
                    currentProject.Folders = new List<Folder>();
                }
                userRules = new BindingList<IRule>(currentProject.GetRules());
                filelist = new BindingList<MyFile>(currentProject.Files);
                folderlist = new BindingList<Folder>(currentProject.Folders);
                FileList.ItemsSource = filelist;
                FolderList.ItemsSource = folderlist;
            }
            else
            {
                currentProject = new RenameProject();
            }
            this.Title = AppTitle + " - " + currentProject.GetName();

            RuleList.ItemsSource = userRules;
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
            for (int i = 0; i < filelist.Count(); i++)
            {
                FileInfo file = new FileInfo(filelist[i].filepath);
                string newfilepath = filelist[i].filepath.Substring(0, filelist[i].filepath.LastIndexOf(@"\") + 1);
                newfilepath += filelist[i].newfilename;
                FileInfo newfile = new FileInfo(newfilepath);
                if (!file.Exists)
                {
                    filelist[i].status = FileNotExistErrorStatus;
                }
                else if (filelist[i].filename.Equals(filelist[i].newfilename))
                {
                    filelist[i].status = FileNameNotChangeErrorStatus;

                }
                else if (newfile.Exists)
                {
                    filelist[i].status = FileDulicateErrorStatus;
                }
                else if (file.Exists && !newfile.Exists)
                {

                    System.IO.File.Move(filelist[i].filepath, newfilepath);

                    if (filelist[i].changeName(newfilepath))
                    {
                        filelist[i].status = BatchingSuccessStatus;
                    }
                    else
                    {
                        filelist[i].status = BatchingUnsuccessErrorStatus;
                    }
                }
            }
        }

        private void RuleList_LayoutUpdated(object sender, EventArgs e)
        {
            // List<string> listOfFileName = new List<string>();
            // List<string> listOfFolderName = new List<string>();

            //  for (int i=0; i < filelist.Count(); i++)
            //  {
            //       listOfFileName.Add(filelist[i].filename);
            //       filelist[i].newfilename = filelist[i].filename;
            //   }

            // for (int i = 0; i < folderlist.Count(); i++)
            // {
            //     listOfFolderName.Add(folderlist[i].foldername);
            //     folderlist[i].newfoldername = folderlist[i].foldername;
            // }

            //   for (int i = 0; i < userRules.Count(); i++)
            //  {
            //      if (userRules[i].IsUse())
            //       {
            //          List<string> temp = userRules[i].Rename(listOfFileName, 1);
            //List<string> temp2 = userRules[i].Rename(listOfFolderName);
            //           for (int j = 0; j < filelist.Count(); j++)
            //          {
            //              filelist[j].newfilename = temp[j];
            //              listOfFileName[j] = temp[j];
            //          }


            // for (int j = 0; j < folderlist.Count(); j++)
            //  {

            //      folderlist[i].newfoldername = temp2[j];
            //  }
            //       }
            //   }

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

            List<string> listOfFileName = new List<string>();

            for (int i = 0; i < filelist.Count(); i++)
            {
                listOfFileName.Add(filelist[i].filename);
                filelist[i].newfilename = filelist[i].filename;
            }

            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(listOfFileName, 1);
                    for (int j = 0; j < filelist.Count(); j++)
                    {
                        filelist[j].newfilename = temp[j];
                        listOfFileName[j] = temp[j];
                    }
                }
            }
        }

        private void Use_Rule_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;
            int index = userRules.IndexOf(rule);
            //code here

            List<string> listOfFileName = new List<string>();

            for (int i = 0; i < filelist.Count(); i++)
            {
                listOfFileName.Add(filelist[i].filename);
                filelist[i].newfilename = filelist[i].filename;
            }

            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(listOfFileName, 1);
                    for (int j = 0; j < filelist.Count(); j++)
                    {
                        filelist[j].newfilename = temp[j];
                        listOfFileName[j] = temp[j];
                    }
                }
            }


        }

        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = RuleComboBox.SelectedIndex;
            if (index >= 0 && index < allRules.Count)
            {
                userRules.Add(allRules[index].Clone());
                RuleComboBox.SelectedIndex = -1;
            }
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("RULE"))
            {
                IRule droppedData = e.Data.GetData("RULE") as IRule;
                IRule target = ((ListBoxItem)(sender)).DataContext as IRule;

                int removedIdx = RuleList.Items.IndexOf(droppedData);
                int targetIdx = RuleList.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    userRules.Insert(targetIdx + 1, droppedData);
                    userRules.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (userRules.Count + 1 > remIdx)
                    {
                        userRules.Insert(targetIdx, droppedData);
                        userRules.RemoveAt(remIdx);
                    }
                }
            }
        }

        private void ListBoxItem_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DataObject data = new DataObject("RULE", draggedItem.DataContext);
                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            double height = this.ActualHeight;
            double width = this.ActualWidth;
            double left = this.Left;
            double top = this.Top;
            if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
            {
                StoreToProject();
                currentProject.ProjectAddress = @"LastProject\Untitled.json";
            }
            lastProject = new LastProject()
            {
                Address = currentProject.ProjectAddress
            };
            lastProject.StoreData(LastProjectAddress);
            currentProject.StoreData(currentProject.ProjectAddress);
        }

        private void Open_Project_Btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files only (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                bool isParseAble = true;
                RenameProject project = null;
                try
                {
                    project = RenameProject.Parser(path);
                }
                catch (Exception)
                {
                    isParseAble = false;
                }
                if (isParseAble)
                {
                    if (project != null)
                    {
                        if (project.Rules != null && project.Folders != null && project.Files != null)
                        {
                            project.ProjectAddress = path;
                            this.Width = project.WindowWidth;
                            this.Height = project.WindowHeight;
                            this.Top = project.WindowTop;
                            this.Left = project.WindowLeft;
                            userRules = new BindingList<IRule>(project.GetRules());
                            filelist = new BindingList<MyFile>(project.Files);
                            folderlist = new BindingList<Folder>(project.Folders);
                            RuleList.ItemsSource = userRules;
                            FileList.ItemsSource = filelist;
                            FolderList.ItemsSource = folderlist;
                            currentProject = project;
                            this.Title = AppTitle + " - " + currentProject.GetName();
                        }
                        else
                        {
                            HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                            {
                                Message = "Invalid file content",
                                Caption = "Open File Error",
                                Button = MessageBoxButton.OK,
                                IconBrushKey = ResourceToken.AccentBrush,
                                IconKey = ResourceToken.ErrorGeometry,
                                StyleKey = "MessageBoxCustom"
                            });
                        }
                    }
                    else
                    {
                        HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                        {
                            Message = "File Not Found",
                            Caption = "Open File Error",
                            Button = MessageBoxButton.OK,
                            IconBrushKey = ResourceToken.AccentBrush,
                            IconKey = ResourceToken.ErrorGeometry,
                            StyleKey = "MessageBoxCustom"
                        });
                    }
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "Invalid file content",
                        Caption = "Open File Error",
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.AccentBrush,
                        IconKey = ResourceToken.ErrorGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                }
            }
        }

        private void Save_Project_Btn_Click(object sender, RoutedEventArgs e)
        {
            double height = this.ActualHeight;
            double width = this.ActualWidth;
            double left = this.Left;
            double top = this.Top;
            

            if (currentProject.ProjectAddress != null && currentProject.ProjectAddress.Length > 0)
            {
                StoreToProject();
            }
            else
            {
                StoreToProject();
                currentProject.ProjectAddress = @"LastProject\Untitled.json";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = currentProject.GetName();
                saveFileDialog.DefaultExt = ".json";
                saveFileDialog.Filter = "JSON files(*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    currentProject.ProjectAddress = path;

                }
            }
            currentProject.StoreData(currentProject.ProjectAddress);
        }

        private void New_Project_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_As_Project_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}