using Newtonsoft.Json.Linq;

namespace ApiBattleNet.Items
{
    public class WoWItemInfo
    {
        #region Fields

        private readonly JObject _jsonObject;

        #endregion

        #region Public Methods

        public T GetProperty<T>(string proName)
        {
            if (_jsonObject == null)
                return default(T);

            var token = _jsonObject.SelectToken(proName);
            return token != null ? token.Value<T>() : default(T);
        }

        #endregion

        #region Constructors

        public WoWItemInfo(int id, string name, string description = null)
        {
            Name = name;
            Id = id;
            Description = description;
        }

        public WoWItemInfo(string jsonString)
        {
            _jsonObject = JObject.Parse(jsonString);
            Id = _jsonObject.SelectToken("id").Value<int>();
            Name = _jsonObject.SelectToken("name").Value<string>();
            Description = _jsonObject.SelectToken("description").Value<string>();
        }

        #endregion

        #region Properties

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        #endregion
    }
}