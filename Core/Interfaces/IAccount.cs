using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccount
    {
        string AccountName { get; set; }
        Currency Ccy { get; }
        double Amount { get; }
        bool IsCalculatedAccount { get; }
    }
}
