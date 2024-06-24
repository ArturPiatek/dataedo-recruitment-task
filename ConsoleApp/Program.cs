using ConsoleApp.Parsers;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var reader = new ImprovedParser();
            reader.Parse("StaticFiles\\sampleFile1.csv", "StaticFiles\\dataSource.csv");
        }
    }
}
