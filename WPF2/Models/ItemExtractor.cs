using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF2.Models
{
    public class ItemExtractor : Notifier
    {
        private string _fullName;
        private string _clearName;
        private string _imageName;

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string ClearName
        {
            get => _clearName;
            set { _clearName = value; OnPropertyChanged(); }
        }

        public string ImageName
        {
            get => _imageName;
            set { _imageName = value; OnPropertyChanged(); }
        }

        public override string ToString() => FullName ?? ClearName ?? ImageName;
    }
}
