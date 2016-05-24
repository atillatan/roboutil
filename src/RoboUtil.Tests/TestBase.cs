using System;
using Xunit;

namespace RoboUtil.Tests
{

    public class TestBase : IDisposable
    {
        public TestBase()
        {
            //initialize test
        }

        public void Login(string username, bool NeedNewLogin)
        {

        }
        public void LogOut()
        {

        }

        public void Dispose()
        {

        }

    }
}
