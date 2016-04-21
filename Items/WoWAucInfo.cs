using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ApiBattleNet.Items
{
    public class WoWAucInfo
    {
        #region Fields
        private readonly JObject _jsonObject;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var st = new StringBuilder();
            if (PetBreedId!=0)
            {
                st.AppendFormat("Owner: {0}-{4}. ItemID: {1} Buy: {2} ID: {3} BidCount: {5} Quantity: {6} PetBreed: {7} SpeciesID: {8} Level: {9} Quality: {10}", Owner, ItemId, Buy, Id, OwnerRealm, BidCount, Quant,PetBreedId,PetSpeciesID,PetLevel,PetQualityId);
                return st.ToString();
                
            }
            if (BonusListId != 0)
            {
                st.AppendFormat("Owner: {0}-{4}. ItemID: {1} BonusList: {7} Buy: {2} ID: {3} BidCount: {5} Quantity: {6}", Owner, ItemId, Buy, Id, OwnerRealm, BidCount, Quant,BonusListId);
                return st.ToString();
                
            }
           st.AppendFormat("Owner: {0}-{4}. ItemID: {1} Buy: {2} ID: {3} BidCount: {5} Quantity: {6}", Owner, ItemId, Buy, Id, OwnerRealm, BidCount, Quant);
            return st.ToString();
        }

        public T GetProperty<T>(string proName)
        {
            if (_jsonObject == null)
                return default(T);

            var token = _jsonObject.SelectToken(proName);
            return token != null ? token.Value<T>() : default(T);
        }
        #endregion

        #region Constructors

        public WoWAucInfo(int id, int itemid, string owner, string ownerreal, Int64 bid, Int64 buyout, int quant)
        {
            
            Id = id;
           ItemId=itemid;
            Owner = owner;
            OwnerRealm = ownerreal;
            BidCount = bid;
            Buy = buyout;
            Quant = quant;

        }

        public WoWAucInfo()
        {
            Id = 0;
            ItemId = 0;
            BonusListId = 0;
            Buy = 0;
            PetSpeciesID = 0;
            PetBreedId = 0;
            PetQualityId = 0;
            PetLevel = 0;
            Quant = 0;
            Context = 0;
            Seed = 0;
            Rand = 0;
            TimeLeft = string.Empty;
            BidCount = 0;
            Owner = string.Empty;
            OwnerRealm = string.Empty;
        }
        public WoWAucInfo(string jsonString)
        {
            _jsonObject = JObject.Parse(jsonString);
            if (_jsonObject != null)
            {
                Id = _jsonObject.SelectToken("auc").Value<int>();
                ItemId = _jsonObject.SelectToken("item").Value<int>();
                Owner = _jsonObject.SelectToken("owner").Value<string>();
                OwnerRealm = _jsonObject.SelectToken("ownerRealm").Value<string>();
                BidCount = _jsonObject.SelectToken("bid").Value<Int64>();
                Buy = _jsonObject.SelectToken("buyout").Value<Int64>();
                Quant = _jsonObject.SelectToken("quantity").Value<int>();
                TimeLeft = _jsonObject.SelectToken("timeLeft").Value<string>();
                Rand = _jsonObject.SelectToken("rand").Value<int>();
                Seed = _jsonObject.SelectToken("seed").Value<Int64>();
                Context = _jsonObject.SelectToken("context").Value<int>();
                PetBreedId = 0;
                PetSpeciesID = 0;
                PetLevel = 0;
                PetQualityId = 0;
                BonusListId = 0;
               // var tok = _jsonObject.SelectToken("modifiers");
                if ( _jsonObject.SelectToken("petBreedId")!=null)
                {
                    var token = _jsonObject.SelectToken("petBreedId");
                    if (token != null) PetBreedId = token.Value<int>();
                    PetSpeciesID = _jsonObject.SelectToken("petSpeciesId").Value<int>();
                    PetLevel = _jsonObject.SelectToken("petLevel").Value<int>();
                    PetQualityId = _jsonObject.SelectToken("petQualityId").Value<int>();


                }
                var jToken = _jsonObject["bonusLists"];
                if (jToken != null)
                {
                    var last = jToken[0]["bonusListId"];
           
                    if (last != null )
                    {
                        BonusListId = last.Value<int>();
                    }
                }
            }
            //var jToken = _jsonObject["modifiers"][0]["type"];
            //if (jToken != null) TypePet = (int)jToken;
        }

      
        #endregion

        #region Properties

        public int Id { get; set; }
        public int ItemId { get; set; }
        public int BonusListId { get; set; }

        public Int64 Buy { get; set; }
        public int PetSpeciesID { get; set; }
        public int PetBreedId { get; set; }
        public int PetQualityId { get; set; }

        public int PetLevel { get; set; }

        //  public int TypePet { get; set; }

        public int Quant { get; set; }
        public int Context { get; set; }

        public long Seed { get; set; }

        public int Rand { get; set; }

        public string TimeLeft { get; set; }

        public Int64 BidCount { get; set; }

        public string Owner { get; set; }
        public string OwnerRealm { get; set; }

        #endregion

    }
}
