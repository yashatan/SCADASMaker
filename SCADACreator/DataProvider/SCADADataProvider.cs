using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class SCADADataProvider
    {
        public List<TagInfo> TagInfos; 
        public List<ConnectDevice> ConnectDevices; 
        public List<AlarmSetting> AlarmSettingList; 

        private static SCADADataProvider instance;
        private int nextTagID;
        private int nextDeviceID;
        private int nextAlarmPointID;
        // Private constructor to prevent instantiation
        private SCADADataProvider()
        {

            TagInfos = new List<TagInfo>();
            ConnectDevices = new List<ConnectDevice>();
            AlarmSettingList = new List<AlarmSetting>();
            //
        }

        // Public static method to get the single instance of the class
        public static SCADADataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SCADADataProvider();
                }
                return instance;
            }
        }

        public void AddListTagInfos(List<TagInfo> tags)
        {
            TagInfos.AddRange(tags);
            nextTagID = TagInfos.Max(m => m.Id) +1;
        }

        public void AddListConnectDevices(List<ConnectDevice> connectDevices)
        {
            ConnectDevices.AddRange(connectDevices);
            nextDeviceID = ConnectDevices.Max(m => m.Id) + 1;
        }

        public void AddListAlarmSettingList(List<AlarmSetting> alarmPoints)
        {
            AlarmSettingList.AddRange(alarmPoints);
            nextAlarmPointID = AlarmSettingList.Max(m => m.Id) + 1;
        }

        public void AddDummyListAlarmPointList()
        {
            DummyData.CreateData();
            AddListAlarmSettingList(DummyData.dummyAlarms);
        }

        public void AddTagInfo(TagInfo tagInfo)
        {
            if (tagInfo != null)
            {
                tagInfo.Id = nextTagID;
                nextTagID++;
                TagInfos.Add(tagInfo);
            }

        }
        public void AddConnectDevice(ConnectDevice connectDevice)
        {
            if(connectDevice != null)
            {
                connectDevice.Id = nextDeviceID;
                nextDeviceID++;
                ConnectDevices.Add(connectDevice);
            }
        }
        public void AddAlarmSetting(AlarmSetting alarmPoint)
        {
            if (alarmPoint != null)
            {
                alarmPoint.Id = nextAlarmPointID;
                nextAlarmPointID++;
                AlarmSettingList.Add(alarmPoint);
            }
        }

        public void Clear()
        {
            TagInfos.Clear();
            ConnectDevices.Clear();
            AlarmSettingList.Clear();
        }
    }


}
