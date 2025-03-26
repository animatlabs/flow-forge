param (
    [string]$WorkflowEngineVersion = "1.0.0",
    [string]$SerilogVersion = "1.0.0",
    [string]$OpenTelemetryVersion = "1.0.0",
    [string]$NuGetApiKey
)

# Ensure NuGet API key is provided
if (-not $NuGetApiKey) {
    Write-Error "NuGet API key is required. Pass it as the -NuGetApiKey parameter."
    exit 1
}

# Function to pack and publish a package
function PackAndPublish {
    param (
        [string]$ProjectPath,
        [string]$PackageName,
        [string]$PackageVersion
    )

    $OutputDir = "./nupkgs"
    dotnet pack $ProjectPath --configuration Release --no-build --output $OutputDir /p:Version=$PackageVersion
    $PackagePath = Join-Path $OutputDir "$PackageName.$PackageVersion.nupkg"
    dotnet nuget push $PackagePath --api-key $NuGetApiKey --source https://api.nuget.org/v3/index.json
}

# Restore dependencies
Write-Host "Restoring dependencies..."
dotnet restore

# Build the solution
Write-Host "Building the solution..."
dotnet build --configuration Release --no-restore

# Pack and publish WorkflowEngine
Write-Host "Packing and publishing SimpleWorkflowEngine..."
PackAndPublish -ProjectPath "./src/SimpleWorkflowEngine/SimpleWorkflowEngine.csproj" -PackageName "SimpleWorkflowEngine" -PackageVersion $WorkflowEngineVersion

# Pack and publish WorkflowEngine.Logger.Serilog
Write-Host "Packing and publishing SimpleWorkflowEngine.Logger.Serilog..."
PackAndPublish -ProjectPath "./src/SimpleWorkflowEngine.Logger.Serilog/SimpleWorkflowEngine.Logger.Serilog.csproj" -PackageName "SimpleWorkflowEngine.Logger.Serilog" -PackageVersion $SerilogVersion

# Pack and publish WorkflowEngine.Middlewares.OpenTelemetry
Write-Host "Packing and publishing SimpleWorkflowEngine.Middlewares.OpenTelemetry..."
PackAndPublish -ProjectPath "./src/SimpleWorkflowEngine.Middlewares.OpenTelemetry/SimpleWorkflowEngine.Middlewares.OpenTelemetry.csproj" -PackageName "SimpleWorkflowEngine.Middlewares.OpenTelemetry" -PackageVersion $OpenTelemetryVersion

Write-Host "All packages have been published successfully."
