# Nomadas Network

- Create code on machine from scratch

`dotnet new sln -o nomadas.network`
`cd nomadas.network`
`dotnet new webapi -o Nomadas.Network`
`dotnet new xunit -o Nomadas.Network.Tests`
`cd Nomadas.Network.Tests`
`dotnet add package Moq`
`dotnet add package Microsoft.AspNetCore.TestHost`
`cd ..`
`dotnet sln add ./Nomadas.Network/Nomadas.Network.csproj`
`dotnet sln add ./Nomadas.Network.Tests/Nomadas.Network.Tests.csproj`
`dotnet add Nomadas.Network.Tests/Nomadas.Network.Tests.csproj reference Nomadas.Network.csproj`

`dotnet new sln -o nomadas.network && cd nomadas.network && dotnet new webapi -o Nomadas.Network && dotnet new xunit -o Nomadas.Network.Tests && cd Nomadas.Network.Tests && dotnet add package Moq && dotnet add package Microsoft.AspNetCore.TestHost && cd .. && dotnet sln add ./Nomadas.Network.Tests/Nomadas.Network.Tests.csproj && dotnet sln add ./Nomadas.Network/Nomadas.Network.csproj && dotnet add Nomadas.Network.Tests/Nomadas.Network.Tests.csproj reference Nomadas.Network/Nomadas.Network.csproj`

- Run code on machine

`dotnet run` or `dotnet watch run`
`curl --insecure https://localhost:5001/health`

- Run tests on machine

`dotnet test`

- Test code on machine with docker

`docker build -t nomadas.network.localimage .`

`docker run -d -p 80:80 --name nomadas.network nomadas.network.localimage`
