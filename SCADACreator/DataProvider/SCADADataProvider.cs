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
        public List<AlarmSetting> AlarmSettings; 
        public List<TagLoggingSetting> TagLoggingSettings; 
        public List<TrendViewSetting> TrendViewSettings; 
        public ProjectInformation ProjectInformation;

        private static SCADADataProvider instance;
        private int nextTagID;
        private int nextDeviceID;
        private int nextAlarmSettingID;
        private int nextTagLoggingSettingID;
        private int nextTrendViewSettingID;
        // Private constructor to prevent instantiation
        private SCADADataProvider()
        {

            TagInfos = new List<TagInfo>();
            ConnectDevices = new List<ConnectDevice>();
            AlarmSettings = new List<AlarmSetting>();
            TagLoggingSettings = new List<TagLoggingSetting>();
            TrendViewSettings = new List<TrendViewSetting>();
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

        public void AddListAlarmSettings(List<AlarmSetting> alarmSettings)
        {
            AlarmSettings.AddRange(alarmSettings);
            nextAlarmSettingID = AlarmSettings.Max(m => m.Id) + 1;
        }

        public void AddListTagLoggingSettings(List<TagLoggingSetting> tagLoggingSettings)
        {
            TagLoggingSettings.AddRange(tagLoggingSettings);
            nextTagLoggingSettingID = TagLoggingSettings.Max(m => m.Id) + 1;
        }
        public void AddListTrendViewSettings(List<TrendViewSetting> trendViewSettings)
        {
            TrendViewSettings.AddRange(trendViewSettings);
            nextTrendViewSettingID = TagLoggingSettings.Max(m => m.Id) + 1;
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
                alarmPoint.Id = nextAlarmSettingID;
                nextAlarmSettingID++;
                AlarmSettings.Add(alarmPoint);
            }
        }

        public void AddTagLoggingSetting(TagLoggingSetting tagLoggingSetting)
        {
            if (tagLoggingSetting != null)
            {
                tagLoggingSetting.Id = nextTagLoggingSettingID;
                nextTagLoggingSettingID++;
                TagLoggingSettings.Add(tagLoggingSetting);
            }
        }

        public void AddTrendViewSetting(TrendViewSetting trendViewSetting)
        {
            if (trendViewSetting != null)
            {
                trendViewSetting.Id = nextTrendViewSettingID;
                nextTrendViewSettingID++;
                TrendViewSettings.Add(trendViewSetting);
            }
        }

        public void Clear()
        {
            TagInfos.Clear();
            ConnectDevices.Clear();
            AlarmSettings.Clear();
            TagLoggingSettings.Clear();
            TrendViewSettings.Clear();
        }
    }


}
