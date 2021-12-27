using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace batchRenameApp
{
    public class LastProject
    {
        public string Address { get; set; }

        public static LastProject Parse(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return null;
            }
            string outJson = File.ReadAllText(path);
            LastProject result = (LastProject)JsonSerializer.Deserialize(outJson, typeof(LastProject));
            return result;
        }

        public void StoreData(string path)
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(path, json);
        }
    }
    public class RenameProject
    {
        public string ProjectAddress { get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double WindowLeft { get; set; }
        public double WindowTop { get; set; }
        public string PresetName { get; set; }

        public int CurrentFilePage { get; set; }
        public int CurrentFolderPage { get; set; }
        public List<RuleContainer> Rules { get; set; }
        public List<MyFile> Files { get; set; }
        public List<Folder> Folders { get; set; }

        public RenameProject()
        {
            CurrentFilePage = 1;
            CurrentFolderPage = 1;
            PresetName = "";
            ProjectAddress = "";
            WindowWidth = 1360;
            WindowHeight = 630;
            WindowLeft = 100;
            WindowTop = 100;
            Rules = null;
            Folders = null;
            Files = null;
        }

        public string GetName()
        {
            if (ProjectAddress != null && ProjectAddress.Length > 0)
            {
                FileInfo file = new FileInfo(ProjectAddress);
                string[] tokens = file.Name.Split(".");
                return tokens[tokens.Length - 2];
            }
            return "Untitled";
        }


        public void StoreData(string path)
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(path, json);
        }

        public static RenameProject Parser(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return null;
            }
            string outJson = File.ReadAllText(path);
            RenameProject result = (RenameProject)JsonSerializer.Deserialize(outJson, typeof(RenameProject));
            return result;
        }

        public List<IRule> GetRules()
        {
            List<IRule> resultRules = new List<IRule>();
            for (int i = 0; i < Rules.Count(); i++)
            {
                IRule newrule = RuleFactory.GetInstance().Create(Rules[i]);
                if (newrule != null)
                    resultRules.Add(newrule);
            }

            return resultRules;
        }

        public bool isDefault()
        {
            if (
                (ProjectAddress == null || ProjectAddress.Length <= 0) &&
                Rules.Count() <= 0 &&
                Files.Count() <= 0 &&
                Folders.Count() <= 0 &&
                PresetName.Length <= 0
            )
            {
                return true;
            }

            return false;

        }
    }
}
