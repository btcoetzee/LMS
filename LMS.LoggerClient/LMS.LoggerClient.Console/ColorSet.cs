using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LoggerClient.Console
{
    public struct ColorSet
    {
        public static readonly ColorSet StandardLoggingColors = new ColorSet(ConsoleColor.Green, ConsoleColor.Black);
        public static readonly ColorSet ErrorLoggingColors = new ColorSet(ConsoleColor.Red, ConsoleColor.Yellow);

        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }

        public ColorSet(ConsoleColor fg, ConsoleColor bg)
        {
            ForegroundColor = fg;
            BackgroundColor = bg;
        }
    }
}
