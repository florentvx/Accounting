using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IHistoricalAccountingData
    {
        AccountingData GetData(DateTime date);
        void Reset(DateTime date, string ccyName, CurrencyStatics ccyStatics);
    }
}
