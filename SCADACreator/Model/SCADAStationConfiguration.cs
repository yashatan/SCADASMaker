using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class SCADAStationConfiguration
    {
        private ProjectInformation projectInformation;

        public ProjectInformation ProjectInformation
        {
            get { return projectInformation; }
            set { projectInformation = value; }
        }
        List<TagInfo> tagInfos;
        public List<TagInfo> TagInfos
        {
            get { return tagInfos; }
            set { tagInfos = value; }
        }
        List<ConnectDevice> connectDevices;
        public List<ConnectDevice> ConnectDevices
        {
            get { return connectDevices; }
            set { connectDevices = value; }
        }
        List<AlarmSetting> alarmSettings;
        public List<AlarmSetting> AlarmSettings
        {
            get { return alarmSettings; }
            set { alarmSettings = value; }
        }
        List<TagLoggingSetting> tagLoggingSettings;
        public List<TagLoggingSetting> TagLoggingSettings
        {
            get { return tagLoggingSettings; }
            set { tagLoggingSettings = value; }
        }
        List<TrendViewSetting> trendViewSettings;
        public List<TrendViewSetting> TrendViewSettings
        {
            get { return trendViewSettings; }
            set { trendViewSettings = value; }
        }
        List<SCADAPage> scadaPages;
        public List<SCADAPage> SCADAPages
        {
            get { return scadaPages; }
            set { scadaPages = value; }
        }
        public SCADAStationConfiguration()
        {

        }
        public void SetTagInfos(List<TagInfo> tagInfos) { this.TagInfos = tagInfos; }
        public void SetConnectDevices(List<ConnectDevice> connectDevices) { this.ConnectDevices = connectDevices; }
        public void SetAlarmSettings(List<AlarmSetting> alarmSettings) { this.AlarmSettings = alarmSettings; }
        public void SetTagLoggingSettings(List<TagLoggingSetting> tagLoggingSettings) { this.TagLoggingSettings = tagLoggingSettings; }
        public void SetTrendViewSettings(List<TrendViewSetting> trendViewSettings) { this.TrendViewSettings = trendViewSettings; }
        public void SetSCADAPages(List<SCADAPage> scadaPages) { this.SCADAPages = scadaPages; }
    }
}
