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
using HandyControl.Data;

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

        int totalRule = 0;
        List<IRule> allRules = new List<IRule>();
        List<String> allRulesName = new List<String>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        BindingList<MyFile> filelist = new BindingList<MyFile>();
        BindingList<Folder> folderlist = new BindingList<Folder>();
        int unnamedPreset = 0;
        List<Preset> presets = new List<Preset>();

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
                if (System.IO.Path.GetExtension(filedir) == String.Empty)
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
           
            //MessageBox.Show("Closed called");
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
                    Message = $"Sace preset succeeded",
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
                        Message = $"Sace preset succeeded!",
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
            int selectedfile = FileList.SelectedIndex;

            Process.Start("explorer.exe", filelist[selectedfile].filepath.Substring(0, filelist[selectedfile].filepath.LastIndexOf(@"\")+1));
        }

        private void deleteFileMenu_Click(object sender, RoutedEventArgs e)
        {
            int selectedfile = FileList.SelectedIndex;
            if (selectedfile >= 0)
            {
                filelist.Remove(filelist[selectedfile]);
            }
        }

        private void deleteFolderMenu_Click(object sender, RoutedEventArgs e)
        {
            int selectedfolder = FolderList.SelectedIndex;
            if (selectedfolder >= 0)
            {
                folderlist.Remove(folderlist[selectedfolder]);
            }
        }

        private void openInFolderExplorer_Click(object sender, RoutedEventArgs e)
        {
            int selectedfolder = FolderList.SelectedIndex;

            Process.Start("explorer.exe", folderlist[selectedfolder].folderpath);
        }

        private void ClearAllFile_Click(object sender, RoutedEventArgs e)
        {
            filelist.Clear();
        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {
            folderlist.Clear();
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