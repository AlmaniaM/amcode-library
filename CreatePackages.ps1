# CreatePackages.ps1
# PowerShell script to create NuGet packages for all AMCode libraries

param(
    [string]$OutputPath = "./packages",
    [string]$Configuration = "Release"
)

Write-Host "Creating NuGet packages for AMCode libraries..." -ForegroundColor Green
Write-Host "Output Path: $OutputPath" -ForegroundColor Yellow
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow

# Create output directory if it doesn't exist
if (!(Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force
    Write-Host "Created output directory: $OutputPath" -ForegroundColor Green
}

# Define the libraries to package
$libraries = @(
    @{
        Name = "AMCode.AI"
        Path = "amcode-library/ailibrary/AMCode.AI/AMCode.AI.csproj"
        Description = "Multi-cloud AI service library with recipe parsing capabilities"
    },
    @{
        Name = "AMCode.Documents"
        Path = "amcode-library/documentlibrary/AMCode.Documents/AMCode.Documents.csproj"
        Description = "Document generation library with recipe-specific features"
    },
    @{
        Name = "AMCode.Exports"
        Path = "amcode-library/exportslibrary/AMCode.Exports/AMCode.Exports.csproj"
        Description = "Export library with recipe-specific export capabilities"
    },
    @{
        Name = "AMCode.Storage"
        Path = "amcode-library/storagelibrary/AMCode.Storage/AMCode.Storage.csproj"
        Description = "Storage library with recipe-specific file management"
    }
)

$successCount = 0
$failureCount = 0

foreach ($library in $libraries) {
    Write-Host "`nProcessing $($library.Name)..." -ForegroundColor Cyan
    
    try {
        # Check if project file exists
        if (!(Test-Path $library.Path)) {
            Write-Host "Project file not found: $($library.Path)" -ForegroundColor Red
            $failureCount++
            continue
        }
        
        Write-Host "Building $($library.Name)..." -ForegroundColor Yellow
        
        # Build the project
        $buildResult = dotnet build $library.Path --configuration $Configuration --no-restore
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Build failed for $($library.Name)" -ForegroundColor Red
            $failureCount++
            continue
        }
        
        Write-Host "Creating NuGet package for $($library.Name)..." -ForegroundColor Yellow
        
        # Create NuGet package
        $packResult = dotnet pack $library.Path --configuration $Configuration --output $OutputPath --no-build
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Package creation failed for $($library.Name)" -ForegroundColor Red
            $failureCount++
            continue
        }
        
        Write-Host "✅ Successfully created package for $($library.Name)" -ForegroundColor Green
        $successCount++
        
    } catch {
        Write-Host "❌ Error processing $($library.Name): $($_.Exception.Message)" -ForegroundColor Red
        $failureCount++
    }
}

Write-Host "`n" + "="*50 -ForegroundColor Green
Write-Host "Package Creation Summary" -ForegroundColor Green
Write-Host "="*50 -ForegroundColor Green
Write-Host "✅ Successful: $successCount" -ForegroundColor Green
Write-Host "❌ Failed: $failureCount" -ForegroundColor Red
Write-Host "Total: $($libraries.Count)" -ForegroundColor Yellow

if ($successCount -eq $libraries.Count) {
    Write-Host "`n🎉 All packages created successfully!" -ForegroundColor Green
    Write-Host "Packages are available in: $OutputPath" -ForegroundColor Yellow
    
    # List created packages
    Write-Host "`nCreated packages:" -ForegroundColor Cyan
    Get-ChildItem $OutputPath -Filter "*.nupkg" | ForEach-Object {
        Write-Host "  - $($_.Name)" -ForegroundColor White
    }
} else {
    Write-Host "`n⚠️ Some packages failed to create. Please check the errors above." -ForegroundColor Yellow
}

Write-Host "`nPackage creation completed." -ForegroundColor Green
