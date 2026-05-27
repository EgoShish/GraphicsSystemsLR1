using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class TimePointModel : NotifyModel
    {
        private DateTime _value; 

        public DateTime Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        // Для ComboBox 1: только дата "27.05.2024"
        public string DateDisplay => Value.ToString("dd.MM.yyyy");

        // Для ComboBox 2 и 3: полная метка "27.05.2024 14:30"
        public string FullDisplay => Value.ToString("dd.MM.yyyy HH:mm");

        // Для SQL-запросов
        public string SqlParam => Value.ToString("yyyy-MM-dd HH:mm:ss");

        public override string ToString() => FullDisplay;
    }
}
