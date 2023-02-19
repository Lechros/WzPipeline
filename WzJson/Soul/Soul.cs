using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzJson.Soul
{
    internal class Soul
    {
        public string name = "";

        public string skill = "";

        public int multiplier;

        public bool magnificent = false;

        public Dictionary<string, int>? option;

        public Dictionary<string, Dictionary<string, int>>? options;

        public bool ShouldSerializemagnificent() => magnificent;

        public bool ShouldSerializeoption() => option != null && option.Count > 0;

        public bool ShouldSerializeoptions() => options != null && options.Count > 0;
    }
}
