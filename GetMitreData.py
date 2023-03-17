import requests

# List of MITRE ATT&CK technique IDs
technique_ids = ["T1003", "T1004", "T1005"]

# Loop through each technique ID
for technique_id in technique_ids:
    # Construct URL for technique page
    url = f"https://attack.mitre.org/techniques/{technique_id}/"

    # Send request to website and get response
    response = requests.get(url)

    # Print HTML content to screen
    print(response.content)




import openpyxl
import re

# Specify the path to the Excel file
file_path = 'path/to/excel_file.xlsx'

# Load the workbook
workbook = openpyxl.load_workbook(file_path)

# Select the worksheet you want to extract data from
worksheet = workbook.active

# Define a set to hold the unique MITRE technique IDs found
technique_ids = set()

# Define the regular expression pattern to match MITRE technique IDs
pattern = r'T\d{4}'

# Loop through each row in column C, starting from the second row
for row in worksheet.iter_rows(min_row=2, min_col=3, max_col=3):
    cell = row[0]
    # Extract all the MITRE technique IDs from the cell using the regex pattern
    matches = re.findall(pattern, cell.value)
    # Add the technique IDs to the set
    technique_ids.update(matches)

# Print the list of technique IDs
print(list(technique_ids))



import openpyxl
import re

# Specify the path to the Excel file
file_path = 'path/to/excel_file.xlsx'

# Load the workbook
workbook = openpyxl.load_workbook(file_path)

# Select the worksheet you want to extract data from
worksheet = workbook.active

# Define a set to hold the unique MITRE technique IDs found
technique_ids = set()

# Define the regular expression pattern to match MITRE technique IDs
pattern = r'T\d{4}'

# Loop through each row in column C, starting from the second row
for row in worksheet.iter_rows(min_row=2, min_col=3, max_col=3):
    cell = row[0]
    if cell.value is not None:  # Skip over empty cells
        # Extract all the MITRE technique IDs from the cell using the regex pattern
        matches = re.findall(pattern, cell.value)
        # Add the technique IDs to the set
        technique_ids.update(matches)

# Print the list of technique IDs
print(list(technique_ids))


