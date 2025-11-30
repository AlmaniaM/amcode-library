#!/bin/bash

# AMCode Library - Project CLI
# Unified CLI for building and testing individual projects
# Works on Ubuntu 20.04+ and macOS

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Configuration
CONFIGURATION="Release"
VERBOSITY="normal"
ACTION=""
PROJECT_NAME=""
PROJECT_PATH=""

# Print usage information
usage() {
    echo -e "${GREEN}AMCode Library - Project CLI${NC}"
    echo ""
    echo "Usage: $0 <action> [options] [project]"
    echo ""
    echo "Actions:"
    echo "  build <project>     Build a specific project"
    echo "  test <project>      Test a specific project"
    echo "  list                List all available projects"
    echo "  list-libs            List all library projects (non-test)"
    echo "  list-tests           List all test projects"
    echo ""
    echo "Options:"
    echo "  -c, --config <config>    Build configuration (Debug|Release, default: Release)"
    echo "  -v, --verbosity <level>   Test verbosity (quiet|minimal|normal|detailed|diagnostic)"
    echo "  -h, --help                Show this help message"
    echo ""
    echo "Project Specification:"
    echo "  Projects can be specified by:"
    echo "    - Name (e.g., 'AMCode.Common')"
    echo "    - Partial name (e.g., 'Common')"
    echo "    - Path (e.g., 'commonlibrary/AMCode.Common/AMCode.Common.csproj')"
    echo ""
    echo "Examples:"
    echo "  $0 build AMCode.Common"
    echo "  $0 build Common -c Debug"
    echo "  $0 test AMCode.Common.UnitTests"
    echo "  $0 test Common -v detailed"
    echo "  $0 list"
    echo ""
}

# Find project by name or path
find_project() {
    local search_term="$1"
    local project_type="$2"  # "lib", "test", or "all"
    
    # If it's already a path to a .csproj file, use it directly
    if [[ "$search_term" == *.csproj ]] && [ -f "$search_term" ]; then
        echo "$search_term"
        return 0
    fi
    
    # If it's a path to a directory, look for .csproj inside
    if [ -d "$search_term" ]; then
        local proj=$(find "$search_term" -maxdepth 1 -name "*.csproj" -type f | head -1)
        if [ -n "$proj" ]; then
            echo "$proj"
            return 0
        fi
    fi
    
    # Search by name
    local projects=()
    
    if [[ "$project_type" == "lib" ]]; then
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            -not -name "*Test*.csproj" -not -name "*Tests.csproj" \
            -not -name "*UnitTest*.csproj" -not -name "*IntegrationTest*.csproj" \
            -not -name "*SQLTest*.csproj" \
            -not -path "*/bin/*" -not -path "*/obj/*" \
            -not -path "*/tools/*" -not -path "*/packages/*")
    elif [[ "$project_type" == "test" ]]; then
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            \( -name "*Test*.csproj" -o -name "*Tests.csproj" -o -name "*UnitTest*.csproj" -o -name "*IntegrationTest*.csproj" -o -name "*SQLTest*.csproj" \) \
            -not -path "*/bin/*" -not -path "*/obj/*")
    else
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            -not -path "*/bin/*" -not -path "*/obj/*" \
            -not -path "*/tools/*" -not -path "*/packages/*")
    fi
    
    # Try exact match first
    for proj in "${projects[@]}"; do
        local basename=$(basename "$proj" .csproj)
        if [[ "$basename" == "$search_term" ]]; then
            echo "$proj"
            return 0
        fi
    done
    
    # Try partial match
    for proj in "${projects[@]}"; do
        local basename=$(basename "$proj" .csproj)
        local path="$proj"
        if [[ "$basename" == *"$search_term"* ]] || [[ "$path" == *"$search_term"* ]]; then
            echo "$proj"
            return 0
        fi
    done
    
    return 1
}

# List all projects
list_projects() {
    local project_type="$1"  # "lib", "test", or "all"
    
    echo -e "${CYAN}=== Available Projects ===${NC}"
    echo ""
    
    if [[ "$project_type" == "lib" ]]; then
        echo -e "${GREEN}Library Projects:${NC}"
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            -not -name "*Test*.csproj" -not -name "*Tests.csproj" \
            -not -name "*UnitTest*.csproj" -not -name "*IntegrationTest*.csproj" \
            -not -name "*SQLTest*.csproj" \
            -not -path "*/bin/*" -not -path "*/obj/*" \
            -not -path "*/tools/*" -not -path "*/packages/*" | sort)
    elif [[ "$project_type" == "test" ]]; then
        echo -e "${GREEN}Test Projects:${NC}"
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            \( -name "*Test*.csproj" -o -name "*Tests.csproj" -o -name "*UnitTest*.csproj" -o -name "*IntegrationTest*.csproj" -o -name "*SQLTest*.csproj" \) \
            -not -path "*/bin/*" -not -path "*/obj/*" | sort)
    else
        echo -e "${GREEN}All Projects:${NC}"
        mapfile -t projects < <(find . -name "*.csproj" -type f \
            -not -path "*/bin/*" -not -path "*/obj/*" \
            -not -path "*/tools/*" -not -path "*/packages/*" | sort)
    fi
    
    for proj in "${projects[@]}"; do
        local basename=$(basename "$proj" .csproj)
        local dir=$(dirname "$proj")
        echo -e "  ${CYAN}$basename${NC}"
        echo -e "    ${BLUE}Path: $proj${NC}"
        
        # Show target framework if available
        if [ -f "$proj" ]; then
            local tf=$(grep -oP '<TargetFramework>net\K[0-9.]+' "$proj" 2>/dev/null | head -1)
            if [ -n "$tf" ]; then
                echo -e "    ${YELLOW}Framework: .NET $tf${NC}"
            fi
        fi
        echo ""
    done
    
    echo -e "${GREEN}Total: ${#projects[@]} project(s)${NC}"
}

