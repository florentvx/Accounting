using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Statics;

namespace Core.Interfaces
{
    public interface IHistoricalAccountingData
    {
        AccountingData GetData(DateTime date);
        void Reset(DateTime date, string ccyName, CurrencyStatics ccyStatics);
        void AddNewDate(DateTime date);
        void CalculateTotal();
    }
}
