using System.Data;
using Newtonsoft.Json;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    public static class VelocipedeCollectionExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            string json = JsonConvert.SerializeObject(list);
            return (DataTable?)JsonConvert.DeserializeObject(json, (typeof(DataTable))) ?? new DataTable();
        }
    }
}
