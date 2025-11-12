#!/usr/bin/env pwsh
# Script para rodar apenas o Web

Write-Host "🌐 Iniciando SuperERP Web..." -ForegroundColor Cyan
Write-Host ""

Set-Location "$PSScriptRoot\src\Presentation\SuperERP.Web"

Write-Host "📦 Compilando..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Compilação concluída" -ForegroundColor Green
    Write-Host ""
    Write-Host "🚀 Iniciando Web..." -ForegroundColor Cyan
    Write-Host ""
    Write-Host "📍 URL: http://localhost:5001" -ForegroundColor Yellow
    Write-Host ""
    
    Start-Sleep -Seconds 2
    Start-Process "http://localhost:5001"
    
    dotnet run --no-build --configuration Release
} else {
    Write-Host "❌ Erro na compilação" -ForegroundColor Red
    exit 1
}
