param (
	[string]$expectedSolutionName,
	[string]$projectName,
	[string]$solutionDirectory,
	[string]$solutionName
)

if ( $Env.SKIP_DOTNET_FORMAT -eq $null ) {
	if ( $expectedSolutionName -eq $solutionName ) {
		Set-Location $solutionDirectory
		dotnet tool restore
		dotnet format $solutionDirectory -f
		dotnet format $solutionName --fix-style warn
		dotnet format $solutionDirectory$projectName.UnitTests/$projectName.UnitTests.csproj --fix-style warn
	}
}