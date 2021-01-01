using Core.Statics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class StaticsTests
    {
        /// Value To String

        [TestMethod]
        public void DollarValueToString()
        {
            CurrencyStatics USDcs = new CurrencyStatics("$", 3, 2);
            bool test1 = USDcs.ValueToString(12345678) == "$ 12,345,678";
            bool test2 = USDcs.ValueToString(12345678.910121) == "$ 12,345,678.91";
            bool test3 = USDcs.ValueToString(1234.0412) == "$ 1,234.04";
            bool test4 = USDcs.ValueToString(-1234.0412) == "$ -1,234.04";
            Assert.IsTrue(test1 && test2 && test3 && test4);
        }

        [TestMethod]
        public void YenValueToString()
        {
            CurrencyStatics JPYcs = new CurrencyStatics("¥", 4, 0);
            bool test1 = JPYcs.ValueToString(12345678) == "¥ 1234,5678";
            bool test2 = JPYcs.ValueToString(12345678.910121) == "¥ 1234,5679";
            bool test3 = JPYcs.ValueToString(1234.0412) == "¥ 1234";
            bool test4 = JPYcs.ValueToString(-1234567891.0412) == "¥ -12,3456,7891";
            Assert.IsTrue(test1 && test2 && test3 && test4);
        }

        [TestMethod]
        public void BitcoinValueToString()
        {
            CurrencyStatics JPYcs = new CurrencyStatics("฿", 3, 8);
            bool test1 = JPYcs.ValueToString(0.123456789) == "฿ 0.12345679";
            bool test2 = JPYcs.ValueToString(0.12345678910121) == "฿ 0.12345679";
            bool test3 = JPYcs.ValueToString(1234.0012340412) == "฿ 1,234.00123404";
            bool test4 = JPYcs.ValueToString(-0.0012345678910412) == "฿ -0.00123457";
            Assert.IsTrue(test1 && test2 && test3 && test4);
        }
    }
}
