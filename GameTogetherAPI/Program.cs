using GameTogetherAPI.Database;
using GameTogetherAPI.Repository;
using GameTogetherAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;

namespace GameTogetherAPI {
    public class Program {
        private static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Database setup
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // JWT setup
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            if (jwtSettings == null) {
                throw new Exception("JWT settings are missing in appsettings.json.");
            }

            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new Exception("JWT SecretKey is missing."));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true
                    };
                });

            builder.Services.AddAuthorization();

            // Service and repository setup
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();

            // Health Check setup
            builder.Services.AddHealthChecks()
                .AddCheck<HealthCheck>("Database_Health_Check");

            // Swagger setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "GameTogether API",
                    Version = "v1",
                    Description = "API for user authentication and management"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your JWT token"
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
                        new string[] {}
                    }
                });
            });

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction()) {
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameTogether API v1");
                    options.RoutePrefix = "";
                });
            }

            // Enable CORS before authentication
            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            // Map Controllers
            app.MapControllers();

            // Map Health Check Endpoint
            app.MapHealthChecks("/healthz");

            app.Run();
        }
    }
}
