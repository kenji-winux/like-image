Sub AddRows()
   Dim lastRow As Long
   Dim i As Long
   
   lastRow = ActiveSheet.Cells(Rows.Count, "A").End(xlUp).Row
   
   For i = lastRow To 1 Step -1
      Range("A" & i + 1 & ":A" & i + 5).EntireRow.Insert Shift:=xlDown
   Next i
End Sub
