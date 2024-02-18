using Api.DbContexts;
using Api.Infrastructure.Repository;
using Api.Infrastructure.Services;
using Api.Model;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var config = builder.Configuration;
    builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetValue<string>("ConString")));

    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("*", builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://localhost:44396/api/User/get-all")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = config["JwtSetting:Issuer"],
            ValidAudience = config["JwtSetting:Audiance"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSetting:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(IdentityModel.AdminPolicyName, p => p.RequireClaim(IdentityModel.AdminUserClaim,
            "true"));
    });
    builder.Services.AddControllers();
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Learning App For FullStack Development",
            Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Need JWT Token",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
        });
    });
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("*");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
catch(Exception ex)
{
    Console.Write(ex.ToString());
}
