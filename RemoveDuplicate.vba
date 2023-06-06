Sub RemoveDuplicateRows()
    Dim ws As Worksheet
    Dim lastRow As Long
    Dim rng As Range
    Dim dict As Object
    
    ' Create a dictionary to store unique combinations of Column G and O values
    Set dict = CreateObject("Scripting.Dictionary")
    
    ' Set the worksheet to work with
    Set ws = ThisWorkbook.Sheets("Sheet1") ' Replace "Sheet1" with your actual sheet name
    
    ' Find the last row of data in Column G
    lastRow = ws.Cells(ws.Rows.Count, "G").End(xlUp).Row
    
    ' Loop through each row from bottom to top
    For i = lastRow To 2 Step -1
        ' Check if the value in Column O is "None"
        If ws.Range("O" & i).Value = "None" Then
            ' Check if the combination of values in Column G and O already exists in the dictionary
            If dict.exists(ws.Range("G" & i).Value & "_" & ws.Range("O" & i).Value) Then
                ' Delete the duplicate row
                ws.Rows(i).Delete
            Else
                ' Add the combination of values in Column G and O to the dictionary if it's not already present
                dict(ws.Range("G" & i).Value & "_" & ws.Range("O" & i).Value) = 1
            End If
        End If
    Next i
    
    ' Clear the dictionary
    Set dict = Nothing
    
    ' Adjust the column widths to fit the content
    ws.Columns.AutoFit
    
    ' Display a message box when the task is completed
    MsgBox "Duplicate rows with 'None' value in Column O have been removed.", vbInformation
End Sub
