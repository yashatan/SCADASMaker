using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADACreator.Utility;

namespace SCADACreator
{
    public class SCADAPage: BaseSCADAPage
    {
        public List<ControlData> ControlDatas { get; set; }
        public SCADAPage()
        {
        }
        public SCADAPage(string name)
        {
            Name = name;
            PageType = 0;
        }
    }
}
