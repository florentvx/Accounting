using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Finance;

namespace TestProject
{
    public static class Init
    {

        public static Currency Ccy1()
        {
            return new Currency("EUR");
        }

        public static Currency Ccy2()
        {
            return new Currency("USD");
        }

        public static Currency Ccy3()
        {
            return new Currency("GBP");
        }

        public static Currency Ccy4()
        {
            return new Currency("JPY");
        }

        public static Asset Asset1()
        {
            return new Asset("BTC");
        }

        public static Asset Asset2()
        {
            return new Asset("AAPL");
        }

        public static Asset Asset3()
        {
            return new Asset("BNP");
        }

        public static Account CreateAccountCurrency1()
        {
            return new Account("Checking", Ccy1(), 1000);
        }

        public static Account CreateAccountCurrency2()
        {
            return new Account("Checking2", Ccy2(), 1500);
        }

        public static Account CreateAccountCurrency3()
        {
            return new Account("ExtraAccount", Ccy3(), 100.0);
        }

        public static Account CreateAccountAsset1()
        {
            return new Account("Wallet", Asset1(), 0.0001);
        }

        public static Account CreateAccountAsset2()
        {
            return new Account("Stocks", Asset2(), 1.0);
        }

        public static Institution CreateInstitution1()
        {
            Institution instit = new Institution("Institution1", Ccy4());
            instit.AddAccount(CreateAccountCurrency1());
            instit.AddAccount(CreateAccountAsset1());
            return instit;
        }

        public static Institution CreateInstitution2()
        {
            Institution instit = new Institution("Institution2", Ccy2());
            instit.AddAccount(CreateAccountCurrency2());
            instit.AddAccount(CreateAccountAsset2());
            return instit;
        }

        public static Institution CreateInstitution3()
        {
            Institution instit = new Institution("Institution3", Ccy1());
            instit.AddAccount(CreateAccountCurrency3());
            return instit;
        }

        public static Category CreateCategory1()
        {
            Category cat = new Category("Category1", Ccy3());
            cat.AddInstitution(CreateInstitution1());
            cat.AddInstitution(CreateInstitution2());
            return cat;
        }

        public static Category CreateCategory2()
        {
            Category cat = new Category("Category", Ccy1());
            cat.AddInstitution(CreateInstitution3());
            return cat;
        }

        public static FXMarket CreateFXMarket()
        {
            FXMarket mkt = new FXMarket(Ccy2());
            mkt.AddQuote(new CurrencyPair(Ccy1(), Ccy2()), 1.1);
            return mkt;
        }

        public static AssetMarket CreateAssetMarket(FXMarket populate_fx = null)
        {
            AssetMarket aMkt = new AssetMarket();
            aMkt.AddQuote(new AssetCcyPair(Asset3(), Ccy1()), 48.5);
            aMkt.AddQuote(new AssetCcyPair(Asset1(), Ccy1()), 15000.0);
            if (populate_fx != null)
                aMkt.PopulateWithFXMarket(populate_fx);
            return aMkt;
        }
    }
}
