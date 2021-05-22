using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace SimpleCmdLine
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Create our own console so we can control where the output is written to.
            IConsole console = new CustomConsole(StandardStreamWriter.Create(tw), StandardStreamWriter.Create(tw));

            var rootCommand = AddOptions(args);

            rootCommand.Handler = CommandHandler.Create((ParseResult parseResult, IConsole console, RootOptions options) =>
                {
                    if (parseResult == null)
                    {
                        Console.WriteLine("No parseResult for root");
                        return;
                    }
                    if (console == null)
                    {
                        Console.WriteLine("No console for root");
                        return;
                    }
                    if (options == null)
                    {
                        Console.WriteLine("No options for root");
                        return;
                    }
                    // Bug fix work-around. https://github.com/dotnet/command-line-api/issues/1284
                    if (parseResult.HasOption("--opt-decimal"))
                    {
                        options.SetOptDecimal(parseResult.ValueForOption<decimal>("--opt-decimal"));
                    }
                    rootHandler(parseResult, console, options);
                });

            await rootCommand.InvokeAsync(args, console);
            Console.Write(StdOut); // Output anything collected to standard console.
            return 0;
        }

        public static void rootHandler(ParseResult parseResult, IConsole console, RootOptions opt)
        {
            console.Out.WriteLine("Parsing root");
            console.Out.WriteLine($"{parseResult}");
            console.Out.WriteLine($"--name {opt.Name}");
            console.Out.WriteLine($"--opt-int {opt.OptInt}");
            console.Out.WriteLine($"--opt-decimal {opt.OptDecimal}");
            console.Out.WriteLine($"--opt-bool={opt.OptBool}");
            if (opt.OptString.Count() > 0)
                console.Out.WriteLine($"--opt-string={opt.OptString.Aggregate((a, b) => $"{a}, {b}")}");
        }

        public static void addHandler(ParseResult parseResult, IConsole console, int operand1, int operand2, IList<int> operands)
        {
            console.Out.WriteLine("Processing add");

            console.Out.WriteLine((operands.Sum()).ToString());
            console.Out.WriteLine((operand1+operand2).ToString());
        }

        public static void subtractHandler(ParseResult parseResult, IConsole console, IList<int> operands)
        {
            console.Out.WriteLine("Processing subtraction");

            if(operands.Count == 0)
                return;

            if(operands.Count == 1)
            {
                operands = operands.Prepend(0).ToList();
            }
            var difference = operands.Aggregate<int>((total, next) =>  total - next);
            console.Out.WriteLine(difference.ToString());
        }

        public static RootCommand AddOptions(string[] args)
        {
            var rootCommand = new RootCommand()
            {
                Description = "Console app to demonstrate System.CommandLine"
            };

            // Create an option called --name that is a string and is required.
            var optName = new Option<string>("--name", description: "Name of person to greet") { IsRequired = true };
            // Add an additional alias for the --name option.
            optName.AddAlias("-n");
            // Add the option to the root command.
            rootCommand.Add(optName);

            // Create and add a new option to the root command.
            // This option is an integer and is not required.
            // Here we pass an array of aliases rather than adding an alias after the initial option is created.
            // We also provide a default value of 47
            rootCommand.Add(new Option<int>(new[] { "--opt-int", "-i" }, description: "An integer option", getDefaultValue: () => { return 47; }));

            rootCommand.Add(new Option<decimal>(new[] { "--opt-decimal" }, description: "A decimal option"));
            rootCommand.Add(new Option<bool>(new[] { "--opt-bool" }, description: "A boolean option"));
            rootCommand.Add(new Option<string>("--opt-string", description: "A string option", arity: new ArgumentArity(1, 2)));

            var addCommand = new Command("add");
            var subtractCommand = new Command("subtract");

            addCommand.AddArgument(new Argument<int>("Operand1"));
            addCommand.AddArgument(new Argument<int>("Operand2"));
            addCommand.Add(new Option<int[]>("--operands", description: "A list of numbers to sum.", arity: ArgumentArity.OneOrMore));

            addCommand.Handler = CommandHandler.Create((ParseResult parseResult, IConsole console, int operand1, int operand2, IList<int> operands) =>
            {
                addHandler(parseResult, console, operand1, operand2, operands);
            });

            subtractCommand.Add(new Option<int[]>("--operands", description: "A list of numbers to subtract.", arity: ArgumentArity.OneOrMore) { IsRequired = true});
            
            subtractCommand.Handler = CommandHandler.Create((ParseResult parseResult, IConsole console, IList<int> operands) =>
            {
                subtractHandler(parseResult, console, operands);
            });

            rootCommand.AddCommand(addCommand);
            rootCommand.AddCommand(subtractCommand);

            return rootCommand;
        }


        /// <summary>
        /// Create our own custom IConsole so we can capture the output of CommandLine
        /// </summary>
        public class CustomConsole : IConsole
        {
            public CustomConsole(IStandardStreamWriter stdOut = null, IStandardStreamWriter stdError = null)
            {
                if (stdOut != null)
                {
                    Out = stdOut;
                }
                else
                {
                    Out = StandardStreamWriter.Create(Console.Out);
                }

                if (stdError != null)
                {
                    Error = stdError;
                }
                else
                {
                    Error = StandardStreamWriter.Create(Console.Error);
                }
            }
            public IStandardStreamWriter Out { get; }
            public IStandardStreamWriter Error { get; }

            public bool IsOutputRedirected { get; } = false;

            public bool IsErrorRedirected { get; } = false;

            public bool IsInputRedirected { get; } = false;
        }

        /// <summary>
        /// Create a text writer for the CustomConsole to use
        /// </summary>
        static MemoryStream ms = new MemoryStream();
        static TextWriter tw = new StreamWriter(ms);

        /// <summary>
        /// Access the current contents of the MemoryStream
        /// </summary>
        /// <returns>The contents of MemoryStream as a string</returns>
        public static string StdOut
        {
            get
            {
                if (ms == null)
                {
                    return String.Empty;
                }
                tw.Flush();

                ms.Position = 0;

                return new StreamReader(ms).ReadToEnd();
            }
        }
    }
}
