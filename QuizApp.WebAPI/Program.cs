using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Business.Services;
using QuizApp.Data.Repositories;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;
using QuizApp.WebAPI.SeedData;
using QuizApp.WebAPI.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext 
builder.Services.AddDbContext<QuizAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuizAppDbConnection"), b => b.MigrationsAssembly("QuizApp.Data"));
});

// Register Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Service
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

// Register Repository and Service for User and Role
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Register Service for Quesion
builder.Services.AddScoped<IQuestionService, QuestionService>();

// Register Service for Quiz
builder.Services.AddScoped<IQuizService, QuizService>();

// Register QuestionController
builder.Services.AddControllers();

// Config Identity 
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<QuizAppDbContext>()
    .AddDefaultTokenProviders();

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/quizapp-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Register configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// **🔹 Gọi SeedData để populate dữ liệu mẫu vào database**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuizAppDbContext>();
        await context.Database.MigrateAsync(); // Đảm bảo database được cập nhật schema mới nhất
        await SeedData.InitializeAsync(services); // Thêm dữ liệu mẫu
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();
