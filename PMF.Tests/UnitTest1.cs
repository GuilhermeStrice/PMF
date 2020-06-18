using PMF.Managers;
using System;
using Xunit;

namespace PMF.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            PackageManager.Start();

            // Do stuff

            PackageManager.Stop();
        }
    }
}
