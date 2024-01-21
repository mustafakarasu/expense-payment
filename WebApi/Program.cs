using Business.Mappings;
using Business.Utilities;
using Microsoft.AspNetCore.HttpOverrides;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddRepositories();
            builder.Services.AddManagers();

            builder.Services.ConfigureDbContext(builder.Configuration);

            builder.Services.AddAuthentication();

            builder.Services.ConfigureCors();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJwt(builder.Configuration);
            builder.Services.AddJwtConfiguration(builder.Configuration);

            builder.Services.ConfigureSwagger();

            builder.Services.ConfigureFormOptions();

            builder.Services.ConfigureFluentValidation();

            builder.Services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.ConfigureExceptionHandler();

            app.Services.MigrateDatabase();

            // Configure the HTTP request pipeline.
            if ( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if ( app.Environment.IsProduction() )
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("CorsPolicy");
            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
