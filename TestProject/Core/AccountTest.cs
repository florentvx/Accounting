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
        public void ItemsList()
        {
            TreeViewMappingElement tvme = new TreeViewMappingElement("test");
            Account acc_ccy = Init.CreateAccountCurrency1();
            bool test1 = acc_ccy.GetItemList().Count() == 0;
            bool test1bis = acc_ccy.GetItemList(tvme).Count() == 0;
            Account acc_asset = Init.CreateAccountAsset1();
            bool test2 = acc_asset.GetItemList().Count() == 0;
            bool test2bis = acc_asset.GetItemList(tvme).Count() == 0;
            Assert.IsTrue(test1 && test1bis && test2 && test2bis);
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

        //[TestMethod]
        //public void TotalAccount_CcyTest()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    FXMarket FXMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket();
        //    IAccount totAcc = acc.GetTotalAccount(FXMkt, aMkt, acc.Ccy.Ccy);
        //    Assert.IsTrue(totAcc.Equals(acc));
        //}

        //[TestMethod]
        //public void TotalAccount_AssetTest()
        //{
        //    Account acc = Init.CreateAccountAsset1();
        //    FXMarket FXMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket(FXMkt);
        //    IAccount totAcc = acc.GetTotalAccount(FXMkt, aMkt, FXMkt.CcyRef);
        //    Assert.IsTrue(totAcc.Equals(acc));
        //}

        //[TestMethod]
        //public void ModifyAmount_CcyTest()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    FXMarket FXMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket();
        //    acc.ModifyAmount(FXMkt, aMkt, "TEST", "10,000");
        //    Assert.IsTrue(acc.Amount == 10000);
        //}

        //[TestMethod]
        //public void ModifyCcy_CcyTest()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    double amount = acc.Amount;
        //    FXMarket fxMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket();
        //    acc.ModifyCcy(fxMkt, aMkt, "NOT USED", Init.Ccy2(), false); //last input not used for Accounts
        //    Assert.IsTrue(  
        //                    acc.Ccy.Ccy == Init.Ccy2()
        //                    && acc.Amount == amount 
        //                    && acc.ConvertedAmount == amount * fxMkt.GetQuote(new CurrencyPair(Init.Ccy2(), Init.Ccy1()))
        //                );
        //}

        //[TestMethod]
        //public void ModifyTotalCcy_CcyTest()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    FXMarket fxMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket();
        //    acc.ModifyTotalCcy(fxMkt, aMkt, fxMkt.CcyRef);
        //    Assert.IsTrue(  
        //                    acc.TotalCcy == fxMkt.CcyRef
        //                    && acc.TotalAmount == acc.Amount * fxMkt.GetQuote(new CurrencyPair(Init.Ccy1(), Init.Ccy2()))
        //                );
        //}

        //[TestMethod]
        //public void ModifyTotalCcy_AssetTest()
        //{
        //    Account acc = Init.CreateAccountAsset1();
        //    FXMarket fxMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket(fxMkt);
        //    acc.ModifyTotalCcy(fxMkt, aMkt, fxMkt.CcyRef);
        //    Assert.IsTrue(  
        //                    acc.TotalCcy == fxMkt.CcyRef
        //                    && acc.TotalAmount == acc.Amount * aMkt.GetQuote(new AssetCcyPair(Init.Asset1(), Init.Ccy2()))
        //                );
        //}

        //[TestMethod]
        //public void ModifyAmount_AssetTest()
        //{
        //    Account acc = Init.CreateAccountAsset1();
        //    FXMarket FXMkt = Init.CreateFXMarket();
        //    acc.ConvertedCcy = FXMkt.CcyRef;
        //    AssetMarket aMkt = Init.CreateAssetMarket(FXMkt);
        //    acc.ModifyTotalCcy(FXMkt, aMkt, FXMkt.CcyRef);
        //    acc.ModifyAmount(FXMkt, aMkt, "NOT USED", "1.00");
        //    Assert.IsTrue(acc.Amount == 1);
        //}

        //[TestMethod]
        //public void ModifyCcy_AssetTest()
        //{
        //    Account acc = Init.CreateAccountAsset1();
        //    double amount = acc.Amount;
        //    FXMarket fxMkt = Init.CreateFXMarket();
        //    acc.ConvertedCcy = fxMkt.CcyRef;
        //    AssetMarket aMkt = Init.CreateAssetMarket(fxMkt);
        //    acc.ModifyTotalCcy(fxMkt, aMkt, fxMkt.CcyRef);
        //    acc.ModifyCcy(fxMkt, aMkt, "NOT USED", Init.Asset3(), false); //last input not used for Accounts
        //    bool testAmount = acc.ConvertedAmount == amount * aMkt.GetQuote(new AssetCcyPair(Init.Asset3(), fxMkt.CcyRef));
        //    Assert.IsTrue(
        //                    acc.Ccy.Asset == Init.Asset3()
        //                    && acc.Amount == amount
        //                    && testAmount
        //                );
        //}

        //[TestMethod]
        //public void GetSummary_CcyTest()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    SummaryReport summ = acc.GetSummary();
        //    Assert.IsTrue(  
        //                    summ.Count == 1
        //                    && summ.Get(Init.Ccy1()) == acc.Amount
        //                );
        //}

        //[TestMethod]
        //public void GetSummary_AssetTest()
        //{
        //    Account acc = Init.CreateAccountAsset1();
        //    SummaryReport summ = acc.GetSummary();
        //    Assert.IsTrue(
        //                    summ.Count == 1
        //                    && summ.Get(Init.Asset1()) == acc.Amount
        //                );
        //}

        //[TestMethod]
        //public void GetTotalAmount()
        //{
        //    Account acc = Init.CreateAccountCurrency1();
        //    FXMarket fxMkt = Init.CreateFXMarket();
        //    AssetMarket aMkt = Init.CreateAssetMarket();
        //    acc.ModifyTotalCcy(fxMkt, aMkt, Init.Ccy2());
        //    double tot = acc.GetTotalAmount(Init.Ccy1(), fxMkt).Value;
        //    Assert.IsTrue(tot == acc.Amount);
        //}


    }
}
