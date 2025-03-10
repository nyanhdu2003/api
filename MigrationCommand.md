# Migrations là một công cụ giúp quản lý các thay đổi trong cấu trúc cơ sở dữ liệu (Database Schema) theo sự thay đổi của mô hình dữ liệu (Model) trong ứng dụng. Nó giúp đồng bộ hóa giữa code-first models và database schema một cách dễ dàng.

## 1. Using the CLI

### Add a migration
```bash
dotnet ef migrations add AddSeedingData --project QuizApp.WebAPI --startup-project QuizApp.WebAPI --context QuizAppDbContext --output-dir Migrations
dotnet ef migrations add [MigrationName] --project QuizApp.Data --startup-project QuizApp.WebAPI --context StorageDbContext --output-dir Migrations/Storage
```

### Update the database
```bash
dotnet ef database update --project QuizApp.WebAPI --startup-project QuizApp.WebAPI --context QuizAppDbContext
dotnet ef database update --project QuizApp.Data --startup-project QuizApp.WebAPI --context StorageDbContext
```

### Roll Back a migration
dotnet ef database update AddAllModelsAndConfigRelationship --project QuizApp.WebAPI --startup-project QuizApp.WebAPI --context QuizAppDbContext
### Drop the database
```bash
dotnet ef database drop --project QuizApp.WebAPI --startup-project QuizApp.WebAPI --context QuizAppDbContext
dotnet ef database drop --project QuizApp.Data --startup-project QuizApp.WebAPI --context StorageDbContext
```

### Remove a migration
```bash
dotnet ef migrations remove --project QuizApp.WebAPI --startup-project QuizApp.WebAPI --context QuizAppDbContext
dotnet ef migrations remove --project QuizApp.Data --startup-project QuizApp.WebAPI --context StorageDbContext
```

## 2. Using the Package Manager Console
### Add a migration
```bash
Add-Migration [MigrationName] -Project QuizApp.Data -StartupProject QuizApp.WebAPI -Context QuizAppDbContext -OutputDir QuizApp.Data/Migrations
```

### Update the database
```bash
Update-Database -Project QuizApp.Data -StartupProject QuizApp.WebAPI -Context QuizAppDbContext
```

### Roll back a migration
```bash
Update-Database [MigrationName] -Project QuizApp.Data -StartupProject QuizApp.WebAPI -Context QuizAppDbContext
```

### Remove a migration
```bash
Remove-Migration -Project QuizApp.Data -StartupProject QuizApp.WebAPI -Context QuizAppDbContext
```

[]: # Path: README.md
