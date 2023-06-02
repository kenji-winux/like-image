
Sub RemoveDuplicateLines()
    Dim lastRow As Long
    Dim i As Long
    
    ' Define the current worksheet
    Dim ws As Worksheet
    Set ws = ActiveSheet
    
    lastRow = ws.Cells(ws.Rows.Count, 2).End(xlUp).Row
    
    ' Loop through each row in the range from bottom to top
    For i = lastRow To 2 Step -1
        ' Check if the values in column B and D match the first row
        If ws.Cells(i, 2).Value = ws.Cells(1, 2).Value And _
           ws.Cells(i, 4).Value = ws.Cells(1, 4).Value Then
            ' Delete the duplicate row
            ws.Rows(i).Delete
        End If
    Next i
    
    ' Clear the filter and remove any remaining duplicates
    ws.Range("B1:D" & lastRow).RemoveDuplicates Columns:=Array(1, 3), Header:=xlYes
End Sub

