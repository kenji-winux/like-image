Sub RemoveDuplicateLines()
    Dim lastRow As Long
    Dim i As Long
    
    ' Define the current worksheet
    Dim ws As Worksheet
    Set ws = ActiveSheet
    
    lastRow = ws.Cells(ws.Rows.Count, 7).End(xlUp).Row
    
    ' Loop through each row in the range from bottom to top
    For i = lastRow To 2 Step -1
        ' Check if the values in column G and O match the first row
        If ws.Cells(i, 7).Value = ws.Cells(1, 7).Value And _
           ws.Cells(i, 15).Value = ws.Cells(1, 15).Value Then
            ' Delete the duplicate row
            ws.Rows(i).Delete
        End If
    Next i
    
    ' Clear the filter and remove any remaining duplicates
    ws.Range("G1:O" & lastRow).RemoveDuplicates Columns:=Array(1, 9), Header:=xlYes
End Sub




