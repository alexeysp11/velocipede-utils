using System.Data;

namespace VelocipedeUtils.Shared.Tests.Core.Compare;

public static class DataTableCompareHelper
{
    public static bool CompareDataTableSchema(DataTable dt1, DataTable dt2)
    {
        if (dt1.Columns.Count != dt2.Columns.Count)
            return false;

        for (int i = 0; i < dt1.Columns.Count; i++)
        {
            if (dt1.Columns[i].ColumnName != dt2.Columns[i].ColumnName ||
                dt1.Columns[i].DataType != dt2.Columns[i].DataType)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareDataTableContent(DataTable dt1, DataTable dt2)
    {
        if (dt1.Rows.Count != dt2.Rows.Count)
            return false;

        // Ensure consistent order for comparison if not already sorted
        // You might need to sort both DataTables by a common key before this step
        // For example: dt1.DefaultView.Sort = "ColumnName ASC"; dt1 = dt1.DefaultView.ToTable();

        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            if (!dt1.Rows[i].ItemArray.SequenceEqual(dt2.Rows[i].ItemArray))
            {
                return false;
            }
        }
        return true;
    }

    public static bool AreDataTablesEquivalent(DataTable? dt1, DataTable? dt2)
    {
        if (dt1 == null || dt2 == null)
            return dt1 == dt2; // Both null is equivalent, one null is not

        if (!CompareDataTableSchema(dt1, dt2))
            return false;

        if (!CompareDataTableContent(dt1, dt2))
            return false;

        return true;
    }
}
