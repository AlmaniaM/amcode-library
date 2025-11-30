#!/bin/bash

# Script to run all tests in AMCode Library
# Works on Ubuntu 20.04+ and macOS

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
# Usage: test-all.sh [Configuration] [Verbosity] [ProjectName]
#   Configuration: Debug or Release (default: Release)
#   Verbosity: quiet, minimal, normal, detailed, diagnostic (default: normal)
#   ProjectName: Optional - test specific project instead of all
CONFIGURATION="${1:-Release}"  # Default to Release
VERBOSITY="${2:-normal}"       # Default to normal, can be quiet, minimal, normal, detailed, diagnostic
SPECIFIC_PROJECT="${3:-}"       # Optional specific project to test

# Main execution
main() {
    echo -e "${GREEN}=== AMCode Library - Test All Projects ===${NC}"
    
    # Get script directory and navigate to project root
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
    PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
    cd "$PROJECT_ROOT"
    
    echo -e "${BLUE}Project root: $PROJECT_ROOT${NC}"
    echo -e "${BLUE}Configuration: $CONFIGURATION${NC}"
    echo -e "${BLUE}Verbosity: $VERBOSITY${NC}"
    
    # Check if .NET is installed
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}.NET SDK is not installed. Please run ./tools/install-dependencies.sh first.${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}Using .NET version: $(dotnet --version)${NC}"
    
    # If specific project is requested, test only that
    if [ -n "$SPECIFIC_PROJECT" ]; then
        echo -e "${YELLOW}Testing specific project: $SPECIFIC_PROJECT${NC}"
        
        # Find the project
        PROJECT_PATH=""
        if [[ "$SPECIFIC_PROJECT" == *.csproj ]] && [ -f "$SPECIFIC_PROJECT" ]; then
            PROJECT_PATH="$SPECIFIC_PROJECT"
        else
            # Search for test project by name
            while IFS= read -r proj; do
                local basename=$(basename "$proj" .csproj)
                if [[ "$basename" == "$SPECIFIC_PROJECT" ]] || [[ "$basename" == *"$SPECIFIC_PROJECT"* ]] || [[ "$proj" == *"$SPECIFIC_PROJECT"* ]]; then
                    PROJECT_PATH="$proj"
                    break
                fi
            done < <(find . -name "*.csproj" -type f \
                \( -name "*Test*.csproj" -o -name "*Tests.csproj" -o -name "*UnitTest*.csproj" -o -name "*IntegrationTest*.csproj" -o -name "*SQLTest*.csproj" \) \
                -not -path "*/bin/*" -not -path "*/obj/*")
        fi
        
        if [ -z "$PROJECT_PATH" ] || [ ! -f "$PROJECT_PATH" ]; then
            echo -e "${RED}Error: Test project not found: $SPECIFIC_PROJECT${NC}"
            echo -e "${YELLOW}Use './tools/project-cli.sh list-tests' to see available test projects${NC}"
            exit 1
        fi
        
        echo -e "${BLUE}Testing: $PROJECT_PATH${NC}"
        if dotnet test "$PROJECT_PATH" \
            -c "$CONFIGURATION" \
            --no-build \
            --verbosity "$VERBOSITY" \
            --logger "console;verbosity=$VERBOSITY" \
            --collect:"XPlat Code Coverage" \
            --results-directory:"TestResults"; then
            echo -e "${GREEN}Tests passed${NC}"
            exit 0
        else
            echo -e "${RED}Tests failed${NC}"
            exit 1
        fi
    fi
    
    # Find all test projects
    # Use process substitution to avoid subshell issues with arrays
    mapfile -t TEST_PROJECTS < <(find . -name "*.csproj" -type f \
        \( -name "*Test*.csproj" -o -name "*Tests.csproj" -o -name "*UnitTest*.csproj" -o -name "*IntegrationTest*.csproj" -o -name "*SQLTest*.csproj" \) \
        -not -path "*/bin/*" -not -path "*/obj/*")
    
    if [ ${#TEST_PROJECTS[@]} -eq 0 ]; then
        echo -e "${YELLOW}No test projects found.${NC}"
        exit 0
    fi
    
    echo -e "${BLUE}Found ${#TEST_PROJECTS[@]} test project(s)${NC}"
    
    # Run tests for each project
    FAILED_TESTS=()
    SUCCESSFUL_TESTS=()
    TOTAL_TESTS=0
    PASSED_TESTS=0
    FAILED_COUNT=0
    
    for proj in "${TEST_PROJECTS[@]}"; do
        echo -e "${YELLOW}Running tests: $proj${NC}"
        
        # Run tests with coverage collection
        if dotnet test "$proj" \
            -c "$CONFIGURATION" \
            --no-build \
            --verbosity "$VERBOSITY" \
            --logger "console;verbosity=$VERBOSITY" \
            --collect:"XPlat Code Coverage" \
            --results-directory:"TestResults"; then
            
            SUCCESSFUL_TESTS+=("$proj")
            echo -e "${GREEN}✓ Tests passed: $proj${NC}"
        else
            FAILED_TESTS+=("$proj")
            FAILED_COUNT=$((FAILED_COUNT + 1))
            echo -e "${RED}✗ Tests failed: $proj${NC}"
        fi
    done
    
    # Summary
    echo -e "${GREEN}=== Test Summary ===${NC}"
    echo -e "${GREEN}Successful test projects: ${#SUCCESSFUL_TESTS[@]}${NC}"
    echo -e "${RED}Failed test projects: ${#FAILED_TESTS[@]}${NC}"
    
    if [ ${#FAILED_TESTS[@]} -gt 0 ]; then
        echo -e "${RED}Failed test projects:${NC}"
        for proj in "${FAILED_TESTS[@]}"; do
            echo -e "${RED}  - $proj${NC}"
        done
        exit 1
    fi
    
    echo -e "${GREEN}=== All tests passed ===${NC}"
    
    # Display test results location
    if [ -d "TestResults" ]; then
        echo -e "${BLUE}Test results and coverage reports are available in: TestResults/${NC}"
    fi
}

# Run main function
main "$@"

