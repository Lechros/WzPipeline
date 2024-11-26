using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzJson.Skill
{
    internal class Skill
    {
        [JsonProperty(Order = 1)]
        public string name;
        public Dictionary<int, Dictionary<string, int>> level;
    }
}
