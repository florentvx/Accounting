using Core;
using Core.Finance;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core
{
    [TestClass]
    public class CategoryTests
    {

        #region IAccountingElement

        [TestMethod]
        public void Cateogry_GetName()
        {
            Category cat = Init.CreateCategory1();
            Assert.IsTrue(cat.GetName() == "Category1");
        }

        [TestMethod]
        public void Category_Ccy()
        {
            Category cat = Init.CreateCategory1();
            Assert.IsTrue(cat.Ccy.Equals(Init.Ccy3()));
        }

        [TestMethod]
        public void Category_GetItem()
        {
            Category cat = Init.CreateCategory1();
            NodeAddress na = new NodeAddress("Root\\Category1");
            Category cat_found = (Category)cat.GetItem(na);
            bool test1 = cat.Equals(cat_found);
            NodeAddress na2 = new NodeAddress("Root\\Category1\\Institution2");
            Institution instit_found = (Institution)cat.GetItem(na2);
            bool test2 = cat.GetInstitution("Institution2").Equals(instit_found);
            NodeAddress na3 = new NodeAddress("Root\\Category1\\Institution1\\Wallet");
            Account acc_found = (Account)cat.GetItem(na3);
            bool test3 = cat.GetInstitution("Institution1").GetAccount("Wallet").Equals(acc_found);
            Assert.IsTrue(test1 && test2 && test3);
        }

        [TestMethod]
        public void Category_GetTreeStructure()
        {
            Category cat = Init.CreateCategory1();
            TreeViewMapping tvm = cat.GetTreeStructure();
            Console.WriteLine(tvm);
            Assert.IsTrue(tvm.Count == 7);
        }

        [TestMethod]
        public void Category_GetNodeType()
        {
            Category cat = Init.CreateCategory1();
            Assert.IsTrue(cat.GetNodeType() == NodeType.Category);
        }

        [TestMethod]
        public void Category_GetTotalAccount()
        {
            Category instit = Init.CreateCategory1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            IAccount totAcc = instit.GetTotalAccount(FXMkt, aMkt, Init.Ccy2());
            Assert.IsTrue(new Price(1000 * 1.1 + 0.0001 * 15000 * 1.1 + 1500 + 1 *100, Init.Ccy2()) == totAcc.Value);
        }

        #endregion

        #region ICategory

        [TestMethod]
        public void Category_CategoryName()
        {
            Category cat = Init.CreateCategory2();
            Assert.IsTrue(cat.CategoryName == cat.GetName());
        }

        #endregion
    }
}
