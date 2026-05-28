using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF2.Models
{
    public class LineChartPoint
    {
        public double Minutes { get; set; }
        public float Temperature { get; set; }
        public float? Speed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
