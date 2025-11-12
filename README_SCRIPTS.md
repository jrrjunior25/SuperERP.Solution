# 🚀 Scripts de Execução - SuperERP

## 📋 Scripts Disponíveis

### 1. **run-all.ps1** - Rodar Tudo
Inicia API e Web em modo produção.

```powershell
.\run-all.ps1
```

**O que faz:**
- ✅ Compila a solução
- ✅ Inicia API (http://localhost:5000)
- ✅ Inicia Web (http://localhost:5001)
- ✅ Abre navegador automaticamente

---

### 2. **run-dev.ps1** - Modo Desenvolvimento
Inicia com hot reload (watch mode).

```powershell
.\run-dev.ps1
```

**O que faz:**
- ✅ Inicia API com `dotnet watch`
- ✅ Inicia Web com `dotnet watch`
- ✅ Hot reload automático ao salvar arquivos
- ✅ Ideal para desenvolvimento

---

### 3. **run-api.ps1** - Apenas API
Inicia somente a API.

```powershell
.\run-api.ps1
```

**URLs:**
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

---

### 4. **run-web.ps1** - Apenas Web
Inicia somente o frontend.

```powershell
.\run-web.ps1
```

**URL:**
- Web: http://localhost:5001

---

### 5. **stop-all.ps1** - Parar Tudo
Para todos os servidores em execução.

```powershell
.\stop-all.ps1
```

**O que faz:**
- ✅ Para todos os processos `dotnet`
- ✅ Limpa memória

---

### 6. **docker-start.ps1** - Infraestrutura
Inicia PostgreSQL, Redis e RabbitMQ.

```powershell
.\docker-start.ps1
```

**Serviços:**
- PostgreSQL: localhost:5433
- RabbitMQ: http://localhost:15672
- Redis: localhost:6379

**Credenciais RabbitMQ:**
- User: `supererp`
- Pass: `Super@ERP2025!`

---

## 🎯 Fluxo Recomendado

### Primeira Execução

```powershell
# 1. Iniciar infraestrutura
.\docker-start.ps1

# 2. Aguardar containers iniciarem (30 segundos)
Start-Sleep -Seconds 30

# 3. Aplicar migrations
cd src\Presentation\SuperERP.API
dotnet ef database update --project ..\..\Infrastructure\SuperERP.Infrastructure

# 4. Voltar para raiz
cd ..\..\..

# 5. Iniciar aplicação
.\run-all.ps1
```

### Desenvolvimento Diário

```powershell
# Modo desenvolvimento (hot reload)
.\run-dev.ps1
```

### Apenas Backend

```powershell
# Apenas API
.\run-api.ps1
```

### Apenas Frontend

```powershell
# Apenas Web
.\run-web.ps1
```

### Parar Tudo

```powershell
# Parar servidores
.\stop-all.ps1

# Parar Docker
cd deploy
docker-compose down
```

---

## 🔧 Requisitos

### Obrigatórios
- ✅ .NET 9 SDK
- ✅ PowerShell 7+

### Opcionais
- Docker Desktop (para infraestrutura)
- PostgreSQL local (alternativa ao Docker)

---

## 📝 Variáveis de Ambiente

### API (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=supererp;Username=supererp;Password=SuperERP@2025!",
    "Redis": "localhost:6379"
  },
  "Gerencianet": {
    "ClientId": "SEU_CLIENT_ID",
    "ClientSecret": "SEU_CLIENT_SECRET",
    "ChavePix": "sua-chave@pix.com"
  }
}
```

### Web (appsettings.json)
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/"
  }
}
```

---

## 🐛 Troubleshooting

### Porta já em uso
```powershell
# Verificar processos na porta 5000
netstat -ano | findstr :5000

# Matar processo
taskkill /PID <PID> /F
```

### Docker não inicia
```powershell
# Verificar status
docker ps

# Ver logs
docker-compose logs

# Reiniciar
docker-compose restart
```

### Erro de compilação
```powershell
# Limpar e recompilar
dotnet clean
dotnet build
```

### Banco de dados não conecta
```powershell
# Verificar se PostgreSQL está rodando
docker ps | findstr postgres

# Testar conexão
psql -h localhost -p 5433 -U supererp -d supererp
```

---

## 🚀 Atalhos Úteis

### Build Rápido
```powershell
dotnet build --no-restore
```

### Limpar Tudo
```powershell
dotnet clean
Remove-Item -Recurse -Force bin,obj
```

### Restaurar Pacotes
```powershell
dotnet restore
```

### Executar Testes
```powershell
dotnet test
```

### Criar Migration
```powershell
cd src\Presentation\SuperERP.API
dotnet ef migrations add NomeDaMigration --project ..\..\Infrastructure\SuperERP.Infrastructure
```

### Aplicar Migration
```powershell
dotnet ef database update --project ..\..\Infrastructure\SuperERP.Infrastructure
```

---

## 📊 Monitoramento

### Logs da API
```powershell
# Ver logs em tempo real
Get-Content src\Presentation\SuperERP.API\logs\supererp-*.log -Wait
```

### Status dos Containers
```powershell
docker-compose ps
```

### Uso de Memória
```powershell
Get-Process dotnet | Select-Object Name, CPU, WorkingSet
```

---

## 🎓 Dicas

1. **Use run-dev.ps1 para desenvolvimento** - Hot reload economiza tempo
2. **Mantenha Docker rodando** - Evita reiniciar infraestrutura
3. **Use stop-all.ps1 antes de fechar** - Libera portas
4. **Verifique logs em caso de erro** - Facilita debug
5. **Atualize migrations regularmente** - Mantém banco sincronizado

---

## 📞 Suporte

Em caso de problemas:
1. Verifique os logs
2. Confirme que todas as portas estão livres
3. Reinicie Docker se necessário
4. Execute `dotnet clean` e recompile

---

**Desenvolvido com ❤️ para facilitar seu desenvolvimento!**
