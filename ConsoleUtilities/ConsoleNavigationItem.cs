using System;

namespace ConsoleUtilities
{
    public class ConsoleNavigationItem
    {
        public ConsoleNavigationItem(string caption)
        {
            Caption = caption ?? throw new ArgumentNullException(nameof(caption));
        }

        public ConsoleNavigationItem(string caption, Action<int, ConsoleNavigationItem> action) : this(caption)
        {
            Action = action;
        }

        public ConsoleNavigationItem(string caption, Action<int, ConsoleNavigationItem> action, object state) : this(
            caption, action)
        {
            State = state;
        }

        public ConsoleNavigationItem(object state) : this(state?.ToString())
        {
            State = state;
        }

        public ConsoleNavigationItem(object state, Action<int, ConsoleNavigationItem> action) : this(state)
        {
            Action = action;
        }

        public Action<int, ConsoleNavigationItem> Action { get; }
        public string Caption { get; }

        public bool IsCallable
        {
            get => Action != null;
        }

        public object State { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Caption;
        }
    }
}