﻿using System;
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
using System.Diagnostics;
using System.Reflection;


namespace batchRenameApp
{
  
    public class Preset
    {
        public string PresetName { get; set; }
        public List<IRule> PresetRules { get; set; }
        public Preset()
        {
            PresetName = "";
            PresetRules = null;
        }
        public Preset(string name, List<IRule> rules)
        {
            PresetName = name;
            PresetRules = rules;
        }
    }
    public partial class MainWindow : Window
    {
        int currentfilepage = 1;
        int currentfolderpage = 1;
        int itemperpage = 6;
        int totalRule = 0;
        List<IRule> allRules = new List<IRule>();
        List<String> allRulesName = new List<String>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<MyFile> datafilelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();

        int unnamedPreset = 0;

        String LastProjectAddress = @"LastProject\LastProject.json";
        String DefaultProjectAddress = @"LastProject\Untitled.json";
        String AppTitle = "Batch Rename";
        RenameProject currentProject = null;
        LastProject lastProject = null;
        int totalPreset = 0;
        List<Preset> presets = new List<Preset>();


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

        private void InitProject()
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
            this.Title = AppTitle + " - " + currentProject.GetName();
            RuleList.ItemsSource = userRules;
        }

        //How to use:
        //StoreRules(userRules, @"D:\JSON\path.json");
        private void StoreRules(BindingList<IRule> rules, string path)
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
        //How to use:
        //List<IRule> rules = ReadRules(@"D:\JSON\path.json");
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
                if (Directory.Exists(filedir))
                {
                    string[] InsideFilesList = Directory.GetFiles(filedir, "*", SearchOption.AllDirectories);
                    foreach (var item in InsideFilesList)
                    {
                        MyFile newfile = new MyFile(item);
                        newfile.fileimage = "images/file.png";
                        filelist.Add(newfile);
                        FileList.ItemsSource = filelist;
                    }

                }
                else
                {
                    MyFile newfile = new MyFile(filedir);
                    newfile.fileimage = "images/file.png";
                    filelist.Add(newfile);
                    FileList.ItemsSource = filelist;
                }
            }
            update_Filepage();
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
            update_Folderpage();
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

        private void CreateJSONFolder()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\JSON";
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folderPath);
            }

        }

        private void CreateLastProjectFolder()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\LastProject";
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folderPath);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateJSONFolder();
            CreateLastProjectFolder();
            FilePagination.MaxPageCount = (int)Math.Ceiling(filelist.Count() * 1.0 / 6);
            FolderPagination.MaxPageCount = (int)Math.Ceiling(folderlist.Count() * 1.0 / 6);

            //get all rule from DLL
            totalRule = RuleFactory.GetInstance().RuleAmount();
            for (int i = 0; i < totalRule; i++)
            {
                allRules.Add(RuleFactory.GetInstance().Create(i));
                allRulesName.Add(allRules[i].GetName());
            }
            RuleComboBox.ItemsSource = allRulesName;

           
            //preset
            DirectoryInfo d = new DirectoryInfo(@"D:\PRESET");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                List<IRule> rulesInPreset = ReadRules($@"D:\PRESET\{file.Name}");

                string[] tokens = file.Name.Split(".");
                string filename = "";
                for(int i = 0; i < tokens.Length - 2; i++)
                {
                    filename += tokens[i] + ".";
                }
                filename += tokens[tokens.Length - 2];
                Preset ps = new Preset(filename, rulesInPreset);
                presets.Add(ps);
                PresetComboBox.Items.Add(filename);
            }    
                
            bool isParseAble = true;
            try
            {
                lastProject = LastProject.Parse(LastProjectAddress);
            }
            catch (Exception)
            {
                lastProject = null;
            }

            if (lastProject != null)
            {
               
                RenameProject project = null;
                try
                {
                    project = RenameProject.Parser(lastProject.Address);
                }
                catch (Exception)
                {
                    isParseAble = false;
                }
                if (isParseAble && project != null && project.Rules != null && project.Folders != null && project.Files != null)
                {
                    project.ProjectAddress = lastProject.Address;
                    currentProject = project;
                }
                else
                {
                    currentProject = new RenameProject();
                }
            }
            else
            {
                currentProject = new RenameProject();
            }

            InitProject();
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

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                addFolder(dialog.FileName);
        }

        private void DropFolderList(object sender, DragEventArgs e)
        {
            string[] droppedFoldernames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string foldername in droppedFoldernames)
            {
                if (Directory.Exists(foldername))
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
                    if (!Directory.Exists(foldername))
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
                filelist[i].changeName();
            }

            for (int i = 0; i < folderlist.Count(); i++)
            {
                folderlist[i].changeName();
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
            List<string> listOfFolderName = new List<string>();

            for (int i = 0; i < filelist.Count(); i++)
            {
                listOfFileName.Add(filelist[i].filename);
                filelist[i].newfilename = filelist[i].filename;
            }

            for (int i = 0; i < folderlist.Count(); i++)
            {
                listOfFolderName.Add(folderlist[i].foldername);
                folderlist[i].newfoldername = folderlist[i].foldername;
            }

            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(listOfFileName, 1);

                    List<string> temp2 = userRules[i].Rename(listOfFolderName, 2);
                    for (int j = 0; j < filelist.Count(); j++)
                    {
                        filelist[j].newfilename = temp[j];
                        listOfFileName[j] = temp[j];
                    }

                    for (int j = 0; j < folderlist.Count(); j++)
                    {
                        folderlist[j].newfoldername = temp2[j];
                        listOfFolderName[j] = temp2[j];
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
            List<string> listOfFolderName = new List<string>();

            for (int i = 0; i < filelist.Count(); i++)
            {
                listOfFileName.Add(filelist[i].filename);
                filelist[i].newfilename = filelist[i].filename;
            }

            for (int i = 0; i < folderlist.Count(); i++)
            {
                listOfFolderName.Add(folderlist[i].foldername);
                folderlist[i].newfoldername = folderlist[i].foldername;
            }

            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(listOfFileName, 1);

                    List<string> temp2 = userRules[i].Rename(listOfFolderName, 2);
                    for (int j = 0; j < filelist.Count(); j++)
                    {
                        filelist[j].newfilename = temp[j];
                        listOfFileName[j] = temp[j];
                    }

                    for (int j = 0; j < folderlist.Count(); j++)
                    {
                        folderlist[j].newfoldername = temp2[j];
                        listOfFolderName[j] = temp2[j];
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
            if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
            {
                StoreToProject();
                currentProject.ProjectAddress = DefaultProjectAddress;
            }
            lastProject = new LastProject()
            {
                Address = currentProject.ProjectAddress
            };
            lastProject.StoreData(LastProjectAddress);
            currentProject.StoreData(currentProject.ProjectAddress);
            for (int i = 1; i <= totalPreset; i++)
            {
                File.Delete($@"D:\JSON\preset{i}.json");
            }
        }

        private void Open_Project_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (!currentProject.isDefaul())
            {
                if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
                {
                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "Do you want to save the currrent project",
                        Caption = "Save Project",
                        Button = MessageBoxButton.YesNo,
                        IconBrushKey = ResourceToken.AccentBrush,
                        IconKey = ResourceToken.AskGeometry,
                        StyleKey = "MessageBoxCustom"
                    });

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            StoreToProject();
                            currentProject.ProjectAddress = DefaultProjectAddress;
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.FileName = currentProject.GetName();
                            saveFileDialog.DefaultExt = ".json";
                            saveFileDialog.Filter = "JSON files(*.json)|*.json";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                string path = saveFileDialog.FileName;
                                currentProject.ProjectAddress = path;
                            }
                            currentProject.StoreData(currentProject.ProjectAddress);
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    StoreToProject();
                    currentProject.StoreData(currentProject.ProjectAddress);
                }
            }
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
                            
                            currentProject = project;
                            InitProject();
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
            if (currentProject.ProjectAddress != null && currentProject.ProjectAddress.Length > 0)
            {
                StoreToProject();
            }
            else
            {
                StoreToProject();
                currentProject.ProjectAddress = DefaultProjectAddress;
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
            if (currentProject.isDefaul())
            {
                return;
            }
            if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
            {
                MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Do you want to save the currrent project",
                    Caption = "Save Project",
                    Button = MessageBoxButton.YesNo,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.AskGeometry,
                    StyleKey = "MessageBoxCustom"
                });

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        StoreToProject();
                        currentProject.ProjectAddress = DefaultProjectAddress;
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.FileName = currentProject.GetName();
                        saveFileDialog.DefaultExt = ".json";
                        saveFileDialog.Filter = "JSON files(*.json)|*.json";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string path = saveFileDialog.FileName;
                            currentProject.ProjectAddress = path;
                        }
                        currentProject.StoreData(currentProject.ProjectAddress);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                StoreToProject();
                currentProject.StoreData(currentProject.ProjectAddress);
            }
            currentProject = new RenameProject();
            InitProject();
            
        }

        private void Save_As_Project_Btn_Click(object sender, RoutedEventArgs e)
        {
            StoreToProject();
            currentProject.ProjectAddress = DefaultProjectAddress;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = currentProject.GetName();
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "JSON files(*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                currentProject.ProjectAddress = path;
            }
            InitProject();
            currentProject.StoreData(currentProject.ProjectAddress);

        }

        private void Clear_All_Rule_Btn_Click(object sender, RoutedEventArgs e)
        {
            userRules.Clear();
        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            int index = PresetComboBox.SelectedIndex;
            if(index != -1)
            {
                userRules = new BindingList<IRule>();
                foreach (var rule in presets[index].PresetRules)
                {
                    userRules.Add(rule.Clone());
                }
                //RuleList.Items.Clear();
                RuleList.ItemsSource = userRules;
            }   
           
        }

       
        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {
          //save preset
            if (presetNameInput.Text == "")
            {
                unnamedPreset++;
                string presetName = "";
                foreach (var item in userRules)
                {
                    presetName += item.GetName() + unnamedPreset.ToString() + " ";
                }
                StoreRules(userRules, $@"D:\JSON\{presetName}.json");
                List<IRule> rulesInPreset = ReadRules($@"D:\JSON\{presetName}.json");
                Preset ps = new Preset(presetName, rulesInPreset);

                presets.Add(ps);
                PresetComboBox.Items.Add(presetName);
                MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Save preset succeeded",
                    Caption = "Save Preset",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.AskGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            else
            {
                string presetNameInput = this.presetNameInput.Text;
                List<IRule> rulesInPreset = new List<IRule>();
                Preset ps = new Preset();
                int indexDuplicate = -1;
                
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].PresetName == presetNameInput)
                    {
                        indexDuplicate = i;
                        MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                        {
                            Message = $"Preset {presetNameInput} already exits. Do you want to replace it?",
                            Caption = "Save Preset",
                            Button = MessageBoxButton.YesNo,
                            IconBrushKey = ResourceToken.AccentBrush,
                            IconKey = ResourceToken.AskGeometry,
                            StyleKey = "MessageBoxCustom"
                        });
                        switch (result)
                        {
                            case MessageBoxResult.Yes: //save and replace preset json
                                PresetComboBox.Items.Remove(presetNameInput);
                                presets.RemoveAt(indexDuplicate);
                                StoreRules(userRules, $@"D:\PRESET\{presetNameInput}.json");
                                rulesInPreset = ReadRules($@"D:\PRESET\{presetNameInput}.json");
                                ps.PresetName = presetNameInput;
                                ps.PresetRules = rulesInPreset;
                                presets.Add(ps);
                                PresetComboBox.Items.Add(presetNameInput);
                                break;
                            case MessageBoxResult.No: //cancle save
                                break;
                        }
                        break;
                    }    
                }
                if(indexDuplicate == -1)
                {
                    StoreRules(userRules, $@"D:\PRESET\{presetNameInput}.json");
                    rulesInPreset = ReadRules($@"D:\PRESET\{presetNameInput}.json");
                    ps.PresetName = presetNameInput;
                    ps.PresetRules = rulesInPreset;
                    presets.Add(ps);
                    PresetComboBox.Items.Add(presetNameInput);
                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "Save preset succeeded!",
                        Caption = "Save Preset",
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.DarkSuccessBrush,
                        IconKey = ResourceToken.CheckedGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                }    
            }
        }
        
        private void openInFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = (currentfilepage - 1) * itemperpage + FileList.SelectedIndex;

            Process.Start("explorer.exe", filelist[selectedfile].filepath.Substring(0, filelist[selectedfile].filepath.LastIndexOf(@"\") + 1));
        }

        private void deleteFileMenu_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = (currentfilepage - 1) * itemperpage + FileList.SelectedIndex;
            if (selectedfile >= 0)
            {
                filelist.Remove(filelist[selectedfile]);
            }
            update_Filepage();
        }

        private void deleteFolderMenu_Click(object sender, RoutedEventArgs e)
        {
            int selectedfolder = (currentfolderpage - 1) * itemperpage + FolderList.SelectedIndex;
            if (selectedfolder >= 0)
            {
                folderlist.Remove(folderlist[selectedfolder]);
            }
            update_Folderpage();
        }

        private void openInFolderExplorer_Click(object sender, RoutedEventArgs e)
        {
            int selectedfolder = (currentfolderpage - 1) * itemperpage + FolderList.SelectedIndex;

            Process.Start("explorer.exe", folderlist[selectedfolder].folderpath);
        }

        private void ClearAllFile_Click(object sender, RoutedEventArgs e)
        {
            filelist.Clear();
            update_Filepage();
        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {
            folderlist.Clear();
            update_Folderpage();
        }


        private void update_Filepage(){
            FilePagination.MaxPageCount = (int)Math.Ceiling(filelist.Count()*1.0/6);
            IEnumerable<MyFile> datafilelist = filelist.Skip((currentfilepage - 1) * itemperpage).Take(itemperpage);
            FileList.ItemsSource = datafilelist;
        }


        private void update_Folderpage(){
            FolderPagination.MaxPageCount = (int)Math.Ceiling(folderlist.Count() * 1.0 / 6);
            IEnumerable<Folder> datafolderlist = folderlist.Skip((currentfolderpage - 1) * itemperpage).Take(itemperpage);
            FolderList.ItemsSource = datafolderlist;
        }

        private void page_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            currentfilepage = e.Info;
            update_Filepage();
        }

        private void page_FolderPageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            currentfolderpage = e.Info;
            update_Folderpage();
        }

        private void Clear_All_Preset_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            presets.Clear();
            PresetComboBox.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@"D:\PRESET");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                file.Delete();
            }
            PresetComboBox.SelectedIndex = -1;
        }
    }
}