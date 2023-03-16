# Prompt the user for the MITRE ATT&CK technique ID
$techniqueID = Read-Host "Enter the MITRE ATT&CK technique ID"

# Build the URL for the MITRE ATT&CK API
$url = "https://attack.mitre.org/api/v1/techniques/$techniqueID/"

# Make a GET request to the MITRE ATT&CK API
$response = Invoke-RestMethod $url

# Extract the data components from the response
$dataComponents = $response.datasources | Select-Object -ExpandProperty name

# Output the data components
Write-Host "Data Components for $techniqueID:`n"
foreach ($component in $dataComponents) {
    Write-Host "- $component"
}
