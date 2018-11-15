using System;
using System.Linq;

namespace ConsoleUtilities
{
    public class ConsoleNavigation
    {
        private static ConsoleNavigation _default = new ConsoleNavigation();
        private ConsoleNavigationTheme _theme = new ConsoleNavigationTheme();

        public static ConsoleNavigation Default
        {
            get => _default;
            set => _default = value ?? new ConsoleNavigation();
        }

        public ConsoleNavigationTheme Theme
        {
            get => _theme;
            set => _theme = value ?? new ConsoleNavigationTheme();
        }

        public void PrintNavigation<T>(T[] objects, Action<int, T> action)
        {
            PrintNavigation(objects, action, null);
        }

        public void PrintNavigation<T>(T[] objects, Action<int, T> action, string message)
        {
            PrintNavigation(objects, objects.Select(arg => action).ToArray(), message);
        }

        public void PrintNavigation(string[] captions)
        {
            PrintNavigation(captions, (string) null);
        }

        public void PrintNavigation(string[] captions, string message)
        {
            PrintNavigation(captions, (Action<int, string>[]) null, message);
        }

        public void PrintNavigation(ConsoleNavigationItem[] items)
        {
            PrintNavigation(items, (string) null);
        }

        public void PrintNavigation(ConsoleNavigationItem[] items, string message)
        {
            PrintNavigation(items, items.Select(item => item.IsCallable ? item.Action : null).ToArray(), message);
        }


        protected virtual void PrintNavigation<T>(T[] objects, Action<int, T>[] actions, string message)
        {
            objects = objects ?? new T[0];
            actions = actions ?? objects.Select(arg => (Action<int, T>) null).ToArray();

            if (objects.Length != actions.Length)
            {
                throw new ArgumentException("Number of objects and number of actions should match.", nameof(actions));
            }

            var consoleColor = Console.ForegroundColor;

            while (true)
            {
                if (objects.Length <= 0)
                {
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        Console.ForegroundColor = Theme.MessageColor;
                        Console.Write("Nothing to show. (Press enter to go back): ");
                    }
                    else
                    {
                        Console.ForegroundColor = Theme.MessageColor;
                        Console.WriteLine("Nothing to show.");
                        Console.Write("{0} (Press enter to go back): ", message);
                    }
                }
                else
                {
                    for (var i = 1; i <= objects.Length; i++)
                    {
                        if (actions[i - 1] != null)
                        {
                            Console.ForegroundColor = Theme.ItemNumberColor;
                            Console.Write("[{0:D2}] ", i);
                            Console.ForegroundColor = Theme.ItemNameColor;
                            Console.WriteLine(objects[i - 1].ToString());
                        }
                        else
                        {
                            Console.ForegroundColor = Theme.DisabledItemNumberColor;
                            Console.Write("[--] ");
                            Console.ForegroundColor = Theme.DisabledItemNameColor;
                            Console.WriteLine(objects[i - 1].ToString());
                        }
                    }

                    Console.ForegroundColor = Theme.MessageColor;

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        Console.Write("Press enter to go back: ");
                    }
                    else
                    {
                        Console.Write("{0} (Press enter to go back): ", message);
                    }
                }

                while (true)
                {
                    Console.ForegroundColor = Theme.UserInputColor;
                    var userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = consoleColor;

                        return;
                    }

                    if (int.TryParse(userInput, out var pathIndex) &&
                        pathIndex - 1 >= 0 &&
                        pathIndex - 1 < objects.Length &&
                        actions[pathIndex - 1] != null)
                    {
                        try
                        {
                            actions[pathIndex - 1].Invoke(pathIndex - 1, objects[pathIndex - 1]);
                        }
                        catch (Exception e)
                        {
                            ConsoleWriter.Default.WriteException(e);
                        }

                        break;
                    }

                    Console.ForegroundColor = Theme.MessageColor;
                    Console.Write("Invalid input, try again (Press enter to go back): ");
                }
            }
        }
    }
}