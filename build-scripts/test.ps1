# Script para executar testes
Write-Host "Executando testes do SuperERP..." -ForegroundColor Green

# Navegar para o diretório raiz
Set-Location $PSScriptRoot\..

# Executar testes
dotnet test --verbosity normal

Write-Host "Testes concluídos!" -ForegroundColor Green
