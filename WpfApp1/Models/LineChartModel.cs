using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class LineChartModel
    {
        // Минуты от начала отсчёта (ось X)
        public double Minutes { get; set; }

        // Температура шпинделя (ось Y1)
        public float Temperature { get; set; }

        // Скорость шпинделя (ось Y2, опционально)
        public float? Speed { get; set; }

        // Полная метка времени (для тултипа)
        public DateTime Timestamp { get; set; }
    }
}
