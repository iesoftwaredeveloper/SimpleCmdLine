using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using Xunit;

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
            Assert.Equal(2, Errors.Count);
        }

        [Fact]
        public void OptionNoArgument_ReturnsFail()
        {
            string[] args = new string[] { "--name" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;

            Assert.Equal(2, Errors.Count);
        }

        [Fact]
        public void OptionWithArgument_ReturnsFail()
        {
            string[] args = new string[] { "--name", "Success" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;
            Assert.Equal(1, Errors.Count);
        }

        [Fact]
        public void OptionWithArgumentAlias_ReturnsFail()
        {
            string[] args = new string[] { "-n", "Success" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;
            Assert.Equal(1, Errors.Count);
        }

        [Fact]
        public void OptionalInt_ReturnsDefault47()
        {
            string[] args = new string[] { "-n", "Success", "--opt-int" };

            RootCommand rootCommand = Program.AddOptions(args);

            var result = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = result.Errors;

            Assert.Equal(2, Errors.Count);
        }

        [Fact]
        public void OptionalInt_ReturnsDefault42()
        {
            string[] args = new string[] { "-n", "Success", "--opt-int", "42" };

            RootCommand rootCommand = Program.AddOptions(args);

            var parseResult = rootCommand.Parse(args);
            IReadOnlyCollection<ParseError> Errors = parseResult.Errors;
            var optInt = parseResult.ValueForOption<int>("--opt-int");
            Assert.Equal(1, Errors.Count);
            Assert.Equal(42, optInt);
        }
    }
}
