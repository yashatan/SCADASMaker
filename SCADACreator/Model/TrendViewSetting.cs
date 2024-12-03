using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACreator.Model
{
    public class TrendViewSetting
    {
        public TrendViewSetting()
        {
            Trends = new List<TrendSetting>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TrendSetting> Trends { get; set; }
    }
}
