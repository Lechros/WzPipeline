using WzJson.Core.Stereotype;

namespace WzJson.Domains.SetItem;

public interface ISetItemNode : INode
{
    public string Name { get; }
    public IEnumerable<int> ItemIds { get; }
    public IEnumerable<IEffectNode> Effects { get; }
    public bool JokerPossible { get; }
    public bool ZeroWeaponJokerPossible { get; }

    public interface IEffectNode
    {
        public int Index { get; }
        public IEnumerable<(string Type, int value)> Properties { get; }
        public IEnumerable<(int OptionCode, int Level)> Options { get; }
    }
}