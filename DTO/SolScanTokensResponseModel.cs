using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolapeSplHeatmap.DTO
{
    public class SolScanTokensResponseModel
    {

        public SplToken[] data { get; set; }
        public int total { get; set; }

        public class Extensions
        {
            public string coingeckoID { get; set; }
            public string coingeckoId { get; set; }
            public string coinmarketcap { get; set; }
            public string facebook { get; set; }
            public string instagram { get; set; }
            public string telegram { get; set; }
            public string twitter { get; set; }
            public string website { get; set; }
            public string youtube { get; set; }
            public string discord { get; set; }
            public string description { get; set; }
            public string serumV3Usdt { get; set; }
            public string serumV3Usdc { get; set; }
            public string waterfallbot { get; set; }
            public string bridgeContract { get; set; }
            public string github { get; set; }
            public string reddit { get; set; }
            public string medium { get; set; }
            public string assetContract { get; set; }
            public string address { get; set; }
        }

        public class Supply
        {
            public object amount { get; set; }
            public int decimals { get; set; }
            public float uiAmount { get; set; }
            public string uiAmountString { get; set; }
        }

        public class Volume
        {
            public float volumeUsd { get; set; }
            public float volume { get; set; }
        }

        public class Coingeckoinfo
        {
            public int? marketCapRank { get; set; }
            public int? coingeckoRank { get; set; }
            public Marketdata marketData { get; set; }
        }

        public class Marketdata
        {
            public float currentPrice { get; set; }
            public float ath { get; set; }
            public float athChangePercentage { get; set; }
            public DateTime athDate { get; set; }
            public float atl { get; set; }
            public float atlChangePercentage { get; set; }
            public DateTime atlDate { get; set; }
            public float marketCap { get; set; }
            public int? marketCapRank { get; set; }
            public long? fullyDilutedValuation { get; set; }
            public float totalVolume { get; set; }
            public float? priceHigh24h { get; set; }
            public float? priceLow24h { get; set; }
            public float? priceChange24h { get; set; }
            public float? priceChangePercentage24h { get; set; }
            public float? priceChangePercentage7d { get; set; }
            public float? priceChangePercentage14d { get; set; }
            public float? priceChangePercentage30d { get; set; }
            public float? priceChangePercentage60d { get; set; }
            public float? priceChangePercentage200d { get; set; }
            public float? priceChangePercentage1y { get; set; }
            public float? marketCapChange24h { get; set; }
            public float? marketCapChangePercentage24h { get; set; }
            public float? totalSupply { get; set; }
            public float? maxSupply { get; set; }
            public float? circulatingSupply { get; set; }
            public DateTime lastUpdated { get; set; }
        }

    }

}