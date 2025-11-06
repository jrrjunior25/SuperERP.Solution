# build.ps1
param([string]$Configuration = "Release")

Write-Host "=== Building SuperERP Solution ===" -ForegroundColor Cyan

Write-Host "
Restoring packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "
Building solution..." -ForegroundColor Yellow
dotnet build --configuration $Configuration --no-restore

Write-Host "
Running tests..." -ForegroundColor Yellow
dotnet test --configuration $Configuration --no-build --verbosity normal

Write-Host "
=== Build completed successfully ===" -ForegroundColor Green
