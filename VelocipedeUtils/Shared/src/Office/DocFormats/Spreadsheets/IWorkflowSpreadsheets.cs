using System.Collections.Generic;
using VelocipedeUtils.Shared.Models.Documents;

namespace VelocipedeUtils.Shared.Office.DocFormats.Spreadsheets;

/// <summary>
/// Interface for using spreadsheets.
/// </summary>
public interface IWorkflowSpreadsheets
{
    /// <summary>
    /// Method for converting a list of SpreadsheetElement into spreadsheet document.
    /// </summary>
    void SpreadsheetElementsToDocument(
        string foldername,
        string filename,
        uint worksheetId,
        string worksheetName,
        List<SpreadsheetElement> elements);
    
    /// <summary>
    /// Given a document name, a worksheet name, the name of the first cell in the contiguous range, 
    /// the name of the last cell in the contiguous range, and the name of the results cell, 
    /// calculates the sum of the cells in the contiguous range and inserts the result into the results cell.
    /// Note: All cells in the contiguous range must contain numbers.
    /// </summary>
    void CalculateSumOfCellRange(
        string docName,
        string worksheetName,
        string firstCellName,
        string lastCellName,
        string resultCell);
}