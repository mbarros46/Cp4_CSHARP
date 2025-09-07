# Documentação do Projeto - Sistema de Gerenciamento de Motos e Pátios da Mottu

## 📋 Contexto e Problema

A Mottu, empresa líder em mobilidade urbana com foco em motocicletas, enfrenta desafios na gestão de sua frota e pátios de estacionamento. Com o crescimento da empresa, tornou-se necessário um sistema robusto para:

1. Gerenciar o inventário de motos
2. Controlar a ocupação dos pátios
3. Rastrear a localização das motos
4. Otimizar o uso do espaço nos pátios

## 🎯 Proposta de Solução

Desenvolvemos uma API RESTful que implementa um sistema de gerenciamento de motos e pátios, seguindo os princípios de Clean Architecture e Domain-Driven Design (DDD). O sistema permite:

### Funcionalidades Principais

1. **Gestão de Motos**
   - Cadastro completo de motos (placa, modelo, ano)
   - Validação de dados (placa válida, ano dentro do range permitido)
   - Rastreamento da localização atual da moto
   - Histórico de movimentações

2. **Gestão de Pátios**
   - Cadastro de pátios com capacidade máxima
   - Controle de ocupação em tempo real
   - Validação de disponibilidade de vagas
   - Relatórios de ocupação

### Modelo de Domínio

#### Entidade: Moto
- **Atributos:**
  - Id (int)
  - Placa (string, 7 caracteres)
  - Ano (int)
  - Modelo (string)
  - PatioId (int, nullable)
- **Regras de Negócio:**
  - Placa deve ter exatamente 7 caracteres
  - Ano deve estar entre 1900 e ano atual
  - Modelo não pode ser vazio
  - Pode ou não estar associada a um pátio

#### Entidade: Patio
- **Atributos:**
  - Id (int)
  - Nome (string)
  - Endereco (string)
  - Capacidade (int)
- **Regras de Negócio:**
  - Nome e endereço são obrigatórios
  - Capacidade deve ser maior que zero
  - Controle de ocupação em tempo real
  - Não pode exceder capacidade máxima

### Relacionamentos

- Uma Moto pode estar em um Pátio (relacionamento 1:N)
- Um Pátio pode conter várias Motos (relacionamento 1:N)
- Quando uma Moto é removida de um Pátio, o relacionamento é atualizado (não deletado)

## 🛠️ Tecnologias e Arquitetura

### Stack Tecnológica
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Oracle Database
- AutoMapper
- Swagger/OpenAPI

### Arquitetura
- **Domain Layer:** Entidades e regras de negócio
- **Application Layer:** DTOs e mapeamentos
- **Infrastructure Layer:** Persistência e repositórios
- **Presentation Layer:** Controllers e endpoints

## 🔄 Fluxo de Dados

1. **Cadastro de Moto:**
   ```
   POST /api/moto
   {
     "placa": "ABC1234",
     "ano": 2024,
     "modelo": "Honda CG 160",
     "patioId": 1
   }
   ```

2. **Cadastro de Pátio:**
   ```
   POST /api/patio
   {
     "nome": "Pátio Central",
     "endereco": "Rua Principal, 123",
     "capacidade": 100
   }
   ```

## 📈 Próximos Passos

1. Implementar autenticação e autorização
2. Adicionar mais validações de negócio
3. Implementar sistema de logs
4. Adicionar testes unitários e de integração
5. Implementar cache para melhor performance
6. Adicionar documentação mais detalhada da API

## 👥 Equipe

- RM556652 - Miguel Barros
- [Adicionar outros membros do grupo]

## 📅 Versão

- Versão: 1.0.0
- Data: Maio/2024 