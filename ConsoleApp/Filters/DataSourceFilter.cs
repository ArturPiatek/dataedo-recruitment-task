using System.Collections.Generic;
using System.Linq;
using ConsoleApp.DTO;

namespace ConsoleApp.Filters
{
    public static class DataSourceFilter
    {
        public static IEnumerable<IGrouping<string, DataSourceObject>> GetGroups(this IEnumerable<DataSourceObject> dataSource, DataSourceObject parent)
            => dataSource
                .Where(child =>
                    child.ParentId == parent.Id &&
                    child.ParentType == parent.Type)
                .GroupBy(x => x.Type);
    }
}
