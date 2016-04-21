using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiBattleNet.Items
{
    public class WoWPets:WoWApi
    {
        public WoWPets(string json) : base(json)
        {
            var jobject = JsonObject;
            SpeciesId = jobject.SelectToken("speciesId").Value<int>();
            PetTypeId = jobject.SelectToken("petTypeId").Value<int>();
            Name = jobject.SelectToken("name").Value<string>();
            Source = jobject.SelectToken("source").Value<string>();
            Ability1Name = jobject["abilities"][0]["name"];
            Ability1Id = jobject["abilities"][0]["id"];
            Ability1Type = jobject["abilities"][0]["petTypeId"];            
            
            Ability2Name = jobject["abilities"][1]["name"];
            Ability2Id = jobject["abilities"][1]["id"];
            Ability2Type = jobject["abilities"][1]["petTypeId"];           
            
            Ability3Name = jobject["abilities"][2]["name"];
            Ability3Id = jobject["abilities"][2]["id"];
            Ability3Type = jobject["abilities"][2]["petTypeId"];            
            
            Ability4Name = jobject["abilities"][3]["name"];
            Ability4Id = jobject["abilities"][3]["id"];
            Ability4Type = jobject["abilities"][3]["petTypeId"];            
            
            Ability5Name = jobject["abilities"][4]["name"];
            Ability5Id = jobject["abilities"][4]["id"];
            Ability5Type = jobject["abilities"][4]["petTypeId"];            
            
            Ability6Name = jobject["abilities"][5]["name"];
            Ability6Id = jobject["abilities"][5]["id"];
            Ability6Type = jobject["abilities"][5]["petTypeId"];           
          
        }

        public JToken Ability6Type { get; set; }

        public JToken Ability6Id { get; set; }

        public JToken Ability6Name { get; set; }

        public JToken Ability5Type { get; set; }

        public JToken Ability5Id { get; set; }

        public JToken Ability5Name { get; set; }

        public JToken Ability4Type { get; set; }

        public JToken Ability4Id { get; set; }

        public JToken Ability4Name { get; set; }

        public JToken Ability3Type { get; set; }

        public JToken Ability3Id { get; set; }

        public JToken Ability3Name { get; set; }

        public JToken Ability2Type { get; set; }

        public JToken Ability2Id { get; set; }

        public JToken Ability2Name { get; set; }

        public JToken Ability1Type { get; set; }

        public JToken Ability1Id { get; set; }

        public JToken Ability1Name { get; set; }

        public string Source { get; set; }

        public WoWPets()
        {
            Name = string.Empty;
            SpeciesId = 0;
            PetTypeId = 0;
        }
        public int PetTypeId { get; set; }

        public int SpeciesId { get; set; }

        public new string Name { get; set; }

        public override string ToString()
        {
            var st = new StringBuilder();
            st.AppendFormat("{0}|{1}\n|{2}\n|{3}\n|{4}|{5}|{6}|{7}|{8}|{9}\n|{10}|{11}|{12}|{13}|{14}|{15}\n|{16}|{17}|{18}|{19}|{20}|{21}\n", Name, SpeciesId, PetTypeId, Source, Ability1Name, Ability2Name, Ability3Name, Ability4Name, Ability5Name, Ability6Name, Ability1Type, Ability2Type, Ability3Type, Ability4Type, Ability5Type, Ability6Type,Ability1Id,Ability2Id,Ability3Id,Ability4Id,Ability5Id,Ability6Id);
            return st.ToString();
        }

        private const string Jsontype = @"{""petTypes"":[{""id"":0,""key"":""humanoid"",""name"":""Гуманоид"",""typeAbilityId"":238,""strongAgainstId"":1,""weakAgainstId"":7},{""id"":1,""key"":""dragonkin"",""name"":""Дракон"",""typeAbilityId"":245,""strongAgainstId"":5,""weakAgainstId"":3},{""id"":2,""key"":""flying"",""name"":""Летающий"",""typeAbilityId"":239,""strongAgainstId"":8,""weakAgainstId"":1},{""id"":3,""key"":""undead"",""name"":""Нежить"",""typeAbilityId"":242,""strongAgainstId"":0,""weakAgainstId"":8},{""id"":4,""key"":""critter"",""name"":""Зверек"",""typeAbilityId"":236,""strongAgainstId"":3,""weakAgainstId"":0},{""id"":5,""key"":""magical"",""name"":""Магический"",""typeAbilityId"":243,""strongAgainstId"":2,""weakAgainstId"":9},{""id"":6,""key"":""elemental"",""name"":""Элементаль"",""typeAbilityId"":241,""strongAgainstId"":9,""weakAgainstId"":4},{""id"":7,""key"":""beast"",""name"":""Животное"",""typeAbilityId"":237,""strongAgainstId"":4,""weakAgainstId"":2},{""id"":8,""key"":""water"",""name"":""Водный"",""typeAbilityId"":240,""strongAgainstId"":6,""weakAgainstId"":5},{""id"":9,""key"":""mechanical"",""name"":""Механизм"",""typeAbilityId"":244,""strongAgainstId"":7,""weakAgainstId"":6}]}";

        private enum TypeEnum
        {
            Humanoid,
            Dragonkin,
            Flying,
            Undead,
            Critter,
            Magical,
            Elemental,
            Beast,
            Water,
            Mechanical

        }

        public class TypePets
        {
            public int Id;
            public string Key;

            [JsonProperty(PropertyName = "name")]

            public string Name;
            public int TypeAbility;
            public int Strong;
            public int Weak;
            // ReSharper disable once CollectionNeverUpdated.Global
            public  TypePets[] ListPets;
            public TypePets()
            {
                ListPets = new TypePets[10];
                var red = new JsonTextReader(new StringReader(Jsontype));
                var list =(JObject) new JsonSerializer().Deserialize(red);
             
             
               
                                
                //for (var i = 0; i < 10; i++)
                //{
                //    List[i].Id = (int) list["petTypes"][0]["id"][i].Value<int>();
                //    List[i].Key = (string)list["petTypes"][0]["key"];
                //    List[i].Name = (string)list["petTypes"][0]["name"];
                //    List[i].TypeAbility = (int)list["petTypes"][0]["typeAbilityId"];
                //    List[i].Strong = (int)list["petTypes"][0]["strongAgainstId"];
                //    List[i].Weak = (int)list["petTypes"][0]["weakAgainstId"];
                //}
            }

            public override string ToString()
            {
                 var st = new StringBuilder();
                foreach (var type in ListPets )
                {
                    st.AppendLine((TypeEnum)type.Id + type.Key + type.Name + type.Strong + type.Weak+type.TypeAbility);
                }
                return st.ToString();
            }
        }
    }
}
