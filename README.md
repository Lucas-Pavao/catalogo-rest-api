# CatalagoApi - REST API em .NET

## Descrição

CatalagoApi é uma REST API desenvolvida em .NET para gerenciar um catálogo de produtos. Esta API permite realizar operações CRUD (Create, Read, Update, Delete) em produtos, categorias e outras entidades relacionadas.

## Requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) instalado na máquina.
- Banco de dados MySQL.

## Configuração do Banco de Dados

Antes de começar, é necessário configurar o banco de dados MySQL. Abra o arquivo `appsettings.json` no projeto e altere as configurações de conexão com o banco de dados:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SeuBancoDeDados;User Id=SeuUsuario;Password=SuaSenha;"
  },
  // ... outras configurações ...
}
```

Certifique-se de substituir "SeuBancoDeDados", "SeuUsuario" e "SuaSenha" pelas informações corretas do seu banco de dados MySQL.

## Executando a Aplicação

1. Abra um terminal na pasta do projeto.

2. Execute o seguinte comando para restaurar as dependências:

```bash
dotnet restore
```

3. Em seguida, execute o comando para aplicar as migrações do banco de dados:

```bash
dotnet ef database update
```

4. Agora, inicie a aplicação com o seguinte comando:

```bash
dotnet run
```

