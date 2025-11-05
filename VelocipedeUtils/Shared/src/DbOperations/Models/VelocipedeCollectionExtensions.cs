using System.Data;
using Newtonsoft.Json;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// Extensions for working with collections.
    /// </summary>
    public static class VelocipedeCollectionExtensions
    {
        /// <summary>
        /// Convert <see cref="IEnumerable{T}"/> collection to <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the origin collection.</typeparam>
        /// <param name="list">List of elements to be converted.</param>
        /// <returns>Object of <see cref="DataTable"/>.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            string json = JsonConvert.SerializeObject(list);
            return (DataTable?)JsonConvert.DeserializeObject(json, typeof(DataTable)) ?? new DataTable();
        }
    }
}
