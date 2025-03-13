# Add package for QuizApp.Models
dotnet add QuizApp.Models package Microsoft.EntityFrameworkCore
dotnet add QuizApp.Models package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Add package for QuizApp.Business
dotnet add QuizApp.Business package Microsoft.EntityFrameworkCore
dotnet add QuizApp.Business package MediatR
dotnet add QuizApp.Business package AutoMapper
dotnet add QuizApp.Business package Newtonsoft.Json

# Add package for QuizApp.Data
dotnet add QuizApp.Data package Microsoft.EntityFrameworkCore
dotnet add QuizApp.Data package Microsoft.EntityFrameworkCore.SqlServer
dotnet add QuizApp.Data package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add QuizApp.Data package Microsoft.AspNetCore.Identity

# Add package for QuizApp.WebAPI
dotnet add QuizApp.WebAPI package Microsoft.EntityFrameworkCore
dotnet add QuizApp.WebAPI package Microsoft.EntityFrameworkCore.SqlServer
dotnet add QuizApp.WebAPI package Microsoft.EntityFrameworkCore.Design
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.Identity.UI
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.WebAPI.NewtonsoftJson
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.WebAPI.Versioning
dotnet add QuizApp.WebAPI package Microsoft.AspNetCore.WebAPI.Versioning.ApiExplorer
dotnet add QuizApp.WebAPI package Swashbuckle.AspNetCore
dotnet add QuizApp.WebAPI package Swashbuckle.AspNetCore.Swagger
dotnet add QuizApp.WebAPI package Swashbuckle.AspNetCore.SwaggerGen
dotnet add QuizApp.WebAPI package Swashbuckle.AspNetCore.SwaggerUI
dotnet add QuizApp.WebAPI package Microsoft.Extensions.Configuration
dotnet add QuizApp.WebAPI package Serilog
dotnet add QuizApp.WebAPI package Serilog.AspNetCore
dotnet add QuizApp.WebAPI package Serilog.Sinks.Console
dotnet add QuizApp.WebAPI package Serilog.Sinks.File


