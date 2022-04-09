using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;
using Core.Statics;

namespace Core.Interfaces
{
    public interface IHistoricalAccountingData
    {
        AccountingData GetData(DateTime date);
        AccountingData GetPreviousData(DateTime date);
        void Reset(DateTime date, string ccyName, CurrencyStatics ccyStatics);
        void AddNewDate(long date_ticks);
        void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair ccyPair, double ccyPairQuote);
        void AddNewAsset(string assetName, AssetStatics assetStatics, double assetCcyPairQuote);
        void PrepareForLoading();
    }
}
