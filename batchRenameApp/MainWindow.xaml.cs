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
using System.Diagnostics;
using System.Reflection;

namespace batchRenameApp
{
    public class AppState
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double WindowLeft { get; set; }
        public double WindowTop { get; set; }

        public List<RuleContainer> Rules { get; set; }
        public List<MyFile> Files { get; set; }
        public List<Folder> Folders { get; set; }

        
        public AppState()
        {
            WindowWidth = 1315;
            WindowHeight = 580;
            WindowLeft = 0;
            WindowTop = 0;
            Rules = null;
            Folders = null;
        }


        public void StoreData(string path)
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(path, json);
        }

        public static AppState Parser(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return null;
            }
            string outJson = File.ReadAllText(path);
            AppState result = (AppState)JsonSerializer.Deserialize(outJson, typeof(AppState));
            return result;
        }

        public List<IRule> GetRules()
        {
            List<IRule> resultRules = new List<IRule>();
            for (int i = 0; i < Rules.Count(); i++)
            {
                resultRules.Add(RuleFactory.GetInstance().Create(Rules[i]));
            }

            return resultRules;
        }
    }

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
        int totalPreset = 0;
        List<Preset> presets = new List<Preset>();
        private List<MyFile> datafilelist1;

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
        private void addFile(string filedir) {
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateJSONFolder();
            AppState lastState = AppState.Parser(@"D:\JSON\path.json");
            if (lastState != null)
            {
                this.Width = lastState.WindowWidth;
                this.Height = lastState.WindowHeight;
                this.Top = lastState.WindowTop;
                this.Left = lastState.WindowLeft;
                userRules = new BindingList<IRule>(lastState.GetRules());
                filelist = new BindingList<MyFile>(lastState.Files);
                folderlist = new BindingList<Folder>(lastState.Folders);
                FileList.ItemsSource = filelist;
                FolderList.ItemsSource = folderlist;
                FilePagination.MaxPageCount = (int)Math.Ceiling(filelist.Count() * 1.0 / 6);
            }
            //get all rule from DLL
            totalRule = RuleFactory.GetInstance().RuleAmount();
            for (int i = 0; i < totalRule; i++)
            {
                allRules.Add(RuleFactory.GetInstance().Create(i));
                allRulesName.Add(allRules[i].GetName());
            }
            RuleComboBox.ItemsSource = allRulesName;
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
            if(index >= 0 && index < allRules.Count)
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
            List<RuleContainer> ruleContainers = new List<RuleContainer>();
            for (int i = 0; i < userRules.Count(); i++)
            {
                ruleContainers.Add(new RuleContainer()
                {
                    Name = userRules[i].GetName(),
                    Data = userRules[i].ToJson()
                });
            }
            AppState state = new AppState()
            {
                WindowHeight = height,
                WindowWidth = width,
                WindowLeft = left,
                WindowTop = top,
                Rules = ruleContainers,
                Files = filelist.ToList(),
                Folders = folderlist.ToList()
            };

            state.StoreData(@"D:\JSON\path.json");
            for (int i = 1; i <= totalPreset; i++)
            {
                File.Delete($@"D:\JSON\preset{i}.json");
            }
            //MessageBox.Show("Closed called");
        }

        private void Clear_All_Rule_Btn_Click(object sender, RoutedEventArgs e)
        {
            userRules.Clear();
        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = PresetComboBox.SelectedIndex;
            userRules = new BindingList<IRule>(presets[index].PresetRules);
            //RuleList.Items.Clear();
            RuleList.ItemsSource = userRules;
        }

       
        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {
            //save preset
            totalPreset++;
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            StoreRules(userRules, folder + $@"\JSON\preset{totalPreset}.json");


            List<IRule> preset = ReadRules(folder + $@"\JSON\preset{totalPreset}.json");
            string presetName = "";
            foreach (var item in preset)
            {
                presetName += item.GetName() + totalPreset.ToString() + " ";
            }
            Preset ps = new Preset(presetName, preset);


            presets.Add(ps);
            PresetComboBox.Items.Add(presetName);
        }
        private void openInFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = (currentfilepage - 1) * itemperpage + FileList.SelectedIndex;

            Process.Start("explorer.exe", filelist[selectedfile].filepath.Substring(0, filelist[selectedfile].filepath.LastIndexOf(@"\")+1));
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
    }
}