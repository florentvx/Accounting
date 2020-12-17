using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Statics;
using Core.Finance;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using Core;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class SeralizationTests
    {
        public string BasePath = @"..\..\TestFile\{name}.json";

        public string SerializeObject(ISerializable input, string testName)
        {
            string fileName = BasePath.Replace("{name}", testName);
            string jsonString = JsonConvert.SerializeObject(input, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
            File.WriteAllText(fileName, jsonString);
            return fileName;
        }

        public T DeserializeObject<T>(string filePath) where T : ISerializable
        {
            T desObject;
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                desObject = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects, NullValueHandling = NullValueHandling.Ignore });
            }
            return desObject;
        }

        #region Finance

        [TestMethod]
        public void Ccy()
        {
            Currency ccy = new Currency("USD");
            string fileName = SerializeObject(ccy, "Ccy");
            Currency desCcy = DeserializeObject<Currency>(fileName);
            Assert.IsTrue(ccy == desCcy);
        }

        [TestMethod]
        public void Asset()
        {
            Asset asset = new Asset("BTC");
            string fileName = SerializeObject(asset, "Asset");
            Asset desAsset = DeserializeObject<Asset>(fileName);
            Assert.IsTrue(asset == desAsset);
        }

        [TestMethod]
        public void CcyPair()
        {
            CurrencyPair cp = new CurrencyPair("EUR", "USD");
            string fileName = SerializeObject(cp, "CcyPair");
            CurrencyPair desCp = DeserializeObject<CurrencyPair>(fileName);
            Assert.IsTrue(cp == desCp);
        }

        [TestMethod]
        public void AssetCcyPair()
        {
            AssetCcyPair acp = new AssetCcyPair(new Asset("Apple"), new Currency("USD"));
            string fileName = SerializeObject(acp, "AssetCcyPair");
            AssetCcyPair desAcp = DeserializeObject<AssetCcyPair>(fileName);
            Assert.IsTrue(acp == desAcp);
        }

        private FXMarket CreateFXMarket()
        {
            FXMarket mkt = new FXMarket(new Currency("USD"));
            mkt.AddQuote(new CurrencyPair("EUR", "USD"), 1.1);
            return mkt;
        }

        [TestMethod]
        public void FxMarket()
        {
            FXMarket mkt = CreateFXMarket();
            string fileName = SerializeObject(mkt, "FXMarket");
            FXMarket desMkt = DeserializeObject<FXMarket>(fileName);
            Assert.IsTrue(mkt == desMkt);
        }

        private AssetMarket CreateAssetMarket()
        {
            AssetMarket aMkt = new AssetMarket();
            aMkt.AddQuote(new AssetCcyPair(new Asset("BNP"), new Currency("EUR")), 48.5);
            aMkt.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("EUR")), 15000.0);
            return aMkt;
        }

        [TestMethod]
        public void AssetMarket()
        {
            AssetMarket aMkt = CreateAssetMarket();
            string fileName = SerializeObject(aMkt, "AssetMarket");
            AssetMarket desAMkt = DeserializeObject<AssetMarket>(fileName);
            Assert.IsTrue(aMkt == desAMkt);
        }

        #endregion

        #region Statics

        [TestMethod]
        public void AssetStatics()
        {
            AssetStatics aS = new AssetStatics("BTC", new Currency("USD"));
            string fileName = SerializeObject(aS, "AssetStatics");
            AssetStatics desAs = DeserializeObject<AssetStatics>(fileName);
            Assert.IsTrue(aS == desAs);
        }

        [TestMethod]
        public void CurrencyStatics()
        {
            CurrencyStatics cs = new CurrencyStatics("$", 3, 2);
            string fileName = SerializeObject(cs, "CurrencyStatics");
            CurrencyStatics desCs = DeserializeObject<CurrencyStatics>(fileName);
            Assert.IsTrue(cs == desCs);
        }

        [TestMethod]
        public void CurrencyAssetStaticsDataBase()
        {
            CurrencyAssetStaticsDataBase casdb = new Core.Statics.CurrencyAssetStaticsDataBase();
            casdb.AddCcy("GBP", new CurrencyStatics("£", 3, 2));
            casdb.AddCcy("EUR", new CurrencyStatics("€", 3, 2));
            casdb.AddCcy("JPY", new CurrencyStatics("¥", 4, 0));
            casdb.RefCcy = new Currency("GBP");
            casdb.AddAsset("BNP", new AssetStatics("BNP", new Currency("EUR")));
            string fileName = SerializeObject(casdb, "CurrencyAssetStaticsDB");
            CurrencyAssetStaticsDataBase desCasdb = DeserializeObject<CurrencyAssetStaticsDataBase>(fileName);
            Assert.IsTrue(casdb == desCasdb);
        }

        #endregion

        #region Core

        private Account CreateAccountCurrency1()
        {
            return new Account("Checking", new Currency("GBP"), 1000);
        }

        private Account CreateAccountCurrency2()
        {
            return new Account("Checking2", new Currency("EUR"), 1500);
        }

        private Account CreateAccountCurrency3()
        {
            return new Account("ExtraAccount", new Asset("EUR"), 100.0);
        }

        private Account CreateAccountAsset1()
        {
            return new Account("Wallet", new Asset("BTC"), 0.0001);
        }

        private Account CreateAccountAsset2()
        {
            return new Account("Stocks", new Asset("AAPL"), 1.0);
        }

        private Institution CreateInstitution1()
        {
            Institution instit = new Institution("Institution1", new Currency("JPY"));
            instit.AddAccount(CreateAccountCurrency1());
            instit.AddAccount(CreateAccountAsset1());
            return instit;
        }

        private Institution CreateInstitution2()
        {
            Institution instit = new Institution("Institution2", new Currency("USD"));
            instit.AddAccount(CreateAccountCurrency2());
            instit.AddAccount(CreateAccountAsset2());
            return instit;   
        }

        private Institution CreateInstitution3()
        {
            Institution instit = new Institution("Institution3", new Currency("EUR"));
            instit.AddAccount(CreateAccountCurrency3());
            return instit;
        }

        private Category CreateCategory1()
        {
            Category cat = new Category("Category1", new Currency("GBP"));
            cat.AddInstitution(CreateInstitution1());
            cat.AddInstitution(CreateInstitution2());
            return cat;
        }

        private Category CreateCategory2()
        {
            Category cat = new Category("Category", new Currency("EUR"));
            cat.AddInstitution(CreateInstitution3());
            return cat;
        }

        [TestMethod]
        public void Account_Ccy()
        {
            Account acc = CreateAccountCurrency1();
            string fileName = SerializeObject(acc, "Account_Ccy");
            Account desAcc = DeserializeObject<Account>(fileName);
            Assert.IsTrue(acc == desAcc);
        }

        [TestMethod]
        public void Account_Asset()
        {
            Account acc = CreateAccountAsset1();
            string fileName = SerializeObject(acc, "Account_Asset");
            Account desAcc = DeserializeObject<Account>(fileName);
            Assert.IsTrue(acc == desAcc);
        }

        [TestMethod]
        public void Institution()
        {
            Institution instit = CreateInstitution1();
            string fileName = SerializeObject(instit, "Institution");
            Institution desInstit = DeserializeObject<Institution>(fileName);
            Assert.IsTrue(instit == desInstit);
        }

        [TestMethod]
        public void Category()
        {
            Category cat = CreateCategory1();
            string fileName = SerializeObject(cat, "Category");
            Category desCat = DeserializeObject<Category>(fileName);
            Assert.IsTrue(cat == desCat);
        }

        private void FillTvme(TreeViewMappingElement tvme)
        {
            TreeViewMappingElement x1 = new TreeViewMappingElement("X1");
            TreeViewMappingElement x2 = new TreeViewMappingElement("X2");
            TreeViewMappingElement x3 = new TreeViewMappingElement("X3");
            TreeViewMappingElement y1 = new TreeViewMappingElement("Y1");
            TreeViewMappingElement y2 = new TreeViewMappingElement("Y2");
            TreeViewMappingElement y3 = new TreeViewMappingElement("Y3");
            TreeViewMappingElement z1 = new TreeViewMappingElement("Z1");
            y1.AddElement(z1);
            x1.AddElement(y1);
            x1.AddElement(y2);
            x2.AddElement(y3);
            tvme.AddElement(x1);
            tvme.AddElement(x2);
            tvme.AddElement(x3);
        }

        [TestMethod]
        public void TreeViewMappingElement()
        {
            TreeViewMappingElement tvme = new TreeViewMappingElement("ROOT");
            FillTvme(tvme);
            string fileName = SerializeObject(tvme, "TreeViewMappingElement");
            TreeViewMappingElement desTvme = DeserializeObject<TreeViewMappingElement>(fileName);
            Assert.IsTrue(tvme == desTvme);
        }

        [TestMethod]
        public void TreeViewMapping()
        {
            List<Category> list = new List<Category> { CreateCategory1(), CreateCategory2() };
            TreeViewMapping tvm = new TreeViewMapping(list);
            string fileName = SerializeObject(tvm, "TreeViewMapping");
            TreeViewMapping desTvm = DeserializeObject<TreeViewMapping>(fileName);
            Assert.IsTrue(tvm == desTvm);
        }

        [TestMethod]
        public void AccountingData()
        {
            List<Category> list = new List<Category> { CreateCategory1(), CreateCategory2() };
            AccountingData ad = new AccountingData(list, CreateFXMarket(), CreateAssetMarket());
            string fileName = SerializeObject(ad, "AccountingData");
            AccountingData desAd = DeserializeObject<AccountingData>(fileName);
            Assert.IsTrue(ad == desAd);
        }

        #endregion
    }
}
