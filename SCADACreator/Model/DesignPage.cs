using SCADACreator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class DesignPage : BaseSCADAPage
    {
        public List<SCADAItem> SCADAItems { get; set; }
        //public List<ControlData> ControlDatas { get; set; }
        public DesignPage()
        {
            SCADAItems = new List<SCADAItem>();
            PageType = 0;
        }
        public DesignPage(string name)
        {
            SCADAItems = new List<SCADAItem>();
            Name = name;
            PageType = 0;
        }
        public bool IsMainPage { get { return (Id == SCADADataProvider.Instance.ProjectInformation.MainPageId); } }
        public bool IsButtonEnabled => !IsMainPage;
    }
}
