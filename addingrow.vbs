Sub MergeCells()
    Dim lastRow As Long
    Dim i As Long
    Dim mergeStart As Range
    Dim mergeEnd As Range
    
    lastRow = Cells(Rows.Count, 1).End(xlUp).Row
    
    For i = 1 To lastRow
        If Not IsEmpty(Cells(i, 1)) Then ' Value found, start of merge
            If mergeStart Is Nothing Then
                Set mergeStart = Cells(i, 1)
            End If
        ElseIf Not mergeStart Is Nothing Then ' No value found, end of merge
            Set mergeEnd = Cells(i - 1, 1)
            Range(mergeStart, mergeEnd.Offset(5, 0)).Merge
            Set mergeStart = Nothing
            Set mergeEnd = Nothing
        End If
    Next
End Sub
