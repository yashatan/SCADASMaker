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
        public List<DesignPage> DesignPages; 
        public List<TablePage> TablePages; 
        public ProjectInformation ProjectInformation;

        private static SCADADataProvider instance;
        private int nextTagID;
        private int nextDeviceID;
        private int nextAlarmSettingID;
        private int nextTagLoggingSettingID;
        private int nextTrendViewSettingID;
        private int nextDesignPageID;
        private int nextTablePageID;
        // Private constructor to prevent instantiation
        private SCADADataProvider()
        {

            TagInfos = new List<TagInfo>();
            ConnectDevices = new List<ConnectDevice>();
            AlarmSettings = new List<AlarmSetting>();
            TagLoggingSettings = new List<TagLoggingSetting>();
            TrendViewSettings = new List<TrendViewSetting>();
            DesignPages = new List<DesignPage>();
            TablePages = new List<TablePage>();
            AddDesignPage(new DesignPage("MainPage"));
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
        public void AddListDesignPages(List<DesignPage> designPages)
        {
            DesignPages.Clear();
            DesignPages.AddRange(designPages);
            nextDesignPageID = designPages.Max(m => m.Id) + 1;
        }

        public void AddListTablePages(List<TablePage> tablePages)
        {
            TablePages.Clear();
            TablePages.AddRange(tablePages);
            nextTablePageID = tablePages.Max(m => m.Id) + 1;
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
        public void AddDesignPage(DesignPage designPage)
        {
            if (designPage != null)
            {
                designPage.Id = nextDesignPageID;
                nextDesignPageID++;
                DesignPages.Add(designPage);
            }
        }

        public void AddTablePage(TablePage tablePage)
        {
            if(tablePage != null)
            {
                tablePage.Id = nextTablePageID;
                nextTablePageID++;
                TablePages.Add(tablePage);
            }
        }

        public void Clear()
        {
            TagInfos.Clear();
            ConnectDevices.Clear();
            AlarmSettings.Clear();
            TagLoggingSettings.Clear();
            TrendViewSettings.Clear();
            DesignPages.Clear();
            TablePages.Clear();
            AddDesignPage(new DesignPage("MainPage"));
        }
    }


}
