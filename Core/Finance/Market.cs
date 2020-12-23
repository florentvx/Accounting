using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Market : IEquatable<Market>
    {
        virtual public Dictionary<IMarketInput, double> GetData()
        {
            throw new NotImplementedException();
        }

        virtual public void SetData(Dictionary<IMarketInput, double> input) { }

        IEnumerable<IMarketInput> Pairs { get { return GetData().Keys; } }

        public Market() { }// _Pairs = new List<IMarketInput> { }; }

        #region IEquatable

        public bool Equals(Market mkt)
        {
            if (mkt == null)
                return false;
            return Tools.CompareDictionary<IMarketInput, double>(GetData(), mkt.GetData());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Market);
        }

        public override int GetHashCode()
        {
            return GetData().GetHashCode();
        }

        public static bool operator ==(Market mkt1, Market mkt2)
        {
            if (mkt1 is null)
            {
                if (mkt2 is null) { return true; }
                return false;
            }
            return mkt1.Equals(mkt2);
        }

        public static bool operator !=(Market mkt1, Market mkt2)
        {
            return !(mkt1 == mkt2);
        }

        #endregion

        virtual public void Reset()
        {
            //_Data = new Dictionary<IMarketInput, double> { };
            //_Pairs = new List<IMarketInput> { };
        }

        public IEnumerable<Tuple<IMarketInput, double>> EnumerateData()
        {
            var data = GetData();
            return Pairs.Select(x => new Tuple<IMarketInput, double>(x, data[x]));
        }

        virtual protected void AddQuoteToDictionary(IMarketInput imi, double value)
        {
            var data = GetData();
            data[imi] = value;
            //_Pairs.Add(imi);
            SetData(data);
        }

        public void AddQuote(IMarketInput imi, double value)
        {
            var data = GetData();
            var presentData = data.Where(x => x.Key.IsEquivalent(imi)).Select(x => x.Key).ToList();
            if (presentData.Count() == 1)
            {
                if (presentData[0].IsEqual(imi))
                    data[presentData[0]] = value;
                else
                    data[presentData[0]] = 1 / value;
                SetData(data);
            }
            else if (presentData.Count() == 0)
            {
                AddQuoteToDictionary(imi, value);
            }
        }

        public void Copy(IMarket imkt)
        {
            var data = GetData();
            data = new Dictionary<IMarketInput, double> { };
            //_Pairs = new List<IMarketInput> { };
            foreach(var item in imkt.EnumerateData())
            {
                //_Pairs.Add(item.Item1);
                data.Add(item.Item1, item.Item2);
            }
            SetData(data);
        }
    }
}
