﻿using Core.Finance;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ModifyAmountEventArgs : EventArgs { }

    [Serializable]
    public class Institution : IInstitution, IEquatable<Institution>, ISerializable
    {
        [JsonProperty]
        string _InstitutionName;
        [JsonProperty]
        Currency _CcyRef;
        [JsonProperty]
        List<Account> _Accounts;

        public Currency CcyRef
        {
            get { return _CcyRef; }
            //set { _CcyRef = value; }
        }

        #region IAccountingElement

        public string GetName() { return InstitutionName; }

        public ICcyAsset Ccy { get { return CcyRef; } }

        public IAccountingElement GetItem(NodeAddress na)
        {
            if (na.GetLabel(NodeType.Category) == InstitutionName)
                throw new KeyNotFoundException($"Node Address not Found: Institution: {InstitutionName} vs NodeAddress {na.GetLast()}");
            if (na.NodeType == NodeType.Institution)
                return (Institution)this.Clone();
            else if (na.NodeType == NodeType.Account)
                return GetAccount(na.GetLast());
            throw new KeyNotFoundException($"Node Address not Found: NodeAddress {na}");
        }

        public TreeViewMapping GetTreeStructure()
        {
            TreeViewMapping tvm = new TreeViewMapping();
            tvm.AddItem_Simple(InstitutionName);
            foreach(Account elemt in Accounts)
                tvm.AddItem_Simple(elemt.GetTreeStructure(), InstitutionName);
            return tvm;
        }

        public IEnumerable<string> AccountNames => _Accounts.Select(x => x.AccountName);

        public NodeType GetNodeType() { return NodeType.Institution; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string name)
        {
            return TotalAccount(mkt, aMkt, ccy, name);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            return GetTotalAccount(mkt, aMkt, ccy, "Total");
        }

        public Price GetTotalAmount(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            return GetTotalAccount(mkt, aMkt, ccy).Value;
        }

        public void Delete(string name)
        {
            if (_Accounts.Count() > 1)
            {
                Account accDel = _Accounts.Where(x => x.AccountName == name).First();
                _Accounts.Remove(accDel);
            }
        }

        public SummaryReport GetSummary()
        {
            SummaryReport sr = new SummaryReport();
            foreach (var item in Accounts)
            {
                sr.Add(item.GetSummary());
            }
            return sr;
        }

        public object Clone()
        {
            Institution res = new Institution(InstitutionName, (Currency)Ccy.Clone());
            foreach (var item in _Accounts)
            {
                Account copyItem = (Account)item.Clone();
                res.AddAccount(copyItem);
            }
            return res;
        }

        #endregion

        #region IInstitution

        public string InstitutionName
        {
            get { return _InstitutionName; }
            set { _InstitutionName = value; }
        }

        public IEnumerable<Account> Accounts { get { return _Accounts; } }

        //public IEnumerable<Account> GetAccounts(TreeViewMappingElement tvme)
        //{
        //    return tvme.Nodes.Select(x => GetAccount(x.Name));
        //}

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return TotalAccount(mkt, aMkt, convCcy, "Total");
        }

        #endregion

        #region IEquatable

        public bool Equals(Institution instit)
        {
            if (instit == null)
                return false;
            if (InstitutionName == instit.InstitutionName
                && Ccy.Equals(instit.Ccy)
                && _Accounts.Count == instit._Accounts.Count)
            {
                for (int i = 0; i < _Accounts.Count; i++)
                {
                    if (_Accounts[i] != instit._Accounts[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Institution);
        }

        public override int GetHashCode()
        {
            int res = _InstitutionName.GetHashCode() + Ccy.GetHashCode();
            foreach (Account item in _Accounts)
                res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(Institution in1, Institution in2)
        {
            if (in1 is null)
            {
                if (in2 is null) { return true; }
                return false;
            }
            return in1.Equals(in2);
        }

        public static bool operator !=(Institution in1, Institution in2)
        {
            return !(in1 == in2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _InstitutionName, typeof(string));
            info.AddValue("Currency", _CcyRef, typeof(Currency));
            info.AddValue("Accounts", _Accounts, typeof(List<Account>));
        }

        public Institution(SerializationInfo info, StreamingContext context)
        {
            _InstitutionName = (string)info.GetValue("Name", typeof(string));
            _CcyRef = (Currency)info.GetValue("Currency", typeof(Currency));
            _Accounts = (List<Account>)info.GetValue("Accounts", typeof(List<Account>));
        }

        #endregion

        public Institution(string name, Currency ccy)
        {
            _InstitutionName = name;
            _CcyRef = ccy;
            _Accounts = new List<Account> { };
        }

        public Account GetAccount(string accountName)
        {
            var list = Accounts.Where(x => x.AccountName == accountName);
            if (list.Count() > 0) { return list.First(); }
            throw new Exception($"Account Name [{accountName}] not found in Insitution [{InstitutionName}]");
        }

        public Account AddAccount(string name, ICcyAsset currency = null, double amount = 0)
        {
            if (currency == null)
                currency = Ccy;
            Account account = new Account(name, currency, amount);
            _Accounts.Add(account);
            return account;
        }

        public void AddAccount(Account acc)
        {
            _Accounts.Add(acc);
        }

        private string GetNewAccountName()
        {
            int i = 0;
            string newNameRef = "New Account";
            string newName = newNameRef;
            while (_Accounts.Where(x => x.AccountName == newName).Count() > 0)
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            return newName;
        }

        public Account AddNewAccount()
        {
            string newName = GetNewAccountName();
            return AddAccount(newName);
        }

        public bool ChangeName(NodeAddress na, string after)
        {
            switch (na.NodeType)
            {
                case NodeType.Account:
                    if (!AccountNames.Contains(after))
                    {
                        GetAccount(na.GetLast()).ChangeName(na, after);
                        return true;
                    }
                    return false;
                case NodeType.Institution:
                    if (na.GetLast() == InstitutionName)
                    {
                        InstitutionName = na.GetLast();
                        return true;
                    }
                    return false;
            }
            throw new NotSupportedException("Invalid NodeAddress");
        }

        //internal void ReorgItems(IEnumerable<string> enumerable)
        //{
        //    List<Account> res = new List<Account> { };
        //    foreach (string item in enumerable)
        //    {
        //        res.Add((Account)GetAccount(item).Clone());
        //    }
        //    _Accounts = res;
        //}

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string overrideAccountName)
        {
            Price total = new Price(0, (Currency)ccy.Clone());
            foreach (var x in _Accounts)
                total += x.GetTotalAccount(mkt, aMkt, ccy).Value;
            Account res = new Account(overrideAccountName, total);
            return res;
        }
    }
}
