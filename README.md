
# Amigo Secreto API

Este projeto consiste em uma API RESTful construída com ASP.NET Core que permite organizar sorteios de Amigo Secreto de forma automática. A aplicação possui funcionalidades para cadastrar participantes, criar grupos, gerar sorteios e enviar os resultados por e-mail.

## Funcionalidades
* **Criar Grupo:** Permite criar um novo grupo para o sorteio.
* **Buscar Grupo:** Permite buscar um grupo pelo seu ID.
* **Adicionar Participantes:** Permite adicionar participantes a um grupo de amigo secreto.
* **Gerar Sorteio:** Gera automaticamente os pares de amigo secreto para cada participante.
* **Enviar E-mails:** Após o sorteio, envia e-mails para cada participante com o nome do seu amigo secreto.
* **Endpoints RESTful:** A API oferece endpoints para interagir com o sistema, como adicionar participantes e gerar o sorteio.

## Tecnologias Usadas
* **ASP.NET Core:** Framework para desenvolvimento da API.
* **Entity Framework Core:** Para interagir com o banco de dados.
* **Papercut SMTP:** Servidor SMTP local para enviar e-mails durante o desenvolvimento.
* **Swagger:** Para documentação e testes da API.
* **PostgreSQL:** Banco de dados utilizado para armazenar informações dos participantes e grupos.

## Endpoints
#### A API oferece os seguintes endpoints:

* **POST /api/Grupos:** Cria um novo grupo de amigo secreto.
* **GET /api/Grupos/{grupoId}:** Retorna os detalhes de um grupo específico pelo seu ID.
* **POST /api/Participantes:** Adiciona um novo participante ao grupo.
* **GET /api/Participantes/grupo/{grupoId}:** Retorna todos os participantes de um grupo.
* **POST /api/Participantes/sorteio/{grupoId}:** Gera o sorteio para o grupo e envia os e-mails.
* **GET /api/Participantes/sorteio/{participanteId}:** Retorna o amigo secreto sorteado para um participante.


## Como rodar o projeto
### Requisitos
* .NET 6 ou superior
* PostgreSQL ou qualquer banco de dados compatível com Entity Framework Core
* Papercut SMTP (para o envio de e-mails durante o desenvolvimento)

### Passos para rodar localmente
* Clone o repositório
* Faça a configuração do appsettings.json
```bash
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AmigoSecretoDB;Username=COLOQUE_SEU_USUARIO;Password=COLOQUE_SUA_SENHA"
  }
```
* Instale as dependências e rode as migrações do Entity Framework
* Execute a API
* Acesse o swagger para testar os endpoints da API diretamente no navegador



## Autores

- [@Leoonpr](https://github.com/Leoonpr/)

