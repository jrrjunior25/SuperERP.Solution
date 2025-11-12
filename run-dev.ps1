#!/usr/bin/env pwsh
# Script para rodar em modo desenvolvimento

Write-Host "🔧 Iniciando SuperERP (Desenvolvimento)..." -ForegroundColor Cyan
Write-Host ""

# Iniciar API
Write-Host "🌐 Iniciando API (http://localhost:5000)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\src\Presentation\SuperERP.API'; dotnet watch run"

Start-Sleep -Seconds 5

# Iniciar Web
Write-Host "🌐 Iniciando Web (http://localhost:5001)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\src\Presentation\SuperERP.Web'; dotnet watch run"

Start-Sleep -Seconds 3

Write-Host ""
Write-Host "✅ Modo desenvolvimento ativo (hot reload)" -ForegroundColor Green
Write-Host ""
Write-Host "📍 URLs:" -ForegroundColor Yellow
Write-Host "   API:     http://localhost:5000" -ForegroundColor White
Write-Host "   Swagger: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   Web:     http://localhost:5001" -ForegroundColor White
Write-Host ""

Start-Sleep -Seconds 2
Start-Process "http://localhost:5001"
