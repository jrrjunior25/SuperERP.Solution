#!/usr/bin/env pwsh
# Script para rodar apenas a API

Write-Host "🌐 Iniciando SuperERP API..." -ForegroundColor Cyan
Write-Host ""

Set-Location "$PSScriptRoot\src\Presentation\SuperERP.API"

Write-Host "📦 Compilando..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Compilação concluída" -ForegroundColor Green
    Write-Host ""
    Write-Host "🚀 Iniciando API..." -ForegroundColor Cyan
    Write-Host ""
    Write-Host "📍 URLs:" -ForegroundColor Yellow
    Write-Host "   API:     http://localhost:5000" -ForegroundColor White
    Write-Host "   Swagger: http://localhost:5000/swagger" -ForegroundColor White
    Write-Host ""
    
    dotnet run --no-build --configuration Release
} else {
    Write-Host "❌ Erro na compilação" -ForegroundColor Red
    exit 1
}
