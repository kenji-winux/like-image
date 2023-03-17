import requests
import pandas as pd
import openpyxl
import pyperclip
from bs4 import BeautifulSoup

while True:
    technique_id = input("Enter a MITRE ATT&CK technique ID (e.g., T1059): ")
    if technique_id == "T000":
        break
    url = f"https://attack.mitre.org/techniques/{technique_id}/"
    response = requests.get(url)
    soup = BeautifulSoup(response.content, 'html.parser')
    table = soup.find('table', {'class': 'table table-bordered table-hover table-condensed'})
    if table:
        df = pd.read_html(str(table))[0].iloc[:, 1:3]
        filename = f"{technique_id}.xlsx"
        with pd.ExcelWriter(filename, mode='w', engine='openpyxl') as writer:
            df.to_excel(writer, index=False, header=False)
            writer.save()
        pyperclip.copy(df.to_string(index=False, header=False))
        print(f"Data for {technique_id} has been copied to the clipboard and saved to {filename}.")
    else:
        print(f"No data found for {technique_id}.")
