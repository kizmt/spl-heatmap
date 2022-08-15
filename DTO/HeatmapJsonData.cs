using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolapeSplHeatmap.DTO
{
    internal class HeatmapJsonData
    {
        public string name { get; set; }
        public List<Child> children { get; set; }

        public class Child
        {
            public float rate { get; set; }
            public string name { get; set; }
            public String tickersymbol { get; set; }
            public float value { get; set; }
            public String url { get; set; }
            public String image { get; set; }
            public float currentprice { get; set; }
        }

    }
}
