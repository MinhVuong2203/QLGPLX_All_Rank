using Microsoft.EntityFrameworkCore;
using QLGPLX.Data;
using QLGPLX.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Đăng ký tất cả service và Repositories
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    //Register Services (có interface)
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    // Register Repositories (không interface)
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
        .AsSelf()
        .WithScopedLifetime()
);

builder.Services.AddDbContext<GplxDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseSwagger();
//app.UseSwaggerUI();

// Port deloy
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//app.Urls.Add($"http://*:{port}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
