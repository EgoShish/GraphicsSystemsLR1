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

        public string DateDisplay => Value.ToString("dd.MM.yyyy");

        public string FullDisplay => Value.ToString("dd.MM.yyyy HH:mm");

        public string SqlParam => Value.ToString("yyyy-MM-dd HH:mm:ss");

        public override string ToString() => FullDisplay;
    }
}
