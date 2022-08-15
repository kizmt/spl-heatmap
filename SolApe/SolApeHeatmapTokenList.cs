using Newtonsoft.Json;
using SolapeSplHeatmap.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolapeSplHeatmap.SolApe
{
    public sealed class SolApeHeatmapTokenList
    {
        private static readonly Lazy<SolApeHeatmapTokenList> lazy =
            new Lazy<SolApeHeatmapTokenList>(() => new SolApeHeatmapTokenList());

        public static SolApeHeatmapTokenList Instance { get { return lazy.Value; } }

        public HeatmapTokenList TokenList { get; set; }

        private SolApeHeatmapTokenList()
        {
            // Initialize myself
            this.TokenList = JsonConvert.DeserializeObject<HeatmapTokenList>(File.ReadAllText("heatmap-token-list-mcap.json"));
        }
    }
}
