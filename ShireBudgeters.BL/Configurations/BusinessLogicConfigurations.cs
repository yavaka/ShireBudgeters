using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShireBudgeters.DA.Configurations;

namespace ShireBudgeters.BL.Configurations;

public static class BusinessLogicConfigurations
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessServices(configuration);

        return services;
    }
}
