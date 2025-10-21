# Spectre.Console.Extensions.Markup

Spectre console Markdown rendering extension

## Overview

This repository contains extensions for [Spectre.Console](https://spectreconsole.net/) to provide syntax highlighting and rendering for various programming languages and markup formats. It includes support for:

- **Spectre.Console.Extensions.Markup**: Core markup rendering extension
- **Spectre.Console.CSharp**: C# syntax highlighting
- **Spectre.Console.Javascript**: JavaScript syntax highlighting
- **Spectre.Console.Sql**: SQL syntax highlighting
- **Spectre.Console.Xml**: XML syntax highlighting

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [GitVersion.Tool](https://github.com/GitTools/GitVersion) (installed globally for version management)
- PowerShell (for build scripts)

## Building

To build the solution:

```bash
cd src
dotnet restore Spectre.Console.Extensions.Markup.sln
dotnet build Spectre.Console.Extensions.Markup.sln --configuration Release
```

## Testing

Run the tests:

```bash
cd src
dotnet test --configuration Release
```

## Packaging

The repository includes a PowerShell build script (`pack.ps1`) that handles building, testing, and packaging.

### Using the Build Script

The `pack.ps1` script provides comprehensive build functionality:

```powershell
.\src\pack.ps1 -Configuration Release -CreateNugetPackages $true -RunGitVersion $true
```

#### Parameters

- **Configuration**: Build configuration (`Debug` or `Release`, default: `Debug`)
- **ForceInstallPackage**: Install the BuildUtils PowerShell module if not present (`$false` by default)
- **BuildArtifactsDirectory**: Directory for build artifacts (default: `./BuildOutput` under the script directory)
- **CreateNugetPackages**: Create NuGet packages (`$false` by default)
- **RunGitVersion**: Run GitVersion to determine version numbers (`$true` by default)

#### What the Script Does

1. **Version Management**: Uses GitVersion to calculate semantic versions based on Git history
2. **Dependency Restoration**: Restores NuGet packages for the solution
3. **Building**: Compiles all projects with appropriate version information
4. **Testing**: Runs unit tests for all test projects
5. **Packaging**: Creates NuGet packages (`.nupkg`) and symbol packages (`.snupkg`) if requested

#### Example Usage

```powershell
# Build in Release mode with packages
.\src\pack.ps1 -Configuration Release -CreateNugetPackages $true

# Force install BuildUtils and specify custom output directory
.\src\pack.ps1 -ForceInstallPackage $true -BuildArtifactsDirectory "C:\BuildArtifacts" -CreateNugetPackages $true
```

## Versioning

This project uses [GitVersion](https://gitversion.net/) for automatic semantic versioning:

- **Continuous Deployment** mode globally
- **Continuous Delivery** mode for feature branches (increments per commit)
- Versions follow the pattern: `Major.Minor.Patch-PreReleaseLabel.PreReleaseNumber`

## CI/CD

The repository uses GitHub Actions for continuous integration:

- **Workflow**: `ci.yml` (CI - Pack)
- **Triggers**: Push to `main`, `develop`, `release/**`, `hotfix/**`, `feature/**` branches
- **Actions**:
  - Builds and tests on Ubuntu with .NET 9.0
  - Generates NuGet packages
  - Publishes packages to GitHub Packages
  - Uploads build artifacts

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests: `.\src\pack.ps1 -Configuration Release`
5. Submit a pull request

## License

See [LICENSE](LICENSE) file.
