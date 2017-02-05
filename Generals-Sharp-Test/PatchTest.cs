using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Generals_Sharp;

namespace Generals_Sharp_Test
{
    /// <summary>
    /// Cant say I understand how this works, but it passes the example shown in the tutorial
    /// https://github.com/vzhou842/generals.io-Node.js-Bot-example/blob/master/main.js#L65
    /// https://github.com/vzhou842/generals.io-Node.js-Bot-example/blob/master/main.js#L66
    /// </summary>
    [TestClass]
    public class PatchTest
    {
        [TestMethod]
        public void FirstExample()
        {
            int[] diff = new int[] { 1, 1, 3 };
            int[] old = new int[] { 0, 0 };

            var m = new Main();
            var ret = m.patch(old, diff);

            Assert.AreEqual(ret[0], 0);
            Assert.AreEqual(ret[1], 3);
            Assert.AreEqual(ret.Count(), 2);
        }

        [TestMethod]
        public void SecondExample()
        {
            int[] diff = new int[] { 0, 1, 2, 1 }; // 1, 1, 3 };
            int[] old = new int[] { 0, 0 };

            var m = new Main();
            var ret = m.patch(old, diff);

            Assert.AreEqual(ret[0], 2);
            Assert.AreEqual(ret[1], 0);
            Assert.AreEqual(ret.Count(), 2);
        }
    }
}
