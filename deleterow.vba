Sub DeleteEmptyRows()
    Dim lastRow As Long
    Dim i As Long
    
    lastRow = ActiveSheet.Cells(Rows.Count, 1).End(xlUp).Row
    
    For i = lastRow To 1 Step -1
        If WorksheetFunction.CountA(Rows(i)) = 0 Then
            Rows(i).Delete
        End If
    Next i
End Sub
