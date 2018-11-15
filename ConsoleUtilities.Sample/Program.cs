using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace ConsoleUtilities.Sample
{
    internal class Program
    {
        private static void Main()
        {
            ConsoleWriter.Default.PrintCaption("Samples");
            ConsoleNavigation.Default.PrintNavigation(new[]
            {
                new ConsoleNavigationItem("Table Sample", (i, o) => TableTest()),
                new ConsoleNavigationItem("Writer Sample", (i, o) => WriterTest()),
                new ConsoleNavigationItem("Navigation Sample", (i, o) => NavigationTest()),
                new ConsoleNavigationItem("Terminal Sample", (i, o) => TerminalTest())
            }, "Select an execution path.");
            ConsoleWriter.Default.PrintWarning("End of application. Press enter to exit.");
            Console.ReadLine();
        }

        private static void NavigationTest()
        {
            ConsoleWriter.Default.PrintCaption("Navigation Sample");
            ConsoleNavigation.Default.PrintNavigation(
                new[]
                {
                    new ConsoleNavigationItem(
                        "Option 1",
                        (i, item) =>
                        {
                            ConsoleNavigation.Default.PrintNavigation(
                                new[]
                                {
                                    new ConsoleNavigationItem(
                                        "Option 1 - 1"
                                    ),
                                    new ConsoleNavigationItem(
                                        "Option 1 - 2"
                                    )
                                });
                        }
                    ),
                    new ConsoleNavigationItem(
                        "Option 2",
                        (i, item) =>
                        {
                            ConsoleWriter.Default.WriteObject(item, 2);
                            ConsoleNavigation.Default.PrintNavigation(
                                new[]
                                {
                                    new ConsoleNavigationItem(
                                        "Option 2 - 1"
                                    ),
                                    new ConsoleNavigationItem(
                                        "Option 2 - 2"
                                    )
                                }
                                , "Do something else");
                        },
                        new
                        {
                            a = new[]
                            {
                                "a",
                                "b"
                            },
                            b = "String",
                            xcz = new
                            {
                                pp = new string[0]
                            }
                        }
                    )
                }
                , "Do something");
            ConsoleWriter.Default.PrintSuccess("End of test execution");
            ConsoleWriter.Default.PrintSeparator();
        }

        private static void TableTest()
        {
            ConsoleWriter.Default.PrintCaption("Table Sample");

            var x = new ConsoleTable(
                new[]
                {
                    "Header 1", "Header 2", "Header 3",
                    "Very long header indeed, and to be sure let add more characters to this header"
                }
            );

            x.Write(new[]
            {
                new[]
                {
                    "Row 1", "Very long row indeed, and to be sure let add more characters to this header", "Row 3",
                    "Row 4"
                },

                new[]
                {
                    "Row 1", "Row 2", "Very long row indeed, and to be sure let add more characters to this header",
                    "Row 4"
                }
            });

            x.Write(new[]
            {
                new[]
                {
                    "Row 1", "Row 2", "Row 3",
                    "Very long row indeed, and to be sure let add more characters to this header"
                },

                new[]
                {
                    "Row 1", "Row 2", "Row 3",
                    "Very long row indeed, and to be sure let add more characters to this header"
                }
            });

            x.Write(
                new string[][]
                {
                }
            );


            x = new ConsoleTable(
                new string[] { }
            );

            x.Write(new[]
            {
                new[]
                {
                    "Row 1", "Very long row indeed, and to be sure let add more characters to this header", "Row 3",
                    "Row 4"
                },

                new[]
                {
                    "Row 1", "Row 2", "Very long row indeed, and to be sure let add more characters to this header",
                    "Row 4"
                }
            });

            x.Write(
                new string[][]
                {
                }
            );

            x = new ConsoleTable(
                new[]
                {
                    "Very long header indeed, and to be sure let add more characters to this header Very long header indeed, and to be sure let add more characters to this header Very long header indeed, and to be sure let add more characters to this header"
                }
            );

            x.Write(
                new string[][]
                {
                }
            );

            ConsoleWriter.Default.PrintSuccess("End of test execution");
            ConsoleWriter.Default.PrintSeparator();
        }

        private static void TerminalTest()
        {
            ConsoleWriter.Default.PrintCaption("Terminal Sample");

            var terminal = new ConsoleTerminal("My Terminal", new Dictionary<string, Action<string[]>>
            {
                {
                    "",
                    delegate
                    {
                        ConsoleWriter.Default.WritePaddedText(
                            $"Write `loading` for multi thread loading test;\r\n`log` for multi thread logging;\r\n`table` for multi thread table redraw;\r\nand `terminal` to open a child terminal.\r\nEnter `exit` or `quit` to exit test.",
                            10, ConsoleColor.Green);
                    }
                },
                {
                    "terminal",
                    strings =>
                    {
                        new ConsoleTerminal("Inner Terminal", (commandName, commandArguments) =>
                        {
                            Console.WriteLine("You wrote `{0}` with arguments `{1}`; write `quit` or `exit` to exit.",
                                commandName, string.Join(" ", commandArguments));
                        }).RunTerminal();
                    }
                },
                {
                    "loading",
                    delegate
                    {
                        Timer timer = null;
                        var progress = 0;
                        timer = new Timer(state =>
                        {
                            Console.CursorLeft = 0;
                            ConsoleWriter.Default.WriteColoredText(progress + "% complete", ConsoleColor.Red);
                            progress += 10;

                            if (progress <= 100)
                            {
                                // ReSharper disable once AccessToModifiedClosure
                                timer?.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                            }
                            else
                            {
                                Console.CursorLeft = 0;
                                Console.CursorTop += 1;

                                ConsoleWriter.Default.WriteColoredTextLine("Done.", ConsoleColor.DarkRed);
                            }
                        }, null, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                    }
                },
                {
                    "log",
                    delegate
                    {
                        Timer timer = null;
                        var eventId = 0;
                        timer = new Timer(state =>
                        {
                            ConsoleWriter.Default.WriteColoredTextLine(
                                $"Event {eventId}: logged some message. Adding more char to make it long. Adding more char to make it long. Adding more char to make it long.",
                                ConsoleColor.Yellow);

                            eventId++;

                            if (eventId <= 10)
                            {
                                // ReSharper disable once AccessToModifiedClosure
                                timer?.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                            }
                            else
                            {
                                ConsoleWriter.Default.WriteColoredTextLine("No more event.", ConsoleColor.DarkYellow);
                            }
                        }, null, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                    }
                },
                {
                    "table",
                    delegate
                    {
                        Timer timer = null;
                        var draws = 0;
                        timer = new Timer(state =>
                        {
                            Console.Clear();
                            Console.CursorTop = 0;
                            Console.CursorLeft = 0;
                            new ConsoleTable(new[] {"Header 1", "Header 2", "Header 3"}).Write(new[]
                            {
                                new[] {"Row 1.1", "Row 1.2", "Row 1.3"},
                                new[] {"Row 2.1", "Row 2.2", "Row 2.3"},
                                new[] {"Row 3.1", "Row 3.2", "Row 3.3"}
                            });

                            draws++;

                            if (draws <= 10)
                            {
                                // ReSharper disable once AccessToModifiedClosure
                                timer?.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                            }
                            else
                            {
                                ConsoleWriter.Default.WriteColoredTextLine("End of table redraws.",
                                    ConsoleColor.DarkCyan);
                            }
                        }, null, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
                    }
                }
            });

            terminal.RunTerminal();
            ConsoleWriter.Default.PrintSuccess("End of test execution");
            ConsoleWriter.Default.PrintSeparator();
        }

        private static void WriterTest()
        {
            ConsoleWriter.Default.PrintCaption("Writer Sample");

            var writer = new ConsoleWriter();
            writer.WriteObject(NetworkInterface.GetAllNetworkInterfaces(), 2);
            Console.WriteLine();

            writer.WriteObject(NetworkInterface.GetAllNetworkInterfaces().ToDictionary(i => i.Name, o => o), 1);
            Console.WriteLine();

            writer.WriteObject(new DirectoryInfo(Environment.CurrentDirectory));
            Console.WriteLine();

            writer.WriteObject(10);
            Console.WriteLine();

            writer.WriteObject(new
            {
                a = new[]
                {
                    "a",
                    "b"
                },
                b = "String",
                xcz = new
                {
                    pp = new string[0]
                }
            }, 2);
            Console.WriteLine();

            writer.WriteException(new Exception("new exception", new Exception("old exception")));
            Console.WriteLine();

            try
            {
                var y = 0;
                var x = 10 / y;
            }
            catch (Exception e)
            {
                writer.WriteException(e);
            }

            Console.WriteLine();


            writer.PrintSuccess("Success message.");
            Console.WriteLine();

            writer.PrintWarning("Warning message.");
            Console.WriteLine();

            writer.PrintError("Error message.");
            Console.WriteLine();

            var response = writer.PrintQuestion("Question");
            writer.PrintMessage($"Answered `{response}`");
            Console.WriteLine();

            var boolResponse = writer.PrintQuestion<bool>("Boolean Question");
            writer.PrintMessage($"Answered `{boolResponse}`");
            Console.WriteLine();

            var intResponse = writer.PrintQuestion<int>("Integer Question");
            writer.PrintMessage($"Answered `{intResponse}`");
            Console.WriteLine();

            ConsoleWriter.Default.PrintSuccess("End of test execution");
            ConsoleWriter.Default.PrintSeparator();
        }
    }
}