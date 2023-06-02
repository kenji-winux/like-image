Sub RemoveDuplicateLines()
    Dim lastRow As Long
    Dim i As Long
    
    ' Define the worksheet and range
    Dim ws As Worksheet
    Set ws = ThisWorkbook.Sheets("Sheet1") ' Replace "Sheet1" with your sheet name
    Dim tableRange As Range
    Set tableRange = ws.Range("A1:B" & ws.Cells(ws.Rows.Count, 1).End(xlUp).Row)
    
    lastRow = tableRange.Rows.Count
    
    ' Loop through each row in the range from bottom to top
    For i = lastRow To 2 Step -1
        ' Check if the values in column A and B match the first row
        If tableRange.Cells(i, 1).Value = tableRange.Cells(1, 1).Value And _
           tableRange.Cells(i, 2).Value = tableRange.Cells(1, 2).Value Then
            ' Delete the duplicate row
            tableRange.Rows(i).Delete
        End If
    Next i
    
    ' Clear the filter and remove any remaining duplicates
    tableRange.RemoveDuplicates Columns:=Array(1, 2), Header:=xlYes
End Sub
