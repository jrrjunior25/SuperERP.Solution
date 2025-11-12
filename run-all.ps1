#!/usr/bin/env pwsh
# Script para rodar todos os servidores do SuperERP

Write-Host "🚀 Iniciando SuperERP..." -ForegroundColor Cyan
Write-Host ""

# Verificar se o .NET 9 está instalado
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ .NET SDK não encontrado. Instale o .NET 9 SDK." -ForegroundColor Red
    exit 1
}
Write-Host "✅ .NET SDK $dotnetVersion encontrado" -ForegroundColor Green

# Compilar solução
Write-Host ""
Write-Host "📦 Compilando solução..." -ForegroundColor Yellow
dotnet build --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro ao compilar a solução" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Compilação concluída" -ForegroundColor Green

# Iniciar API
Write-Host ""
Write-Host "🌐 Iniciando API (http://localhost:5000)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\src\Presentation\SuperERP.API'; dotnet run --no-build --configuration Release"

# Aguardar API iniciar
Start-Sleep -Seconds 5

# Iniciar Web
Write-Host "🌐 Iniciando Web (http://localhost:5001)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\src\Presentation\SuperERP.Web'; dotnet run --no-build --configuration Release"

# Aguardar Web iniciar
Start-Sleep -Seconds 3

Write-Host ""
Write-Host "✅ Servidores iniciados com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "📍 URLs disponíveis:" -ForegroundColor Yellow
Write-Host "   API:     http://localhost:5000" -ForegroundColor White
Write-Host "   Swagger: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   Web:     http://localhost:5001" -ForegroundColor White
Write-Host ""
Write-Host "💡 Pressione Ctrl+C nas janelas para parar os servidores" -ForegroundColor Gray
Write-Host ""

# Abrir navegador
Start-Sleep -Seconds 2
Start-Process "http://localhost:5001"
