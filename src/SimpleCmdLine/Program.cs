using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace SimpleCmdLine
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand()
            {
                Description = "Console app to demonstrate System.CommandLine"
            };

            rootCommand.Add(new Option<string>("--name", description: "Name of person to greet") { IsRequired=true});

            rootCommand.Handler = CommandHandler.Create<string>(RootCmd);

            return rootCommand.Invoke(args);
        }

        public static void RootCmd(string name = "World")
        {
            Console.WriteLine($"Hello {name}!");
        }
    }
}
