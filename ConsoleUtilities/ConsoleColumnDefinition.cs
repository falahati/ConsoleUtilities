using System;

namespace ConsoleUtilities
{
    public class ConsoleColumnDefinition
    {
        private string _header;
        private int _widthFactor = 1;

        public ConsoleColumnDefinition(string headerCaption)
        {
            Header = headerCaption;
        }

        public string Header
        {
            get => _header;
            set => _header = value ?? string.Empty;
        }

        public ConsoleColor? HeaderColor { get; set; }
        public ConsoleColor? RowColor { get; set; }

        public int WidthFactor
        {
            get => _widthFactor;
            set
            {
                if (WidthFactor < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Width factor must be greater then one.");
                }

                _widthFactor = value;
            }
        }
    }
}