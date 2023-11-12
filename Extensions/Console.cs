
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Extensions
{
    public static class ConsoleHelpers
    {

        public static string WriteLine<T>(this T entity, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
                IgnoreReadOnlyFields = true,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var result = entity != null ? JsonSerializer.Serialize(entity, typeof(T), options): "null";

            Console.WriteLine(result);
            Console.ResetColor();
            return result;
        }

        public static string Padding(int pad = 0)
        {
            var index = pad * 3;
            var padded = "                                                                       "[..index];
            return padded;
        }

        public static string WriteTrace(this string message, int pad = 0)
        {
            return $"TR:{Padding(pad)}{message}".WriteLine(ConsoleColor.Gray);
        }

        public static string WriteInfo(this string message, int pad = 0)
        {
            return $"IN:{Padding(pad)}{message}".WriteLine(ConsoleColor.Cyan);
        }
        public static string WriteNote(this string message, int pad = 0)
        {
            return $"NO:{Padding(pad)}{message}".WriteLine(ConsoleColor.DarkMagenta);
        }

        public static string WriteWarning(this string message, int pad = 0)
        {
            return $"WA:{Padding(pad)}{message}".WriteLine(ConsoleColor.Yellow);
        }

        public static string WriteError(this string message, int pad = 0)
        {
            return $"ER:{Padding(pad)}{message}".WriteLine(ConsoleColor.Red);
        }

        public static string WriteSuccess(this string message, int pad = 0)
        {
            return $"SU:{Padding(pad)}{message}".WriteLine(ConsoleColor.Green);
        }


        public static string WriteLine(this string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(message);
            Console.ResetColor();
            return message;
        }

        public static string Write(this string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            Console.Write(message);
            Console.ResetColor();
            return message;
        }

        public static string WriteInLine(this string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            Console.Write(message.Trim() + " "); //make sure there's a space at the end of the message since it's inline.
            Console.ResetColor();
            return message;
        }

    }
}
