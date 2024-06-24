using System.Collections.Concurrent;
using System.Text;
using ConsoleApp.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.Logging;

namespace ConsoleApp.Utilities
{
    public static class Logger
    {
        private static readonly ConcurrentBag<string> InvalidImportedObjects = new ConcurrentBag<string>();
        
        public static void RegisterInvalidRow(ImportedObject importedObject, string info)
        {
            var result = new StringBuilder();

            result.Append(info);
            result.Append($"Type: {importedObject.Type} |");
            result.Append($"Name: {importedObject.Name} |");
            result.Append($"Schema: {importedObject.Schema} |");
            result.Append($"ParentName: {importedObject.ParentName} |");
            result.Append($"ParentType: {importedObject.ParentType} |");
            result.Append($"ParentSchema: {importedObject.ParentSchema}");
            
            InvalidImportedObjects.Add(result.ToString());
        }
        
        public static void LogErrors()
        {
            using (var factory = LoggerFactory.Create(builder => builder.AddConsole()))
            {
                var logger = factory.CreateLogger("invalid row");
                foreach (var invalidImportedObject in InvalidImportedObjects)
                {
                    logger.LogError(invalidImportedObject);
                }
            }
        }
    }
}
