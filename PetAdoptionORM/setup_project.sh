#!/bin/bash
mkdir -p /Users/muhammeteminbas/Desktop/PetAdoptionORM
cd /Users/muhammeteminbas/Desktop/PetAdoptionORM

dotnet new sln -n PetAdoptionORM

dotnet new mvc -n PetAdoptionORM
dotnet new classlib -n PetAdoptionORM.Data
dotnet new classlib -n PetAdoptionORM.Model

dotnet sln PetAdoptionORM.sln add PetAdoptionORM/PetAdoptionORM.csproj
dotnet sln PetAdoptionORM.sln add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj
dotnet sln PetAdoptionORM.sln add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj

dotnet add PetAdoptionORM/PetAdoptionORM.csproj reference PetAdoptionORM.Data/PetAdoptionORM.Data.csproj
dotnet add PetAdoptionORM/PetAdoptionORM.csproj reference PetAdoptionORM.Model/PetAdoptionORM.Model.csproj
dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj reference PetAdoptionORM.Model/PetAdoptionORM.Model.csproj

# Use 10.0.0 because 10.0.9 might not be published if they just typed an arbitrary version. Wait, FirstProjectORM had 10.0.9, so I will stick to 10.0.9
dotnet add PetAdoptionORM/PetAdoptionORM.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM/PetAdoptionORM.csproj package Microsoft.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM/PetAdoptionORM.csproj package Microsoft.EntityFrameworkCore.Design -v 10.0.9
dotnet add PetAdoptionORM/PetAdoptionORM.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 10.0.9
dotnet add PetAdoptionORM/PetAdoptionORM.csproj package Microsoft.EntityFrameworkCore.Tools -v 10.0.9

dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj package Microsoft.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj package Microsoft.EntityFrameworkCore.Design -v 10.0.9
dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 10.0.9
dotnet add PetAdoptionORM.Data/PetAdoptionORM.Data.csproj package Microsoft.EntityFrameworkCore.Tools -v 10.0.9

dotnet add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj package Microsoft.EntityFrameworkCore -v 10.0.9
dotnet add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj package Microsoft.EntityFrameworkCore.Design -v 10.0.9
dotnet add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 10.0.9
dotnet add PetAdoptionORM.Model/PetAdoptionORM.Model.csproj package Microsoft.EntityFrameworkCore.Tools -v 10.0.9

rm -f PetAdoptionORM.Data/Class1.cs
rm -f PetAdoptionORM.Model/Class1.cs

echo "Project structure created successfully!"
