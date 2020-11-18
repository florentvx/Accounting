using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Market
    {
        public Dictionary<IMarketInput, double> _Data;
        List<IMarketInput> _Pairs;

        public Market()
        {
            _Data = new Dictionary<IMarketInput, double> { };
            _Pairs = new List<IMarketInput> { };
        } 

        virtual public void Reset()
        {
            _Data = new Dictionary<IMarketInput, double> { };
            _Pairs = new List<IMarketInput> { };
        }

        public IEnumerable<Tuple<IMarketInput, double>> EnumerateData()
        {
            return _Pairs.Select(x => new Tuple<IMarketInput, double>(x, _Data[x]));
        }

        virtual protected void AddQuoteToDictionary(IMarketInput imi, double value)
        {
            _Data[imi] = value;
            _Pairs.Add(imi);
        }

        public void AddQuote(IMarketInput imi, double value)
        {
            var presentData = _Data.Where(x => x.Key.IsEquivalent(imi)).Select(x => x.Key).ToList();
            if (presentData.Count() == 1)
            {
                if (presentData[0].IsEqual(imi))
                    _Data[presentData[0]] = value;
                else
                    _Data[presentData[0]] = 1 / value;
            }
            else if (presentData.Count() == 0)
            {
                AddQuoteToDictionary(imi, value);
            }
        }
    }
}
