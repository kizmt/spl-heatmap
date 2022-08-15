using Newtonsoft.Json;
using RestSharp;
using SolapeSplHeatmap.DTO;
using SolapeSplHeatmap.SolApe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SolapeSplHeatmap.SolScan
{
    public class SolScanService
    {
        public static async Task<SolScanTokensResponseModel> GetTokens(Int32 offset, Int32 limit, String sortBy)
        {
            // Fetch data using restsharp          
            var client = new RestClient(Constants.SolScanApiUrl);

            var request = new RestRequest(Constants.SolScanTokensEndpoint, Method.Get);
            request.AddQueryParameter("sortBy", sortBy);
            request.AddQueryParameter("limit", limit);
            request.AddQueryParameter("offset", offset);

            var response = await client.ExecuteAsync(request);

            // Parse it
            try
            {
                SolScanTokensResponseModel responseModel = JsonConvert.DeserializeObject<SolScanTokensResponseModel>(response.Content);
                return responseModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return new SolScanTokensResponseModel();
            }
        }

        public static async Task<List<SplToken>> GetSplTokens(String sortBy = "market_cap", Int32 minimumHolderCount = 2000, Int32 maxResults = 50)
        {
            // Set up the while loop for processing
            Boolean continueFetching = true;
            Int32 loopCounter = 0;
            Int32 limit = 50;

            List<SplToken> tokens = new List<SplToken>();

            while (continueFetching)
            {
                SolScanTokensResponseModel responseModel = await GetTokens(loopCounter * limit, limit, sortBy);
                if (responseModel.data.Length == 0)
                {
                    continueFetching = false;
                    continue; // Break out
                }
                else
                {
                    // Process and add
                    Int32 beforeValue = tokens.Count;
                    tokens.AddRange(ProcessTokensResponseModel(responseModel, minimumHolderCount));

                    if (tokens.Count <= beforeValue)
                    {
                        continueFetching = false;
                        break;
                    }

                    // Are we at / or exceeding the max results incl. filtering logic? Return our max.
                    if (tokens.Count >= maxResults)
                    {
                        continueFetching = false;
                        break;
                    }

                }

                loopCounter++;
            }

            return tokens.Take(maxResults).ToList();
        }

        private static List<SplToken> ProcessTokensResponseModel(SolScanTokensResponseModel responseModel, Int32 minimumHolderCount)
        {
            List<SplToken> returnValue = new List<SplToken>();

            foreach (var token in responseModel.data)
            {
                if (token.holder < minimumHolderCount)
                    continue; // Skip

                if (!SolApeService.IncludeToken(token.tokenSymbol)
                    || token.tokenSymbol == "AVDO"
                    || token.tokenSymbol == "SAFU"
                    || token.tokenSymbol == "BTC"
                    )
                    continue; // Skip

                // Process
                returnValue.Add(token);
            }

            return returnValue;
        }
    }
}
