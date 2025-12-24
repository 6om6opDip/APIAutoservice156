using Microsoft.EntityFrameworkCore;
using APIAutoservice156.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using APIAutoservice156.Repositories;
using APIAutoservice156.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Контроллеры
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// База данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Репозитории
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Сервисы
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication - КРИТИЧЕСКИ ВАЖНЫЙ БЛОК
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

// Проверка ключа
if (string.IsNullOrEmpty(secretKey))
{
    Console.WriteLine("❌ ОШИБКА: Jwt:SecretKey не найден в appsettings.json!");
    throw new Exception("Jwt:SecretKey не настроен");
}

Console.WriteLine($"🔐 JWT Конфигурация:");
Console.WriteLine($"🔐 Issuer: {jwtSettings["Issuer"]}");
Console.WriteLine($"🔐 Audience: {jwtSettings["Audience"]}");
Console.WriteLine($"🔐 SecretKey длина: {secretKey.Length} символов");

if (secretKey.Length < 32)
{
    Console.WriteLine($"⚠️ ВНИМАНИЕ: SecretKey слишком короткий! Минимум 32 символа, а у вас {secretKey.Length}");
}

// Для отладки: выводим первые 10 символов ключа
Console.WriteLine($"🔐 SecretKey (первые 10 символов): {secretKey.Substring(0, Math.Min(10, secretKey.Length))}...");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        // ВАЖНО: Отключаем автоматический ответ 401 для отладки
        options.IncludeErrorDetails = true;
        options.Challenge = "Bearer";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],           // "AutoserviceAPI"
            ValidAudience = jwtSettings["Audience"],       // "AutoserviceClient"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,

            // Для отладки: логируем процесс валидации
            LogValidationExceptions = true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"\n❌❌❌ JWT ОШИБКА АУТЕНТИФИКАЦИИ ❌❌❌");
                Console.WriteLine($"Тип исключения: {context.Exception.GetType().FullName}");
                Console.WriteLine($"Сообщение: {context.Exception.Message}");

                if (context.Exception.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {context.Exception.InnerException.Message}");
                }

                // Детальная диагностика
                if (context.Exception is SecurityTokenInvalidSignatureException)
                {
                    Console.WriteLine("ДЕТАЛИ: Проблема с подписью токена!");
                    Console.WriteLine($"SecretKey длина: {secretKey.Length}");
                    Console.WriteLine($"SecretKey (первые 20): {secretKey.Substring(0, Math.Min(20, secretKey.Length))}");
                }
                else if (context.Exception is SecurityTokenInvalidIssuerException)
                {
                    Console.WriteLine($"ДЕТАЛИ: Issuer не совпадает. Ожидается: '{jwtSettings["Issuer"]}'");
                }
                else if (context.Exception is SecurityTokenInvalidAudienceException)
                {
                    Console.WriteLine($"ДЕТАЛИ: Audience не совпадает. Ожидается: '{jwtSettings["Audience"]}'");
                }
                else if (context.Exception is SecurityTokenExpiredException)
                {
                    Console.WriteLine("ДЕТАЛИ: Токен просрочен!");
                }

                Console.WriteLine("Stack Trace:");
                Console.WriteLine(context.Exception.StackTrace);
                Console.WriteLine("❌❌❌ КОНЕЦ ОШИБКИ ❌❌❌\n");

                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                Console.WriteLine($"\n⚠️⚠️⚠️ JWT CHALLENGE (401) ⚠️⚠️⚠️");
                Console.WriteLine($"Error: {context.Error}");
                Console.WriteLine($"ErrorDescription: {context.ErrorDescription}");
                Console.WriteLine($"ErrorUri: {context.ErrorUri}");
                Console.WriteLine($"AuthenticateFailure: {context.AuthenticateFailure?.Message}");
                Console.WriteLine("⚠️⚠️⚠️ КОНЕЦ CHALLENGE ⚠️⚠️⚠️\n");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine($"\n✅✅✅ JWT ТОКЕН ВАЛИДЕН! ✅✅✅");
                Console.WriteLine($"Пользователь: {context.Principal?.Identity?.Name}");
                Console.WriteLine($"Аутентифицирован: {context.Principal?.Identity?.IsAuthenticated}");

                Console.WriteLine($"Claims:");
                foreach (var claim in context.Principal?.Claims ?? [])
                {
                    Console.WriteLine($"  {claim.Type} = {claim.Value}");
                }
                Console.WriteLine("✅✅✅ КОНЕЦ ВАЛИДАЦИИ ✅✅✅\n");

                return Task.CompletedTask;
            },

            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine($"\n🔐🔐🔐 ПОЛУЧЕН ТОКЕН 🔐🔐🔐");
                    Console.WriteLine($"Полный токен: {token}");

                    try
                    {
                        // Пробуем декодировать токен без проверки подписи
                        var handler = new JwtSecurityTokenHandler();
                        if (handler.CanReadToken(token))
                        {
                            var jwtToken = handler.ReadJwtToken(token);
                            Console.WriteLine($"Header: {JsonSerializer.Serialize(jwtToken.Header)}");
                            Console.WriteLine($"Payload: {JsonSerializer.Serialize(jwtToken.Payload)}");
                            Console.WriteLine($"Issuer: {jwtToken.Issuer}");
                            Console.WriteLine($"Audience: {string.Join(", ", jwtToken.Audiences)}");
                            Console.WriteLine($"Expires: {jwtToken.ValidTo}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка декодирования токена: {ex.Message}");
                    }

                    Console.WriteLine("🔐🔐🔐 КОНЕЦ ТОКЕНА 🔐🔐🔐\n");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Autoservice API",
        Version = "v1",
        Description = "API для автосервиса"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен в формате: Bearer {ваш_токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Autoservice API v1");
        c.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Инициализация БД
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Database.CanConnect())
    {
        Console.WriteLine("✅ Подключено к БД!");
        db.Database.EnsureCreated();

        // Инициализируем если нет пользователей
        if (!db.Users.Any())
        {
            DbInitializer.Initialize(db);
            Console.WriteLine("✅ Тестовые данные созданы!");
        }
        else
        {
            Console.WriteLine($"✅ В БД уже есть {db.Users.Count()} пользователей");
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                Console.WriteLine($"   - {user.Username} ({user.Email}), Роль: {user.Role}");
            }
        }

        Console.WriteLine($"✅ Клиентов в БД: {db.Clients.Count()}");
        Console.WriteLine($"✅ Сервисов в БД: {db.Services.Count()}");
    }
    else
    {
        Console.WriteLine("❌ Не удалось подключиться к БД");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Ошибка БД: {ex.Message}");
    Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
}

app.Run();