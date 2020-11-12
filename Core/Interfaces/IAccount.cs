using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IAccount
    {
        string AccountName { get; set; }
        Currency Ccy { get; }
        Currency ConvertedCcy { get; }
        double Amount { get; }
        double ConvertedAmount { get; }
        bool IsCalculatedAccount { get; }
    }
}
