
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Worktile.Views;

namespace Worktile.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var page = new LightMainPage();
            await page.UpdateMessageBadgeAsync(2);

            Assert.AreEqual(2, Worktile.App.UnreadMessageCount);
        }
    }
}
