using System;
using System.Linq;

namespace ConsoleUtilities
{
    public class ConsoleTable
    {
        private readonly ConsoleColumnDefinition[] _columns;
        private ConsoleTableTheme _theme = new ConsoleTableTheme();

        public ConsoleTable(ConsoleColumnDefinition[] columns)
        {
            _columns = columns ?? new ConsoleColumnDefinition[0];
            AutoSize = false;
        }

        public ConsoleTable(string[] headers)
        {
            _columns = headers.Select(s => new ConsoleColumnDefinition(s)).ToArray();
            AutoSize = true;
        }

        public bool AutoSize { get; set; }

        private string[] Headers
        {
            get => _columns.Select(column => column.Header).ToArray();
        }

        public ConsoleTableTheme Theme
        {
            get => _theme;
            set => _theme = value ?? new ConsoleTableTheme();
        }

        public virtual void Write(string[][] rows)
        {
            Write(Console.BufferWidth, rows);
        }

        public virtual void Write(int totalWidth, string[][] rows)
        {
            totalWidth -= 4; // right line + left line + start padding (min one) + end new line

            var columnWidthFactors = GetColumnWidthFactors(totalWidth, rows);

            if (columnWidthFactors.Length == 0)
            {
                columnWidthFactors = new[] {1d};
            }

            var usableWidth = totalWidth - Math.Max(columnWidthFactors.Length - 1, 0); // one separator per column

            if (usableWidth <= columnWidthFactors.Length)
            {
                throw new ArgumentException("Insufficient space for the drawing of the table.", nameof(totalWidth));
            }

            var totalWidthFactor = columnWidthFactors.Sum();
            var columnWidths = columnWidthFactors.Select(i => (int) Math.Floor(usableWidth / totalWidthFactor * i))
                .ToArray();

            var startPadding = new string(' ', (int) Math.Floor((usableWidth - columnWidths.Sum()) / 2d) + 1);
            var endPadding = Environment.NewLine;

            var hasHeader = Headers.Length > 0;
            var hasRow = rows.Length > 0;

            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = Theme.BorderColor;

            Console.Write(startPadding);
            WriteTopBorder(columnWidths, hasHeader);
            Console.Write(endPadding);

            if (hasHeader)
            {
                Console.Write(startPadding);
                WriteHeader(columnWidths);
                Console.Write(endPadding);

                Console.Write(startPadding);
                WriteHeaderSeparator(columnWidths);
                Console.Write(endPadding);
            }

            if (hasRow)
            {
                for (var y = 1; y <= rows.Length; y++)
                {
                    Console.Write(startPadding);
                    WriteRow(rows[y - 1], columnWidths);
                    Console.Write(endPadding);

                    if (y < rows.Length)
                    {
                        Console.Write(startPadding);
                        WriteRowSeparator(columnWidths);
                        Console.Write(endPadding);
                    }
                }
            }

            Console.Write(startPadding);
            WriteBottomBorder(columnWidths, hasRow);
            Console.Write(endPadding);

            Console.ForegroundColor = consoleColor;
        }

