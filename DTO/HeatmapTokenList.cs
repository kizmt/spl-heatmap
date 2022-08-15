using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolapeSplHeatmap.DTO
{
    public class HeatmapTokenList
    {
        public List<TokenItem> tokens { get; set; }

        public class TokenItem
        {
            public string address { get; set; }
            public string name { get; set; }
            public string logoURI { get; set; }
        }

    }
}
