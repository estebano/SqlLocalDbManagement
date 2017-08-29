using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlLocalDBManagementLib;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var basic = new Basic();
            var names = basic.GetInstancesNames();
            var infos = basic.GetInstanceStatus(names[0]);
            var result = basic.Start(names[0]);
        }
    }
}
