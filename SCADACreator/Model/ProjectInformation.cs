using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class ProjectInformation
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsNewProject {  get; set; }
        public ProjectInformation()
        {
            this.IsNewProject = true;
            this.Name = "NewSCADAProject";
            this.FilePath = "";
        }
    }
}
