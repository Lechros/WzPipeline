using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;

namespace WzPipeline.Tests;

public class AppExecutionTests
{
    [Test]
    public void Resolve_IncludesDependenciesInTopologicalOrder()
    {
        var dependencyId = new PipelineId("Dependency");
        var targetId = new PipelineId("Target");
        var registry = new PipelineRegistry();
        registry.Register<TestPipeline>(dependencyId).Create(_ => new TestPipeline(dependencyId));
        registry.Register<TestPipeline>(targetId).DependsOn(dependencyId)
            .Create(_ => new TestPipeline(targetId));

        var plan = registry.Resolve([targetId]);

        plan.RequestedPipelineIds.Should().Equal(targetId);
        plan.ActivePipelineIds.Should().Equal(dependencyId, targetId);
        plan.IsRequested(dependencyId).Should().BeFalse();
    }

    [Test]
    public void Resolve_RejectsDependencyCycles()
    {
        var firstId = new PipelineId("First");
        var secondId = new PipelineId("Second");
        var registry = new PipelineRegistry();
        registry.Register<TestPipeline>(firstId).DependsOn(secondId)
            .Create(_ => new TestPipeline(firstId));
        registry.Register<TestPipeline>(secondId).DependsOn(firstId)
            .Create(_ => new TestPipeline(secondId));

        var action = () => registry.Resolve([firstId]);

        action.Should().Throw<InvalidOperationException>().WithMessage("*cycle*");
    }

    [Test]
    public async Task Execute_DoesNotCreateDispatcherForEmptySelection()
    {
        var sourceCreated = false;
        var dispatchers = new SourceRegistry();
        dispatchers.Register<int>(new SourceId("Numbers"), _ =>
        {
            sourceCreated = true;
            return [1];
        });

        await new PipelineExecutor(dispatchers).ExecuteAsync(
            new PipelineRegistry().Resolve([]), EmptyServiceProvider.Instance);

        sourceCreated.Should().BeFalse();
    }

    [Test]
    public async Task Execute_SharesOneDispatcherAcrossMultiplePipelines()
    {
        var dispatcherId = new SourceId("Numbers");
        var firstId = new PipelineId("First");
        var secondId = new PipelineId("Second");
        var sourceCreationCount = 0;
        var pipelines = new PipelineRegistry();
        pipelines.Register<TestPipeline>(firstId).Create(_ => new TestPipeline(firstId))
            .Consumes(dispatcherId, pipeline => pipeline.Input);
        pipelines.Register<TestPipeline>(secondId).Create(_ => new TestPipeline(secondId))
            .Consumes(dispatcherId, pipeline => pipeline.Input);
        var dispatchers = new SourceRegistry();
        dispatchers.Register<int>(dispatcherId, _ =>
        {
            sourceCreationCount++;
            return [1, 2, 3];
        });

        var result = await new PipelineExecutor(dispatchers).ExecuteAsync(
            pipelines.Resolve([firstId, secondId]), EmptyServiceProvider.Instance);

        sourceCreationCount.Should().Be(1);
        result.GetRequiredPipeline<TestPipeline>(firstId).Received.Should().Equal(1, 2, 3);
        result.GetRequiredPipeline<TestPipeline>(secondId).Received.Should().Equal(1, 2, 3);
    }

    [Test]
    public async Task Export_OnlyRunsForDirectlyRequestedPipelines()
    {
        var dependencyId = new PipelineId("Dependency");
        var targetId = new PipelineId("Target");
        var dependencyExports = 0;
        var targetExports = 0;
        var registry = new PipelineRegistry();
        registry.Register<CompletedPipeline>(dependencyId)
            .Create(_ => new CompletedPipeline(dependencyId))
            .Exports((_, _) =>
            {
                dependencyExports++;
                return Task.CompletedTask;
            });
        registry.Register<CompletedPipeline>(targetId)
            .DependsOn(dependencyId)
            .Create(_ => new CompletedPipeline(targetId))
            .Exports((_, _) =>
            {
                targetExports++;
                return Task.CompletedTask;
            });
        var execution = await new PipelineExecutor(new SourceRegistry()).ExecuteAsync(
            registry.Resolve([targetId]), EmptyServiceProvider.Instance);

        await new PipelineExportRunner().ExportAsync(
            execution, EmptyServiceProvider.Instance, new PipelineExportOptions
            {
                OutputRootPath = "."
            });

        dependencyExports.Should().Be(0);
        targetExports.Should().Be(1);
    }

    private sealed class TestPipeline : IPipeline
    {
        public TestPipeline(PipelineId id)
        {
            Id = id;
            var input = new ActionBlock<int>(value => Received.Add(value));
            Input = input;
            Completion = input.Completion;
        }

        public PipelineId Id { get; }
        public ITargetBlock<int> Input { get; }
        public List<int> Received { get; } = [];
        public Task Completion { get; }
    }

    private sealed class CompletedPipeline(PipelineId id) : IPipeline
    {
        public PipelineId Id { get; } = id;
        public Task Completion => Task.CompletedTask;
    }

    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public static readonly EmptyServiceProvider Instance = new();
        public object? GetService(Type serviceType) => null;
    }
}
