# Nomadas Network

## Intructions to run the project

Add `.env` file with variable (for docker compose env variables):

    ASPNETCORE_ENVIRONMENT=Development
    DB_PASSWORD=myPass
    ADMIN_LOGIN=SA
    DB_SERVER=localhost
    DB_NAME=nomadas-network
    DB_PORT=1433

Add `.vscode/launch.json` (normally added by vscode itself) but we need to add env variables inside "configurations" section and inside ".NET Core Launch (web)" option:

    "env": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "DB_PASSWORD": "myPass",
        "ADMIN_LOGIN": "SA",
        "DB_SERVER": "localhost",
        "DB_NAME": "nomadas-network",
        "DB_PORT": "1433"
    },

## TO-DO

- Force https
- Add and use Authentication system and samples

## Create code on machine from scratch

    dotnet new sln -o nomadas.network
    cd nomadas.network
    dotnet new webapi -o Nomadas.Network
    dotnet new xunit -o Nomadas.Network.Tests
    cd Nomadas.Network.Tests
    dotnet add package Moq
    dotnet add package Microsoft.AspNetCore.TestHost
    dotnet add package Microsoft.AspNetCore.Mvc.Testing
    dotnet add package Microsoft.EntityFrameworkCore.InMemory
    dotnet add package Microsoft.EntityFrameworkCore
    cd ..
    dotnet sln add ./Nomadas.Network/Nomadas.Network.csproj
    dotnet sln add ./Nomadas.Network.Tests/Nomadas.Network.Tests.csproj
    dotnet add Nomadas.Network.Tests/Nomadas.Network.Tests.csproj reference Nomadas.Network.csproj
    cd Nomadas.Network
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    cd ..

Code to add a new project in one command line

    dotnet new sln -o nomadas.network && cd nomadas.network && dotnet new webapi -o Nomadas.Network && dotnet new xunit -o Nomadas.Network.Tests && cd Nomadas.Network.Tests && dotnet add package Moq && dotnet add package Microsoft.AspNetCore.Mvc.Testing && dotnet add package Microsoft.AspNetCore.TestHost && dotnet add package Microsoft.EntityFrameworkCore.InMemory && dotnet add package Microsoft.EntityFrameworkCore &&  cd .. && dotnet sln add ./Nomadas.Network.Tests/Nomadas.Network.Tests.csproj && dotnet sln add ./Nomadas.Network/Nomadas.Network.csproj && dotnet add Nomadas.Network.Tests/Nomadas.Network.Tests.csproj reference Nomadas.Network/Nomadas.Network.csproj && cd Nomadas.Network && dotnet add package Microsoft.EntityFrameworkCore.SqlServer && cd ..

## Run code on machine

    dotnet run
    dotnet watch run
    curl --insecure https://localhost:5001/health

## Run tests on machine

    dotnet test

## Test code on machine with docker

    docker build -t nomadas.network.localimage .
    docker run -d -p 80:80 --name nomadas.network nomadas.network.localimage

## Tagging the project

    git tag
    git tag -a 0.1 -m "Basic project working with endpoint, docker and tests running"
    git tag -d 0.1
    git push origin 0.1

## Tagging in docker

    docker tag goodbytes23/nomadas.network goodbytes23/nomadas.network:0.1
    docker push goodbytes23/nomadas.network

## Running SQL Server

    docker run -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=<YourStrong@Passw0rd>" \
    -e 'MSSQL_PID=Express' \
    -p 1433:1433 \
    -v sqlvolume:/var/opt/mssql \
    -d mcr.microsoft.com/mssql/server:2019-latest

## Migrations

    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef migrations remove

## Dotnet report test coverage

    dotnet test --collect:"XPlat Code Coverage"
    dotnet tool install --global dotnet-reportgenerator-globaltool
    reportgenerator "-reports:OpenCover.xml" "-targetdir:coveragereport" reporttypes:Html
