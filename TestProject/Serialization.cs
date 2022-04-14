using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Statics;
using Core.Finance;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using Core;
using System.Collections.Generic;

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

        

        [TestMethod]
        public void FxMarket()
        {
            FXMarket mkt = Init.CreateFXMarket();
            string fileName = SerializeObject(mkt, "FXMarket");
            FXMarket desMkt = DeserializeObject<FXMarket>(fileName);
            Assert.IsTrue(mkt == desMkt);
        }


        [TestMethod]
        public void AssetMarket()
        {
            AssetMarket aMkt = Init.CreateAssetMarket();
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
            CurrencyStatics cs = new CurrencyStatics("USD", "$", 3, 2);
            string fileName = SerializeObject(cs, "CurrencyStatics");
            CurrencyStatics desCs = DeserializeObject<CurrencyStatics>(fileName);
            Assert.IsTrue(cs == desCs);
        }

        private CurrencyAssetStaticsDataBase GetCcyDB()
        {
            CurrencyAssetStaticsDataBase casdb = new global::Core.Statics.CurrencyAssetStaticsDataBase();
            casdb.AddCcy("GBP", new CurrencyStatics("GBP", "£", 3, 2));
            casdb.AddCcy("EUR", new CurrencyStatics("EUR", "€", 3, 2, new CurrencyPair("GBP","EUR")));
            casdb.AddCcy("JPY", new CurrencyStatics("JPY", "¥", 4, 0, new CurrencyPair("GBP", "JPY")));
            casdb.RefCcy = new Currency("GBP");
            casdb.AddAsset("BNP", new AssetStatics("BNP", new Currency("EUR")));
            return casdb;
        }

        [TestMethod]
        public void CurrencyAssetStaticsDataBase()
        {
            CurrencyAssetStaticsDataBase casdb = GetCcyDB();
            string fileName = SerializeObject(casdb, "CurrencyAssetStaticsDB");
            CurrencyAssetStaticsDataBase desCasdb = DeserializeObject<CurrencyAssetStaticsDataBase>(fileName);
            Assert.IsTrue(casdb == desCasdb);
        }

        #endregion

        #region Core

        [TestMethod]
        public void Account_Ccy()
        {
            Account acc = Init.CreateAccountCurrency1();
            string fileName = SerializeObject(acc, "Account_Ccy");
            Account desAcc = DeserializeObject<Account>(fileName);
            Assert.IsTrue(acc == desAcc);
        }

        [TestMethod]
        public void Account_Asset()
        {
            Account acc = Init.CreateAccountAsset1();
            string fileName = SerializeObject(acc, "Account_Asset");
            Account desAcc = DeserializeObject<Account>(fileName);
            Assert.IsTrue(acc == desAcc);
        }

        [TestMethod]
        public void Institution()
        {
            Institution instit = Init.CreateInstitution1();
            string fileName = SerializeObject(instit, "Institution");
            Institution desInstit = DeserializeObject<Institution>(fileName);
            Assert.IsTrue(instit == desInstit);
        }

        [TestMethod]
        public void Category()
        {
            Category cat = Init.CreateCategory1();
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

        //[TestMethod]
        //public void TreeViewMapping()
        //{
        //    List<Category> list = new List<Category> { Init.CreateCategory1(), Init.CreateCategory2() };
        //    TreeViewMapping tvm = new TreeViewMapping(list);
        //    string fileName = SerializeObject(tvm, "TreeViewMapping");
        //    TreeViewMapping desTvm = DeserializeObject<TreeViewMapping>(fileName);
        //    Assert.IsTrue(tvm == desTvm);
        //}

        //[TestMethod]
        //public void AccountingData()
        //{
        //    List<Category> list = new List<Category> { Init.CreateCategory1(), Init.CreateCategory2() };
        //    AccountingData ad = new AccountingData(list, Init.CreateFXMarket(), Init.CreateAssetMarket());
        //    string fileName = SerializeObject(ad, "AccountingData");
        //    AccountingData desAd = DeserializeObject<AccountingData>(fileName);
        //    Assert.IsTrue(ad == desAd);
        //}



        #endregion

        //[TestMethod]
        //public void HistoricalAccountingData()
        //{
        //    HistoricalAccountingData had = new HistoricalAccountingData();
        //    had.SetCcyDB(GetCcyDB());

        //    // Create AccData 1
        //    FXMarket fx1 = Init.CreateFXMarket();
        //    fx1.CcyRef = had.CcyDB.RefCcy;
        //    fx1.AddQuote(new CurrencyPair("GBP", "EUR"), 1.1);
        //    fx1.AddQuote(new CurrencyPair("GBP", "JPY"), 130);
        //    AssetMarket amkt1 = Init.CreateAssetMarket();
        //    amkt1.AddQuote(new AssetCcyPair(new Asset("AAPL"), new Currency("USD")), 1234.56);
        //    amkt1.PopulateWithFXMarket(fx1);
        //    List<Category> list = new List<Category> { Init.CreateCategory1(), Init.CreateCategory2() };
        //    AccountingData ad1 = new AccountingData(list, fx1, amkt1);
        //    had.AddData(new DateTime(2020, 1, 1), ad1);

        //    // Create AccData2
        //    FXMarket fx2 = new FXMarket();
        //    fx2.Copy(fx1);
        //    fx2.AddQuote(new CurrencyPair("EUR", "USD"), 1.2);
        //    fx2.AddQuote(new CurrencyPair("GBP", "EUR"), 1.15);
        //    AssetMarket amkt2 = new AssetMarket();
        //    amkt2.Copy(amkt1);
        //    amkt2.AddQuote(new AssetCcyPair(new Asset("BNP"), new Currency("EUR")), 50.0);
        //    amkt2.PopulateWithFXMarket(fx2);
        //    AccountingData ad2 = new AccountingData(list, fx2, amkt2);
        //    had.AddData(new DateTime(2020, 2, 3), ad2);

        //    // Test
        //    string fileName = SerializeObject(had, "HistoricalAccountingData");
        //    HistoricalAccountingData desHad = DeserializeObject<HistoricalAccountingData>(fileName);
        //    Assert.IsTrue(had == desHad);
        //}
    }
}
