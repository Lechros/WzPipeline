namespace WzJson.V2.Domains.Gear.Models;

public class GearPotential
{
    public required int Id { get; set; }

    public required int Grade { get; set; }

    public required string Summary { get; set; }

    public GearOption Option { get; set; } = new();
}