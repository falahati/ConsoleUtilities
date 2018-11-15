using System;

namespace ConsoleUtilities
{
    public class ConsoleTableTheme
    {
        public virtual char BorderBottom { get; } = '═';
        public virtual char BorderBottomLeft { get; } = '╚';
        public virtual char BorderBottomRight { get; } = '╝';
        public virtual ConsoleColor BorderColor { get; } = ConsoleColor.DarkGray;
        public virtual char BorderHeaderBottom { get; } = '╧';
        public virtual char BorderHeaderLeft { get; } = '╠';
        public virtual char BorderHeaderRight { get; } = '╣';
        public virtual char BorderHeaderTop { get; } = '╤';
        public virtual char BorderLeft { get; } = '║';


        public virtual char BorderRight { get; } = '║';
        public virtual char BorderRowBottom { get; } = '╧';
        public virtual char BorderRowLeft { get; } = '╟';
        public virtual char BorderRowRight { get; } = '╢';
        public virtual char BorderRowTop { get; } = '╤';


        public virtual char BorderTop { get; } = '═';
        public virtual char BorderTopLeft { get; } = '╔';
        public virtual char BorderTopRight { get; } = '╗';
        public virtual char HeaderCross { get; } = '╪';


        public virtual char HeaderHorizontalSeparator { get; } = '═';
        public virtual ConsoleColor HeaderTextColor { get; } = ConsoleColor.White;
        public virtual char HeaderVerticalSeparator { get; } = '│';
        public virtual char RowCross { get; } = '┼';

        public virtual char RowHorizontalSeparator { get; } = '─';
        public virtual ConsoleColor RowTextColor { get; } = ConsoleColor.Gray;
        public virtual char RowVerticalSeparator { get; } = '│';


        public virtual string TextTail { get; } = "...";
    }
}