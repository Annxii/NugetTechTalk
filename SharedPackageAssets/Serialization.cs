using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirSupport.TechTalk
{
    public class Serialization
    {
        private readonly static JsonSerializerSettings settings;

        static Serialization()
        {
            settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                FloatParseHandling = FloatParseHandling.Double,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
