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

namespace TestProject.Core
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

        [TestMethod]
        public void Institution_GetItem()
        {
            Institution instit = Init.CreateInstitution2();
            NodeAddress na = new NodeAddress("Root\\Cat\\Institution2");
            Institution inst_found = (Institution)instit.GetItem(na);
            bool test1 = instit.Equals(inst_found);
            NodeAddress na2 = new NodeAddress("Root\\Cat\\Institution2\\Checking2");
            Account acc_found = (Account)instit.GetItem(na2);
            bool test2 = instit.GetAccount("Checking2").Equals(acc_found);
            Assert.IsTrue(test1 && test2);
        }

        [TestMethod]
        public void Institution_TreeStructure()
        {
            Institution instit = Init.CreateInstitution2();
            TreeViewMapping tvm = instit.GetTreeStructure();
            Assert.IsTrue(tvm.Count == 3);
        }

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
            instit.ChangeName(new NodeAddress("Root\\Cateogry\\Institution2\\Stocks"), "Schtonks");
            var names = instit.AccountNames.ToList();
            Assert.IsTrue(names[0] == "Checking2" && names[1] == "Schtonks");
        }
    }
}