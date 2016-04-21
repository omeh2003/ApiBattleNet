using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace ApiBattleNet.Items
{
    public abstract class WoWApi
    {
        protected readonly JObject JsonObject;


        protected WoWApi(string json)
        {
            JsonObject = JObject.Parse(json);
        }

        protected WoWApi()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }

        public virtual T GetProperty<T>(string proName)
        {
            if (JsonObject == null)
                return default(T);

            var token = JsonObject.SelectToken(proName);
            return token != null ? token.Value<T>() : default(T);
        }

        public abstract override string ToString();
    }
}
