using System;
using System.Linq;
using ConsoleApp.DTO;

namespace ConsoleApp.TextManipulators
{
    public static class Writer
    {
        public static void WriteDataSource(DataSourceObject dataSource)
        {
            if (dataSource == null)
                return;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Formatter.FormatDataSource(dataSource));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(Formatter.FormatDescription(dataSource.Description, 0));
            Console.ResetColor();
        }
        
        public static void WriteChildrenGroup(IGrouping<string, DataSourceObject> childrenGroup)
        {
            if (childrenGroup == null || !childrenGroup.Any())
                return;
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(Formatter.FormatChildrenGroup(childrenGroup));
            Console.ResetColor();
        }
        
        public static void WriteChild(DataSourceObject child)
        {
            if (child == null)
                return;
            
            Console.WriteLine(Formatter.FormatChild(child));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Formatter.FormatDescription(child.Description, 2));
            Console.ResetColor();
        }
        
        public static void WriteSubChildrenGroup(IGrouping<string, DataSourceObject> subChildrenGroup)
        {
            if (subChildrenGroup == null || !subChildrenGroup.Any())
                return;
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(Formatter.FormatSubChildrenGroup(subChildrenGroup));
            Console.ResetColor();
        }
        
        public static void WriteSubChild(DataSourceObject subChild)
        {
            if (subChild == null)
                return;
            
            Console.WriteLine(Formatter.FormatSubChild(subChild));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Formatter.FormatDescription(subChild.Description, 4));
            Console.ResetColor();
        }
    }
}
