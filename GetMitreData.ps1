# Replace the list of technique IDs with your own list
$techniqueIds = @('T1003', 'T1053', 'T1134')

foreach ($techniqueId in $techniqueIds) {
    # Send a GET request to the MITRE ATT&CK website for the specified technique ID
    $response = Invoke-WebRequest -Uri "https://attack.mitre.org/techniques/$techniqueId/"

    # Extract the Detection table from the HTML content of the response
    $table = $response.Content | Select-Xml -XPath "//section[@id='Detection']//table" | Select-Object -ExpandProperty Node

    # Output the table to the console
    Write-Host "Detection table for $techniqueId:"
    Write-Host $table.OuterHtml
    Write-Host "`n"
}
