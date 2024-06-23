using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADACreator.ViewModel;

namespace SCADACreator
{
    public class TagInfo: BaseViewModel
    {
        public TagInfo()
        {

        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ConnectDevice _deviceAttach;

        public ConnectDevice DeviceAttach
        {
            get { return _deviceAttach; }
            set { _deviceAttach = value; }
        }


        private string _memoryAddress;

        public string MemoryAddress
        {
            get { return _memoryAddress; }
            set { _memoryAddress = value; }
        }

    }
}
