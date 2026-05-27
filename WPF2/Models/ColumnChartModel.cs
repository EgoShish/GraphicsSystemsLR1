using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF2.Models
{
    public class ColumnChartModel
    {
        public int Index { get; set; }

        public double DurationMinutes { get; set; }

        public float Temperature { get; set; }

        public DateTime Timestamp { get; set; }

        public SKColor BarColor { get; set; }

        public static ColumnChartModel Create(int index, DateTime time, float temp, double duration)
        {
            SKColor color;
            if (temp < 20)
                color = new SKColor(0, 0, 255);      
            else if (temp <= 60)
                color = new SKColor(0, 200, 0);     
            else
                color = new SKColor(255, 0, 0);     

            return new ColumnChartModel
            {
                Index = index,
                DurationMinutes = duration,
                Temperature = temp,
                Timestamp = time,
                BarColor = color
            };
        }
    }
}
