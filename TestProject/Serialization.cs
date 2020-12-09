using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Statics;
using Core.Finance;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

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
            FXMarket mkt = new FXMarket(new Currency("USD"));
            mkt.AddQuote(new CurrencyPair("EUR", "USD"), 1.1);
            string fileName = SerializeObject(mkt, "FXMarket");
            FXMarket desMkt = DeserializeObject<FXMarket>(fileName);
            Assert.IsTrue(mkt == desMkt);
        }

        [TestMethod]
        public void AssetMarket()
        {
            AssetMarket aMkt = new AssetMarket();
            aMkt.AddQuote(new AssetCcyPair(new Asset("BNP"), new Currency("EUR")), 48.5);
            aMkt.AddQuote(new AssetCcyPair(new Asset("BTC"), new Currency("EUR")), 15000.0);
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
    }
}
