@echo off
REM CreatePackages.bat
REM Batch script to create NuGet packages for all AMCode libraries

setlocal enabledelayedexpansion

set OUTPUT_PATH=./packages
set CONFIGURATION=Release

echo Creating NuGet packages for AMCode libraries...
echo Output Path: %OUTPUT_PATH%
echo Configuration: %CONFIGURATION%

REM Create output directory if it doesn't exist
if not exist "%OUTPUT_PATH%" (
    mkdir "%OUTPUT_PATH%"
    echo Created output directory: %OUTPUT_PATH%
)

set SUCCESS_COUNT=0
set FAILURE_COUNT=0

REM Process AMCode.AI
echo.
echo Processing AMCode.AI...
call :ProcessLibrary "AMCode.AI" "amcode-library/ailibrary/AMCode.AI/AMCode.AI.csproj"

REM Process AMCode.Documents
echo.
echo Processing AMCode.Documents...
call :ProcessLibrary "AMCode.Documents" "amcode-library/documentlibrary/AMCode.Documents/AMCode.Documents.csproj"

REM Process AMCode.Exports
echo.
echo Processing AMCode.Exports...
call :ProcessLibrary "AMCode.Exports" "amcode-library/exportslibrary/AMCode.Exports/AMCode.Exports.csproj"

REM Process AMCode.Storage
echo.
echo Processing AMCode.Storage...
call :ProcessLibrary "AMCode.Storage" "amcode-library/storagelibrary/AMCode.Storage/AMCode.Storage.csproj"

REM Display summary
echo.
echo ==================================================
echo Package Creation Summary
echo ==================================================
echo Successful: %SUCCESS_COUNT%
echo Failed: %FAILURE_COUNT%
echo Total: 4

if %SUCCESS_COUNT%==4 (
    echo.
    echo All packages created successfully!
    echo Packages are available in: %OUTPUT_PATH%
    echo.
    echo Created packages:
    dir "%OUTPUT_PATH%\*.nupkg" /b
) else (
    echo.
    echo Some packages failed to create. Please check the errors above.
)

echo.
echo Package creation completed.
pause
goto :eof

:ProcessLibrary
set LIBRARY_NAME=%~1
set LIBRARY_PATH=%~2

echo Building %LIBRARY_NAME%...
dotnet build "%LIBRARY_PATH%" --configuration %CONFIGURATION% --no-restore
if %ERRORLEVEL% neq 0 (
    echo Build failed for %LIBRARY_NAME%
    set /a FAILURE_COUNT+=1
    goto :eof
)

echo Creating NuGet package for %LIBRARY_NAME%...
dotnet pack "%LIBRARY_PATH%" --configuration %CONFIGURATION% --output "%OUTPUT_PATH%" --no-build
if %ERRORLEVEL% neq 0 (
    echo Package creation failed for %LIBRARY_NAME%
    set /a FAILURE_COUNT+=1
    goto :eof
)

echo Successfully created package for %LIBRARY_NAME%
set /a SUCCESS_COUNT+=1
goto :eof
