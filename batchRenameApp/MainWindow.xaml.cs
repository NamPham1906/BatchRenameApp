using Contract;
using HandyControl.Data;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace batchRenameApp
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public int currentfilepage { get; set; }

        int currentfolderpage = 1;
        int itemperpage = 7;
        int totalRule = 0;
        int autoSaveTime = 1;
        bool IsAutoSave = true;
        List<IRule> allRules = new List<IRule>();
        List<String> allRulesName = new List<String>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<MyFile> datafilelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();
        DispatcherTimer dispatcherTimer = null;
        int startTimerSecond = 0;
        String LastProjectAddress = @"LastProject\lastprojectaddress.json";
        String DefaultProjectAddress = @"LastProject\Untitled.json";
        String AppTitle = "Batch Rename";
        RenameProject currentProject = null;
        LastProject lastProject = null;
        List<Preset> presets = new List<Preset>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void StoreToProject()
        {
            double height = this.ActualHeight;
            double width = this.ActualWidth;
            double left = this.Left;
            double top = this.Top;
            currentProject.CurrentFilePage = currentfilepage;
            currentProject.CurrentFolderPage = currentfolderpage;
            int presetIndex = PresetComboBox.SelectedIndex;
            if(presetIndex <= 0)
            {
                currentProject.PresetName = "";
            }
            else
            {
                currentProject.PresetName = presets[presetIndex].PresetName;
            }
            List<RuleContainer> ruleContainers = new List<RuleContainer>();
            for (int i = 0; i < userRules.Count(); i++)
            {
                ruleContainers.Add(new RuleContainer()
                {
                    Name = userRules[i].GetName(),
                    Data = userRules[i].ToJson()
                });
            }
            int selectedPreset = PresetComboBox.SelectedIndex;
            if(selectedPreset >= 0)
            {
                currentProject.PresetName = presets[selectedPreset].PresetName;
            }
            else
            {
                currentProject.PresetName = "";
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
            currentfilepage = currentProject.CurrentFilePage;
            currentfolderpage = currentProject.CurrentFolderPage;
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
            if (currentProject.PresetName.Length > 0)
            {
                int n = presets.Count();
                bool isFound = false;
                for (int i = 0; i < n; i++)
                {
                    if (presets[i].PresetName == currentProject.PresetName)
                    {
                        PresetComboBox.SelectedIndex = i;
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    Preset ps = new Preset()
                    {
                        PresetName = currentProject.PresetName,
                        PresetRules = currentProject.GetRules()
                    };
                    presets.Add(ps);
                    PresetComboBox.Items.Add(ps.PresetName);
                    PresetComboBox.SelectedIndex = PresetComboBox.Items.Count - 1;

                }
            }
            else
            {
                PresetComboBox.SelectedIndex = -1;
            }

            userRules = new BindingList<IRule>(currentProject.GetRules());
            filelist = new BindingList<MyFile>(currentProject.Files);
            folderlist = new BindingList<Folder>(currentProject.Folders);
            FileList.ItemsSource = filelist;
            FolderList.ItemsSource = folderlist;
            RuleList.ItemsSource = userRules;
            FilePagination.PageIndex = currentfilepage;
            FolderPagination.PageIndex = currentfolderpage;
            presetNameInput.Text = currentProject.PresetName;
            NumberOfFiles.DataContext = filelist.Count();

            update_Filepage();
            update_Folderpage();
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
                        filelist.Add(newfile);
                        FileList.ItemsSource = filelist;
                    }

                }
                else
                {
                    MyFile newfile = new MyFile(filedir);
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
            if (droppedFilenames != null)
            {
                foreach (string filename in droppedFilenames)
                {

                    addFile(filename);
                }
            }
        }

        private void CreateDLLFolder()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\DLL";
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folderPath);
            }

        }

        private void CreatePresetFolder()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\PRESET";
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

        public void UpdatePreview()
        {
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDLLFolder();
            CreatePresetFolder();
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
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\PRESET";
            DirectoryInfo d = new DirectoryInfo(folderPath);
          
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                List<IRule> rulesInPreset = ReadRules($@"PRESET\{file.Name}");

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
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            if (IsAutoSave)
            {
                dispatcherTimer.Start();
            }
            
        }
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(startTimerSecond >= autoSaveTime*60)
            {
                startTimerSecond = 0;
                StoreToProject();
                if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
                {
                    currentProject.ProjectAddress = DefaultProjectAddress;
                }
                currentProject.StoreData(currentProject.ProjectAddress);
            }
            startTimerSecond++;
            CommandManager.InvalidateRequerySuggested();
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
            if (droppedFoldernames != null)
            {

                foreach (string foldername in droppedFoldernames)
                {
                    if (Directory.Exists(foldername))
                    {
                        addFolder(foldername);
                    }
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
            update_Filepage();

            for (int i = 0; i < folderlist.Count(); i++)
            {
                folderlist[i].changeName();
            }
            update_Folderpage();
        }


        //private void RuleList_LayoutUpdated(object sender, EventArgs e)
        //{
        //    // List<string> listOfFileName = new List<string>();
        //    // List<string> listOfFolderName = new List<string>();

        //    //  for (int i=0; i < filelist.Count(); i++)
        //    //  {
        //    //       listOfFileName.Add(filelist[i].filename);
        //    //       filelist[i].newfilename = filelist[i].filename;
        //    //   }

        //    // for (int i = 0; i < folderlist.Count(); i++)
        //    // {
        //    //     listOfFolderName.Add(folderlist[i].foldername);
        //    //     folderlist[i].newfoldername = folderlist[i].foldername;
        //    // }

        //    //   for (int i = 0; i < userRules.Count(); i++)
        //    //  {
        //    //      if (userRules[i].IsUse())
        //    //       {
        //    //          List<string> temp = userRules[i].Rename(listOfFileName, 1);
        //    //List<string> temp2 = userRules[i].Rename(listOfFolderName);
        //    //           for (int j = 0; j < filelist.Count(); j++)
        //    //          {
        //    //              filelist[j].newfilename = temp[j];
        //    //              listOfFileName[j] = temp[j];
        //    //          }


        //    // for (int j = 0; j < folderlist.Count(); j++)
        //    //  {

        //    //      folderlist[i].newfoldername = temp2[j];
        //    //  }
        //    //       }
        //    //   }

        //}

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
            UpdatePreview();
        }

        private void Use_Rule_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;
            int index = userRules.IndexOf(rule);
            //code here
            UpdatePreview();
            //List<string> listOfFileName = new List<string>();
            //List<string> listOfFolderName = new List<string>();

            //for (int i = 0; i < filelist.Count(); i++)
            //{
            //    listOfFileName.Add(filelist[i].filename);
            //    filelist[i].newfilename = filelist[i].filename;
            //}

            //for (int i = 0; i < folderlist.Count(); i++)
            //{
            //    listOfFolderName.Add(folderlist[i].foldername);
            //    folderlist[i].newfoldername = folderlist[i].foldername;
            //}

            //for (int i = 0; i < userRules.Count(); i++)
            //{
            //    if (userRules[i].IsUse())
            //    {
            //        List<string> temp = userRules[i].Rename(listOfFileName, 1);

            //        List<string> temp2 = userRules[i].Rename(listOfFolderName, 2);
            //        for (int j = 0; j < filelist.Count(); j++)
            //        {
            //            filelist[j].newfilename = temp[j];
            //            listOfFileName[j] = temp[j];
            //        }

            //        for (int j = 0; j < folderlist.Count(); j++)
            //        {
            //            folderlist[j].newfoldername = temp2[j];
            //            listOfFolderName[j] = temp2[j];
            //        }
            //    }
            //}
        }
        bool isSingleUncheck = false;
        private void Use_Rule_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;
            int index = userRules.IndexOf(rule);
            //code here
            UpdatePreview();
            if (!isAllRuleUnCheck)
            {
                isSingleUncheck = true;
                All_Rule.IsChecked = false;
                isSingleUncheck = false;
            }
            //List<string> listOfFileName = new List<string>();
            //List<string> listOfFolderName = new List<string>();

            //for (int i = 0; i < filelist.Count(); i++)
            //{
            //    listOfFileName.Add(filelist[i].filename);
            //    filelist[i].newfilename = filelist[i].filename;
            //}

            //for (int i = 0; i < folderlist.Count(); i++)
            //{
            //    listOfFolderName.Add(folderlist[i].foldername);
            //    folderlist[i].newfoldername = folderlist[i].foldername;
            //}

            //for (int i = 0; i < userRules.Count(); i++)
            //{
            //    if (userRules[i].IsUse())
            //    {
            //        List<string> temp = userRules[i].Rename(listOfFileName, 1);

            //        List<string> temp2 = userRules[i].Rename(listOfFolderName, 2);
            //        for (int j = 0; j < filelist.Count(); j++)
            //        {
            //            filelist[j].newfilename = temp[j];
            //            listOfFileName[j] = temp[j];
            //        }

            //        for (int j = 0; j < folderlist.Count(); j++)
            //        {
            //            folderlist[j].newfoldername = temp2[j];
            //            listOfFolderName[j] = temp2[j];
            //        }
            //    }
            //}
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
                UpdatePreview();
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
            StoreToProject();
            if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0)
            {
                currentProject.ProjectAddress = DefaultProjectAddress;
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
            if (!currentProject.isDefault())
            {
                if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0 ||  currentProject.ProjectAddress == DefaultProjectAddress)
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
            if (currentProject.ProjectAddress != null && currentProject.ProjectAddress.Length > 0 && currentProject.ProjectAddress != DefaultProjectAddress)
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
            this.Title = AppTitle + " - " + currentProject.GetName();
        }

        private void New_Project_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (currentProject.isDefault())
            {
                return;
            }
            if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0 || currentProject.ProjectAddress == DefaultProjectAddress)
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
            UpdatePreview();
        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            int index = PresetComboBox.SelectedIndex;
            if(index != -1)
            {
                presetNameInput.Text = presets[index].PresetName;
                userRules = new BindingList<IRule>();
                foreach (var rule in presets[index].PresetRules)
                {
                    if (rule!=null) userRules.Add(rule.Clone());
                }
                //RuleList.Items.Clear();
                StoreToProject();
                RuleList.ItemsSource = userRules;
                UpdatePreview();
            }
        }

       
        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {
          //save preset
            if (presetNameInput.Text == "")
            {
                MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "You must enter the preset name",
                    Caption = "Save Preset",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.WarningGeometry,
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
                                StoreRules(userRules, $@"PRESET\{presetNameInput}.json");
                                rulesInPreset = ReadRules($@"PRESET\{presetNameInput}.json");
                                ps.PresetName = presetNameInput;
                                ps.PresetRules = rulesInPreset;
                                presets.Add(ps);
                                PresetComboBox.Items.Add(presetNameInput);
                                PresetComboBox.SelectedIndex = PresetComboBox.Items.Count - 1;
                                break;
                            case MessageBoxResult.No: //cancle save
                                break;
                        }
                        break;
                    }    
                }
                if(indexDuplicate == -1)
                {
                    StoreRules(userRules, $@"PRESET\{presetNameInput}.json");
                    rulesInPreset = ReadRules($@"PRESET\{presetNameInput}.json");
                    ps.PresetName = presetNameInput;
                    ps.PresetRules = rulesInPreset;
                    presets.Add(ps);
                    PresetComboBox.Items.Add(presetNameInput);
                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "Save preset succeeded!",
                        Caption = "Save Preset",
                        
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.SuccessBrush,
                        IconKey = ResourceToken.SuccessGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                    PresetComboBox.SelectedIndex = PresetComboBox.Items.Count - 1;
                }
            }
        }
        
        private void openInFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = (currentfilepage - 1) * itemperpage + FileList.SelectedIndex;

            Process.Start("explorer.exe", "/select, \"" + filelist[selectedfile].filepath + "\"");
        }

        private void openThisFile_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = (currentfilepage - 1) * itemperpage + FileList.SelectedIndex;

            Process.Start("explorer.exe", filelist[selectedfile].filepath);
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
            if(filelist.Count == 0)
            {
                return;
            }    
            MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
            {
                Message = "Do you want to clear all current files?",
                Caption = "Clear All Files",
                Button = MessageBoxButton.YesNo,
                IconBrushKey = ResourceToken.AccentBrush,
                IconKey = ResourceToken.AskGeometry,
                StyleKey = "MessageBoxCustom"
            });

            switch (result)
            {
                case MessageBoxResult.Yes:
                    filelist.Clear();
                    update_Filepage();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            
        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {
            if (folderlist.Count == 0)
            {
                return;
            }
            MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
            {
                Message = "Do you want to clear all current folders?",
                Caption = "Clear All Folders",
                Button = MessageBoxButton.YesNo,
                IconBrushKey = ResourceToken.AccentBrush,
                IconKey = ResourceToken.AskGeometry,
                StyleKey = "MessageBoxCustom"
            });

            switch (result)
            {
                case MessageBoxResult.Yes:
                    folderlist.Clear();
                    update_Folderpage();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            
        }


        private void update_Filepage(){
            FilePagination.MaxPageCount = (int)Math.Ceiling(filelist.Count()*1.0/itemperpage);
            IEnumerable<MyFile> datafilelist = filelist.Skip((currentfilepage - 1) * itemperpage).Take(itemperpage);
            FileList.ItemsSource = datafilelist;
           
            int batchingSuccess = 0;
            int batchingError = 0;
            for (int i = 0; i< filelist.Count(); i++)
            {
                if (filelist[i].status.Contains("Error")){
                    batchingError++;
                }
                else if (filelist[i].status.Contains("Batching successfully")){
                    batchingSuccess++;
                }
            }
            NumberOfFiles.DataContext = filelist.Count();
            NumberOfBatchingFiles.DataContext = batchingSuccess;
            NumberOfErrorFiles.DataContext = batchingError;
        }


        private void update_Folderpage(){
            FolderPagination.MaxPageCount = (int)Math.Ceiling(folderlist.Count() * 1.0 / itemperpage);
            IEnumerable<Folder> datafolderlist = folderlist.Skip((currentfolderpage - 1) * itemperpage).Take(itemperpage);
            FolderList.ItemsSource = datafolderlist;

            int batchingSuccess = 0;
            int batchingError = 0;
            for (int i = 0; i < folderlist.Count(); i++)
            {
                if (folderlist[i].status.Contains("Error"))
                {
                    batchingError++;
                }
                else if (folderlist[i].status.Contains("Batching successfully"))
                {
                    batchingSuccess++;
                }
            }
            NumberOfFolders.DataContext = folderlist.Count();
            NumberOfBatchingFolders.DataContext = batchingSuccess;
            NumberOfErrorFolders.DataContext = batchingError;
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
            presetNameInput.Text = "";
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folderPath = Path.GetDirectoryName(exePath);
            folderPath += @"\PRESET";
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                file.Delete();
            }
            PresetComboBox.SelectedIndex = -1;
        }

     

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FILE"))
            {
                MyFile droppedData = e.Data.GetData("FILE") as MyFile;
                MyFile target = ((ListViewItem)(sender)).DataContext as MyFile;

                int removedIdx =filelist.IndexOf(droppedData);
                int targetIdx = filelist.IndexOf(target);
                if (removedIdx<0 || targetIdx<0||removedIdx>filelist.Count || targetIdx>filelist.Count)
                {
                    return;
                } else if (removedIdx < targetIdx)
                {
                    filelist.Insert(targetIdx + 1, droppedData);
                    filelist.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (filelist.Count + 1 > remIdx)
                    {
                        filelist.Insert(targetIdx, droppedData);
                        filelist.RemoveAt(remIdx);
                    }
                }
            }
            update_Filepage();
        }

        private void FileList_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListViewItem)
            {
                ListViewItem draggedItem = sender as ListViewItem;
                DataObject data = new DataObject("FILE", draggedItem.DataContext);
                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void DragOverFilePage(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {

           
        }

        private void DragOverFilePage(object sender, DragEventArgs e)
        {
            System.Windows.Controls.Border item = e.OriginalSource as System.Windows.Controls.Border;
            bool isDragable = true;
            if (item == null)
            {
                isDragable = false;
            }
            else
            {
                var s3 = item.Child as System.Windows.Controls.StackPanel;
                if (s3 == null)
                {
                    isDragable = false;
                }
                else
                {
                    var s4 = s3.TemplatedParent as System.Windows.Controls.RadioButton;
                    if (s4 == null)
                    {
                        isDragable = false;
                    }
                    else
                    {
                        string s5 = (string)s4.Content;
                        int newpage = Int32.Parse(s5);
                        if (newpage > 0 && newpage <= FilePagination.MaxPageCount)
                        {
                            currentfilepage = newpage;
                            FilePagination.PageIndex = newpage;
                            page_FilePageUpdated(this, new FunctionEventArgs<int>(newpage));
                            update_Filepage();
                        }
                        else
                        {
                            isDragable = false;
                        }
                    }
                    
                }
            }
 
            if (!isDragable)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
 
        }

        private void page_FilePageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            currentfilepage = e.Info;
            update_Filepage();
        }


        private void CutContractKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (currentProject.ProjectAddress != null && currentProject.ProjectAddress.Length > 0 && currentProject.ProjectAddress != DefaultProjectAddress)
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
                    saveFileDialog.Filter = "JSON files(.json)|.json";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string path = saveFileDialog.FileName;
                        currentProject.ProjectAddress = path;

                    }
                }
                currentProject.StoreData(currentProject.ProjectAddress);
                this.Title = AppTitle + " - " + currentProject.GetName();
            } 
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (currentProject.isDefault())
                {
                    return;
                }
                if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0 || currentProject.ProjectAddress == DefaultProjectAddress)
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
            else if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (!currentProject.isDefault())
                {
                    if (currentProject.ProjectAddress == null || currentProject.ProjectAddress.Length <= 0 || currentProject.ProjectAddress == DefaultProjectAddress)
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
        }



        private void Auto_Save_Check_Checked(object sender, RoutedEventArgs e)
        {
            IsAutoSave = (bool)Auto_Save_Check.IsChecked;
            startTimerSecond = 0;
            if (IsAutoSave)
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Start();
                }
            }
            else
            {
                if(dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                }
            }
        }

        private void Auto_Save_Check_Unchecked(object sender, RoutedEventArgs e)
        {
            IsAutoSave = (bool)Auto_Save_Check.IsChecked;
            startTimerSecond = 0;
            if (IsAutoSave)
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Start();
                }
            }
            else
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                }
            }
        }
        private void FolderList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem)
            {
                ListViewItem draggedItem = sender as ListViewItem;
                DataObject data = new DataObject("FOLDER", draggedItem.DataContext);
                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void FolderList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FOLDER"))
            {
                Folder droppedData = e.Data.GetData("FOLDER") as Folder;
                Folder target = ((ListViewItem)(sender)).DataContext as Folder;

                int removedIdx = folderlist.IndexOf(droppedData);
                int targetIdx = folderlist.IndexOf(target);
                if (removedIdx < 0 || targetIdx < 0 || removedIdx > folderlist.Count || targetIdx > folderlist.Count)
                {
                    return;
                }
                else if (removedIdx < targetIdx)
                {
                    folderlist.Insert(targetIdx + 1, droppedData);
                    folderlist.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (folderlist.Count + 1 > remIdx)
                    {
                        folderlist.Insert(targetIdx, droppedData);
                        folderlist.RemoveAt(remIdx);
                    }
                }
            }
            update_Folderpage();
        }

        private void DragOverFolderPage(object sender, DragEventArgs e)
        {
            System.Windows.Controls.Border item = e.OriginalSource as System.Windows.Controls.Border;
            bool isDragable = true;
            if (item == null)
            {
                isDragable = false;
            }
            else
            {
                var s3 = item.Child as System.Windows.Controls.StackPanel;
                if (s3 == null)
                {
                    isDragable = false;
                }
                else
                {
                    var s4 = s3.TemplatedParent as System.Windows.Controls.RadioButton;
                    if (s4 == null)
                    {
                        isDragable = false;
                    }
                    else
                    {
                        string s5 = (string)s4.Content;
                        int newpage = int.Parse(s5);
                        if (newpage > 0 && newpage <= FolderPagination.MaxPageCount)
                        {
                            currentfolderpage = newpage;
                            FolderPagination.PageIndex = newpage;
                            page_FolderPageUpdated(this, new FunctionEventArgs<int>(newpage));
                            update_Folderpage();
                        }
                        else
                        {
                            isDragable = false;
                        }
                    }

                }
            }

            if (!isDragable)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
        }

        private void StartBatchingToFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                for (int i = 0; i < filelist.Count(); i++)
                {
                    filelist[i].changeNameToFolder(dialog.FileName);
                }

                for (int i = 0; i < folderlist.Count(); i++)
                {
                    folderlist[i].changeNameToFolder(dialog.FileName);
                }
            }
                
        }

        bool isAllRuleUnCheck = false;
        private void All_Rule_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isSingleUncheck)
            {
                isSingleUncheck = false;
            }
            else
            {
                isAllRuleUnCheck = true;
                foreach (var rule in userRules)
                {
                    rule.SetIsUse(false);
                    //UpdatePreview();
                }
                isAllRuleUnCheck = false;
            }
        }

        private void All_Rule_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var rule in userRules)
            {
                rule.SetIsUse(true);
                //UpdatePreview();
            }
        }


        private void ClearAllNonExistingFilePath_Click(object sender, RoutedEventArgs e)
        {
            int n = filelist.Count() - 1;

            for (int i= n; i>=0; i--)
            {
        
                if(!filelist[i].checkExist()){
                    filelist.Remove(filelist[i]);
                }
            }
            update_Filepage();
        }

        private void ClearAllNonExistingFolderPath_Click(object sender, RoutedEventArgs e)
        {
            int n = folderlist.Count() - 1;

            for (int i = n; i >= 0; i--)
            {

                if (!folderlist[i].checkExist())
                {
                    folderlist.Remove(folderlist[i]);
                }
            }
            update_Folderpage();
        }

        private void Browse_Rule_Btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter= "DLL files only (*.dll)|*.dll";
            if (openFileDialog.ShowDialog() == true)
            {
                if (RuleFactory.GetInstance().AddRuleFromDLL(openFileDialog.FileName))
                {
                    totalRule = RuleFactory.GetInstance().RuleAmount();
                    allRules = new List<IRule>();
                    allRulesName = new List<string>();
                    for (int i = 0; i < totalRule; i++)
                    {
                        allRules.Add(RuleFactory.GetInstance().Create(i));
                        allRulesName.Add(allRules[i].GetName());
                    }
                    RuleComboBox.ItemsSource = allRulesName;

                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "The chosen rule was added successfully.",
                        Caption = "Add new rule success",
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.SuccessBrush,
                        IconKey = ResourceToken.SuccessGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                }
                else
                {
                    // xuat thong bao
                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = "The chosen rule can not be added correctly. Please try again.",
                        Caption = "Fail to add the new rule",
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.DangerBrush,
                        IconKey = ResourceToken.ErrorGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                }
               
               
            }
          
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = FileList.ActualHeight - 50;
            itemperpage = (int)height / 49;
            update_Folderpage();
            update_Filepage();
        }

    }
}