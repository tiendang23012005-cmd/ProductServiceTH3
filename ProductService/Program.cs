using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductService.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONTROLLER
// ========================================
builder.Services.AddControllers();


// ========================================
// DATABASE
// ========================================
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    ));


// ========================================
// JWT
// ========================================
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new Exception(
                 "JWT Key ch?a ???c c?u hình"
             );

var jwtIssuer = builder.Configuration["Jwt:Issuer"];

var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });


// ========================================
// AUTHORIZATION
// ========================================
builder.Services.AddAuthorization();


// ========================================
// SWAGGER
// ========================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Nh?p JWT theo d?ng: Bearer {token}"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        }
    );
});


var app = builder.Build();


// ========================================
// SWAGGER
// ========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// ========================================
// HTTPS
// ========================================
app.UseHttpsRedirection();


// ========================================
// AUTHENTICATION
// ========================================
app.UseAuthentication();


// ========================================
// AUTHORIZATION
// ========================================
app.UseAuthorization();


// ========================================
// CONTROLLER
// ========================================
app.MapControllers();


app.Run();