#!/usr/bin/env pwsh
# Script para parar todos os servidores

Write-Host "🛑 Parando servidores SuperERP..." -ForegroundColor Yellow
Write-Host ""

# Parar processos dotnet
$processes = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue

if ($processes) {
    Write-Host "Encontrados $($processes.Count) processos dotnet" -ForegroundColor Cyan
    
    foreach ($process in $processes) {
        try {
            Stop-Process -Id $process.Id -Force
            Write-Host "✅ Processo $($process.Id) parado" -ForegroundColor Green
        } catch {
            Write-Host "⚠️  Erro ao parar processo $($process.Id)" -ForegroundColor Yellow
        }
    }
    
    Write-Host ""
    Write-Host "✅ Todos os servidores foram parados" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Nenhum servidor em execução" -ForegroundColor Gray
}
