using System;

namespace ConsoleUtilities
{
    public class ConsoleNavigationTheme
    {
        public virtual ConsoleColor DisabledItemNameColor { get; } = ConsoleColor.Gray;
        public virtual ConsoleColor DisabledItemNumberColor { get; } = ConsoleColor.DarkGray;
        public virtual ConsoleColor ItemNameColor { get; } = ConsoleColor.White;
        public virtual ConsoleColor ItemNumberColor { get; } = ConsoleColor.Green;
        public virtual ConsoleColor MessageColor { get; } = ConsoleColor.Gray;
        public virtual ConsoleColor UserInputColor { get; } = ConsoleColor.White;
    }
}