using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.DependencyInjection;
using WzPipeline.Application;
using WzPipeline.Core.Pipeline.Runner;
using WzPipeline.Shared;

namespace WzPipeline.Tests;

public class DependencyInjectionTests
{
    private IWzProvider wzProvider;
    private ServiceCollection services;
    private Workflow workflow;
    private string path;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        wzProvider = TestUtils.CreateWzProvider();
    }

    [SetUp]
    public void SetUp()
    {
        services = new ServiceCollection();
        workflow = new Workflow(services, wzProvider);
        services.AddSingleton<IPipelineRunner, TestUtils.NoopPipelineRunner>();
        path = Guid.NewGuid().ToString();
        if (File.Exists(path) || Directory.Exists(path))
        {
            throw new Exception("File already exists");
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }

    [Test]
    public void ExclusiveEquipData()
    {
        workflow.AddExclusiveEquipDataJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void GearData()
    {
        workflow.AddGearDataJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void GearIcon()
    {
        workflow.AddGearIconJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void GearIconOrigin()
    {
        workflow.AddGearIconOriginJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void GearRawIcon()
    {
        workflow.AddGearRawIconJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void GearRawIconOrigin()
    {
        workflow.AddGearRawIconOriginJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void ItemIcon()
    {
        workflow.AddItemIconJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void ItemIconOrigin()
    {
        workflow.AddItemIconOriginJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void ItemRawIcon()
    {
        workflow.AddItemRawIconJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void ItemRawIconOrigin()
    {
        workflow.AddItemRawIconOriginJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void SetItemData()
    {
        workflow.AddSetItemDataJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }

    [Test]
    public void SoulData()
    {
        workflow.AddSoulDataJob(path);
        var serviceProvider = services.BuildServiceProvider();
        workflow.Run(serviceProvider);
    }
}