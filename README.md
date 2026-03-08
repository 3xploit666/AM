<div align="center">

# AM

**AMSI Bypass via Resurrection Technique**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://www.microsoft.com/windows)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

*Proof of concept demonstrating AMSI bypass using code resurrection with payload linking*

</div>

---

## Overview

**AM** is a C# proof-of-concept that demonstrates how the Windows Antimalware Scan Interface (AMSI) can be bypassed using the resurrection technique. This tool patches AMSI in-memory, allowing subsequent PowerShell payloads to execute without being scanned.

The project includes a launcher/loader component that integrates with C2 frameworks like Covenant for payload delivery.

## How It Works

1. **AMSI Patch** — Modifies the `AmsiScanBuffer` function in memory to neutralize scanning
2. **Payload Link** — Fetches and executes the payload after AMSI is disabled
3. **Loader Integration** — Compatible with Covenant and similar C2 frameworks

## Screenshots

**AMSI Detection (Before Bypass):**

[![Screenshot-5.png](https://i.postimg.cc/KcL3kWdC/Screenshot-5.png)](https://postimg.cc/grz21gcK)

**Successful Bypass & Execution:**

[![Screenshot-4.png](https://i.postimg.cc/kMyhmTTP/Screenshot-4.png)](https://postimg.cc/bdsR09Xm)

**Launcher/Loader Integration:**

[![Screenshot-8.png](https://i.postimg.cc/Gh9LT7gv/Screenshot-8.png)](https://postimg.cc/nCbb6kJL)

**Covenant C2 Panel:**

[![Screenshot-9.png](https://i.postimg.cc/wTtzjt3V/Screenshot-9.png)](https://postimg.cc/8jV9yzRJ)

## Build

```bash
# .NET SDK required
dotnet build -c Release
```

## Usage

```bash
# Run the AMSI bypass loader
.\AM.exe
```

## Legal Disclaimer

> **This tool is intended for authorized penetration testing and security research only.** Unauthorized use against systems you do not own or have explicit permission to test is illegal. The author assumes no liability for misuse of this software.

## Author

**[@3xploit666](https://github.com/3xploit666)**

---

<div align="center">

*For educational and authorized security testing purposes only.*

</div>
