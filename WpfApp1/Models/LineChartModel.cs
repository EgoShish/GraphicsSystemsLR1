using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class LineChartModel
    {
        public double Minutes { get; set; }

        public float Temperature { get; set; }

        public float? Speed { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
