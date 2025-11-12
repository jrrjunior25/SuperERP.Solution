#!/usr/bin/env pwsh
# Script para iniciar infraestrutura com Docker

Write-Host "🐳 Iniciando infraestrutura Docker..." -ForegroundColor Cyan
Write-Host ""

# Verificar se Docker está instalado
$dockerVersion = docker --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Docker não encontrado. Instale o Docker Desktop." -ForegroundColor Red
    exit 1
}
Write-Host "✅ Docker encontrado: $dockerVersion" -ForegroundColor Green

# Iniciar containers
Write-Host ""
Write-Host "🚀 Iniciando containers..." -ForegroundColor Yellow
Set-Location "$PSScriptRoot\deploy"

docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Containers iniciados com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📍 Serviços disponíveis:" -ForegroundColor Yellow
    Write-Host "   PostgreSQL:  localhost:5433" -ForegroundColor White
    Write-Host "   RabbitMQ:    http://localhost:15672" -ForegroundColor White
    Write-Host "   Redis:       localhost:6379" -ForegroundColor White
    Write-Host ""
    Write-Host "🔑 Credenciais RabbitMQ:" -ForegroundColor Yellow
    Write-Host "   User: supererp" -ForegroundColor White
    Write-Host "   Pass: Super@ERP2025!" -ForegroundColor White
    Write-Host ""
    
    # Mostrar status dos containers
    Write-Host "📊 Status dos containers:" -ForegroundColor Cyan
    docker-compose ps
} else {
    Write-Host "❌ Erro ao iniciar containers" -ForegroundColor Red
    exit 1
}
