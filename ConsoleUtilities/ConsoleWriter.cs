using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace ConsoleUtilities
{
    public class ConsoleWriter
    {
        private static ConsoleWriter _default = new ConsoleWriter();
        private ConsoleWriterTheme _theme = new ConsoleWriterTheme();

        public static ConsoleWriter Default
        {
            get => _default;
            set => _default = value ?? new ConsoleWriter();
        }

        public ConsoleWriterTheme Theme
        {
            get => _theme;
            set => _theme = value ?? new ConsoleWriterTheme();
        }

        public void PrintCaption(string caption)
        {
            var sideArm = new string(Theme.CaptionSideArm,
                (int) Math.Floor((Console.BufferWidth - (caption.Length + 4)) / 2d));
            Console.WriteLine();
            WriteColoredTextLine($"{sideArm} [{caption}] {sideArm}", Theme.CaptionColor);
            Console.WriteLine();
        }

        public void PrintError(string message)
        {
            Console.WriteLine();
            WriteColoredTextLine(Theme.ErrorPrefix + message, Theme.ErrorColor);
        }

        public void PrintMessage(string message)
        {
            WriteColoredTextLine(Theme.MessagePrefix + message, Theme.MessageColor);
        }

        public string PrintQuestion(string message)
        {
            var response = "";
            var color = Console.ForegroundColor;

            while (string.IsNullOrWhiteSpace(response))
            {
                WriteColoredText(Theme.QuestionPrefix + message + ": ", Theme.QuestionColor);
                Console.ForegroundColor = Theme.UserInputColor;
                response = Console.ReadLine();
            }

            Console.ForegroundColor = color;

            return response.Trim();
        }

        // ReSharper disable once ExcessiveIndentation
        // ReSharper disable once MethodTooLong
        public T PrintQuestion<T>(string message)
        {
            while (true)
            {
                var response = PrintQuestion(message);

                try
                {
                    if (typeof(T) == typeof(bool))
                    {
                        if (bool.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(uint))
                    {
                        if (uint.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        if (int.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(short))
                    {
                        if (short.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(ushort))
                    {
                        if (ushort.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        if (long.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(ulong))
                    {
                        if (ulong.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(byte))
                    {
                        if (byte.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(decimal))
                    {
                        if (decimal.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        if (double.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(char))
                    {
                        if (char.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        if (float.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(sbyte))
                    {
                        if (sbyte.TryParse(response, out var result))
                        {
                            return (T) Convert.ChangeType(result, typeof(T));
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T) Convert.ChangeType(response, typeof(T));
                    }
                    else
                    {
                        return (T) Convert.ChangeType(response, typeof(T));
                    }
                }
                catch (InvalidCastException)
                {
                    // ignore
                }
            }
        }

        public void PrintSeparator()
        {
            Console.WriteLine();
            WriteColoredTextLine(new string(Theme.CaptionSideArm, Console.BufferWidth - 1), Theme.SeparatorColor);
            Console.WriteLine();
        }

        public void PrintSuccess(string message)
        {
            Console.WriteLine();
            WriteColoredTextLine(Theme.SuccessPrefix + message, Theme.SuccessColor);
        }

        public void PrintWarning(string message)
        {
            Console.WriteLine();
            WriteColoredTextLine(Theme.WarningPrefix + message, Theme.WarningColor);
        }

        public virtual void WriteColoredText(string str, ConsoleColor color)
        {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = consoleColor;
        }

        public virtual void WriteColoredTextLine(string str, ConsoleColor color)
        {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = consoleColor;
        }

        public virtual void WriteException(Exception ex)
        {
            WriteException(ex, 0);
        }

        public virtual void WriteObject(object obj, int maxDepth = 1)
        {
            WriteObject(obj, maxDepth + 1, 0);
            WriteColoredTextLine(";", Theme.SchemaColor);
        }

        public virtual void WritePaddedText(string text, int padding, ConsoleColor color)
        {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            foreach (var str in text.Split('\r', '\n').Where(s => s.Trim().Length > 0))
            {
                var remaining = Console.BufferWidth - (padding + 1);
                var len = 0;

                while (len < str.Length)
                {
                    Console.Write(new string(' ', padding));
                    Console.WriteLine(str.Substring(len, Math.Min(remaining, str.Length - len)));
                    len += remaining;
                }
            }

            Console.ForegroundColor = consoleColor;
        }

        protected virtual void WriteException(Exception ex, int padding)
        {
            WriteColoredText(new string(' ', padding * 3) + "(", Theme.ExceptionTypeColor);
            WriteColoredText(ex.GetType().Name, Theme.ExceptionTypeColor);
            WriteColoredText(") ", Theme.ExceptionTypeColor);
            WriteColoredTextLine(ex.Message, Theme.ExceptionMessageColor);

            if (ex.StackTrace != null)
            {
                WritePaddedText(ex.StackTrace.Trim(), padding * 3 + 2, Theme.ExceptionStackColor);
            }

            if (ex is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions != null)
                {
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        WriteException(exception, padding + 1);
                    }
                }
            }

            if (ex.InnerException != null)
            {
                WriteException(ex.InnerException, padding + 1);
            }
        }

        // ReSharper disable once ExcessiveIndentation
        protected virtual void WriteObject(object obj, int maxDepth, int depth)
        {
            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDepth), "maxDepth must be equal or greater than zero.");
            }

            var startPadding = new string(' ', depth * 3);

            try
            {
                if (obj?.GetType().Name != null)
                {
                    WriteColoredText("(", Theme.SchemaColor);
                    WriteColoredText(obj.GetType().Name, Theme.TypeColor);
                    WriteColoredText(") ", Theme.SchemaColor);
                }

                if (maxDepth == 0 || obj == null || obj.GetType().IsValueType || obj is string)
                {
                    WriteColoredText(obj?.ToString() ?? "[NULL]", Theme.ValueColor);

                    return;
                }

                WriteColoredTextLine("{", Theme.SchemaColor);

                var i = 0;

                if (obj is IDictionary dictionary)
                {
                    foreach (var dicKey in dictionary.Keys)
                    {
                        if (i > 0)
                        {
                            WriteColoredTextLine(", ", Theme.SchemaColor);
                        }

                        WriteColoredText(startPadding + "   [", Theme.SchemaColor);
                        WriteColoredText(dicKey.ToString(), Theme.PropertyColor);
                        WriteColoredText("]: ", Theme.SchemaColor);
                        WriteObject(dictionary[dicKey], maxDepth - 1, depth + 1);
                        i++;
                    }
                }
                else if (obj is IEnumerable enumerable)
                {
                    foreach (var arrayItem in enumerable)
                    {
                        if (i > 0)
                        {
                            WriteColoredTextLine(", ", Theme.SchemaColor);
                        }

                        WriteColoredText(startPadding + "   [", Theme.SchemaColor);
                        WriteColoredText(i.ToString(), Theme.PropertyColor);
                        WriteColoredText("]: ", Theme.SchemaColor);
                        WriteObject(arrayItem, maxDepth - 1, depth + 1);
                        i++;
                    }
                }
                else
                {
                    foreach (var propertyInfo in obj.GetType().GetProperties().OrderBy(info => info.Name))
                    {
                        if (propertyInfo.CanRead)
                        {
                            object value;

                            try
                            {
                                value = propertyInfo.GetValue(obj);
                            }
                            catch (TargetInvocationException ex)
                            {
                                value = ex.InnerException?.GetType().ToString();
                            }
                            catch (Exception ex)
                            {
                                value = ex.GetType().ToString();
                            }

                            if (i > 0)
                            {
                                WriteColoredTextLine(", ", Theme.SchemaColor);
                            }

                            WriteColoredText(startPadding + "   " + propertyInfo.Name, Theme.PropertyColor);
                            WriteColoredText(": ", Theme.SchemaColor);
                            WriteObject(value, maxDepth - 1, depth + 1);
                            i++;
                        }
                    }
                }

                Console.WriteLine();
                WriteColoredText(startPadding + "}", Theme.SchemaColor);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }
    }
}