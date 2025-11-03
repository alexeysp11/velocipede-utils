
dotnet build

dotnet ef migrations add InitialCreate --project VelocipedeUtils.Ubp.Core/VelocipedeUtils.Ubp.Core.csproj --startup-project VelocipedeUtils.Ubp/VelocipedeUtils.Ubp.csproj
dotnet ef database update --project VelocipedeUtils.Ubp.Core/VelocipedeUtils.Ubp.Core.csproj --startup-project VelocipedeUtils.Ubp/VelocipedeUtils.Ubp.csproj
