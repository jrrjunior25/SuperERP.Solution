# 🔌 Guia de Integração - Super ERP

## 📋 Índice
1. [PIX - Gerencianet/Efí](#pix-gerencianet)
2. [NF-e/NFC-e - ACBr](#nfe-acbr)
3. [Certificado Digital](#certificado-digital)
4. [Configuração](#configuracao)

---

## 🟢 PIX - Gerencianet/Efí

### Pré-requisitos
1. Conta na Gerencianet (https://gerencianet.com.br)
2. Credenciais de API (Client ID e Client Secret)
3. Chave PIX cadastrada

### Configuração

**appsettings.json**
```json
{
  "Gerencianet": {
    "ClientId": "Client_Id_xxxxx",
    "ClientSecret": "Client_Secret_xxxxx",
    "ChavePix": "sua-chave@pix.com",
    "Homologacao": true
  }
}
```

### Exemplo de Uso

**Gerar PIX**
```http
POST /api/pix/gerar
Content-Type: application/json

{
  "empresaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "chavePix": "sua-chave@pix.com",
  "valor": 100.50,
  "expiracaoMinutos": 30,
  "clienteId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "vendaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "infoAdicional": "Pagamento Venda #123"
}
```

**Resposta**
```json
{
  "sucesso": true,
  "txId": "7d9f3f8e2a1b4c5d6e7f8g9h0i1j2k3l",
  "qrCode": "00020126580014br.gov.bcb.pix...",
  "qrCodeBase64": "iVBORw0KGgoAAAANSUhEUgAA...",
  "pixCopiaECola": "00020126580014br.gov.bcb.pix...",
  "dataExpiracao": "2025-01-15T15:30:00Z"
}
```

### Webhook (Confirmação Automática)

Configure o webhook na Gerencianet para receber notificações de pagamento:

```
URL: https://seu-dominio.com/api/webhooks/pix
```

---

## 📄 NF-e/NFC-e - ACBr

### Pré-requisitos
1. Certificado Digital A1 ou A3
2. Cadastro na SEFAZ do seu estado
3. Ambiente de homologação configurado

### Certificado Digital

**Formato A1 (.pfx)**
- Armazene em local seguro
- Use senha forte
- Renove antes do vencimento

**Instalação**
```bash
# Windows
certutil -importpfx certificado.pfx

# Linux
openssl pkcs12 -in certificado.pfx -out certificado.pem
```

### Configuração

**appsettings.json**
```json
{
  "NFe": {
    "Homologacao": true,
    "CertificadoPath": "C:\\certificados\\certificado.pfx",
    "SenhaCertificado": "senha123"
  }
}
```

### Exemplo de Uso

**Emitir NF-e**
```http
POST /api/nfe/emitir
Content-Type: application/json

{
  "emitenteCnpj": "12345678000190",
  "emitenteRazaoSocial": "EMPRESA LTDA",
  "emitenteNomeFantasia": "EMPRESA",
  "emitenteLogradouro": "RUA EXEMPLO",
  "emitenteNumero": "123",
  "emitenteBairro": "CENTRO",
  "emitenteCidade": "SAO PAULO",
  "emitenteUF": "SP",
  "emitenteCEP": "01234567",
  "destinatarioCpfCnpj": "12345678901",
  "destinatarioNome": "CLIENTE EXEMPLO",
  "numero": "1",
  "serie": "1",
  "modelo": "55",
  "itens": [
    {
      "codigo": "001",
      "descricao": "PRODUTO TESTE",
      "ncm": "12345678",
      "cfop": "5102",
      "unidadeComercial": "UN",
      "quantidade": 1,
      "valorUnitario": 100.00,
      "valorTotal": 100.00
    }
  ],
  "certificadoDigital": "base64_do_certificado",
  "senhaCertificado": "senha123",
  "homologacao": true
}
```

**Resposta**
```json
{
  "sucesso": true,
  "chaveAcesso": "35250112345678000190550010000000011234567890",
  "protocolo": "135250000000001",
  "numeroNota": "1",
  "serie": "1",
  "dataAutorizacao": "2025-01-15T14:30:00Z",
  "xmlNota": "<?xml version=\"1.0\"...",
  "mensagem": "NF-e autorizada com sucesso"
}
```

### Códigos CFOP Comuns

| CFOP | Descrição |
|------|-----------|
| 5102 | Venda de mercadoria adquirida de terceiros |
| 5405 | Venda de mercadoria adquirida de terceiros em operação com mercadoria sujeita ao regime de substituição tributária |
| 6102 | Venda de mercadoria adquirida de terceiros (interestadual) |

### NCM (Nomenclatura Comum do Mercosul)

Consulte em: https://www.gov.br/receitafederal/pt-br/assuntos/aduana-e-comercio-exterior/manuais/ncm

---

## 🔐 Segurança

### Certificado Digital

**Armazenamento Seguro**
```bash
# Linux - Permissões restritas
chmod 600 /app/certificados/certificado.pfx
chown app:app /app/certificados/certificado.pfx
```

**Variáveis de Ambiente (Recomendado)**
```bash
export NFE_CERT_PATH="/app/certificados/certificado.pfx"
export NFE_CERT_PASSWORD="senha_segura"
export GERENCIANET_CLIENT_ID="Client_Id_xxxxx"
export GERENCIANET_CLIENT_SECRET="Client_Secret_xxxxx"
```

### Criptografia de Dados Sensíveis

```csharp
// Criptografar antes de salvar no banco
var dadosCriptografados = _encryptionService.Encrypt(dadosSensiveis);
```

---

## 🧪 Testes

### Ambiente de Homologação

**PIX - Gerencianet**
- Use credenciais de homologação
- Não há cobrança real
- Simule pagamentos via dashboard

**NF-e - SEFAZ**
- Configure `Homologacao: true`
- Use CNPJ de teste: 99999999000191
- Notas não têm validade fiscal

### Dados de Teste

**CPF Válido (Teste)**: 123.456.789-09
**CNPJ Válido (Teste)**: 11.222.333/0001-81

---

## 📊 Monitoramento

### Logs

```bash
# Ver logs em tempo real
tail -f logs/supererp-20250115.log

# Buscar erros
grep "ERROR" logs/supererp-*.log
```

### Health Checks

```http
GET /health
```

**Resposta**
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy",
    "redis": "Healthy",
    "gerencianet": "Healthy",
    "sefaz": "Healthy"
  }
}
```

---

## 🚀 Deploy

### Docker

```bash
# Build
docker build -t supererp-api -f deploy/Dockerfile.api .

# Run
docker run -d \
  -p 5000:8080 \
  -e NFE_CERT_PATH=/app/certs/cert.pfx \
  -e NFE_CERT_PASSWORD=senha \
  -v /path/to/certs:/app/certs \
  supererp-api
```

### Kubernetes

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: supererp-secrets
type: Opaque
data:
  nfe-cert-password: base64_encoded_password
  gerencianet-client-secret: base64_encoded_secret
```

---

## 📞 Suporte

### Gerencianet
- Documentação: https://dev.gerencianet.com.br
- Suporte: suporte@gerencianet.com.br

### SEFAZ
- Portal NF-e: https://www.nfe.fazenda.gov.br
- Consulte a SEFAZ do seu estado

### ACBr
- Documentação: http://acbr.sourceforge.net
- Fórum: https://www.projetoacbr.com.br/forum

---

## ⚠️ Troubleshooting

### PIX não gera QR Code
- Verifique credenciais
- Confirme chave PIX cadastrada
- Verifique conectividade com API

### NF-e rejeitada
- Valide certificado digital
- Verifique dados obrigatórios
- Consulte código de rejeição SEFAZ

### Certificado expirado
```bash
# Verificar validade
openssl pkcs12 -in certificado.pfx -nokeys | openssl x509 -noout -dates
```

---

## 📚 Referências

- [Manual NF-e 4.0](https://www.nfe.fazenda.gov.br/portal/principal.aspx)
- [API PIX Gerencianet](https://dev.gerencianet.com.br/docs/api-pix)
- [Tabela CFOP](https://www.gov.br/receitafederal/pt-br/assuntos/orientacao-tributaria/tributos/CFOP.pdf)
- [Consulta NCM](https://www.gov.br/receitafederal/pt-br/assuntos/aduana-e-comercio-exterior/manuais/ncm)
