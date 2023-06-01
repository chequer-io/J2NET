# J2NET
[![Nuget](https://img.shields.io/nuget/v/J2NET)](https://www.nuget.org/packages/J2NET/)  
Provides an environment for running Java in .NET

## Support
| Platform | Architecture           | Package |
| -------- | ---------------------- | ------- |
| macOS    | Intel (X64)            | [![J2NET.Runtime.macOS-x64](https://img.shields.io/nuget/v/J2NET.Runtime.macOS-x64)](https://www.nuget.org/packages/J2NET.Runtime.macOS-x64/) |
| macOS    | Apple Silicon (ARM64)  | [![J2NET.Runtime.macOS-arm64](https://img.shields.io/nuget/v/J2NET.Runtime.macOS-arm64)](https://www.nuget.org/packages/J2NET.Runtime.macOS-arm64/) |
| Linux    | X64                    | [![J2NET.Runtime.linux-x64](https://img.shields.io/nuget/v/J2NET.Runtime.linux-x64)](https://www.nuget.org/packages/J2NET.Runtime.linux-x64/) |
| Linux    | ARM64                  | [![J2NET.Runtime.linux-arm64](https://img.shields.io/nuget/v/J2NET.Runtime.linux-arm64)](https://www.nuget.org/packages/J2NET.Runtime.linux-arm64/) |
| Alpine   | X64.                   | [![J2NET.Runtime.linux-musl-x64](https://img.shields.io/nuget/v/J2NET.Runtime.linux-musl-x64)](https://www.nuget.org/packages/J2NET.Runtime.linux-musl-x64/) |
| Windows  | X64                    | [![J2NET.Runtime.win-x64](https://img.shields.io/nuget/v/J2NET.Runtime.win-x64)](https://www.nuget.org/packages/J2NET.Runtime.win-x64/) |
| Windows  | X86                    | [![J2NET.Runtime.win-x86](https://img.shields.io/nuget/v/J2NET.Runtime.win-x86)](https://www.nuget.org/packages/J2NET.Runtime.win-x86/) |

## Getting Started
### 1. Install NuGet package
Install the latest version of the **J2NET** package from NuGet.

### 2. Add Runtime package reference
Paste the following XML into your Project(*.csproj / .vbproj / .fsproj*) file.

```xml
<PropertyGroup>
    <RuntimeVersion>1.3.0</RuntimeVersion>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::OSX)))' == 'true'">OSX</OSPlatform>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Linux)))' == 'true'">Linux</OSPlatform>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Windows)))' == 'true'">Windows</OSPlatform>
    <OSArchitecture>$([System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture)</OSArchitecture>
</PropertyGroup>

<ItemGroup>
    <PackageReference Condition=" '$(OSPlatform)' == 'OSX' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.macOS-x64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'OSX' And '$(OSArchitecture)' == 'ARM64' " Include="J2NET.Runtime.macOS-arm64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Linux' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.linux-x64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Linux' And '$(OSArchitecture)' == 'ARM64' " Include="J2NET.Runtime.linux-arm64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Windows' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.win-x64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Windows' And '$(OSArchitecture)' == 'X86' " Include="J2NET.Runtime.win-x86" Version="$(RuntimeVersion)" />
</ItemGroup>
```

### 3. Run the Code!
```csharp
var process = JavaRuntime.ExecuteJar("../../../Sample/Sample.jar");
process.WaitForExit();
```
