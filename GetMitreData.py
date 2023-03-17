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





import requests
from bs4 import BeautifulSoup
import openpyxl

# specify the URL of the website to scrape
url = 'https://www.example.com'

# specify the class name of the table to extract
table_class = 'my-table-class'

# download the HTML page using the requests library
response = requests.get(url)

# parse the HTML page using the BeautifulSoup library
soup = BeautifulSoup(response.content, 'html.parser')

# find the table with the specified class using the find() method
table = soup.find('table', {'class': table_class})

# create a new Excel workbook and worksheet
workbook = openpyxl.Workbook()
worksheet = workbook.active

# check if the table was found
if table is not None:
    # iterate over each row in the table and write the second and third columns to the worksheet
    for row in table.find_all('tr'):
        cells = row.find_all('td')
        if len(cells) >= 3:
            worksheet.append([cells[1].text, cells[2].text])
else:
    print('Table not found on the page.')

# save the Excel workbook to a file
workbook.save('output.xlsx')
