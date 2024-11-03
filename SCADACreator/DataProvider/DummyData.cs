using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCADACreator
{
    public class DummyData
    {
        public static List<ConnectDevice> connectDevices;
        public static List<TagInfo> tagInfos;

        public static void CreateData()
        {
            //connectDevices = new List<ConnectDevice>();

            //ConnectDevice device1 = new ConnectDevice();
            //device1.Name = "device1";
            //device1.ConnectionType = (int)ConnectDevice.ConnnectionType.emS7;
            //device1.Destination = "192.168.1.100";

            //ConnectDevice device2 = new ConnectDevice();
            //device2.Name = "device2";
            //device2.ConnectionType = ConnectDevice.ConnnectionType.emS7;
            //device2.Destination = "192.168.1.100";

            //ConnectDevice device3 = new ConnectDevice();
            //device3.Name = "device3";
            //device3.ConnectionType = ConnectDevice.ConnnectionType.emS7;
            //device3.Destination = "192.168.1.100";

            //connectDevices.Add(device1);
            //connectDevices.Add(device2);
            //connectDevices.Add(device3);

            //TagInfo tag1 = new TagInfo();
            //tag1.Name = "MotorStatus1";
            //tag1.DeviceAttach = device1;
            //tag1.MemoryAddress = "DB1.DBX2.4";

            //TagInfo tag2 = new TagInfo();
            //tag2.Name = "MotorStatus2";
            //tag2.DeviceAttach = device2;
            //tag2.MemoryAddress = "DB2.DBX2.4";

            SCADACreator_DBEntities db = new SCADACreator_DBEntities();
            connectDevices = db.ConnectDevices.ToList();

            //tagInfos = new List<TagInfo> { tag1, tag2 };
        }
        public DummyData()
        {

        }

    }
}
