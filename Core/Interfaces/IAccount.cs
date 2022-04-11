using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Interfaces
{
    public interface IAccount : IAccountingElement
    {
        string AccountName { get; set; }
        Price Value { get; }
        double Amount { get; set; }

    }
}
