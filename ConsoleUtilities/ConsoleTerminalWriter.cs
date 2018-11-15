using System;
using System.IO;
using System.Text;

namespace ConsoleUtilities
{
    internal class ConsoleTerminalWriter : TextWriter
    {
        private readonly string[] _commandHistory;
        private readonly string _pointerName;
        private readonly ConsoleTerminalTheme _theme;
        private TextWriter _consoleOutput;

        public ConsoleTerminalWriter(string pointerName, ConsoleTerminalTheme theme, string[] commandHistory)
        {
            _pointerName = pointerName;
            _commandHistory = commandHistory;
            _theme = theme ?? new ConsoleTerminalTheme();
            TerminalPosition = Console.CursorTop + 1;
            _consoleOutput = Console.Out;
            Console.SetOut(this);
            Console.CursorVisible = false;
        }

        protected string CurrentCommand { get; set; } = string.Empty;

        /// <inheritdoc />
        public override Encoding Encoding
        {
            get => _consoleOutput.Encoding;
        }

        /// <inheritdoc />
        public override IFormatProvider FormatProvider
        {
            get => _consoleOutput.FormatProvider;
        }

        /// <inheritdoc />
        public override string NewLine
        {
            get => _consoleOutput.NewLine;
        }

        protected int TerminalPosition { get; set; }

        /// <inheritdoc />
        public override void Write(char value)
        {
            // ignore
        }

        public override void Write(string output)
        {
            var x = Console.CursorLeft;
            var y = Console.CursorTop;

            for (var i = TerminalPosition; i <= y + 1; i++)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = i;
                _consoleOutput.Write(new string(' ', Console.BufferWidth - 1));
            }

            Console.CursorLeft = x;
            Console.CursorTop = y;

            _consoleOutput.Write(output);

            TerminalPosition = Console.CursorTop + 1;

            WritePointer();
        }

        public override void WriteLine(string output)
        {
            Write(output + NewLine);
        }

        // ReSharper disable once ExcessiveIndentation
        public virtual string ReadCommand()
        {
            Array.Copy(_commandHistory, 1, _commandHistory, 0, _commandHistory.Length - 1);
            _commandHistory[_commandHistory.Length - 1] = string.Empty;

            var commandHistoryPosition = 0;

            WritePointer();

            while (true)
            {
                var key = Console.ReadKey();

                var isText = (byte) key.KeyChar >= 32 && (byte) key.KeyChar <= 126;

                if (!isText)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Backspace:
                        case ConsoleKey.Delete:

                            if (CurrentCommand.Length > 0)
                            {
                                CurrentCommand = CurrentCommand.Substring(0, CurrentCommand.Length - 1);
                                _commandHistory[_commandHistory.Length - 1] = CurrentCommand;
                            }

                            commandHistoryPosition = 0;

                            break;
                        case ConsoleKey.DownArrow:

                            for (var i = commandHistoryPosition - 1; i > 0; i--)
                            {
                                commandHistoryPosition = i;
                                var historyCommand = _commandHistory[_commandHistory.Length - i];

                                if (!string.IsNullOrWhiteSpace(historyCommand))
                                {
                                    CurrentCommand = historyCommand;

                                    break;
                                }
                            }

                            break;
                        case ConsoleKey.Enter:
                            WritePointer(false);
                            Console.CursorTop += 2;
                            TerminalPosition += 2;

                            return CurrentCommand;
                        case ConsoleKey.UpArrow:

                            for (var i = commandHistoryPosition + 1; i <= _commandHistory.Length; i++)
                            {
                                commandHistoryPosition = i;
                                var historyCommand = _commandHistory[_commandHistory.Length - i];

                                if (!string.IsNullOrWhiteSpace(historyCommand))
                                {
                                    CurrentCommand = historyCommand;

                                    break;
                                }
                            }

                            break;
                    }
                }
                else
                {
                    CurrentCommand += key.KeyChar;
                    _commandHistory[_commandHistory.Length - 1] = CurrentCommand;
                    commandHistoryPosition = 0;
                }

                WritePointer();
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Console.SetOut(_consoleOutput);
                _consoleOutput = null;
                Console.CursorVisible = true;
            }

            base.Dispose(disposing);
        }

        protected void WritePointer(bool withCursor = true)
        {
            var x = Console.CursorLeft;
            var y = Console.CursorTop;
            var color = Console.ForegroundColor;

            Console.CursorLeft = 0;
            Console.CursorTop = TerminalPosition;

            // Clear terminal line
            _consoleOutput.Write(new string(' ', Console.BufferWidth - 1));
            Console.CursorLeft = 0;

            Console.ForegroundColor = _theme.PointerColor;
            _consoleOutput?.Write(_pointerName);

            Console.ForegroundColor = _theme.UserInputColor;
            _consoleOutput?.Write(CurrentCommand);

            if (withCursor)
            {
                Console.ForegroundColor = _theme.CursorColor;
                _consoleOutput?.Write("_");
            }

            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.ForegroundColor = color;
        }
    }
}