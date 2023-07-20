Sub ReplaceValues()
    Dim ws As Worksheet
    Dim lastRow As Long
    Dim i As Long
    Dim replaceValues As Variant
    Dim replaceCodes As Variant
    
    ' Define the worksheet where the data is imported
    Set ws = ActiveSheet
    
    ' Define the last row with data in column A (adjust column as needed)
    lastRow = ws.Cells(ws.Rows.Count, "A").End(xlUp).Row
    
    ' Define the values and their corresponding codes for replacement
    replaceValues = Array("NA", "None", "Telemetry", "General", "Tactic", "Technique")
    replaceCodes = Array(0, 1, 2, 3, 4, 5, 6)
    
    ' Loop through each cell in columns A, B, and C and perform the replacement
    For i = 1 To lastRow
        ' Replace the value in column A
        ws.Cells(i, "A").Value = ReplaceValue(ws.Cells(i, "A").Value, replaceValues, replaceCodes)
        
        ' Replace the value in column B
        ws.Cells(i, "B").Value = ReplaceValue(ws.Cells(i, "B").Value, replaceValues, replaceCodes)
        
        ' Replace the value in column C
        ws.Cells(i, "C").Value = ReplaceValue(ws.Cells(i, "C").Value, replaceValues, replaceCodes)
    Next i
    
    MsgBox "Replacement completed!", vbInformation
End Sub

Function ReplaceValue(value As Variant, replaceValues As Variant, replaceCodes As Variant) As Variant
    Dim i As Long
    
    ' Loop through the replaceValues array and check if the value matches any of them
    For i = LBound(replaceValues) To UBound(replaceValues)
        If value = replaceValues(i) Then
            ' If a match is found, return the corresponding replaceCodes value
            ReplaceValue = replaceCodes(i)
            Exit Function
        End If
    Next i
    
    ' If no match is found, return the original value
    ReplaceValue = value
End Function
