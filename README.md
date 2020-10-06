# J2NET
![Nuget](https://img.shields.io/nuget/v/J2NET)  
Provides an environment for running Java in .NET

## Getting Started
### 1. Install NuGet package
Install the latest version of the J2NET package from NuGet.

### 2. Add Runtime package reference
Paste the following XML into your Project(.csproj, .vbproj, .fsproj) file.

```xml
<PropertyGroup>
    <RuntimeVersion>1.1.0</RuntimeVersion>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::OSX)))' == 'true'">OSX</OSPlatform>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Linux)))' == 'true'">Linux</OSPlatform>
    <OSPlatform Condition="'$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Windows)))' == 'true'">Windows</OSPlatform>
    <OSArchitecture>$([System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture)</OSArchitecture>
</PropertyGroup>

<ItemGroup Label="Development" Condition=" '$(RID)' == '' And '$(Packaging)' == '' ">
    <PackageReference Condition=" '$(OSPlatform)' == 'OSX' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.Mac" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Linux' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.Linux" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Windows' And '$(OSArchitecture)' == 'X64' " Include="J2NET.Runtime.Win64" Version="$(RuntimeVersion)" />
    <PackageReference Condition=" '$(OSPlatform)' == 'Windows' And '$(OSArchitecture)' == 'X86' " Include="J2NET.Runtime.Win32" Version="$(RuntimeVersion)" />
</ItemGroup>
```

### 3. Run the Code!
```csharp
var process = JavaRuntime.ExecuteJar("../../../Sample/Sample.jar");
process.WaitForExit();
```
