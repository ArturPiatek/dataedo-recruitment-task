using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp.DTO;

namespace ConsoleApp.Utilities
{
    public static class Importer
    {
        private static readonly Dictionary<string, sbyte> Header = new Dictionary<string, sbyte>();
        
        public static void Import(string importFilePath, ICollection<ImportedObject> importedObjects)
        {
            using (var fileStream = new FileStream(importFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, options: FileOptions.SequentialScan))
            using (var streamReader = new StreamReader(fileStream))
            {
                ExtractHeader(streamReader);

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    
                    var values = line.Split(';');
                
                    var importedObject = new ImportedObject
                    {
                        Type = GetValue(values, nameof(ImportedObject.Type)).Clear().ToUpper(),
                        Name = GetValue(values, nameof(ImportedObject.Name)).Clear(),
                        Schema = GetValue(values, nameof(ImportedObject.Schema)).Clear(),
                        ParentName = GetValue(values, nameof(ImportedObject.ParentName)).Clear(),
                        ParentType = GetValue(values, nameof(ImportedObject.ParentType)).Clear(),
                        ParentSchema = GetValue(values, nameof(ImportedObject.ParentSchema)).Clear(),
                        Title = GetValue(values, nameof(ImportedObject.Title)),
                        Description = GetValue(values, nameof(ImportedObject.Description)),
                        CustomField1 = GetValue(values, nameof(ImportedObject.CustomField1)),
                        CustomField2 = GetValue(values, nameof(ImportedObject.CustomField2)),
                        CustomField3 = GetValue(values, nameof(ImportedObject.CustomField3))
                    };

                    importedObjects.Add(importedObject);
                }
            }
        }
        
        private static void ExtractHeader(StreamReader streamReader)
        {
            Header.Clear();
            
            var header = streamReader.ReadLine();

            if (string.IsNullOrWhiteSpace(header))
                throw new Exception("Header does not exist.");
                    
            var headerValues = header.Split(';');
                
            for (sbyte i = 0; i < headerValues.Length; i++)
            {
                Header.Add(headerValues[i], i);
            }
        }
        
        private static sbyte TryGetRowValueIndex(string valueName)
        {
            var success = Header.TryGetValue(valueName, out var index);

            if (success)
                return index;

            return -1;
        }

        private static string GetValue(string[] values, string argumentName)
        {
            var index = TryGetRowValueIndex(argumentName);
            if (index < 0)
                return string.Empty;
            
            var value = values[index];

            return !string.IsNullOrWhiteSpace(value) ? value : string.Empty;
        }
    }
}
