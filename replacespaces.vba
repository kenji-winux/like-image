Sub ReplaceSpaces()
    Dim cell As Range
    Dim lastRow As Long
    Dim i As Long
    
    lastRow = ActiveSheet.Cells(Rows.Count, 1).End(xlUp).Row
    
    For i = 1 To lastRow
        For Each cell In Range("A" & i)
            cell.Value = Replace(cell.Value, "   ", "/")
        Next cell
    Next i
End Sub
