
export DATABASE_URL=localhost
export Identity__Key=key
dotnet ef migrations add $1 -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
