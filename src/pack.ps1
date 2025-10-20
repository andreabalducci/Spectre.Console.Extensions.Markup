<#
.SYNOPSIS
Build, run tests and optionally create NuGet packages for Spectre.Console.Extensions.Markup

.DESCRIPTION
This script builds the solution, optionally runs GitVersion to compute versions,
runs tests and (optionally) creates NuGet packages. It's adapted to the
Spectre.Console.Extensions.Markup repository layout.

.PARAMETER Configuration
Debug or Release

.PARAMETER ForceInstallPackage
If $true installs BuildUtils module.

.PARAMETER BuildArtifactsDirectory
Directory where artifacts (test results, packages) will be placed. If empty,
uses ./BuildOutput under the script directory.

.PARAMETER CreateNugetPackages
If $true the script will create nuget packages.

.PARAMETER RunGitVersion
If $true attempt to run GitVersion (requires BuildUtils/Invoke-GitVersion helper).
#>

Param(
    [string]  $Configuration = "Debug",
    [Boolean] $ForceInstallPackage = $false,
    [string]  $BuildArtifactsDirectory = "",
    [Boolean] $CreateNugetPackages = $false,
    [Boolean] $RunGitVersion = $true
)

# Install and import BuildUtils (provides Invoke-GitVersion, Assert-LastExecution, etc.)
if ($ForceInstallPackage -eq $true) {
    Write-Host "Installing BuildUtils module (current user)..."
    Install-Package BuildUtils -Confirm:$false -Scope CurrentUser -Force -ErrorAction SilentlyContinue
}

try {
    Import-Module BuildUtils -ErrorAction Stop
    Write-Host "BuildUtils module imported. Using Assert-LastExecution from module if available."
}
catch {
    Write-Warning "BuildUtils module could not be imported. Falling back to local Assert-LastExecution implementation."
    function Assert-LastExecution {
        Param(
            [string] $message = "Command failed",
            [bool] $haltExecution = $false
        )
        if ($LASTEXITCODE -ne 0) {
            Write-Error $message
            if ($haltExecution) { exit 1 }
        }
    }
}

# Where the script lives
$runningDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

if ($BuildArtifactsDirectory -ne "") {
    $publishDirectory = $BuildArtifactsDirectory
} else {
    $publishDirectory = [System.IO.Path]::Combine($runningDirectory, "BuildOutput")
}

Write-Host "Artifacts will be placed in: $publishDirectory"

# Ensure publish directory
if (Test-Path $publishDirectory) { Remove-Item -Recurse -Force -Path $publishDirectory -ErrorAction SilentlyContinue }
New-Item -ItemType Directory -Path $publishDirectory -Force | Out-Null

# repository-specific solution and projects
$solutionName = [System.IO.Path]::Combine($runningDirectory, "Spectre.Console.Extensions.Markup.sln")
# projects to pack: find all csproj under the repository and exclude test/example projects
$projectsToPack = Get-ChildItem -Path $runningDirectory -Recurse -Filter "*.csproj" | Where-Object { $_.Name -notmatch 'Tests' -and $_.Name -notmatch 'Example' }

if (-not $projectsToPack -or $projectsToPack.Count -eq 0) {
    Write-Warning "No projects found to pack."
}
else {
    Write-Host "Projects to pack:"
    $projectsToPack | ForEach-Object { Write-Host " - $($_.FullName)" }
}

Write-Host "Solution: $solutionName"

if ($RunGitVersion) {
    Write-Host "Attempting to obtain GitVersion information..."

    # If CI (GitHub Actions) provided GitVersion values via environment variables, use them
    if ($env:GITVERSION_ASSEMBLYVERSION -and $env:GITVERSION_FILEVERSION -and $env:GITVERSION_NUGETVERSION -and $env:GITVERSION_INFORMATIONALVERSION) {
        Write-Host "Using GitVersion values from environment variables provided by CI."
        $assemblyVersion = $env:GITVERSION_ASSEMBLYVERSION
        $assemblyFileVersion = $env:GITVERSION_FILEVERSION
        $nugetVersion = $env:GITVERSION_NUGETVERSION
        $assemblyInformationalVersion = $env:GITVERSION_INFORMATIONALVERSION
    }
    else {
        Write-Host "No GitVersion env vars found; attempting to restore dotnet tools and run GitVersion locally (if available)..."
        dotnet tool restore
        if ($LASTEXITCODE -ne 0) { Write-Warning "dotnet tool restore failed or no tools to restore." }

        try {
            $gitversion = Invoke-GitVersion -ConfigurationFile "$runningDirectory/.config/GitVersion.yml" -ErrorAction Stop
            Write-Host "GitVersion found: $($gitversion.fullSemver)"
            $assemblyVersion = $gitversion.assemblyVersion
            $assemblyFileVersion = $gitversion.assemblyFileVersion
            $nugetVersion = $gitversion.nugetVersion
            $assemblyInformationalVersion = $gitversion.assemblyInformationalVersion
        }
        catch {
            Write-Warning "Invoke-GitVersion not available or failed, falling back to 1.0.0"
            $assemblyVersion = "1.0.0"
            $assemblyFileVersion = "1.0.0"
            $nugetVersion = "1.0.0"
            $assemblyInformationalVersion = "1.0.0"
        }
    }
}
else {
    $assemblyVersion = "1.0.0"
    $assemblyFileVersion = "1.0.0"
    $nugetVersion = "1.0.0"
    $assemblyInformationalVersion = "1.0.0"
}

Write-Host "AssemblyVersion: $assemblyVersion FileVersion: $assemblyFileVersion NuGet: $nugetVersion"

Write-Host "Restoring solution dependencies..."
dotnet restore "$solutionName"
Assert-LastExecution -message "Unable to restore dependencies of the solution." -haltExecution $true

Write-Host "Building solution ($Configuration)..."
dotnet build "$solutionName" -p:IncludeSymbols=true --configuration $Configuration /p:AssemblyVersion=$assemblyVersion /p:FileVersion=$assemblyFileVersion /p:InformationalVersion=$assemblyInformationalVersion
Assert-LastExecution -message "Build failed." -haltExecution $true

Write-Host "Running tests..."
$testProjects = Get-ChildItem -Path $runningDirectory -Recurse -Filter "*.Tests.csproj" -ErrorAction SilentlyContinue
foreach ($tp in $testProjects) {
    Write-Host "Running tests for $($tp.FullName)"
    dotnet test "$($tp.FullName)" --no-build --configuration $Configuration --results-directory "$publishDirectory" --logger "trx;"
}

if ($CreateNugetPackages) {
    if ($projectsToPack -and $projectsToPack.Count -gt 0) {
        foreach ($proj in $projectsToPack) {
            Write-Host "Packing project $($proj.FullName)"
            dotnet pack $proj.FullName -o $publishDirectory --configuration $Configuration /p:PackageVersion=$nugetVersion /p:AssemblyVersion=$assemblyVersion /p:FileVersion=$assemblyFileVersion /p:InformationalVersion=$assemblyInformationalVersion -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
            Assert-LastExecution -message "dotnet pack failed for $($proj.FullName)" -haltExecution $true
        }
    }
    else {
        Write-Warning "No projects determined to pack. Skipping dotnet pack."
    }
}

Write-Host "All done. Artifacts are in: $publishDirectory"
exit 0
