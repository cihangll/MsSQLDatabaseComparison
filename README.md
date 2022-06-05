# MsSQL Database Comparison

- [.NET 6](https://github.com/dotnet/core)
- [Blazor Server](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [MudBlazor](https://mudblazor.com)
- [Dapper](https://github.com/DapperLib/Dapper)

## Goals 

The goal of this repository is to compare 2 same mssql database instance.

## Installation

Open up your Command Prompt / Powershell and run the following command to install this repository.

```git
git clone https://github.com/cihangll/MsSQLDatabaseComparison.git
```

Open `appsettings.json` file and change ConnectionStrings section. For example;

```json
"ConnectionStrings": {
	"SourceConnectionString": "Server=.;Database=SourceAdventureWorksDW2019;Trusted_Connection=True;",
	"TargetConnectionString": "Server=.;Database=TargetAdventureWorksDW2019;Trusted_Connection=True;"
}
```

Execute the following command to open app.

```cmd
cd .\MsSQLDatabaseComparison\
dotnet run
```

![image](https://user-images.githubusercontent.com/6229029/172060443-10e88fa3-8454-4fd5-a755-5bcd4e7811b3.png)

## Application Features

- Table comparison
- View comparison
- Stored procedure comparison
- Function comparison
- Table column comparison
  - Columns
  - Data Types
  - IsNullable
  - Numeric
  - Default Column


## Screenshots

![image](https://user-images.githubusercontent.com/6229029/172060741-6aa713ce-b815-4da2-b343-5ccd3f30138c.png)

![image](https://user-images.githubusercontent.com/6229029/172060800-addc3c02-dd9d-4d9e-95a4-0606868c75da.png)

![image](https://user-images.githubusercontent.com/6229029/172060756-c858d680-b03d-4005-9b65-2e5a3425c666.png)

![image](https://user-images.githubusercontent.com/6229029/172060771-baedd248-515e-4a05-8b19-8625e5c876ac.png)

![image](https://user-images.githubusercontent.com/6229029/172060778-5b581ecb-077f-4a71-b7c9-e5e9cc727a0f.png)

![image](https://user-images.githubusercontent.com/6229029/172060787-894d5177-4d6a-4f9b-aa7e-0bbc47b20055.png)
