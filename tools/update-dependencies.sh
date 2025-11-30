#!/bin/bash

# Script to update .NET package dependencies for AMCode Library
# Works on Ubuntu 20.04+ and macOS

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Main execution
main() {
    echo -e "${GREEN}=== AMCode Library - Update Dependencies ===${NC}"
    
    # Get script directory and navigate to project root
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
    PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
    cd "$PROJECT_ROOT"
    
    echo -e "${BLUE}Project root: $PROJECT_ROOT${NC}"
    
    # Check if .NET is installed
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}.NET SDK is not installed. Please run ./tools/install-dependencies.sh first.${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}Using .NET version: $(dotnet --version)${NC}"
    
    # Install dotnet-outdated-tool if not already installed
    echo -e "${YELLOW}Checking for dotnet-outdated-tool...${NC}"
    if dotnet tool list -g | grep -q "dotnet-outdated-tool"; then
        echo -e "${GREEN}dotnet-outdated-tool is already installed${NC}"
    else
        echo -e "${YELLOW}Installing dotnet-outdated-tool globally...${NC}"
        dotnet tool install -g dotnet-outdated-tool
        echo -e "${GREEN}dotnet-outdated-tool installed successfully${NC}"
    fi
    
    # Update tool if needed
    echo -e "${YELLOW}Updating dotnet-outdated-tool...${NC}"
    dotnet tool update -g dotnet-outdated-tool 2>/dev/null || true
    
    # Run dotnet outdated on the solution
    if [ -f "AMCode.sln" ]; then
        echo -e "${YELLOW}Checking for outdated packages in AMCode.sln...${NC}"
        dotnet outdated AMCode.sln
        
        echo -e "${BLUE}To update packages, review the output above and update PackageReference versions in .csproj files${NC}"
    else
        echo -e "${YELLOW}AMCode.sln not found. Checking individual projects...${NC}"
        
        # Find all .csproj files
        while IFS= read -r proj; do
            if [ -f "$proj" ]; then
                echo -e "${BLUE}Checking: $proj${NC}"
                dotnet outdated "$proj" || echo -e "${YELLOW}Warning: Could not check $proj${NC}"
            fi
        done < <(find . -name "*.csproj" -type f -not -path "*/bin/*" -not -path "*/obj/*")
    fi
    
    # Optionally, restore packages after checking
    echo -e "${YELLOW}Restoring packages...${NC}"
    if [ -f "AMCode.sln" ]; then
        dotnet restore AMCode.sln
    else
        while IFS= read -r proj; do
            if [ -f "$proj" ]; then
                dotnet restore "$proj" || true
            fi
        done < <(find . -name "*.csproj" -type f -not -path "*/bin/*" -not -path "*/obj/*")
    fi
    
    echo -e "${GREEN}=== Dependency update check complete ===${NC}"
    echo -e "${BLUE}Note: This script only checks for outdated packages. To update, manually edit .csproj files with new versions.${NC}"
}

# Run main function
main "$@"

