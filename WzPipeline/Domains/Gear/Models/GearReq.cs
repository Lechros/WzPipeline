namespace WzPipeline.Domains.Gear.Models;

public class GearReq
{
    public int Level { get; set; }
    public required GearReqJob Job { get; set; }
    public int? Gender { get; set; }
}

public class GearReqJob
{
    public int Class { get; set; }
    public int[]? Jobs { get; set; }
    public int[]? FullJobs { get; set; }
}