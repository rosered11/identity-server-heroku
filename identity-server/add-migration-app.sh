
export Database_Connection=localhost
export Identity__Key=key
dotnet ef migrations add $1 -c ApplicationDbContext -o Migrations/ApplicationDb
