using System;
using FreshFruit.DAL;
using FreshFruit.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FreshFruit.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Templates_T1 t = new Templates_T1();
            TemplatesModel m = new TemplatesModel();
            t.AddObject(m);
        }


    }
}
