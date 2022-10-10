/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System.Linq;
using System.Collections.Generic;

namespace QuantConnect.Securities.IndexOption
{
    /// <summary>
    /// Index Option Symbol
    /// </summary>
    public static class IndexOptionSymbol
    {
        private static Dictionary<string, string> _indexOptionToIndex = new Dictionary<string, string>
        {
            { "SPXW", "SPX" },
            { "VIXW", "VIX" },
            { "NDXP", "NDX" },
            { "NQX", "NDX" },
        };

        /// <summary>
        /// These are known assets that are weeklies or end-of-month settled contracts.
        /// </summary>
        private static readonly HashSet<string> _nonStandardIndexOptionTickers = new()
        {
            "SPXW",
            "VIXW",
            // PM-Settled
            "NDXP",
            // reduced value index options, 20%
            "NQX"
        };

        /// <summary>
        /// Supported index option tickers
        /// </summary>
        public static readonly HashSet<string> SupportedIndexOptionTickers = new string[] { "SPX", "NDX", "VIX" }
            .Union(_nonStandardIndexOptionTickers)
            .ToHashSet();

        /// <summary>
        /// Determines if the Index Option Symbol is for a monthly contract
        /// </summary>
        /// <param name="symbol">Index Option Symbol</param>
        /// <returns>True if monthly contract, false otherwise</returns>
        public static bool IsStandard(Symbol symbol)
        {
            if (symbol.ID.Market != Market.USA)
            {
                return true;
            }

            return _nonStandardIndexOptionTickers.Contains(symbol.ID.Symbol);
        }

        /// <summary>
        /// Checks if the ticker provided is a supported Index Option
        /// </summary>
        /// <param name="ticker">Ticker of the index option</param>
        /// <returns>true if the ticker matches an index option's ticker</returns>
        /// <remarks>
        /// This is only used in IB brokerage, since they don't distinguish index options
        /// from regular equity options. When we do the conversion from a contract to a SecurityType,
        /// the only information we're provided that can reverse it to the <see cref="SecurityType.IndexOption"/>
        /// enum value is the ticker.
        /// </remarks>
        public static bool IsIndexOption(string ticker)
        {
            return SupportedIndexOptionTickers.Contains(ticker.ToUpper());
        }

        /// <summary>
        /// Maps an index option ticker to its underlying index ticker
        /// </summary>
        /// <param name="indexOption">Index option ticker to map to the underlying</param>
        /// <returns>Index ticker</returns>
        public static string MapToUnderlying(string indexOption)
        {
            if(_indexOptionToIndex.TryGetValue(indexOption, out var index))
            {
                return index;
            }

            return indexOption;
        }
    }
}
