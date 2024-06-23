using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADACreator.ViewModel;


namespace SCADACreator
{
    public class ConnectDevice : BaseViewModel
    {
        public ConnectDevice()
        {
            _connectionType = ConnnectionType.emS7;
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public enum ConnnectionType
        {
            emS7,
            emTCP,
            emOPCUA
        }


        private ConnnectionType _connectionType;

        public ConnnectionType ConnectionType
        {
            get { return _connectionType; }
            set { _connectionType = value; }
        }


        private string _destination ="";

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        private string  _s7PLCType;

        public string  S7PLCType
        {
            get { return _s7PLCType; }
            set { _s7PLCType = value; }
        }

        private int _s7PLCSlot;

        public int S7PLCSlot
        {
            get { return _s7PLCSlot; }
            set { _s7PLCSlot = value; }
        }

        private int _s7PLCRack;

        public int S7PLCRack
        {
            get { return _s7PLCRack; }
            set { _s7PLCRack = value; }
        }

        private int _modbusPort;

        public int ModbusPort
        {
            get { return _modbusPort; }
            set { _modbusPort = value; }
        }

        private string _opcuaUserName = "";

        public string OPCUAUserName
        {
            get { return _opcuaUserName; }
            set { _opcuaUserName = value; }
        }

        private string _opcuaUserPassword ="";

        public string OPCUAUserPassword
        {
            get { return _opcuaUserPassword; }
            set { _opcuaUserPassword = value; }
        }

    }
}
