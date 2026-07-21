using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WzPipeline.Application.Shared.Json;

public static class JsonServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationJson(this IServiceCollection services)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<JsonConverter, PointArrayConverter>());
        services.TryAddSingleton<JsonSerializer>(CreateSerializer);
        return services;
    }

    private static JsonSerializer CreateSerializer(IServiceProvider services)
    {
        return JsonSerializer.Create(new JsonSerializerSettings
        {
            Converters = services.GetServices<JsonConverter>().ToList(),
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        });
    }
}