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
        public static List<AlarmSetting> dummyAlarms;

        public static void CreateData()
        {

            dummyAlarms = new List<AlarmSetting>();
            AlarmSetting alarmPoint0 = new AlarmSetting();

            alarmPoint0.Id = 0;
            alarmPoint0.Name = "HIHI";
            alarmPoint0.Text = "Level is too high";
            alarmPoint0.Type = AlarmSetting.AlarmType.Warning;
            alarmPoint0.TriggerTag = SCADADataProvider.Instance.TagInfos.Where(m => m.Name == "Level_value").FirstOrDefault();
            alarmPoint0.Limit = 50.5;
            alarmPoint0.LimitMode = AlarmSetting.LimitType.Higher;

            AlarmSetting alarmPoint1 = new AlarmSetting()
            {
                Id = 1,
                Name = "LO",
                Text = "Level is low",
                Type = AlarmSetting.AlarmType.Error,
                TriggerTag = SCADADataProvider.Instance.TagInfos.Where(m => m.Name == "Level_value").FirstOrDefault(),
                Limit = 15.5,
                LimitMode = AlarmSetting.LimitType.Lower
            };
            AlarmSetting alarmPoint2 = new AlarmSetting()
            {
                Id = 2,
                Name = "HIHI",
                Text = "Level is too high",
                Type = AlarmSetting.AlarmType.Error,
                TriggerTag = SCADADataProvider.Instance.TagInfos.Where(m => m.Name == "Level_value").FirstOrDefault(),
                Limit = 10.9,
                LimitMode = AlarmSetting.LimitType.Lower
            };
            dummyAlarms.Add(alarmPoint0);
            dummyAlarms.Add(alarmPoint1);
            dummyAlarms.Add(alarmPoint2);
        }
        public DummyData()
        {

        }

    }
}
