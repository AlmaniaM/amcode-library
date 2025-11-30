#!/bin/bash

# Script to install/update .NET SDK and restore NuGet packages for AMCode Library
# Works on Ubuntu 20.04+ and macOS

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Detect OS
detect_os() {
    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        if [ -f /etc/os-release ]; then
            . /etc/os-release
            if [[ "$ID" == "ubuntu" ]]; then
                echo "ubuntu"
                return
            fi
        fi
        echo "linux"
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        echo "macos"
    else
        echo "unknown"
    fi
}

# Check if .NET SDK is installed
check_dotnet_installed() {
    if command -v dotnet &> /dev/null; then
        local installed_version=$(dotnet --version 2>/dev/null)
        echo "$installed_version"
    else
        echo ""
    fi
}

# Get required .NET SDK versions from projects
get_required_versions() {
    local versions=()
    
    # Check for global.json files
    while IFS= read -r file; do
        if [ -f "$file" ]; then
            local version=$(grep -oP '"version":\s*"\K[^"]+' "$file" 2>/dev/null | head -1)
            if [ -n "$version" ]; then
                versions+=("$version")
            fi
        fi
    done < <(find . -name "global.json" -type f 2>/dev/null)
    
    # Check project files for target frameworks
    while IFS= read -r file; do
        if [ -f "$file" ]; then
            local tf=$(grep -oP '<TargetFramework>net\K[0-9.]+' "$file" 2>/dev/null | head -1)
            if [ -n "$tf" ]; then
                # Convert net8.0 -> 8.0, net9.0 -> 9.0
                local major=$(echo "$tf" | cut -d. -f1)
                versions+=("$major.0")
            fi
        fi
    done < <(find . -name "*.csproj" -type f 2>/dev/null)
    
    # Return unique versions, sorted
    printf '%s\n' "${versions[@]}" | sort -u
}

# Install .NET SDK on Ubuntu
install_dotnet_ubuntu() {
    local version=$1
    local major=$(echo "$version" | cut -d. -f1)
    
    echo -e "${YELLOW}Installing .NET $version SDK on Ubuntu...${NC}"
    
    # Detect Ubuntu version
    if [ -f /etc/os-release ]; then
        . /etc/os-release
        UBUNTU_VERSION=$(echo "$VERSION_ID" | cut -d. -f1)
    else
        UBUNTU_VERSION="20"
    fi
    
    # Add Microsoft package repository
    if [ ! -f /etc/apt/sources.list.d/microsoft-prod.list ]; then
        wget https://packages.microsoft.com/config/ubuntu/${UBUNTU_VERSION}.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb
        sudo dpkg -i /tmp/packages-microsoft-prod.deb
        rm /tmp/packages-microsoft-prod.deb
    fi
    
    # Update package list
    sudo apt-get update
    
    # Install .NET SDK
    if [[ "$major" == "8" ]]; then
        sudo apt-get install -y dotnet-sdk-8.0
    elif [[ "$major" == "9" ]]; then
        # .NET 9 might not be available via apt on Ubuntu 20.04, use install script
        if ! sudo apt-get install -y dotnet-sdk-9.0 2>/dev/null; then
            echo -e "${YELLOW}.NET 9.0 not available via apt. Using official install script...${NC}"
            curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 9.0
            export DOTNET_ROOT="$HOME/.dotnet"
            export PATH="$DOTNET_ROOT:$PATH"
        fi
    else
        echo -e "${RED}Unsupported .NET version: $version${NC}"
        return 1
    fi
    
    echo -e "${GREEN}.NET $version SDK installed successfully${NC}"
}

# Install .NET SDK on macOS
install_dotnet_macos() {
    local version=$1
    local major=$(echo "$version" | cut -d. -f1)
    
    echo -e "${YELLOW}Installing .NET $version SDK on macOS...${NC}"
    
    # Check if Homebrew is installed
    if command -v brew &> /dev/null; then
        echo -e "${YELLOW}Installing via Homebrew...${NC}"
        if [[ "$major" == "8" ]]; then
            brew install --cask dotnet-sdk8
        elif [[ "$major" == "9" ]]; then
            brew install --cask dotnet-sdk9
        else
            brew install --cask dotnet-sdk
        fi
    else
        echo -e "${YELLOW}Homebrew not found. Installing via official installer...${NC}"
        # Download and install .NET SDK
        curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel "$major.0" --version "$version"
        
        # Add to PATH if not already there
        if ! grep -q 'export DOTNET_ROOT=' ~/.zshrc 2>/dev/null && ! grep -q 'export DOTNET_ROOT=' ~/.bash_profile 2>/dev/null; then
            echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> ~/.zshrc 2>/dev/null || echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> ~/.bash_profile
            echo 'export PATH="$DOTNET_ROOT:$PATH"' >> ~/.zshrc 2>/dev/null || echo 'export PATH="$DOTNET_ROOT:$PATH"' >> ~/.bash_profile
            echo -e "${YELLOW}Added .NET to PATH. Please restart your terminal or run: source ~/.zshrc${NC}"
        fi
        
        export DOTNET_ROOT="$HOME/.dotnet"
        export PATH="$DOTNET_ROOT:$PATH"
    fi
    
    echo -e "${GREEN}.NET $version SDK installed successfully${NC}"
}

