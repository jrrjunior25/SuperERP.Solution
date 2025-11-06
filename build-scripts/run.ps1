# run.ps1 - Executa a API localmente

Write-Host "=== Iniciando SuperERP API ===" -ForegroundColor Cyan

Set-Location "src\Presentation\SuperERP.API"
dotnet run
