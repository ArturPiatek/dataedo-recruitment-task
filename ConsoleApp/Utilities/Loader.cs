using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp.DTO;

namespace ConsoleApp.Utilities
{
    public static class Loader
    {
        public static void Load(string dataSourcePath, ICollection<DataSourceObject> dataSource)
        {
            using (var fileStream = new FileStream(dataSourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, options: FileOptions.SequentialScan))
            using (var streamReader = new StreamReader(fileStream))
            {
                // Ignore first line with attributes' names
                streamReader.ReadLine();
                
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    
                    var values = line.Split(';');
                
                    var dataSourceObject = new DataSourceObject
                    {
                        Id = Convert.ToInt32(values[0]),
                        Type = values[1],
                        Name = values[2],
                        Schema = values[3],
                        ParentId = !string.IsNullOrEmpty(values[4]) ? Convert.ToInt32(values[4]) : default,
                        ParentType = values[5],
                        Title = values[6],
                        Description = values[7],
                        CustomField1 = values[8],
                        CustomField2 = values[9],
                        CustomField3 = values[10]
                    };

                    dataSource.Add(dataSourceObject);
                }
            }
        }
    }
}
