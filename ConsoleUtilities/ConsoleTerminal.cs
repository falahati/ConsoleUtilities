using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUtilities
{
    public class ConsoleTerminal
    {
        private readonly Action<string, string[]> _action;
        private readonly Dictionary<string, Action<string[]>> _actions;
        private readonly string[] _commandHistory;
        private ConsoleTerminalTheme _theme = new ConsoleTerminalTheme();


        public ConsoleTerminal(string pointerName, Dictionary<string, Action<string[]>> actions) : this(pointerName,
            actions, 50)
        {
        }

        public ConsoleTerminal(string pointerName, Dictionary<string, Action<string[]>> actions, int historyLength)
        {
            PointerName = pointerName;
            _commandHistory = new string[historyLength];
            // ReSharper disable once EventExceptionNotDocumented
            _actions = actions.OrderBy(pair => pair.Key.Length).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public ConsoleTerminal(string pointerName, Action<string, string[]> action)
        {
            PointerName = pointerName;
            _action = action;
        }

        public string PointerName { get; }

        public ConsoleTerminalTheme Theme
        {
            get => _theme;
            set => _theme = value ?? new ConsoleTerminalTheme();
        }

        public void RunTerminal()
        {
            do
            {
                var writer = new ConsoleTerminalWriter(PointerName + "> ", Theme, _commandHistory);
                var userInputAsText = writer.ReadCommand();
                writer.Dispose();
                var userInputWithArguments = userInputAsText.Trim().Split(' ');
                var commandName = userInputWithArguments[0].Trim();
                var commandArguments = userInputWithArguments.Skip(1).ToArray();

                if (commandName.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    commandName.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var selectedAction =
                    _actions?.Where(pair =>
                            pair.Key.Trim().Equals(commandName, StringComparison.CurrentCultureIgnoreCase))
                        // ReSharper disable once EventExceptionNotDocumented
                        .Select(pair => pair.Value).FirstOrDefault();

                if (selectedAction == null && _action == null)
                {
                    Console.WriteLine("Bad command. Try again.");

                    continue;
                }

                try
                {
                    selectedAction?.Invoke(commandArguments);
                    _action?.Invoke(commandName, commandArguments);
                }
                catch (Exception e)
                {
                    ConsoleWriter.Default.WriteException(e);
                }
            } while (true);
        }
    }
}