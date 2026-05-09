using Backend.Configurations;
using Backend.Repository;
using Backend.Service;
using Backend.Service.Interface;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QLGPLX.Data;
using QLGPLX.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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



builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddSingleton(provider =>   
{
    var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new Cloudinary(account);
});


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

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
