using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibReConnect.Logger
{
    /// <summary>
    /// ReConnect Logger 类
    /// @RED
    /// </summary>
    internal class Logger
    {
        public static void Info(String message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[ReConnect] - {message}");
            Console.ResetColor();
        }

        public static void Error(String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ReConnect] - {message}");
            Console.ResetColor();
        }
    }
}
