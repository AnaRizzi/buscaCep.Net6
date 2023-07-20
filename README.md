# API que realiza busca de CEP
```
Criada em .NET 6
Utiliza o recurso de Minimal API
Utiliza IMediatR para chamar os Handlers
Utiliza Redis para salvar cache

O endpoint ViaCep consulta a API da ViaCep através do Refit.

O endpoint Correios consulta o Web Service dos Correios (WSDL) através do Service Reference.

Foi adicionado o DockerFile e o Docker-compose, para que seja possível rodar a aplicação (e o Redis) dentro do Docker.
```
Para rodar no Docker:
- Abrir o cmd dentro da pasta do projeto.
- Digitar "docker compose build" para construir a imagem do projeto.
- Digitar "docker compose up -d" para rodar os containers.
- O Swagger da API ficará disponível no endereço: http://localhost:5299/swagger/index.html
- Para parar os containers, digitar: "docker compose stop".
- Para remover os containers, digitar: "docker compose down".
- Se for necessário a remoção da imagem criada, digitar "docker images", copiar o id da imagem, e digitar: "docker rmi <id-da-imagem>".
