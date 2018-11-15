using System;

namespace ConsoleUtilities
{
    public class ConsoleTerminalTheme
    {
        public virtual ConsoleColor CursorColor { get; } = ConsoleColor.DarkGray;
        public virtual ConsoleColor PointerColor { get; } = ConsoleColor.Gray;
        public virtual ConsoleColor UserInputColor { get; } = ConsoleColor.White;
    }
}