import requests
from bs4 import BeautifulSoup
from tabulate import tabulate

while True:
    technique_id = input("Enter a MITRE ATT&CK technique ID (enter T0000 to exit): ")
    if technique_id == "T0000":
        break
    url = f"https://attack.mitre.org/techniques/{technique_id}/"
    response = requests.get(url)
    soup = BeautifulSoup(response.content, 'html.parser')
    table = soup.find('table', {'class': 'table table-bordered table-hover table-striped'})
    if table is None:
        print("No table found for this technique ID.")
        continue
    headers = [th.text.strip() for th in table.find_all('th')]
    data = []
    for row in table.find_all('tr'):
        cols = row.find_all('td')
        if len(cols) >= 3:
            data.append([cols[1].text.strip(), cols[2].text.strip()])
    print(tabulate(data, headers=headers[1:]))
