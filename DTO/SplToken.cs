using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SolapeSplHeatmap.DTO.SolScanTokensResponseModel;

namespace SolapeSplHeatmap.DTO
{
    public class SplToken
    {
        public string mintAddress { get; set; }
        public string tokenSymbol { get; set; }
        public string tokenName { get; set; }
        public int decimals { get; set; }
        public string icon { get; set; }
        public string website { get; set; }
        public string twitter { get; set; }
        public Extensions extensions { get; set; }
        public bool tokenHolder { get; set; }
        public int marketCapRank { get; set; }
        public Supply supply { get; set; }
        public int holder { get; set; }
        public float? priceUst { get; set; }
        public float? marketCapFD { get; set; }
        public Volume volume { get; set; }
        public Coingeckoinfo coingeckoInfo { get; set; }
        public string[] tag { get; set; }
    }
}
