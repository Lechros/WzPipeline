using WzPipeline.Core.Pipeline;
using WzPipeline.Core.Pipeline.Graph;

namespace WzPipeline.Application;

internal class ConfigContext(string name)
{
    public GraphPipelineConfig Config { get; } = Builders.GraphPipelineBuilder(name);
    
    private readonly Dictionary<string, object> _configLookup = new();

    public TConfig GetConfigOrCreate<TConfig>(string key, Func<TConfig> factory)
    {
        if (!_configLookup.TryGetValue(key, out var config))
        {
            config = factory();
            _configLookup[key] = config!;
        }

        return (TConfig)config!;
    }
}