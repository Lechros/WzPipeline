using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WzJson.SetItem
{
    internal class SetItem
    {
        [JsonPropertyOrder(1)]
        public string name = "";
        [JsonPropertyOrder(2)]
        public List<int> itemIDs = new();
        [JsonPropertyOrder(3)]
        public SortedDictionary<string, Dictionary<string, int>> effects = new();
        [JsonPropertyOrder(4)]
        public bool jokerPossible;
        [JsonPropertyOrder(5)]
        public bool zeroWeaponJokerPossible;

        public bool ShouldSerializename() => !string.IsNullOrEmpty(name);
        public bool ShouldSerializeitems() => itemIDs.Count > 0;
        public bool ShouldSerializeeffect() => effects.Count > 0;
        public bool ShouldSerializejokerPossible() => jokerPossible != false;
        public bool ShouldSerializezeroWeaponJokerPossible() => zeroWeaponJokerPossible != false;
    }
}
