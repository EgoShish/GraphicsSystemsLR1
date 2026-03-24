using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsPanelStanki
{
    public class ItemExtractor
    {
        public string FullName { get; set; }
        public string ClearName { get; set; }
        public string ImageName { get; set; }
        public override string ToString()
        {
            return FullName;
        }
    }
}
