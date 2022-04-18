using Core;
using Core.Finance;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject;

namespace Test.Core
{
    [TestClass]
    public class InstitutionTest
    {
        [TestMethod]
        public void Institution_CcyRef()
        {
            Institution instit = Init.CreateInstitution1();
            Assert.IsTrue(instit.CcyRef == Init.Ccy4());
        }

        [TestMethod]
        public void Institution_InstitutionName()
        {
            Institution instit = Init.CreateInstitution2();
            Assert.IsTrue(instit.GetName() == instit.InstitutionName);
        }

        //[TestMethod]
        //public void Institution_ItemList()
        //{
        //    Institution instit = Init.CreateInstitution2();
        //    string name_test = "Checking2";
        //    string name_test_2 = "Stocks";
        //    TreeViewMappingElement tvme = new TreeViewMappingElement("TEST");
        //    IEnumerable<IAccountingElement> list_0 = instit.GetItemList(tvme);
        //    bool test_0 = list_0.Count() == 0;
        //    tvme.AddElement(new TreeViewMappingElement(name_test));
        //    IEnumerable<IAccountingElement> list_1 = instit.GetItemList(tvme);
        //    bool test_1 = (list_1.Count() == 1) && list_1.First().GetName() == name_test;
        //    tvme.AddElement(new TreeViewMappingElement(name_test_2));
        //    IEnumerable<IAccountingElement> list_2 = instit.GetItemList(tvme);
        //    bool test_2 = (list_2.Count() == 2);
        //    Assert.IsTrue(test_0 && test_1 && test_2);
        //}

        [TestMethod]
        public void Institution_NodeTypeTest()
        {
            Institution instit = Init.CreateInstitution3();
            Assert.IsTrue(instit.GetNodeType() == NodeType.Institution);
        }

        [TestMethod]
        public void Institution_TotalAccount()
        {
            Institution instit = Init.CreateInstitution1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            IAccount totAcc = instit.GetTotalAccount(FXMkt, aMkt, FXMkt.CcyRef);
            Assert.IsTrue(new Price(1000 * 1.1 + 0.0001 * 15000 * 1.1, FXMkt.CcyRef) == totAcc.Value);
        }

        [TestMethod]
        public void Institution_GetAccount()
        {
            Institution instit = Init.CreateInstitution3();
            Assert.IsTrue(instit.GetAccount("ExtraAccount") == Init.CreateAccountCurrency3());
        }

        [TestMethod]
        public void Institition_GetSummary()
        {
            Institution instit = Init.CreateInstitution2();
            SummaryReport summ = instit.GetSummary();
            Assert.IsTrue(
                summ.Count == 2
                && summ.Get(Init.Ccy2()) == instit.GetAccount("Checking2").Amount
                && summ.Get(Init.Asset2()) == instit.GetAccount("Stocks").Amount
            );
        }

        [TestMethod]
        public void Institution_AddAccount()
        {
            Institution instit = Init.CreateInstitution1();
            instit.AddAccount(Init.CreateAccountCurrency3());
            Assert.IsTrue(instit.Accounts.Last() == Init.CreateAccountCurrency3());
        }

        [TestMethod]
        public void Institution_AddAccount_2()
        {
            Institution instit = Init.CreateInstitution1();
            Account new_acc = Init.CreateAccountCurrency3();
            instit.AddAccount(new_acc.AccountName, new_acc.Ccy, new_acc.Amount);
            Assert.IsTrue(instit.Accounts.Last() == Init.CreateAccountCurrency3());
        }

        [TestMethod]
        public void Institution_AddNewAccount()
        {
            Institution instit = Init.CreateInstitution2();
            instit.AddNewAccount();
            instit.AddNewAccount();
            var names = instit.Accounts.Select(x => x.AccountName).ToList();
            bool test = names.Count() == 4;
            if (test) { 
                test = names[2] == "New Account" 
                    && names[3] == "New Account - 1"
                    && instit.GetAccount("New Account").Ccy == instit.Ccy; }
            Assert.IsTrue(test);
        }

        [TestMethod]
        public void Institution_ChangeName()
        {
            Institution instit = Init.CreateInstitution2();
            instit.ChangeName("Stocks", "Schtonks");
            var names = instit.AccountNames.ToList();
            Assert.IsTrue(names[0] == "Checking2" && names[1] == "Schtonks");
        }
    }
}