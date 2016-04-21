using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ApiBattleNet.Items
{
    public class WoWUnitInfo
    {
        #region Fields

        private readonly JObject _jsonObject;

        #endregion

        #region Constructors

        public WoWUnitInfo(string name, int arena2V2, int arena3V3, int arena5V5, int rbg, bool glad, bool arenamaster,
            bool pretendent, bool r21550, bool r31550, bool r32200, int hk,bool doubleagent)
        {
            Name = name;
            Arena2V2 = arena2V2;
            Arena3V3 = arena3V3;
            Arena5V5 = arena5V5;
            Rbg = rbg;
            Glad = glad;
            ArenaMaster = arenamaster;
            Pretendent = pretendent;
            R21550 = r21550;
            R31550 = r31550;
            R32200 = r32200;
            HK = hk;
            DoubleAgent = doubleagent;
        }

        public WoWUnitInfo(string name,string jsonString)
        {
           
            
                _jsonObject = JObject.Parse(jsonString.Normalize());
            
          
           
            Name = name;
            Arena2V2 =
                _jsonObject.SelectToken("pvp")
                    .SelectToken("brackets")
                    .SelectToken("ARENA_BRACKET_2v2")
                    .SelectToken("rating")
                    .Value<int>();
            Arena3V3 =
                _jsonObject.SelectToken("pvp")
                    .SelectToken("brackets")
                    .SelectToken("ARENA_BRACKET_3v3")
                    .SelectToken("rating")
                    .Value<int>();
            Arena5V5 =
                _jsonObject.SelectToken("pvp")
                    .SelectToken("brackets")
                    .SelectToken("ARENA_BRACKET_5v5")
                    .SelectToken("rating")
                    .Value<int>();
            Rbg =
                _jsonObject.SelectToken("pvp")
                    .SelectToken("brackets")
                    .SelectToken("ARENA_BRACKET_RBG")
                    .SelectToken("rating")
                    .Value<int>();
            var achev = _jsonObject.SelectToken("achievements").SelectToken("achievementsCompleted").Values<int>();
            var enumerable = achev as IList<int> ?? achev.ToList();
            Glad = enumerable.Contains(2091);
            ArenaMaster = enumerable.Contains(1174);
            Pretendent = enumerable.Contains(2090);
            R21550 = enumerable.Contains(399);
            R31550 = enumerable.Contains(402);
            R32200 = enumerable.Contains(1160);
            HK = _jsonObject.SelectToken("totalHonorableKills").Value<int>();
            DoubleAgent = enumerable.Contains(7380);
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public int Arena2V2 { get; private set; }
        public int Arena3V3 { get; private set; }
        public int Arena5V5 { get; private set; }
        public int Rbg { get; private set; }
        public bool Glad { get; private set; }
        public bool ArenaMaster { get; private set; }
        public bool Pretendent { get; private set; }
        public bool R21550 { get; private set; }
        public bool R31550 { get; private set; }
        public bool R32200 { get; private set; }
        public int HK { get; private set; }
        public bool DoubleAgent { get; private set; }

        #endregion

        #region Public Methods

        public T GetProperty<T>(string proName)
        {
            if (_jsonObject == null)
                return default(T);

            var token = _jsonObject.SelectToken(proName);
            return token != null ? token.Value<T>() : default(T);
        }

        public override string ToString()
        {
            var st = new StringBuilder();
            st.AppendFormat("{0}. ХК-{2}, Гладиатор-{1}, Аренамастер-{5}, 2vs2-{3}, 3vs3-{4}, 1550 в двойке-{6}, 1550 в тройке-{7}, 2200 в тройке-{8}, Двойнойагент-{9}", Name, Glad, HK, Arena2V2, Arena3V3, ArenaMaster, R21550, R31550, R32200,DoubleAgent);
            return st.ToString();
        }

        #endregion
    }
}