# Main execution
main() {
    echo -e "${GREEN}=== AMCode Library - Dependency Installation Script ===${NC}"
    
    # Get script directory and navigate to project root
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
    PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
    cd "$PROJECT_ROOT"
    
    echo -e "${BLUE}Project root: $PROJECT_ROOT${NC}"
    
    # Detect OS
    OS=$(detect_os)
    echo -e "${YELLOW}Detected OS: $OS${NC}"
    
    if [[ "$OS" != "ubuntu" && "$OS" != "macos" ]]; then
        echo -e "${RED}Unsupported OS: $OS. This script supports Ubuntu 20.04+ and macOS.${NC}"
        exit 1
    fi
    
    # Get required .NET versions
    echo -e "${YELLOW}Analyzing project files for required .NET SDK versions...${NC}"
    REQUIRED_VERSIONS=$(get_required_versions | sort -u)
    
    if [ -z "$REQUIRED_VERSIONS" ]; then
        echo -e "${YELLOW}No specific version requirements found. Defaulting to .NET 8.0 and 9.0...${NC}"
        REQUIRED_VERSIONS="8.0 9.0"
    fi
    
    echo -e "${BLUE}Required .NET SDK versions: $REQUIRED_VERSIONS${NC}"
    
    # Check installed .NET versions
    INSTALLED_VERSION=$(check_dotnet_installed)
    
    if [ -z "$INSTALLED_VERSION" ]; then
        echo -e "${YELLOW}.NET SDK not found. Installing required versions...${NC}"
        
        for version in $REQUIRED_VERSIONS; do
            if [[ "$OS" == "ubuntu" ]]; then
                install_dotnet_ubuntu "$version"
            elif [[ "$OS" == "macos" ]]; then
                install_dotnet_macos "$version"
            fi
        done
        
        # Verify installation
        INSTALLED_VERSION=$(check_dotnet_installed)
        if [ -z "$INSTALLED_VERSION" ]; then
            echo -e "${RED}Failed to install .NET SDK. Please install manually.${NC}"
            exit 1
        fi
    else
        echo -e "${GREEN}.NET SDK is installed: $INSTALLED_VERSION${NC}"
        
        # Check if we need additional versions
        for version in $REQUIRED_VERSIONS; do
            local major=$(echo "$version" | cut -d. -f1)
            local installed_major=$(echo "$INSTALLED_VERSION" | cut -d. -f1)
            
            if [[ "$major" != "$installed_major" ]]; then
                echo -e "${YELLOW}Installing additional .NET SDK version: $version${NC}"
                if [[ "$OS" == "ubuntu" ]]; then
                    install_dotnet_ubuntu "$version"
                elif [[ "$OS" == "macos" ]]; then
                    install_dotnet_macos "$version"
                fi
            fi
        done
    fi
    
    # Verify .NET is accessible
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}.NET is not in PATH. Please restart your terminal or add it manually.${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}Using .NET version: $(dotnet --version)${NC}"
    echo -e "${GREEN}Installed SDKs:${NC}"
    dotnet --list-sdks
    
    # Restore NuGet packages for the solution
    echo -e "${YELLOW}Restoring NuGet packages...${NC}"
    
    if [ -f "AMCode.sln" ]; then
        echo -e "${BLUE}Restoring packages for AMCode.sln...${NC}"
        dotnet restore AMCode.sln
    else
        echo -e "${YELLOW}AMCode.sln not found. Restoring packages for individual projects...${NC}"
        
        # Find and restore all .csproj files
        find . -name "*.csproj" -type f -not -path "*/bin/*" -not -path "*/obj/*" | while read -r proj; do
            echo -e "${BLUE}Restoring: $proj${NC}"
            dotnet restore "$proj" || echo -e "${YELLOW}Warning: Failed to restore $proj${NC}"
        done
    fi
    
    echo -e "${GREEN}=== Dependency installation complete ===${NC}"
}

# Run main function
main "$@"

