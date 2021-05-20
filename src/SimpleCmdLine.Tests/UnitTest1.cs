using SimpleCmdLine;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Xunit;
using System.CommandLine.Parsing;

namespace SimpleCmdLine.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void NoArgs_ReturnsFail()
        {
            string[] args = new string[] { };
            
            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;
            Assert.Equal(1, Errors.Count);
        }

        [Fact]
        public void OptionNoArgument_ReturnsFail()
        {
            string[] args = new string[] { "--name" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;
            Assert.Equal(1, Errors.Count);
        }

        [Fact]
        public void OptionWithArgument_ReturnsFail()
        {
            string[] args = new string[] { "--name", "Success" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;
            Assert.Equal(0, Errors.Count);
        }

    }
}
