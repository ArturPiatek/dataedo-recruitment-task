using System.Collections.Generic;
using ConsoleApp.DTO;
using ConsoleApp.Utilities;

namespace ConsoleApp.Parsers
{
    public class ImprovedParser
    {
        private readonly List<ImportedObject> _importedObjects = new List<ImportedObject>();
        private readonly List<DataSourceObject> _dataSource = new List<DataSourceObject>();

        public void Parse(string importFilePath, string dataSourcePath)
        {
            Importer.Import(importFilePath, _importedObjects);
            Loader.Load(dataSourcePath, _dataSource);
            Matcher.MatchAndUpdate(_importedObjects, _dataSource);
            Printer.Print(_dataSource);
        }
    }
}
