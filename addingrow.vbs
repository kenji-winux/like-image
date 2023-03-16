Sub MergeCells()
    Dim lastRow As Long
    Dim i As Long
    Dim j As Long
    Dim mergeStart As Range
    Dim mergeEnd As Range
    Dim currentCell As Range
    Dim currentCellVal As String
    
    lastRow = Cells(Rows.Count, 1).End(xlUp).Row
    
    For i = 1 To lastRow
        Set mergeStart = Nothing
        Set mergeEnd = Nothing
        For j = 1 To 7 ' Merge from column A to column G
            Set currentCell = Cells(i, j)
            currentCellVal = Trim(currentCell.Value)
            If currentCellVal <> "" And mergeStart Is Nothing Then ' Value found, start of merge
                Set mergeStart = Cells(i, j)
            ElseIf currentCellVal = "" And Not mergeStart Is Nothing And mergeEnd Is Nothing Then ' No value found, end of merge
                Set mergeEnd = Cells(i, j - 1)
                Range(mergeStart, mergeEnd).Merge
                Set mergeStart = Nothing
                Set mergeEnd = Nothing
            End If
        Next
        If Not mergeStart Is Nothing And mergeEnd Is Nothing Then ' Merge until the end of the row
            Set mergeEnd = Cells(i, 7)
            Range(mergeStart, mergeEnd).Merge
            Set mergeStart = Nothing
            Set mergeEnd = Nothing
        End If
    Next
End Sub
