if (-not ([System.Environment]::OSVersion.Platform -eq "Win32NT")) {
    throw "Not supported platform."
}

Push-Location $PSScriptRoot

$OutDir = Join-Path (Get-Location) "pack"

function Remove-OutputDir {
    if (Test-Path $OutDir) {
        Remove-Item $OutDir -Recurse -Force
    }
}

try {
    Remove-OutputDir

    $nuget = Resolve-Path "Tools\nuget.exe"
    $Jobs = @()

    # Build Core
    $Jobs += Start-Job -ScriptBlock {
        param($Target, $OutDir)
        Push-Location $Target
        dotnet pack -o $OutDir --nologo
        Pop-Location
    } -ArgumentList (Resolve-Path "Core\J2NET"), $OutDir

    # Build Runtimes
    Get-ChildItem -Path "Runtimes\*\*.nuspec" | ForEach-Object {
        $Jobs += Start-Job -ScriptBlock {
            param($nuget, $NuspecPath, $OutDir)
            & $nuget "pack" $NuspecPath -OutputDirectory $OutDir
        } -ArgumentList $nuget, $_, $OutDir
    }

    $Jobs | Wait-Job | Receive-Job
    
}
catch {
    Remove-OutputDir
    throw
}
finally {
    Pop-Location    
}
