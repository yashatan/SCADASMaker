using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADACreator.Utility;

namespace SCADACreator
{
    public class SCADAPage
    {
        public List<SCADAItem> SCADAItems {  get; set; }
        public List<ControlData> ControlDatas { get; set; }
        public SCADAPage()
        {
            SCADAItems = new List<SCADAItem>();
        }
        public SCADAPage(string name)
        {
            SCADAItems = new List<SCADAItem>();
            Name = name;
        }
        public int Id { get; set; }
        public int PageType { get; set; }
        public string Name { get; set; }
    }
}
