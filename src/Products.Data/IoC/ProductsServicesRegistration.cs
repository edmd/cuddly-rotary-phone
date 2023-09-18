using Microsoft.Extensions.DependencyInjection;
using Products.Data.Persistence;

namespace Products.Data.IoC
{
    public static class ProductsServicesRegistration
    {
        public static IServiceCollection RegistraterDataServices(this IServiceCollection services)
        {
            // Repositories and services registration
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddDbContext<ProductsDbContext>();

            return services;
        }
    }
}