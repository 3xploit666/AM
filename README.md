<div align="center">

# AmsiResurrect

**AMSI Bypass via Resurrection Technique**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET_4.7.2-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://www.microsoft.com/windows)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

*In-memory AMSI patch using reflection-based assembly loading and encrypted API resolution*

</div>

---

## Overview

**AmsiResurrect** patches the Windows Antimalware Scan Interface (AMSI) in-memory by modifying `AmsiScanBuffer` to return `E_INVALIDARG` (`0x80070057`). This neutralizes AMSI scanning for the current process, allowing subsequent PowerShell payloads to execute unscanned.

The project includes a loader generator script that creates a self-contained PowerShell payload for use with C2 frameworks like Covenant.

## How It Works

```
1. Assembly loaded via reflection  →  [Assembly]::Load(bytes)
2. Resurrect.Patch() called        →  Resolves amsi.dll + AmsiScanBuffer (AES-encrypted strings)
3. AmsiScanBuffer patched           →  mov eax, 0x80070057; ret
4. Remote payload downloaded        →  IEX via WebClient
```

## Architecture

```
AmsiResurrect/
├── AM/
│   ├── Program.cs          — Core AMSI patch logic with AES-encrypted API resolution
│   ├── Apis.cs             — P/Invoke declarations (NativeMethods)
│   └── AM.csproj           — .NET Framework 4.7.2 project
├── ConvertBytes2.ps1       — Loader generator script
└── README.md
```

| File | Purpose |
|------|---------|
| `Program.cs` | Patches `AmsiScanBuffer` using `VirtualProtect` + `Marshal.Copy` |
| `Apis.cs` | `NativeMethods` class with `LoadLibrary`, `GetProcAddress`, `VirtualProtect` |
| `ConvertBytes2.ps1` | Converts compiled DLL to byte array and generates PowerShell loader |

## Technical Details

- **Patch**: `mov eax, 0x80070057; ret` — forces `AmsiScanBuffer` to return `E_INVALIDARG`
- **Encryption**: API strings (`amsi.dll`, `AmsiScanBuffer`) encrypted with AES-256-CBC
- **Key Derivation**: PBKDF2 (`Rfc2898DeriveBytes`) with 1000 iterations
- **Target**: .NET Framework 4.7.2, AnyCPU

## Build

```bash
# Visual Studio
Open AM.sln → Build → Release

# Command line (requires MSBuild)
msbuild AM.sln /p:Configuration=Release
```

## Usage

### 1. Build the DLL

```bash
msbuild AM.sln /p:Configuration=Release
```

### 2. Generate the Loader

```powershell
.\ConvertBytes2.ps1
# [***PATH FILE***]: .\AM\bin\Release\AM.dll
# [Payload URL]: https://your-c2/payload.ps1
# [+] Loader generated: C:\Users\<user>\loader.ps1
```

### 3. Execute

The generated `loader.ps1` will:
1. Load the assembly via reflection
2. Call `[AmsiResurrect.Resurrect]::Patch()` to disable AMSI
3. Download and execute the remote payload

## Screenshots

**AMSI Detection (Before Bypass):**

[![Screenshot-5.png](https://i.postimg.cc/KcL3kWdC/Screenshot-5.png)](https://postimg.cc/grz21gcK)

**Successful Bypass & Execution:**

[![Screenshot-4.png](https://i.postimg.cc/kMyhmTTP/Screenshot-4.png)](https://postimg.cc/bdsR09Xm)

**Launcher/Loader Integration:**

[![Screenshot-8.png](https://i.postimg.cc/Gh9LT7gv/Screenshot-8.png)](https://postimg.cc/nCbb6kJL)

**Covenant C2 Panel:**

[![Screenshot-9.png](https://i.postimg.cc/wTtzjt3V/Screenshot-9.png)](https://postimg.cc/8jV9yzRJ)

## Legal Disclaimer

> **This tool is intended for authorized penetration testing and security research only.** Unauthorized use against systems you do not own or have explicit permission to test is illegal. The author assumes no liability for misuse of this software.

## Author

**[@3xploit666](https://github.com/3xploit666)**

---

<div align="center">

*For educational and authorized security testing purposes only.*

</div>
