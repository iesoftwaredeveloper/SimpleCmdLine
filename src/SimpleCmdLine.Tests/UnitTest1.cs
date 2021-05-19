using System;
using Xunit;
using SimpleCmdLine;

namespace SimpleCmdLine.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void NoArgs_ReturnsFail()
        {
            string[] args = new string[]{};
            var result = Program.Main(args);
            Assert.Equal(1, result);
        }

        [Fact]
        public void OptionNoArgument_ReturnsFail()
        {
            string[] args = new string[]{ "--name" };
            var result = Program.Main(args);
            Assert.Equal(1, result);
        }

        [Fact]
        public void OptionWithArgument_ReturnsFail()
        {
            string[] args = new string[]{ "--name", "Success"};
            var result = Program.Main(args);
            Assert.Equal(0, result);
        }
    }
}
