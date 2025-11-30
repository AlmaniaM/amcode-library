#!/bin/bash

# Script to build all projects in AMCode Library
# Works on Ubuntu 20.04+ and macOS

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
# Usage: build-all.sh [Configuration] [ProjectName]
#   Configuration: Debug or Release (default: Release)
#   ProjectName: Optional - build specific project instead of all
CONFIGURATION="${1:-Release}"  # Default to Release, can be overridden with Debug
SPECIFIC_PROJECT="${2:-}"       # Optional specific project to build

# Main execution
main() {
    echo -e "${GREEN}=== AMCode Library - Build All Projects ===${NC}"
    
    # Get script directory and navigate to project root
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
    PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
    cd "$PROJECT_ROOT"
    
    echo -e "${BLUE}Project root: $PROJECT_ROOT${NC}"
    echo -e "${BLUE}Configuration: $CONFIGURATION${NC}"
    
    # Check if .NET is installed
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}.NET SDK is not installed. Please run ./tools/install-dependencies.sh first.${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}Using .NET version: $(dotnet --version)${NC}"
    
    # If specific project is requested, build only that
    if [ -n "$SPECIFIC_PROJECT" ]; then
        echo -e "${YELLOW}Building specific project: $SPECIFIC_PROJECT${NC}"
        
        # Find the project
        PROJECT_PATH=""
        if [[ "$SPECIFIC_PROJECT" == *.csproj ]] && [ -f "$SPECIFIC_PROJECT" ]; then
            PROJECT_PATH="$SPECIFIC_PROJECT"
        else
            # Search for project by name
            while IFS= read -r proj; do
                local basename=$(basename "$proj" .csproj)
                if [[ "$basename" == "$SPECIFIC_PROJECT" ]] || [[ "$basename" == *"$SPECIFIC_PROJECT"* ]] || [[ "$proj" == *"$SPECIFIC_PROJECT"* ]]; then
                    PROJECT_PATH="$proj"
                    break
                fi
            done < <(find . -name "*.csproj" -type f -not -path "*/bin/*" -not -path "*/obj/*")
        fi
        
        if [ -z "$PROJECT_PATH" ] || [ ! -f "$PROJECT_PATH" ]; then
            echo -e "${RED}Error: Project not found: $SPECIFIC_PROJECT${NC}"
            echo -e "${YELLOW}Use './tools/project-cli.sh list' to see available projects${NC}"
            exit 1
        fi
        
        echo -e "${BLUE}Building: $PROJECT_PATH${NC}"
        if dotnet build "$PROJECT_PATH" -c "$CONFIGURATION" --no-restore; then
            echo -e "${GREEN}Project built successfully${NC}"
            exit 0
        else
            echo -e "${RED}Project build failed${NC}"
            exit 1
        fi
    fi
    
    # Build the solution if it exists
    if [ -f "AMCode.sln" ]; then
        echo -e "${YELLOW}Building solution: AMCode.sln${NC}"
        dotnet build AMCode.sln -c "$CONFIGURATION" --no-restore
        
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}Solution built successfully${NC}"
        else
            echo -e "${RED}Solution build failed${NC}"
            exit 1
        fi
    else
        echo -e "${YELLOW}AMCode.sln not found. Building individual projects...${NC}"
        
        # Find all project directories (excluding test projects for now)
        # Use process substitution to avoid subshell issues with arrays
        mapfile -t PROJECTS < <(find . -name "*.csproj" -type f \
            -not -name "*Test*.csproj" -not -name "*Tests.csproj" -not -name "*UnitTest*.csproj" -not -name "*IntegrationTest*.csproj" -not -name "*SQLTest*.csproj" \
            -not -path "*/bin/*" -not -path "*/obj/*" \
            -not -path "*/tools/*" -not -path "*/packages/*" -not -path "*/phase-5b-testing/*")
        
        # Build each project
        FAILED_PROJECTS=()
        SUCCESSFUL_PROJECTS=()
        
        for proj in "${PROJECTS[@]}"; do
            echo -e "${BLUE}Building: $proj${NC}"
            if dotnet build "$proj" -c "$CONFIGURATION" --no-restore; then
                SUCCESSFUL_PROJECTS+=("$proj")
                echo -e "${GREEN}✓ Built: $proj${NC}"
            else
                FAILED_PROJECTS+=("$proj")
                echo -e "${RED}✗ Failed: $proj${NC}"
            fi
        done
        
        # Summary
        echo -e "${GREEN}=== Build Summary ===${NC}"
        echo -e "${GREEN}Successful: ${#SUCCESSFUL_PROJECTS[@]}${NC}"
        echo -e "${RED}Failed: ${#FAILED_PROJECTS[@]}${NC}"
        
        if [ ${#FAILED_PROJECTS[@]} -gt 0 ]; then
            echo -e "${RED}Failed projects:${NC}"
            for proj in "${FAILED_PROJECTS[@]}"; do
                echo -e "${RED}  - $proj${NC}"
            done
            exit 1
        fi
    fi
    
    echo -e "${GREEN}=== Build complete ===${NC}"
}

# Run main function
main "$@"

