using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WzPipeline.Shared.Exporter;
using WzPipeline.Shared.Json;

namespace WzPipeline.Application.DependencyInjection;

public static class Shared
{
    public static IServiceCollection TryAddJsonSerializer(this IServiceCollection services)
    {
        services.AddSingleton<JsonConverter, PointArrayConverter>();
        services.TryAddSingleton(typeof(JsonSerializer), JsonSerializerFactory);

        return services;
    }

    private static JsonSerializer JsonSerializerFactory(IServiceProvider serviceProvider)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = serviceProvider.GetServices<JsonConverter>().ToList(),
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        return JsonSerializer.Create(settings);
    }

    public static IServiceCollection TryAddDictionaryJsonWriterFactory(this IServiceCollection services)
    {
        services.TryAddJsonSerializer();
        services.AddSingleton<DictionaryJsonWriterFactory>();

        return services;
    }
}