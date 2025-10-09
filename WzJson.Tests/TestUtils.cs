using WzComparerR2.WzLib;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Linear;
using WzJson.Core.Pipeline.Runner;
using WzJson.Core.Stereotype;
using WzJson.Shared;

namespace WzJson.Tests;

internal static class TestUtils
{
    public const string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";

    public static IWzProvider CreateWzProvider()
    {
        return new WzProvider(BaseWzPath);
    }

    public class DefaultNode(Wz_Node node) : INode
    {
        public static DefaultNode Create(Wz_Node node)
        {
            return new DefaultNode(node);
        }

        public string Id => node.Text;
    }

    public class NoopPipelineRunner : IPipelineRunner
    {
        public IStepState Run(PipelineRoot root, IProgress<IStepState>? progress = null)
        {
            if (!ContainsExporter(root))
            {
                throw new ArgumentException("No Exporter found");
            }

            SetSingleValueHolderValue(root);
            return new StepState();
        }

        private static bool ContainsExporter(IStep step)
        {
            return step is IExporterStep || step.Children.Any(ContainsExporter);
        }

        private static void SetSingleValueHolderValue(IStep step)
        {
            if (step.Type == StepType.Exporter)
            {
                var exporterStep = (IExporterStep)step;
                var exporter = exporterStep.Exporter;
                var type = exporter.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SingleValueHolder<>))
                {
                    var valueType = type.GetGenericArguments()[0];
                    exporter.Export(Activator.CreateInstance(valueType)!);
                }
            }

            foreach (var child in step.Children)
            {
                SetSingleValueHolderValue(child);
            }
        }

        private class StepState : IStepState
        {
            public string Name => "Noop";
            public IEnumerable<IStepState> Children => [];
            public NodeStatus Status => NodeStatus.Complete;
            public int Count => 0;
            public int? TotalCount => 0;
            public DateTime? StartTime => null;
            public DateTime? EndTime => null;
            public TimeSpan Duration => TimeSpan.Zero;
        }
    }
}