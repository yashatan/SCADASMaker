using SCADACreator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class DesignPage
    {
        public List<SCADAItem> SCADAItems { get; set; }
        //public List<ControlData> ControlDatas { get; set; }
        public DesignPage()
        {
            SCADAItems = new List<SCADAItem>();
        }
        public DesignPage(string name)
        {
            SCADAItems = new List<SCADAItem>();
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMainPage { get { return (Id == SCADADataProvider.Instance.ProjectInformation.MainPageId); } }
        public bool IsButtonEnabled => !IsMainPage;
    }
}
