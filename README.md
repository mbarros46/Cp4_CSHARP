# Cp4_CSHARP

## 👥 Integrantes

- RM556652 - Miguel Barros
- RM558042 - Thomas Rodrigues
- RM556826 - Pedro Valentim 


## 📝 Descrição do Domínio

O sistema gerencia motos e pátios para uma empresa de mobilidade urbana. Permite cadastrar motos (modelo, placa, status, ano, pátio) e pátios (nome, endereço, capacidade, ocupação), controlando a alocação das motos nos pátios e garantindo regras de negócio como capacidade máxima e validação de dados.

### Entidades principais
- **Moto**: Id, Modelo, Placa, Status, Ano, PatioId (opcional)
- **Pátio**: Id, Nome, Endereco, Capacidade, OcupacaoAtual

## 🚀 Instruções de Execução

1. **Restaurar dependências:**
	 ```sh
	 dotnet restore
	 ```
2. **Gerar/atualizar o banco de dados:**
	 ```sh
	 dotnet ef database update
	 ```
3. **Executar a aplicação:**
	 ```sh
	 dotnet run
	 ```
4. Acesse a documentação Swagger em: `https://localhost:7208/swagger` (ou porta configurada)

## 📦 Exemplos de Requisições

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