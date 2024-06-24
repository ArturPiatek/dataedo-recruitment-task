using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DTO;

namespace ConsoleApp.Utilities
{
    public static class Matcher
    {
        public static void MatchAndUpdate(ICollection<ImportedObject> importedObjects, List<DataSourceObject> dataSource)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 3
            };
                
            Parallel.ForEach(importedObjects, options,  importedObject =>
            {
                var matches = dataSource
                    .Where(source =>
                        source.Type == importedObject.Type &&
                        source.Name == importedObject.Name &&
                        source.Schema == importedObject.Schema)
                    .ToList();
                
                if (!matches.Any())
                {
                    Logger.RegisterInvalidRow(importedObject, "[MISMATCH]");
                    return;
                }
                
                if (matches.Count == 1)
                {
                    var match = matches.FirstOrDefault();
                    if (match == null)
                    {
                        Logger.RegisterInvalidRow(importedObject, "[MISMATCH]");
                        return;
                    }
            
                    if (match.ParentId > 0 || !string.IsNullOrWhiteSpace(importedObject.ParentType))
                    {
                        var parentValid = ValidateParent(importedObject, match, dataSource);
                        if (!parentValid)
                        {
                            Logger.RegisterInvalidRow(importedObject, "[PARENT MISMATCH]");
                            return;
                        }
                    }
                    
                    AssignValues(importedObject, match);
                    return;
                }
                
                Logger.RegisterInvalidRow(importedObject, "[MULTIPLE MATCHES]");
            });
            
            Logger.LogErrors();
        }

        private static void AssignValues(ImportedObject importedObject, DataSourceObject match)
        {
            match.Title = importedObject.Title;
            match.Description = importedObject.Description;
            match.CustomField1 = importedObject.CustomField1;
            match.CustomField2 = importedObject.CustomField2;
            match.CustomField3 = importedObject.CustomField3;
        }

        private static bool ValidateParent(ImportedObject importedObject, DataSourceObject match, List<DataSourceObject> dataSource)
        {
            var parent = dataSource.Find(source =>
                source.Id == match.ParentId &&
                source.Type == match.ParentType);
            
            return parent?.Name == importedObject.ParentName 
                && parent?.Schema == importedObject.ParentSchema 
                && parent?.Type == importedObject.ParentType;
        }
    }
}
