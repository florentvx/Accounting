using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Accounting
{
    public class Presenter
    {
        private readonly IView _view;
        private IAccountingData _ad;

        public Presenter(IView view, IAccountingData ad)
        {
            _view = view;
            _ad = ad;
        }
    }
}
