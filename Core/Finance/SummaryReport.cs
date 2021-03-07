using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class SummaryReport
    {
        Dictionary<ICcyAsset, double> Data;

        public IEnumerable<ICcyAsset> Ccies { get { return Data.Keys; } }

        public void Set(ICcyAsset iCcyAsset, double amount)
        {
            if (amount != 0) { Data[(ICcyAsset)iCcyAsset.Clone()] = amount; }
        }

        public double Get(ICcyAsset iCcyAsset)
        {
            return Ccies.Contains(iCcyAsset) ? Data[iCcyAsset] : 0;
        }

        public SummaryReport(ICcyAsset iCcyAsset = null, double amount = 0)
        {
            Data = new Dictionary<ICcyAsset, double> { };
            Set(iCcyAsset, amount);
        }

        public void Add(SummaryReport sr)
        {
            var allCcies = Ccies.Union(sr.Ccies).ToList();
            foreach (ICcyAsset item in allCcies)
            {
                Set(item, Get(item) + sr.Get(item));
            }
        }
    }
}
