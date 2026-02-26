# ConectaAtende API

API REST desenvolvida em **.NET 8** para gerenciamento de **contatos** e **tickets de atendimento**, com sistema de **triagem configurável**, persistência **InMemory** e projeto de **benchmarks para análise de performance**.

Este projeto foi desenvolvido como parte de um **exercício acadêmico de reescrita de sistema legado**, com foco na aplicação de **Clean Architecture**, separação de responsabilidades e boas práticas de engenharia de software.

---

# 📚 Contexto Acadêmico

A ConectaAtende API simula a reescrita do núcleo de um sistema legado utilizado para:

- Gerenciamento de contatos
- Controle de atendimentos (tickets)
- Organização de fila de atendimento
- Aplicação de políticas de triagem
- Avaliação de desempenho com benchmarks

O objetivo foi aplicar conceitos reais de mercado como:

- Arquitetura limpa
- Modelagem de domínio
- Separação em camadas
- Desacoplamento entre domínio e infraestrutura
- Persistência em memória
- Medição de performance

---

# 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture**, organizado em camadas independentes:

```
ConectaAtende.sln

src/

 ├─ ConectaAtende.Domain
 │   ├─ Entities
 │   ├─ Enums
 │   └─ Repositories

 ├─ ConectaAtende.Application
 │   └─ Services

 ├─ ConectaAtende.Infrastructure
 │   └─ Repositories

 ├─ ConectaAtende.Communication
 │   ├─ Requests
 │   └─ Responses

 └─ ConectaAtende.Api
     └─ Controllers


benchmarks/

 └─ ConectaAtende.Benchmarks
```

# 👤 Módulo de Contacts

Responsável pelo gerenciamento do catálogo de contatos.

## Funcionalidades

- Criar contato
- Buscar contato por ID
- Listar contatos paginados
- Excluir contato
- Manter lista de contatos recentes

## Endpoints

```
POST   /contacts

GET    /contacts/{id}

GET    /contacts?page=&pageSize=

DELETE /contacts/{id}

GET    /contacts/recent
```

---

# 🎫 Módulo de Tickets

Responsável pelo gerenciamento de atendimentos.

## Funcionalidades

- Criar ticket
- Buscar ticket por ID
- Listar tickets
- Fechar ticket
- Excluir ticket
- Enfileirar ticket
- Desenfileirar ticket
- Obter próximo ticket

## Endpoints

```
POST   /tickets

GET    /tickets/{id}

GET    /tickets?page=&pageSize=

PUT    /tickets/{id}/close

DELETE /tickets/{id}

POST   /tickets/enqueue/{id}

POST   /tickets/dequeue

GET    /tickets/next
```

---

# 🧠 Sistema de Triagem

Responsável por determinar qual ticket será atendido.

Implementado no serviço:

```
TriageService
```

Permite:

- Obter próximo ticket
- Alterar política de triagem

Endpoints:

```
GET  /triage/policy

POST /triage/policy
```

---

# 📦 Persistência

Persistência realizada em memória utilizando:

```
ConcurrentDictionary
```

Repositórios:

```
InMemoryContactRepository

InMemoryTicketRepository
```

Características:

- Thread-safe
- Alta performance
- Sem dependência externa
- Fácil substituição futura por banco de dados real

---

# 📊 Projeto de Benchmarks

Projeto separado:

```
ConectaAtende.Benchmarks
```

Utiliza:

```
BenchmarkDotNet
```

Objetivo:

Medir performance de:

- Inserção de contatos
- Busca por ID
- Busca paginada
- Criação de tickets
- Operações em memória

Executar benchmarks:

```
dotnet run -c Release --project ConectaAtende.Benchmarks
```

Executar em modo Release é obrigatório.

---

# 🔧 Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- C#
- Clean Architecture
- BenchmarkDotNet
- Dependency Injection
- ConcurrentDictionary

---

# ▶️ Como executar a API

Clone o repositório:

```
git clone https://github.com/seu-usuario/conectaatende-api.git
```

Entre na pasta:

```
cd conectaatende-api
```

Execute a API:

```
dotnet run --project ConectaAtende.Api
```

Swagger disponível em:

```
https://localhost:5001/swagger
```

---

# ▶️ Como executar os Benchmarks

Execute:

```
dotnet run -c Release --project ConectaAtende.Benchmarks
```

---

# 🎯 Objetivos Acadêmicos Alcançados

✔ Aplicação de Clean Architecture  
✔ Separação de responsabilidades  
✔ API REST funcional  
✔ Persistência InMemory  
✔ Sistema de triagem implementado  
✔ Estrutura desacoplada e escalável  
✔ Projeto de benchmark funcional  

---

# 🚀 Melhorias Futuras

- Persistência com banco de dados (SQL Server ou PostgreSQL)
- Testes unitários
- Autenticação e autorização
- Cache
- Deploy em cloud (Azure ou AWS)

---

# 👨‍💻 Autor

Projeto desenvolvido como exercício acadêmico para prática de:

- Arquitetura de software
- Modelagem de domínio
- Desenvolvimento backend com .NET 8
- Análise de performance com BenchmarkDotNet
