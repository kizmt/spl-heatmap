using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SolapeSplHeatmap.DTO;
using System.Net;
using static SolapeSplHeatmap.DTO.HeatmapJsonData;

namespace SolapeSplHeatmap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // Buidld it
            HeatmapJsonData heatmapData = new HeatmapJsonData();
            heatmapData.children = new List<Child>();

            // Run on demand, generate heatmap with data.
            Int32 tokenIndex = 1;
            foreach (var token in SolScan.SolScanService.GetSplTokens().Result)
            {
                Console.WriteLine($"Token {tokenIndex} is `{token.tokenName}`. Price change % 24h: `{token.coingeckoInfo.marketData.priceChangePercentage24h}` Market cap: `{(Double)token.marketCapFD}`. Holders: `{token.holder}`.");

                //if (tokenIndex >= 13
                //    && !token.tokenName.Contains("staked", StringComparison.OrdinalIgnoreCase)
                //    && !token.tokenName.Contains("UST", StringComparison.OrdinalIgnoreCase))
                //{

                // Add to the json structure                
                Child child = new Child();
                child.rate = token.coingeckoInfo.marketData.priceChangePercentage24h.HasValue ? token.coingeckoInfo.marketData.priceChangePercentage24h.Value : 0;
                child.value = token.coingeckoInfo.marketData.fullyDilutedValuation.GetValueOrDefault() == 0 ? token.marketCapFD.GetValueOrDefault() : token.coingeckoInfo.marketData.fullyDilutedValuation.GetValueOrDefault();
                child.name = token.tokenName;
                child.tickersymbol = token.tokenSymbol;
                child.currentprice = token.priceUst.Value;
                child.image = GetIconUrl(token.tokenSymbol, token.icon, config["iconfolderabsolute"], config["iconsfolderrelative"]);
                child.url = "https://solapeswap.io/#/market/4zffJaPyeXZ2wr4whHgP39QyTfurqZ2BEd4M5W6SEuon";

                heatmapData.children.Add(child);
                //}

                tokenIndex++;
            }

            // Write-out the json data:
            File.WriteAllText(config["jsondata"], JsonConvert.SerializeObject(heatmapData));
        }

        private static String GetIconUrl(String tokenSymbol, String webUrl, String iconFolder, String relativeIconsFolder)
        {
            var files = Directory.GetFiles(iconFolder, $"{tokenSymbol}.*");
            if (files.Length > 0)
            {
                return @$"{relativeIconsFolder}\{Path.GetFileName(files[0])}";
            }
            else
            {
                // Can we find it online over at solape?
                var url = $"https://solapeswap.io/icons/tokens/{tokenSymbol.ToUpper()}.png";
                System.Diagnostics.Debug.WriteLine($"Trying to fetch online from `{url}`.");

                WebClient webClient = new WebClient();
                try
                {
                    webClient.DownloadFile(url, Path.Combine(iconFolder, $"{tokenSymbol}.png"));
                    System.Diagnostics.Debug.WriteLine($"Retrieved `{url}`!");
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to fetch `{url}`.");
                }
            }

            return webUrl;
        }
    }
}