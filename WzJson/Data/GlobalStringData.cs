using WzJson.Common;
using WzJson.Common.Data;

namespace WzJson.Data;

public class GlobalStringData : IData
{
    public required WzStringData Consume { get; init; }
    public required WzStringData Eqp { get; init; }
    public required WzStringData Skill { get; init; }
}