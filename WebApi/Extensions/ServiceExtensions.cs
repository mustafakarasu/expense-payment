using System.Text;
using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories;
using Domain.ConfigurationModels;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Business.Managers;
using Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Features;
using Business.Validators.Payments;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExpenseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    opt => opt.MigrationsAssembly("DataAccess"));
            });
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, Role>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                    o.User.RequireUniqueEmail = true;
                    o.Lockout = new LockoutOptions()
                    {
                        AllowedForNewUsers = false
                    };
                })
                .AddEntityFrameworkStores<ExpenseContext>()
                .AddDefaultTokenProviders();
        }
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["ValidIssuer"],
                        ValidAudience = jwtSettings["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IRepositoryService, RepositoryService>();
        }
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationManager>();
            services.AddScoped<IEmployeeService, EmployeeManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IPaymentMethodService, PaymentMethodManager>();
            services.AddScoped<IPaymentService, PaymentManager>();
            services.AddScoped<IExpenseService, ExpenseManager>();
            services.AddScoped<IReportService, ReportManager>();
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Expense Payment API",
                    Version = "v1",
                    Description = "Expense Payment API by Mustafa Karasu",
                    Contact = new OpenApiContact
                    {
                        Name = "Mustafa Karasu",
                        Email = "mustafa.karasu@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/mustafa-karasu/"),
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureFormOptions(this IServiceCollection services)
        {
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<PaymentCreationDtoValidator>();
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            ValidatorOptions.Global.LanguageManager.Enabled = false;
        }
    }
}
