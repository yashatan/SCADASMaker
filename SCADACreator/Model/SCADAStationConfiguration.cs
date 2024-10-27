using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator
{
    public class SCADAStationConfiguration
    {
        List<ControlData> controlDatas;
        List<TagInfo> tagInfos;
        List<ConnectDevice> connectDevices;
        public SCADAStationConfiguration()
        {
            
        }
        public void SetControlDatas(List<ControlData> controlDatas) { this.controlDatas = controlDatas; }
        public void SetTagInfos(List<TagInfo> tagInfos) { this.tagInfos = tagInfos; }
        public void SetConnectDevices(List<ConnectDevice> connectDevices) { this.connectDevices = connectDevices; }
    }
}
