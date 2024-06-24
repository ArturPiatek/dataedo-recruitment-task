using System.Collections.Generic;
using System.Linq;
using ConsoleApp.DTO;
using ConsoleApp.Filters;
using ConsoleApp.TextManipulators;

namespace ConsoleApp.Utilities
{
    public static class Printer
    {
        public static void Print(ICollection<DataSourceObject> dataSource)
        {
            foreach (var dataSourceObject in dataSource.OrderBy(x => x.Type))
            {
                switch (dataSourceObject.Type)
                {
                    case "DATABASE":
                    case "GLOSSARY":
                    case "DOMAIN":
                        Writer.WriteDataSource(dataSourceObject);
            
                        // direct children of database like tables, procedures, lookups
                        var childrenGroups = dataSource.GetGroups(dataSourceObject);
                        
                        foreach (var childrenGroup in childrenGroups)
                        {
                            Writer.WriteChildrenGroup(childrenGroup);
                        
                            foreach (var child in childrenGroup.OrderBy(x => x.Name))
                            {
                                // direct sub children like columns, parameters, values
                                var subChildrenGroups = dataSource.GetGroups(child);
                        
                                Writer.WriteChild(child);
                        
                                foreach (var subChildrenGroup in subChildrenGroups)
                                {
                                    Writer.WriteSubChildrenGroup(subChildrenGroup);
                        
                                    foreach (var subChild in subChildrenGroup.OrderBy(x => x.Name))
                                    {
                                        Writer.WriteSubChild(subChild);
                                    }
                                }
                            }
                        }     
                        
                        break;
                }
            }
        }
    }
}
