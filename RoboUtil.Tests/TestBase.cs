using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoboUtil.Tests
{
    [TestClass]
    public class TestBase: IDisposable
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public void Login(string username, bool NeedNewLogin)
        {
            
        }
        public void LogOut()
        {

        }

        [TestInitialize()]
        public void Initialize()
        {          

            //start your service 

            // start client connect
        }

        [TestCleanup()]
        public void Cleanup()
        {
            //stop WCF
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        public void Dispose()
        {
            
        }

        

    }
}
