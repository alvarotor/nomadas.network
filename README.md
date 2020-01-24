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
`dotnet add Tests/Tests.csproj reference nomadas.network.csproj`

- Run code on machine

`dotnet run` or `dotnet watch run`
`curl --insecure https://localhost:5001/health`

- Run tests on machine

`dotnet test`

- Test code on machine with docker

`docker build -t nomadas.network.localimage .`

`docker run -d -p 80:80 --name nomadas.network nomadas.network.localimage`
