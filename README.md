# Mottu Fleet API (CP5 - 2TDS)

## 👥 Integrantes

- RM556652 - Miguel Barros
- RM558042 - Thomas Rodrigues
- RM556826 - Pedro Valentim
- RM554727 - Victor Rocha

## 📝 Descrição do Projeto e Objetivo do CP5

API de exemplo para gestão de motocicletas e pátios. O objetivo do CP5 é implementar um backend completo que permita cadastrar, listar, atualizar e remover recursos do domínio, além de demonstrar integrações com bases relacionais e não-relacionais, validação, mapeamento e documentação.

## 🚀 Como executar (mínimos obrigatórios)

Pré-requisitos:
- .NET SDK 8.0+
- (Opcional, recomendado) Docker para executar MongoDB localmente

Exemplo rápido com Docker para MongoDB:

```powershell
docker run -d --name mongodb -p 27017:27017 mongo:6.0
```

String de conexão (usar em `appsettings.json` → `Mongo:ConnectionString`):

```
mongodb://localhost:27017
```

Executando a aplicação:

```powershell
dotnet restore
dotnet build
dotnet run
```

Ao rodar, verifique a URL de escuta informada no console (ex.: http://localhost:5000).

## Swagger e versão

- Swagger UI disponível em: `/docs` (documenta a versão `v1`).
  - Ex.: `http://localhost:5000/docs`

## Health checks

- `/health` — status geral da aplicação
- `/health/live` — liveness probe (se a aplicação está viva)
- `/health/ready` — readiness probe (se a aplicação está pronta para receber tráfego)

Esses endpoints são úteis para orquestradores e monitoramento.

## Estrutura de pastas (resumo das camadas)

- `Controllers/` — endpoints HTTP
- `Application/` — DTOs, services, validações e mapeamentos
- `Domain/` — entidades, enums e interfaces de repositório
- `Infrastructure/` — implementações de persistência e integrações
  - `Infrastructure/Mongo` — MongoSettings, MongoDbContext, MongoMotoRepository, extensões de serviço
  - `Infrastructure/EF` — contexto EF para Oracle
- `Program.cs` — configuração de DI, HealthChecks, API Versioning e Swagger

## Commits semânticos

Foram utilizados commits semânticos (ex.: `feat:`, `fix:`, `chore:`, `refactor:`) para manter o histórico organizado.

## Observações finais

- A implementação atual utiliza MongoDB para armazenar motos (via `MongoMotoRepository`) e EF/Oracle para outras entidades como pátios.
- Posso adicionar um `docker-compose.yml` para facilitar testes locais (subindo a API e o Mongo juntos). Caso queira, posso criar.

## Conteúdo original (exemplos de requisições)

### Motos

**GET todas as motos**
```
GET /api/motos
```

**GET moto por ID**
```
GET /api/motos/{id}
```

**POST criar moto**
```
POST /api/motos
Content-Type: application/json
{
    "modelo": "Honda CG 160",
    "placa": "ABC1234",
    "status": "Active",
    "ano": 2024,
    "patioId": "GUID_DO_PATIO"
}
```

**PUT atualizar moto**
```
PUT /api/motos/{id}
Content-Type: application/json
{
    "modelo": "Honda CG 160",
    "placa": "DEF5678",
    "status": "Inactive",
    "ano": 2023,
    "patioId": "GUID_DO_PATIO"
}
```

**DELETE moto**
```
DELETE /api/motos/{id}
```

### Pátios

**GET todos os pátios**
```
GET /api/patios
```

**GET pátio por ID**
```
GET /api/patios/{id}
```

**POST criar pátio**
```
POST /api/patios
Content-Type: application/json
{
    "nome": "Pátio Central",
    "endereco": "Rua Principal, 123",
    "capacidade": 100
}
```

**PUT atualizar pátio**
```
PUT /api/patios/{id}
Content-Type: application/json
{
    "nome": "Pátio Central",
    "endereco": "Rua Nova, 456",
    "capacidade": 120
}
```

**DELETE pátio**
```
DELETE /api/patios/{id}
```