using Microsoft.IdentityModel.Tokens;
using Products.Api.Config;
using Products.Api.Middleware;
using Products.Data.IoC;

namespace Products.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var identityServerUrl = builder.Configuration.GetValue<string>("IdentityServerUrl");

        // Add services to the container.
        builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
        {
            options.Authority = identityServerUrl; // "https://localhost:7205";
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false
            };
        });

        builder.Services.AddLogging();
        builder.Services.RegistraterDataServices();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSwaggerDocumentation();

        builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();

    }
}
