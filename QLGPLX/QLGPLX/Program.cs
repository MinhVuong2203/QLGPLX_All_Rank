using Backend.Configurations;
using Backend.Data;
using Backend.Repository;
using Backend.Service;
using Backend.Service.Interface;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLGPLX.Mapping;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QLGPLX API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token. Ví dụ: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

    // Register Services có interface
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()

    // Register Repositories không interface
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

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("JWT Key chưa được cấu hình");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Mặc định tất cả API đều cần đăng nhập
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    // Các quyền đơn
    options.AddPolicy("QUAN_LY_CAN_BO", policy =>
        policy.RequireClaim("permission", "QUAN_LY_CAN_BO"));

    options.AddPolicy("QUAN_LY_HO_SO", policy =>
        policy.RequireClaim("permission", "QUAN_LY_HO_SO"));

    options.AddPolicy("DUYET_HO_SO", policy =>
        policy.RequireClaim("permission", "DUYET_HO_SO"));

    options.AddPolicy("QUAN_LY_KY_THI", policy =>
        policy.RequireClaim("permission", "QUAN_LY_KY_THI"));

    options.AddPolicy("NHAP_KET_QUA_THI", policy =>
        policy.RequireClaim("permission", "NHAP_KET_QUA_THI"));

    options.AddPolicy("CAP_GPLX", policy =>
        policy.RequireClaim("permission", "CAP_GPLX"));

    options.AddPolicy("GIA_HAN_GPLX", policy =>
        policy.RequireClaim("permission", "GIA_HAN_GPLX"));

    // Các quyền ghép
    options.AddPolicy("HO_SO", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("permission", "QUAN_LY_HO_SO") ||
            context.User.HasClaim("permission", "DUYET_HO_SO")));

});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();