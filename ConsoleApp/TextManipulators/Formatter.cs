using System.Linq;
using System.Text;
using ConsoleApp.DTO;

namespace ConsoleApp.TextManipulators
{
    public static class Formatter
    {
        public static string FormatDataSource(DataSourceObject dataSource)
        {
            if (dataSource == null)
                return string.Empty;
            
            var result = new StringBuilder();

            result.Append(!string.IsNullOrWhiteSpace(dataSource.Type) ? dataSource.Type : "[Type unavailable]");
            result.Append(!string.IsNullOrWhiteSpace(dataSource.Name) ? $" '{dataSource.Name}" : ".[Name unavailable]");
            result.Append(!string.IsNullOrWhiteSpace(dataSource.Title) ? $" ({dataSource.Title})" : " [Title unavailable]'");

            return result.ToString();
        }
        
        public static string FormatChildrenGroup(IGrouping<string, DataSourceObject> childrenGroup)
        {
            if (childrenGroup == null || !childrenGroup.Any())
                return string.Empty;
            
            var result = new StringBuilder("\t");
            
            result.Append(!string.IsNullOrWhiteSpace(childrenGroup.Key) ? $"{childrenGroup.Key}S" : "[Group Key unavailable]");
            result.Append($" ({childrenGroup.Count()}):");

            return result.ToString();
        }
        
        public static string FormatChild(DataSourceObject child)
        {
            if (child == null)
                return string.Empty;
            
            var result = new StringBuilder("\t\t");

            result.Append(!string.IsNullOrWhiteSpace(child.Schema) ? child.Schema : "[Schema unavailable]");
            result.Append(!string.IsNullOrWhiteSpace(child.Name) ? $".{child.Name}" : ".[Name unavailable]");
            result.Append(!string.IsNullOrWhiteSpace(child.Title) ? $" ({child.Title})" : " [Title unavailable]");

            return result.ToString();
        }
        
        public static string FormatSubChildrenGroup(IGrouping<string, DataSourceObject> subChildrenGroup)
        {
            if (subChildrenGroup == null || !subChildrenGroup.Any())
                return string.Empty;
            
            var result = new StringBuilder("\t\t\t");
            
            result.Append(!string.IsNullOrWhiteSpace(subChildrenGroup.Key) ? $"{subChildrenGroup.Key}S" : "[Group Key unavailable]");
            result.Append($" ({subChildrenGroup.Count()}):");

            return result.ToString();
        }
        
        public static string FormatSubChild(DataSourceObject subChild)
        {
            if (subChild == null)
                return string.Empty;
            
            var result = new StringBuilder("\t\t\t\t");

            result.Append(!string.IsNullOrWhiteSpace(subChild.Name) ? $"{subChild.Name}" : ".[Name unavailable]");
            result.Append(!string.IsNullOrWhiteSpace(subChild.Title) ? $" ({subChild.Title})" : " [Title unavailable]");

            return result.ToString();
        }
        
        public static string FormatDescription(string description, int indentation)
        {
            var result = new StringBuilder(indentation)
                .Insert(0, "\t", indentation);
            
            return string.IsNullOrWhiteSpace(description) 
                ? result.Append("Description unavailable.").ToString() 
                : result.Append(description).ToString();
        }
    }
}
