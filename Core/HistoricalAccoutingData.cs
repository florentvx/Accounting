﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Statics;

namespace Core
{
    public class HistoricalAccoutingData: IHistoricalAccountingData
    {
        SortedDictionary<DateTime, AccountingData> _Data;
        CurrencyAssetStaticsDataBase _CcyDB;

        public CurrencyAssetStaticsDataBase CcyDB { get { return _CcyDB; } }

        public HistoricalAccoutingData()
        {
            _Data = new SortedDictionary<DateTime, AccountingData> { };
            _CcyDB = new CurrencyAssetStaticsDataBase();
        }

        public IEnumerable<DateTime> Dates { get { return _Data.Keys; } }

        public AccountingData GetData(DateTime date) { return _Data[date]; }

        public void AddData(DateTime date, AccountingData ad)
        {
            if (!_Data.ContainsKey(date))
                _Data.Add(date, ad);
            else
                throw new Exception($"the Following date already exists {date}");
        }

        public void Reset(DateTime date, string ccy, CurrencyStatics cs)
        {
            _Data.Clear();
            _CcyDB.Reset();
            _CcyDB.AddRefCcy(ccy, cs);
            AccountingData ad = new AccountingData(_CcyDB);
            _Data[date] = ad;
        }

        public void SetCcyDB(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
        }
    }
}