        protected virtual string AlignCenter(string text, int width)
        {
            if (text.Length > width)
            {
                if (Theme.TextTail.Length >= width)
                {
                    text = text.Substring(0, width);
                }
                else
                {
                    text = text.Substring(0, width - Theme.TextTail.Length) + Theme.TextTail;
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }

            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        protected virtual double[] GetColumnWidthFactors(int totalWidth, string[][] rows)
        {
            var numberOfColumns = Math.Max(rows.DefaultIfEmpty().Max(cells => cells?.Length ?? 0), _columns.Length);
            var usableWidth = totalWidth - (numberOfColumns - 1);
            var columnWidth = (int) Math.Floor(usableWidth / (double) numberOfColumns);

            if (!AutoSize)
            {
                return Enumerable.Range(0, numberOfColumns)
                    .Select(i => _columns.Length > i ? _columns[i].WidthFactor : 1d).ToArray();
            }

            var rowsWithHeader = rows.Concat(new[] {Headers}).ToArray();
            var maxColumnLength = Enumerable.Range(0, numberOfColumns)
                .Select(i =>
                    (double) rowsWithHeader
                        .Where(row => row.Length > i).Select(row => row[i].Length)
                        .DefaultIfEmpty()
                        .Max()
                ).ToArray();
            var totalFreeSpace = maxColumnLength.Where(d => d < columnWidth).Select(d => columnWidth - d).Sum();
            var totalSizeNeeded = maxColumnLength.Where(d => d > columnWidth).Select(d => d - columnWidth).Sum();

            return maxColumnLength.Select(l =>
                    l <= columnWidth ? l : columnWidth + totalFreeSpace / totalSizeNeeded * (l - columnWidth))
                .ToArray();
        }

        protected virtual void WriteBottomBorder(int[] columnWidths, bool hasRow)
        {
            Console.Write(Theme.BorderBottomLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                Console.Write(new string(Theme.BorderBottom, columnWidths[x - 1]));

                if (x < columnWidths.Length)
                {
                    Console.Write(hasRow ? Theme.BorderRowBottom.ToString() : Theme.BorderHeaderBottom.ToString());
                }
            }

            Console.Write(Theme.BorderBottomRight.ToString());
        }

        protected virtual void WriteHeader(int[] columnWidths)
        {
            Console.Write(Theme.BorderLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                if (_columns.Length >= x)
                {
                    Console.ForegroundColor = _columns[x - 1].HeaderColor ?? Theme.HeaderTextColor;
                    Console.Write(AlignCenter(_columns[x - 1].Header, columnWidths[x - 1]));
                }
                else
                {
                    Console.ForegroundColor = Theme.HeaderTextColor;
                    Console.Write(AlignCenter(string.Empty, columnWidths[x - 1]));
                }

                Console.ForegroundColor = Theme.BorderColor;

                if (x < columnWidths.Length)
                {
                    Console.Write(Theme.HeaderVerticalSeparator.ToString());
                }
            }

            Console.Write(Theme.BorderRight.ToString());
        }

        protected virtual void WriteHeaderSeparator(int[] columnWidths)
        {
            Console.Write(Theme.BorderHeaderLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                Console.Write(new string(Theme.HeaderHorizontalSeparator, columnWidths[x - 1]));

                if (x < columnWidths.Length)
                {
                    Console.Write(Theme.HeaderCross.ToString());
                }
            }

            Console.Write(Theme.BorderHeaderRight.ToString());
        }

        protected virtual void WriteRow(string[] row, int[] columnWidths)
        {
            Console.Write(Theme.BorderLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                Console.ForegroundColor =
                    (_columns.Length >= x ? _columns[x - 1].RowColor : null) ?? Theme.RowTextColor;

                var text = row.Length >= x ? row[x - 1] : string.Empty;
                Console.Write(AlignCenter(text, columnWidths[x - 1]));

                Console.ForegroundColor = Theme.BorderColor;

                if (x < columnWidths.Length)
                {
                    Console.Write(Theme.RowVerticalSeparator.ToString());
                }
            }

            Console.Write(Theme.BorderRight.ToString());
        }

        protected virtual void WriteRowSeparator(int[] columnWidths)
        {
            Console.Write(Theme.BorderRowLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                Console.Write(new string(Theme.RowHorizontalSeparator, columnWidths[x - 1]));

                if (x < columnWidths.Length)
                {
                    Console.Write(Theme.RowCross.ToString());
                }
            }

            Console.Write(Theme.BorderRowRight.ToString());
        }

        protected virtual void WriteTopBorder(int[] columnWidths, bool hasHeader)
        {
            Console.Write(Theme.BorderTopLeft.ToString());

            for (var x = 1; x <= columnWidths.Length; x++)
            {
                Console.Write(new string(Theme.BorderTop, columnWidths[x - 1]));

                if (x < columnWidths.Length)
                {
                    Console.Write(hasHeader ? Theme.BorderHeaderTop.ToString() : Theme.BorderRowTop.ToString());
                }
            }

            Console.Write(Theme.BorderTopRight.ToString());
        }
    }
}