# Build a project
build_project() {
    local project_path="$1"
    
    if [ ! -f "$project_path" ]; then
        echo -e "${RED}Error: Project file not found: $project_path${NC}"
        return 1
    fi
    
    local basename=$(basename "$project_path" .csproj)
    echo -e "${GREEN}=== Building: $basename ===${NC}"
    echo -e "${BLUE}Configuration: $CONFIGURATION${NC}"
    echo -e "${BLUE}Project: $project_path${NC}"
    echo ""
    
    if dotnet build "$project_path" -c "$CONFIGURATION" --no-restore; then
        echo -e "${GREEN}✓ Build successful: $basename${NC}"
        return 0
    else
        echo -e "${RED}✗ Build failed: $basename${NC}"
        return 1
    fi
}

# Test a project
test_project() {
    local project_path="$1"
    
    if [ ! -f "$project_path" ]; then
        echo -e "${RED}Error: Project file not found: $project_path${NC}"
        return 1
    fi
    
    # Check if it's a test project
    local basename=$(basename "$project_path" .csproj)
    if [[ "$basename" != *"Test"* ]] && [[ "$basename" != *"Tests"* ]]; then
        echo -e "${YELLOW}Warning: '$basename' doesn't appear to be a test project${NC}"
        read -p "Continue anyway? (y/N) " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            return 1
        fi
    fi
    
    echo -e "${GREEN}=== Testing: $basename ===${NC}"
    echo -e "${BLUE}Configuration: $CONFIGURATION${NC}"
    echo -e "${BLUE}Verbosity: $VERBOSITY${NC}"
    echo -e "${BLUE}Project: $project_path${NC}"
    echo ""
    
    if dotnet test "$project_path" \
        -c "$CONFIGURATION" \
        --no-build \
        --verbosity "$VERBOSITY" \
        --logger "console;verbosity=$VERBOSITY" \
        --collect:"XPlat Code Coverage" \
        --results-directory:"TestResults"; then
        echo -e "${GREEN}✓ Tests passed: $basename${NC}"
        return 0
    else
        echo -e "${RED}✗ Tests failed: $basename${NC}"
        return 1
    fi
}

# Parse command line arguments
parse_args() {
    while [[ $# -gt 0 ]]; do
        case $1 in
            build|test|list|list-libs|list-tests)
                ACTION="$1"
                shift
                ;;
            -c|--config)
                CONFIGURATION="$2"
                shift 2
                ;;
            -v|--verbosity)
                VERBOSITY="$2"
                shift 2
                ;;
            -h|--help)
                usage
                exit 0
                ;;
            -*)
                echo -e "${RED}Unknown option: $1${NC}"
                usage
                exit 1
                ;;
            *)
                if [ -z "$PROJECT_NAME" ]; then
                    PROJECT_NAME="$1"
                else
                    echo -e "${RED}Multiple project names specified${NC}"
                    usage
                    exit 1
                fi
                shift
                ;;
        esac
    done
}

# Main execution
main() {
    # Get script directory and navigate to project root
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
    PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
    cd "$PROJECT_ROOT"
    
    # Parse arguments
    parse_args "$@"
    
    # Check if .NET is installed
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}.NET SDK is not installed. Please run ./tools/install-dependencies.sh first.${NC}"
        exit 1
    fi
    
    # Handle actions
    case "$ACTION" in
        list)
            list_projects "all"
            ;;
        list-libs)
            list_projects "lib"
            ;;
        list-tests)
            list_projects "test"
            ;;
        build)
            if [ -z "$PROJECT_NAME" ]; then
                echo -e "${RED}Error: No project specified for build action${NC}"
                usage
                exit 1
            fi
            
            PROJECT_PATH=$(find_project "$PROJECT_NAME" "lib")
            if [ -z "$PROJECT_PATH" ]; then
                echo -e "${RED}Error: Project not found: $PROJECT_NAME${NC}"
                echo -e "${YELLOW}Use '$0 list-libs' to see available library projects${NC}"
                exit 1
            fi
            
            build_project "$PROJECT_PATH"
            ;;
        test)
            if [ -z "$PROJECT_NAME" ]; then
                echo -e "${RED}Error: No project specified for test action${NC}"
                usage
                exit 1
            fi
            
            PROJECT_PATH=$(find_project "$PROJECT_NAME" "test")
            if [ -z "$PROJECT_PATH" ]; then
                echo -e "${RED}Error: Test project not found: $PROJECT_NAME${NC}"
                echo -e "${YELLOW}Use '$0 list-tests' to see available test projects${NC}"
                exit 1
            fi
            
            test_project "$PROJECT_PATH"
            ;;
        "")
            echo -e "${RED}Error: No action specified${NC}"
            usage
            exit 1
            ;;
        *)
            echo -e "${RED}Error: Unknown action: $ACTION${NC}"
            usage
            exit 1
            ;;
    esac
}

# Run main function
main "$@"

