using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WzJson.ItemOption
{
    public class ItemOption
    {
        public ItemOption()
        {
            level = new JObject();
            for (int i = 1; i <= 25; i++)
            {
                level.Add(i.ToString(), new JObject());
            }
        }

        [JsonProperty(Order = 1)] public int optionType;
        [JsonProperty(Order = 2)] public int reqLevel;

        [JsonProperty(Order = 3, PropertyName = "string")]
        public string? @string;

        [JsonProperty(Order = 4)] public JObject level;

        public void addOption(string level, string option, JToken? value)
        {
            (this.level.GetValue(level) as JObject)!.Add(option, value);
        }

        public bool ShouldSerializeoptionType() => optionType > 0;
        public bool ShouldSerializereqLevel() => reqLevel > 0;
    }
}