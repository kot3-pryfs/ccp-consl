using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Lab1_2V10;

namespace UnitTestLab2
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void GetSummBySpecificCatalog_ThrowsArgumentException()
        {
            var _rp = new ResultsProcessing();
            _rp.ReadFromFile($@"D:\Ynic\Lab2V10\Lab2V10\Lab2V10\Data.txt");
            Assert.ThrowsException<ArgumentException>(() => _rp.GetSummBySpecificCatalog("fgfg"));
        }

        [TestMethod]
        public void GetSummBySpecificCatalog_ReturnsCorrectResult()
        {
            var _rp = new ResultsProcessing();
            _rp.ReadFromFile($@"D:\Ynic\Lab2V10\Lab2V10\Lab2V10\Data.txt");
            var summResult= _rp.GetSummBySpecificCatalog("Промтовары");
            Assert.AreEqual(summResult, 7052);
        }
    }
}
