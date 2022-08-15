using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SolapeSplHeatmap.DTO.HeatmapTokenList;

namespace SolapeSplHeatmap.SolApe
{
    public class SolApeService
    {
        public static Boolean IncludeToken(String tokenName)
        {
            return SolApeHeatmapTokenList.Instance.TokenList.tokens.Any(t => t.name.Equals(tokenName, StringComparison.OrdinalIgnoreCase));
        }

        public static TokenItem GetToken(String tokenName)
        {
            return SolApeHeatmapTokenList.Instance.TokenList.tokens.FirstOrDefault(t => t.name.Equals(tokenName, StringComparison.OrdinalIgnoreCase));
        }

    }
}
