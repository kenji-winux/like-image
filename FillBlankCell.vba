Sub FillBlankCells()
    Dim rng As Range
    Dim cell As Range
    Dim lastRow As Long
    Dim columnToFill As Range
    Dim valueToFill As Variant
    
    ' Set the range to the selected column
    Set rng = Selection
    
    ' Check if only one column is selected
    If rng.Columns.Count > 1 Then
        MsgBox "Please select only one column.", vbExclamation
        Exit Sub
    End If
    
    ' Check if there are any blank cells in the selected range
    If WorksheetFunction.CountBlank(rng) = 0 Then
        MsgBox "No blank cells found in the selected column.", vbInformation
        Exit Sub
    End If
    
    ' Determine the last row in the selected column
    lastRow = rng.Cells(rng.Rows.Count, 1).End(xlUp).Row
    
    ' Loop through each cell in the selected column
    For Each cell In rng
        ' Check if the current cell is blank
        If cell.Value = "" Then
            ' Set the column to fill as the current column
            Set columnToFill = rng.Columns(1)
            
            ' Loop through the cells above the current cell
            For i = cell.Row - 1 To 1 Step -1
                ' Check if the cell above is not blank
                If Not IsEmpty(columnToFill.Cells(i)) Then
                    ' Get the value to fill from the cell above
                    valueToFill = columnToFill.Cells(i).Value
                    Exit For
                End If
            Next i
            
            ' Fill the current blank cell with the value from above
            cell.Value = valueToFill
        End If
    Next cell
    
    MsgBox "Blank cells filled successfully.", vbInformation
End Sub

