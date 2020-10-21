﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountingElement
    {
        string GetName();
        IEnumerable<IAccountingElement> GetItemList();
        IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme);
        NodeType GetNodeType();
        IAccount GetTotalAccount();
        IAccount GetTotalAccount(string name);
        void ModifyAmount(string v, object valueAmount);
        void ModifyCcy(string v, object valueCcy, bool isLastRow);
    }
}