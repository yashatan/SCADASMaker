using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator.Model
{
    public class TagLoggingSetting
    {
        public TagLoggingSetting() {
            LoggingCycle = CycleType.em1Sec;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public TagInfo Tag { get; set; }

       public enum CycleType
        {
            em1Sec = 0,
            em2Sec,
            em5Sec,
            em10Sec,
            em1min,
            em5min,
            em10min,
            em1hour
        }

        public CycleType LoggingCycle { get; set; }
    }
}
