using Jering.Javascript.NodeJS;
using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Gear.Models;

namespace WzJson.V2.Domains.Gear.Processors;

public class GearRawAttributesProcessor(INodeJSService nodeJsService, string jsPath) : AbstractProcessor<MalibGear, MalibGear>
{
    public override IEnumerable<MalibGear> Process(IEnumerable<MalibGear> models)
    {
        var input = models.ToArray();
        // Inputs are processed in batch to optimize js call overhead
        var task = nodeJsService.InvokeFromFileAsync<MalibGear[]>(jsPath, args: [input]);
        var result = task.Result;
        if (result == null)
        {
            throw new ApplicationException("Result is null");
        }

        if (result.Length != input.Length)
        {
            throw new ApplicationException(
                $"Result length({result.Length}) is not equal to input length({input.Length})");
        }

        return result;
    }
}