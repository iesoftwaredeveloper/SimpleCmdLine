using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace SimpleCmdLine
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = AddOptions(args);

            rootCommand.Handler = CommandHandler.Create<string>(RootCmd);

            return rootCommand.Invoke(args);
        }

        public static void RootCmd(string name = "World")
        {
            Console.WriteLine($"Hello {name}!");
        }

        public static RootCommand AddOptions(string[] args)
        {
            var rootCommand = new RootCommand()
            {
                Description = "Console app to demonstrate System.CommandLine"
            };

            var optName = new Option<string>("--name", description: "Name of person to greet") { IsRequired = true };
            optName.AddAlias("-n");
            rootCommand.Add(optName);

            return rootCommand;
        }

    }
}
