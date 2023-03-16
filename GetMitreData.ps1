# Prompt the user for a Mitre ATT&CK technique ID
$techniqueID = Read-Host "Enter Mitre ATT&CK Technique ID"

# Construct the API URL for retrieving the data components for the specified technique ID
$url = "https://mitre-attack.github.io/attack-navigator/enterprise-layer/techniques/" + $techniqueID + ".json"

# Invoke the API to retrieve the data components for the specified technique ID
$data = Invoke-RestMethod $url

# Extract the data components from the retrieved JSON data
$dataComponents = $data."x_mitre_data_components"

# Output the data components
Write-Output "Data Components for Technique $techniqueID:`n"
foreach ($component in $dataComponents) {
    Write-Output $component
}
