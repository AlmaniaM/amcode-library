#!/bin/bash
# CreatePackages.sh
# Shell script to create NuGet packages for all AMCode libraries

OUTPUT_PATH="./packages"
CONFIGURATION="Release"

echo "Creating NuGet packages for AMCode libraries..."
echo "Output Path: $OUTPUT_PATH"
echo "Configuration: $CONFIGURATION"

# Create output directory if it doesn't exist
if [ ! -d "$OUTPUT_PATH" ]; then
    mkdir -p "$OUTPUT_PATH"
    echo "Created output directory: $OUTPUT_PATH"
fi

SUCCESS_COUNT=0
FAILURE_COUNT=0

# Function to process a library
process_library() {
    local library_name="$1"
    local library_path="$2"
    
    echo ""
    echo "Processing $library_name..."
    
    # Check if project file exists
    if [ ! -f "$library_path" ]; then
        echo "Project file not found: $library_path"
        ((FAILURE_COUNT++))
        return 1
    fi
    
    echo "Building $library_name..."
    
    # Build the project
    if ! dotnet build "$library_path" --configuration "$CONFIGURATION" --no-restore; then
        echo "Build failed for $library_name"
        ((FAILURE_COUNT++))
        return 1
    fi
    
    echo "Creating NuGet package for $library_name..."
    
    # Create NuGet package
    if ! dotnet pack "$library_path" --configuration "$CONFIGURATION" --output "$OUTPUT_PATH" --no-build; then
        echo "Package creation failed for $library_name"
        ((FAILURE_COUNT++))
        return 1
    fi
    
    echo "✅ Successfully created package for $library_name"
    ((SUCCESS_COUNT++))
    return 0
}

# Process all libraries
process_library "AMCode.AI" "ailibrary/AMCode.AI/AMCode.AI.csproj"
process_library "AMCode.Documents" "documentlibrary/AMCode.Documents/AMCode.Documents.csproj"
process_library "AMCode.Exports" "exportslibrary/AMCode.Exports/AMCode.Exports.csproj"
process_library "AMCode.Storage" "storagelibrary/AMCode.Storage/AMCode.Storage.csproj"

# Display summary
echo ""
echo "=================================================="
echo "Package Creation Summary"
echo "=================================================="
echo "✅ Successful: $SUCCESS_COUNT"
echo "❌ Failed: $FAILURE_COUNT"
echo "Total: 4"

if [ $SUCCESS_COUNT -eq 4 ]; then
    echo ""
    echo "🎉 All packages created successfully!"
    echo "Packages are available in: $OUTPUT_PATH"
    echo ""
    echo "Created packages:"
    ls -la "$OUTPUT_PATH"/*.nupkg
else
    echo ""
    echo "⚠️ Some packages failed to create. Please check the errors above."
fi

echo ""
echo "Package creation completed."
