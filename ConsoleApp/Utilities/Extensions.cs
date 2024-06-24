using System;
using System.Text;

namespace ConsoleApp.Utilities
{
    public static class Extensions
    {
        public static string Clear(this string input)
        {
            if (input == null)
                return string.Empty;
        
            var sb = new StringBuilder(input.Trim());
            
            sb.Replace(Environment.NewLine, "");

            return sb.ToString();
        }
    }
}
