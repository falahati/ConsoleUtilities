using System;

namespace ConsoleUtilities
{
    public class ConsoleWriterTheme
    {
        public virtual ConsoleColor CaptionColor { get; } = ConsoleColor.Cyan;
        public virtual char CaptionSideArm { get; } = '=';
        public virtual ConsoleColor ErrorColor { get; } = ConsoleColor.Red;
        public virtual string ErrorPrefix { get; } = "> ";
        public virtual ConsoleColor ExceptionMessageColor { get; } = ConsoleColor.White;
        public virtual ConsoleColor ExceptionStackColor { get; } = ConsoleColor.Gray;
        public virtual ConsoleColor ExceptionTypeColor { get; } = ConsoleColor.DarkRed;
        public virtual ConsoleColor MessageColor { get; } = ConsoleColor.Gray;
        public virtual string MessagePrefix { get; } = "> ";
        public virtual ConsoleColor PropertyColor { get; } = ConsoleColor.Gray;
        public virtual ConsoleColor QuestionColor { get; } = ConsoleColor.Gray;
        public virtual string QuestionPrefix { get; } = ">> ";
        public virtual ConsoleColor SchemaColor { get; } = ConsoleColor.DarkGray;
        public virtual char SeparatorChar { get; } = '-';
        public virtual ConsoleColor SeparatorColor { get; } = ConsoleColor.DarkGray;
        public virtual ConsoleColor SuccessColor { get; } = ConsoleColor.Green;
        public virtual string SuccessPrefix { get; } = "> ";
        public virtual ConsoleColor TypeColor { get; } = ConsoleColor.DarkRed;
        public virtual ConsoleColor UserInputColor { get; } = ConsoleColor.White;
        public virtual ConsoleColor ValueColor { get; } = ConsoleColor.White;
        public virtual ConsoleColor WarningColor { get; } = ConsoleColor.Yellow;
        public virtual string WarningPrefix { get; } = "> ";
    }
}