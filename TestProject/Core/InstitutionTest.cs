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
        public void CcyRef()
        {
            Institution instit = Init.CreateInstitution1();
            Assert.IsTrue(instit.CcyRef == Init.Ccy4());
        }

        [TestMethod]
        public void InstitutionName()
        {
            Institution instit = Init.CreateInstitution2();
            Assert.IsTrue(instit.GetName() == instit.InstitutionName);
        }

        [TestMethod]
        public void ItemList()
        {
            Institution instit = Init.CreateInstitution2();
            string name_test = "Checking2";
            string name_test_2 = "Stocks";
            TreeViewMappingElement tvme = new TreeViewMappingElement("TEST");
            IEnumerable<IAccountingElement> list_0 = instit.GetItemList(tvme);
            bool test_0 = list_0.Count() == 0;
            tvme.AddElement(new TreeViewMappingElement(name_test));
            IEnumerable<IAccountingElement> list_1 = instit.GetItemList(tvme);
            bool test_1 = (list_1.Count() == 1) && list_1.First().GetName() == name_test;
            tvme.AddElement(new TreeViewMappingElement(name_test_2));
            IEnumerable<IAccountingElement> list_2 = instit.GetItemList(tvme);
            bool test_2 = (list_2.Count() == 2);
            Assert.IsTrue(test_0 && test_1 && test_2);
        }

        [TestMethod]
        public void NodeTypeTest()
        {
            Institution instit = Init.CreateInstitution3();
            Assert.IsTrue(instit.GetNodeType() == NodeType.Institution);
        }

        [TestMethod]
        public void TotalAccount_AssetTest()
        {
            Institution instit = Init.CreateInstitution1();
            FXMarket FXMkt = Init.CreateFXMarket();
            AssetMarket aMkt = Init.CreateAssetMarket();
            IAccount totAcc = instit.GetTotalAccount(FXMkt, aMkt, FXMkt.CcyRef);
            Assert.IsTrue(new Price(1000 * 1.1 + 0.0001 * 15000 * 1.1, FXMkt.CcyRef) == totAcc.Value);
        }
    }
}