Sub GetTechniqueData()

    Dim techniqueIDs As Variant
    techniqueIDs = InputBox("Enter technique IDs separated by commas (e.g. T1595,T1059):")
    
    Dim techniqueData As Variant
    techniqueData = Split(techniqueIDs, ",")
    
    Dim outputWorkbook As Workbook
    Set outputWorkbook = Workbooks.Add
    
    Dim outputWorksheet As Worksheet
    Set outputWorksheet = outputWorkbook.Worksheets(1)
    
    outputWorksheet.Range("A1").Value = "Technique ID"
    outputWorksheet.Range("B1").Value = "Data Sources"
    outputWorksheet.Range("C1").Value = "Data Components"
    
    Dim rowIndex As Integer
    rowIndex = 2
    
    For Each techniqueID In techniqueData
    
        Dim techniqueURL As String
        techniqueURL = "https://attack.mitre.org/technique/" & techniqueID & "/"
        
        Dim xmlhttp As Object
        Set xmlhttp = CreateObject("MSXML2.XMLHTTP")
        
        xmlhttp.Open "GET", techniqueURL, False
        xmlhttp.Send
        
        Dim htmlDoc As Object
        Set htmlDoc = CreateObject("HTMLDocument")
        htmlDoc.body.innerHTML = xmlhttp.responseText
        
        Dim dataSources As Object
        Set dataSources = htmlDoc.getElementsByClassName("field-external_references")(0).getElementsByTagName("li")
        
        Dim dataComponents As Object
        Set dataComponents = htmlDoc.getElementsByClassName("field-data_components")(0).getElementsByTagName("li")
        
        outputWorksheet.Range("A" & rowIndex).Value = techniqueID
        
        Dim dataSourceList As String
        For Each dataSource In dataSources
            dataSourceList = dataSourceList & dataSource.innerText & ", "
        Next dataSource
        If Len(dataSourceList) > 0 Then
            dataSourceList = Left(dataSourceList, Len(dataSourceList) - 2)
        End If
        outputWorksheet.Range("B" & rowIndex).Value = dataSourceList
        
        Dim dataComponentList As String
        For Each dataComponent In dataComponents
            dataComponentList = dataComponentList & dataComponent.innerText & ", "
        Next dataComponent
        If Len(dataComponentList) > 0 Then
            dataComponentList = Left(dataComponentList, Len(dataComponentList) - 2)
        End If
        outputWorksheet.Range("C" & rowIndex).Value = dataComponentList
        
        rowIndex = rowIndex + 1
        
    Next techniqueID
    
    outputWorksheet.Columns.AutoFit
    outputWorkbook.SaveAs "TechniqueData.xlsx"
    outputWorkbook.Close

End Sub
