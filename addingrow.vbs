Option Explicit

Dim objExcel, objWorkbook, objWorksheet
Dim lastRow, i, j, mergeStart, mergeEnd, currentCell, currentCellVal

Set objExcel = CreateObject("Excel.Application")
Set objWorkbook = objExcel.Workbooks.Open("C:\path\to\your\file.xlsx")
Set objWorksheet = objWorkbook.Worksheets(1)

lastRow = objWorksheet.Cells(objWorksheet.Rows.Count, "A").End(-4162).Row ' -4162 is equivalent to xlUp constant

For i = 1 To lastRow
    mergeStart = ""
    mergeEnd = ""
    For j = 1 To 7 ' Merge from column A to column G
        currentCell = objWorksheet.Cells(i, j)
        currentCellVal = Trim(currentCell.Value)
        If currentCellVal = "" And mergeStart = "" Then ' No value found, start of merge
            mergeStart = objWorksheet.Cells(i, 1)
        ElseIf currentCellVal <> "" And mergeStart <> "" Then ' Value found, end of merge
            mergeEnd = objWorksheet.Cells(i, j - 1)
            objWorksheet.Range(mergeStart, mergeEnd).Merge
            mergeStart = ""
            mergeEnd = ""
        End If
    Next
    If mergeStart <> "" And mergeEnd = "" Then ' Merge until the end of the row
        mergeEnd = objWorksheet.Cells(i, 7)
        objWorksheet.Range(mergeStart, mergeEnd).Merge
    End If
Next

objWorkbook.Save
objWorkbook.Close
objExcel.Quit

Set objWorksheet = Nothing
Set objWorkbook = Nothing
Set objExcel = Nothing
