
using Microsoft.Extensions.DependencyInjection;

namespace FoundryRulesAndUnits;

public class CodeStatus
{
    public string Version()
    {
        var version = GetType().Assembly.GetName().Version ?? new Version(0, 0, 0, 0);
        var ver = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        return ver;
    }   

}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFoundryRulesAndUnits(this IServiceCollection services)
    {
        return services;
    }
}
