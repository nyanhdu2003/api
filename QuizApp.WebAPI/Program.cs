using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Business.Services;
using QuizApp.Data.Repositories;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;
using QuizApp.WebAPI.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Config Identity 
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<QuizAppDbContext>()
    .AddDefaultTokenProviders();


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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
await app.RunAsync();

