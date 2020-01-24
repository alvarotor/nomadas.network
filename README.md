# Nomadas Network

- Create code on machine from scratch

`mkdir nomadas.network && cd nomadas.network`
`dotnet new webapi`
`dotnet new sln`
`dotnet new xunit -o Tests`
`cd Tests`
`dotnet add package Moq`
`cd ..`
`dotnet sln add ./Tests/Tests.csproj`
`dotnet add nomadas.network.csproj reference Tests/Tests.csproj`

- Run code on machine

`dotnet run`

- Run tests on machine

`dotnet test`

- Test code on machine with docker

`docker build -t nomadas.network.localimage .`

`docker run -d -p 80:80 --name nomadas.network nomadas.network.localimage`
