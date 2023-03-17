import requests
from bs4 import BeautifulSoup

# specify the URL of the website to scrape
url = 'https://www.example.com'

# download the HTML page using the requests library
response = requests.get(url)

# parse the HTML page using the BeautifulSoup library
soup = BeautifulSoup(response.content, 'html.parser')

# find the table with the specified class using the find() method
table = soup.find('table', {'class': 'my-table-class'})

# check if the table was found
if table is not None:
    # iterate over each row in the table and print the data
    for row in table.find_all('tr'):
        # iterate over each cell in the row and print the data
        for cell in row.find_all('td'):
            print(cell.text, end='\t')
        print()
else:
    print('Table not found on the page.')
