# Prompt the user for a Mitre ATT&CK technique ID
$techniqueID = Read-Host "Enter Mitre ATT&CK Technique ID"

# Construct the API URL for retrieving the data components for the specified technique ID
$url = "https://cti-taxii.mitre.org/stix/collections/enterprise-attack--4e2e2b6e-0e4b-4ea4-9ac4-aa04c14eeeb3/objects?match[type]=attack-pattern&match[external_references.external_id]=$techniqueID"

# Set the headers for the API request
$headers = @{"User-Agent"="PowerShell"}

# Invoke the API to retrieve the data components for the specified technique ID
$data = Invoke-RestMethod -Uri $url -Headers $headers

# Extract the data components from the retrieved JSON data
$dataComponents = $data.objects.properties.x_mitre_data_components

# Output the data components
Write-Output "Data Components for Technique $techniqueID:`n"
foreach ($component in $dataComponents) {
    Write-Output $component
}
