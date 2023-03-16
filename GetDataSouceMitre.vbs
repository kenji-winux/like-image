Option Explicit

Const xlUp = -4162

' Function to extract data sources and components from MITRE ATT&CK website
Function GetMITREData(techniqueID)
    Dim htmlDoc, htmlTable, htmlRows, htmlCols, htmlLink, dataSource, dataComponent
    Dim i, j, k, n
    Set htmlDoc = CreateObject("HTMLDocument")
    htmlDoc.Load ("https://attack.mitre.org/techniques/" & techniqueID & "/")

    Set htmlTable = htmlDoc.getElementsByTagName("table")(0)
    Set htmlRows = htmlTable.getElementsByTagName("tr")

    n = 0
    For i = 0 To htmlRows.Length - 1
        Set htmlCols = htmlRows(i).getElementsByTagName("td")
        If htmlCols.Length = 2 Then
            dataSource = htmlCols(0).innerText
            dataComponent = htmlCols(1).innerText

            ' Remove links from data components
            For j = 0 To htmlCols(1).getElementsByTagName("a").Length - 1
                Set htmlLink = htmlCols(1).getElementsByTagName("a")(j)
                dataComponent = Replace(dataComponent, htmlLink.innerText, "")
            Next

            ' Split data components into individual items
            For Each dataComponent In Split(dataComponent, ", ")
                n = n + 1
                GetMITREData(n, 0) = techniqueID
                GetMITREData(n, 1) = dataSource
                GetMITREData(n, 2) = Trim(dataComponent)
            Next
        End If
    Next

    Set htmlDoc = Nothing
End Function

' Sub to write MITRE data to Excel file
Sub WriteMITREDataToExcel(techniqueIDs, outputFile)
    Dim excelApp, excelBook, excelSheet, row, i
    Set excelApp = CreateObject("Excel.Application")
    excelApp.Visible = True
    Set excelBook = excelApp.Workbooks.Add()
    Set excelSheet = excelBook.Sheets(1)

    ' Write headers to Excel file
    excelSheet.Cells(1, 1).Value = "Technique ID"
    excelSheet.Cells(1, 2).Value = "Data Source"
    excelSheet.Cells(1, 3).Value = "Data Component"

    ' Write MITRE data to Excel file
    row = 2
    For i = 0 To UBound(techniqueIDs)
        Dim techniqueData, j
        techniqueData = GetMITREData(techniqueIDs(i))
        For j = 0 To UBound(techniqueData, 2)
            excelSheet.Cells(row, 1).Value = techniqueData(j, 0)
            excelSheet.Cells(row, 2).Value = techniqueData(j, 1)
            excelSheet.Cells(row, 3).Value = techniqueData(j, 2)
            row = row + 1
        Next
    Next

    ' Auto-fit columns and save Excel file
    excelSheet.Cells.Select
    excelSheet.Cells.EntireColumn.AutoFit
    excelBook.SaveAs outputFile

    Set excelSheet = Nothing
    Set excelBook = Nothing
    excelApp.Quit
    Set excelApp = Nothing
End Sub

' Example usage: WriteMITREDataToExcel(Array("T1595", "T1110"), "output.xlsx")
