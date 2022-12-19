# ASP.NET Core 6 (MVC)
Hotel Booking System (NCC L5DC Project)

> Migration Command

```
dotnet ef migrations add InitMigration --project .\Panda.HotelBooking\Panda.HotelBooking.csproj
```
```
dotnet ef database update --project .\Panda.HotelBooking\Panda.HotelBooking.csproj
```

> Clean Architecture Project Db Migration Command

```
dotnet ef migrations add initmigration --context ApplicationDbContext --project .\Infrastructure\Infrastructure.csproj --startup-project .\WebMvcUI\WebMvcUI.csproj
```
```
dotnet ef database update --context ApplicationDbContext --project .\Infrastructure\Infrastructure.csproj --startup-project .\WebMvcUI\WebMvcUI.csproj
```
