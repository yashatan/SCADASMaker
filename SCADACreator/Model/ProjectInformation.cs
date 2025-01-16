using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class ProjectInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsNewProject {  get; set; }
        public int MainPageId {  get; set; }
        public ProjectInformation()
        {
            this.IsNewProject = true;
            this.Name = "NewSCADAProject";
            this.FilePath = "";
            MainPageId = 0;
        }
        public string GetDBPath()
        {
            string dbPath;
            dbPath = $"{Path.GetDirectoryName(FilePath)}\\{Name}.db";
            return dbPath;
        }
    }
}
