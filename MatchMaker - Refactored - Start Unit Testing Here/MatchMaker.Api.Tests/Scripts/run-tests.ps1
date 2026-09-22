# Ensure any error stops the script
$ErrorActionPreference = "Stop"

$testProjectDir=$args[0]
$coverageFileName="coverage.info"

# Building the project first prevents issues collecting coverage
dotnet build $testProjectDir\MatchMaker.Api.Tests.csproj

# Run tests and collect coverage
Write-Host "`nStarting test execution..."
$testOutput = dotnet test $testProjectDir\MatchMaker.Api.Tests.csproj --collect "XPlat Code Coverage;Format=lcov" --no-build
$coverageFilePath = $testOutput | Select-String $coverageFileName | ForEach-Object { $_.Line.Trim() } | Join-String -Separator ';'

# Generate the coverage report
$coverageDir = Join-Path $testProjectDir "coverage"
if (-not (Test-Path $coverageDir)) {
    New-Item -ItemType Directory -Path $coverageDir | Out-Null
}
Copy-Item $coverageFilePath $coverageDir\lcov.info

Write-Host "`nGenerating code coverage report..."
reportgenerator -reports:$coverageDir\lcov.info -targetdir:$coverageDir\report
