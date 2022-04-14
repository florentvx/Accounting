using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Core.Finance
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FXMarket : Market, IMarket, ISerializable
    {
        [JsonProperty]
        public Currency CcyRef { get; set; }

        [JsonProperty]
        public List<KeyValuePair<CurrencyPair, double>> _Data;

        public override Dictionary<IMarketInput, double> GetData()
        {
            return _Data.ToDictionary(x => (IMarketInput)x.Key, x => x.Value);
        }

        public override void SetData(Dictionary<IMarketInput, double> input)
        {
            _Data = input.Select(x => new KeyValuePair<CurrencyPair, double> ((CurrencyPair)x.Key, x.Value)).ToList();
        }

        public FXMarket() { _Data = new List<KeyValuePair<CurrencyPair, double>> { }; }

        public FXMarket(Currency ccy)
        {
            CcyRef = ccy;
            _Data = new List<KeyValuePair<CurrencyPair, double>> { };
        }

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CcyRef", CcyRef, typeof(Currency));
            info.AddValue("Dict", _Data, typeof(List<KeyValuePair<CurrencyPair, double>>));
        }

        public FXMarket(SerializationInfo info, StreamingContext context)
        {
            CcyRef = (Currency)info.GetValue("CcyRef", typeof(Currency));
            _Data = (List<KeyValuePair<CurrencyPair, double>>)info.GetValue("Dict", typeof(List<KeyValuePair<CurrencyPair, double>>));
        }

        #endregion

        // simplest GetQuote function that looks if adequate data is there
        private double? GetQuote_Simple(CurrencyPair cp)
        {
            var filtered_list = _Data.Where(x => x.Key.IsEquivalent(cp)).Select(x => x);
            if (filtered_list.Count() == 0) return null;
            if (filtered_list.Count() == 1)
            {
                KeyValuePair<CurrencyPair, double> cp_data = filtered_list.First();
                if (cp_data.Key.Equals(cp)) return cp_data.Value;
                return 1 / cp_data.Value;
            }
            throw new NotImplementedException("filtered list should have only one item...");

        }

        // List all the currencies linked to `ccy` (not part of the `excludedCcies` list)
        private IEnumerable<Currency> GetAllConnectedCcy(ICcyAsset ccy)
        {
            return _Data    .Where( x => x.Key.Contains(ccy) )
                            .Select( x => x.Key.Other(ccy).Ccy );
        }


        // Calcule through a triangular operation (because you have to!)
        private double Aux_GetQuote(CurrencyPair ccyPair)
        {
            var connectedCcies = GetAllConnectedCcy(ccyPair.Ccy);
            if (connectedCcies.Count() == 0)
                return 0;
            else if (connectedCcies.Contains(ccyPair.CcyPrice))
                throw new InvalidOperationException("ccyPrice should not be part of the commected ccies by construction");
            else
            {
                foreach (var ccy_k in connectedCcies)
                {
                    // made a strong assumption that there is a triangular relationship
                    double value = GetQuote_Simple(new CurrencyPair(ccyPair.Ccy, ccy_k)).Value;
                    double? value2 = GetQuote_Simple(new CurrencyPair(ccy_k.Ccy, ccyPair.CcyPrice));
                    if (value2.HasValue)
                        return value * value2.Value;
                }
            }
            throw new Exception("Error");
        }

        public double GetQuote(CurrencyPair ccyPair)
        {
            if (ccyPair.IsIdentity)
                return 1.0;

            double? naive_value = GetQuote_Simple(ccyPair);
            if (naive_value != null)
                return naive_value.Value;

            return Aux_GetQuote(ccyPair);
        }

        public Price Convert(Price p, Currency ccy)
        {
            if (!p.Ccy.IsCcy())
                throw new InvalidOperationException($"price input has to be a currency not an asset: {p}");
            double conv = GetQuote(new CurrencyPair(p.Ccy, ccy));
            return new Price(conv * p.Amount, ccy);
        }

        internal Price Convert(Price price, Currency ccy, AssetMarket aMkt)
        {
            return Convert(price.Ccy.IsCcy()? price : aMkt.PriceAsset(price), ccy);
        }


        public IEnumerable<string> GetAvailableCurrencies()
        {
            var Ccy1List = _Data.Select(x => x.Key.Ccy.ToString());
            var Ccy2List = _Data.Select(x => x.Key.CcyPrice.ToString());
            var res = Ccy1List.Union(Ccy2List).ToList();
            if (!res.Contains(CcyRef.ToString()))
                res.Add(CcyRef.ToString());
            return res;
        }

        internal void SetCcyRef(Currency ccy)
        {
            CcyRef = ccy;
        }

        public bool IsCcy(string ccy)
        {
            return GetAvailableCurrencies().Contains(ccy);
        }

        public void Copy(FXMarket fx)
        {
            Copy((IMarket)fx);
            CcyRef = fx.CcyRef;
        }
    }
}
