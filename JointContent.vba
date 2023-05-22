Sub JoinColumnsWithSeparator()
    Dim selectedRange As Range
    Dim firstColumn As Range
    Dim secondColumn As Range
    Dim outputColumn As Range
    Dim i As Long
    
    ' Check if two columns are selected
    If Selection.Columns.Count <> 2 Then
        MsgBox "Please select two columns to join.", vbExclamation
        Exit Sub
    End If
    
    ' Set the selected columns
    Set selectedRange = Selection
    Set firstColumn = selectedRange.Columns(1)
    Set secondColumn = selectedRange.Columns(2)
    
    ' Set the output column next to the selected columns
    Set outputColumn = selectedRange.Offset(0, 2).Resize(selectedRange.Rows.Count)
    
    ' Loop through each row in the selected range
    For i = 1 To selectedRange.Rows.Count
        ' Join the values with a ":" separator and store it in the output column
        outputColumn.Cells(i, 1).Value = firstColumn.Cells(i, 1).Value & ":" & secondColumn.Cells(i, 1).Value
    Next i
    
    MsgBox "Columns joined successfully.", vbInformation
End Sub
