using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Finance;
using Core.Interfaces;
using TestProject;


namespace Test.Core
{
    [TestClass]
    public class AccountTests
    { 
        [TestMethod]
        public void CcyRef()
        {
            Account acc_ccy = Init.CreateAccountCurrency1();
            bool test1 = acc_ccy.Ccy.Equals(Init.Ccy1());
            Account acc_asset = Init.CreateAccountAsset1();
            bool test2 = acc_asset.Ccy.Equals(Init.Asset1());
            Assert.IsTrue(test1 && test2);
        }

        [TestMethod]
        public void Account_GetItem()
        {
            Account acc_ccy = Init.CreateAccountCurrency1();
            NodeAddress na = new NodeAddress("Root\\Cat\\Inst\\Checking");
            Account acc_found = (Account)acc_ccy.GetItem(na);
            Assert.IsTrue(acc_ccy.Equals(acc_found));
        }

        [TestMethod]
        public void Account_TreeStructure()
        {
            Account acc_ccy = Init.CreateAccountCurrency1();
            TreeViewMapping tvm_ccy = acc_ccy.GetTreeStructure();
            bool test1 = tvm_ccy.Count == 1;
            Account acc_asset = Init.CreateAccountAsset1();
            TreeViewMapping tvm_asset = acc_asset.GetTreeStructure();
            bool test2 = tvm_asset.Count == 1;
            Assert.IsTrue(test1 && test2);
        }

        [TestMethod]
        public void NodeTypeTest()
        {
            Account acc_ccy = Init.CreateAccountCurrency1();
            bool test1 = acc_ccy.GetNodeType() == NodeType.Account;
            Account acc_asset = Init.CreateAccountAsset1();
            bool test2 = acc_asset.GetNodeType() == NodeType.Account;
            Assert.IsTrue(test1 && test2);
        }

        [TestMethod]
        public void TotalAccount_CcyTest()
        {
            Account acc = Init.CreateAccountCurrency1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            IAccount totAcc = acc.GetTotalAccount(FXMkt, aMkt, acc.Ccy.Ccy, "Checking");
            Assert.IsTrue(totAcc.Equals(acc));
        }

        [TestMethod]
        public void TotalAccount_AssetTest()
        {
            Account acc = Init.CreateAccountAsset1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            IAccount totAcc = acc.GetTotalAccount(FXMkt, aMkt, FXMkt.CcyRef);
            Price manual_price = FXMkt.Convert(aMkt.PriceAsset(acc.Value), FXMkt.CcyRef);
            Assert.IsTrue(manual_price == totAcc.Value);
        }

        [TestMethod]
        public void ModifyAmount_CcyTest()
        {
            Account acc = Init.CreateAccountCurrency1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            acc.ModifyAmount(10000);
            Assert.IsTrue(acc.Amount == 10000);
        }

        [TestMethod]
        public void ModifyCcy_CcyTest()
        {
            Account acc = Init.CreateAccountCurrency1();
            double amount = acc.Amount;
            FXMarket fxMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            acc.ModifyCcy(Init.Ccy2());
            Assert.IsTrue(
                            acc.Ccy.Ccy == Init.Ccy2()
                            && acc.Amount == amount
                        );
        }

        [TestMethod]
        public void ModifyAmount_AssetTest()
        {
            Account acc = Init.CreateAccountAsset1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            acc.ModifyAmount(1.0);
            Assert.IsTrue(acc.Amount == 1);
        }

        [TestMethod]
        public void ModifyCcy_AssetTest()
        {
            Account acc = Init.CreateAccountAsset1();
            double amount = acc.Amount;
            FXMarket fxMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            acc.ModifyCcy(Init.Asset3()); //last input not used for Accounts
            Assert.IsTrue(
                            acc.Ccy.Asset == Init.Asset3()
                            && acc.Amount == amount
                        );
        }

        [TestMethod]
        public void GetSummary_CcyTest()
        {
            Account acc = Init.CreateAccountCurrency1();
            SummaryReport summ = acc.GetSummary();
            Assert.IsTrue(
                            summ.Count == 1
                            && summ.Get(Init.Ccy1()) == acc.Amount
                        );
        }

        [TestMethod]
        public void GetSummary_AssetTest()
        {
            Account acc = Init.CreateAccountAsset1();
            SummaryReport summ = acc.GetSummary();
            Assert.IsTrue(
                            summ.Count == 1
                            && summ.Get(Init.Asset1()) == acc.Amount
                        );
        }

        [TestMethod]
        public void GetTotalAmount()
        {
            Account acc = Init.CreateAccountCurrency1();
            FXMarket fxMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            Price tot = acc.GetTotalAmount(Init.Ccy1(), fxMkt, aMkt);
            Assert.IsTrue(tot == acc.Value);
        }


    }
}
