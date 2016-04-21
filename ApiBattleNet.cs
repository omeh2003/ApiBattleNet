/*
 * Simple Interface for the WOW API RESTful Service
 * By: F Vicaria
 * Updated: 02/11/2014
 * Documentation:
 * http://blizzard.github.io/api-wow-docs/
 * 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ApiBattleNet.Items;
using Newtonsoft.Json.Linq;

namespace ApiBattleNet
{
    public delegate void ItemResponseHandler(WoWItemInfo item);
    public delegate void UserResponseHandler(WoWUnitInfo unit);
    public delegate void AucResponseHandler(List<WoWAucInfo> unit);
    public delegate void PetResponseHandler(WoWPets item);


    public static class ApiBattleNet
    {
        private static Regions _region;
        private static Locales _locale;

        static ApiBattleNet() 
        {
            BaseUrl = @"http://eu.battle.net/api/wow/";
            if (!Directory.Exists("DisplayId")) Directory.CreateDirectory("DisplayId");
            if (!Directory.Exists("UnitInfo")) Directory.CreateDirectory("UnitInfo");
            if (!Directory.Exists("AucInfo")) Directory.CreateDirectory("AucInfo");
            if (!Directory.Exists("PetsInfo")) Directory.CreateDirectory("PetsInfo");
            _region = Regions.Europe;
            _locale = Locales.ru_RU;
        }

        public static Regions Region
        {
            get { return _region; }
            set
            {
                if (value == _region)
                    return;

                _region = value;
                _locale = Locales.NONE;
                switch (value)
                {
                    case Regions.US:
                        BaseUrl = @"http://us.battle.net/api/wow/";
                        break;
                    case Regions.Europe:
                        BaseUrl = @"http://eu.battle.net/api/wow/";
                        break;
                    case Regions.Korea:
                        BaseUrl = @"http://kr.battle.net/api/wow/";
                        break;
                    case Regions.Taiwan:
                        BaseUrl = @"http://tw.battle.net/api/wow/";
                        break;
                    case Regions.China:
                        BaseUrl = @"http://www.battlenet.com.cn/api/wow/";
                        break;
                    default:
                        BaseUrl = @"http://eu.battle.net/api/wow/";
                        break;
                }

            }
        }
        public static Locales Locale
        {
            get { return _locale; }
            set
            {
                if (value == _locale)
                    return;

                _locale = value;
                switch (value)
                {
                    case Locales.en_US:
                    case Locales.es_MX:
                    case Locales.pt_BR:
                        _region = Regions.US;
                        BaseUrl = @"http://us.battle.net/api/wow/";
                        break;
                    case Locales.en_GB:
                    case Locales.es_ES:
                    case Locales.fr_FR:
                    case Locales.ru_RU:
                    case Locales.de_DE:
                    case Locales.pt_PT:
                    case Locales.it_IT:
                        _region = Regions.Europe;
                        BaseUrl = @"http://eu.battle.net/api/wow/";
                        break;
                    case Locales.ko_KR:
                        _region = Regions.Korea;
                        BaseUrl = @"http://kr.battle.net/api/wow/";
                        break;
                    case Locales.zh_TW:
                        _region = Regions.Taiwan;
                        BaseUrl = @"http://tw.battle.net/api/wow/";
                        break;
                    case Locales.zh_CN:
                        _region = Regions.China;
                        BaseUrl = @"http://www.battlenet.com.cn/api/wow/";
                        break;
                    default:
                        _region = Regions.Europe;
                        BaseUrl = @"http://eu.battle.net/api/wow/";
                        break;
                }
            }
        }
        public static string BaseUrl { get; private set; }

        public static async void GetItemInfoFromId(int displayId, ItemResponseHandler handler)
        {
            if (handler == null)
                return;
            var fileitem = Path.Combine(@"DisplayId\", displayId + @".xml");
            if (File.Exists(fileitem))
            {
                var jsonString = ReadFileInString(fileitem);
                
                var info = new WoWItemInfo(jsonString);

                handler(info);
                return;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("item/" + displayId);

                    if (!response.IsSuccessStatusCode)
                    {
                        handler(null);
                        return;
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                    Save(fileitem, jsonString);
                    var info = new WoWItemInfo(jsonString);

                    handler(info);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static async void GetUnitInfoFromNameandRealm(string name,string realm, UserResponseHandler handler)
        {
            if (handler == null)
                return;
            realm = Path.GetInvalidFileNameChars().Aggregate(realm, (current, nameChar) => current.Replace(nameChar, '_'));
            var fileitem = Path.Combine(@"UnitInfo\", name +@"_"+realm+ @".xml");
            if (File.Exists(fileitem))
            {
                var jsonString = ReadFileInString(fileitem);
                
                var info = new WoWUnitInfo(name,jsonString);

                handler(info);
                return;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("character/" + realm + "/" + name + "?fields=pvp,achievements");
                  
                    if (!response.IsSuccessStatusCode)
                    {
                        handler(null);
                        return;
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                    Save(fileitem, jsonString);
                    var info = new WoWUnitInfo(name,jsonString);

                    handler(info);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async void GetAucInfo(AucResponseHandler handler,string realm)
        {
           // GetDataFromFile(handler);
            if (handler == null)
                return;
           
            var fileitem = Path.Combine(@"AucInfo\", realm+".json");
            var filelast = Path.Combine(@"AucInfo\", realm+".last");
           
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("/api/wow/auction/data/" + realm);

                    if (!response.IsSuccessStatusCode)
                    {
                        handler(null);
                        return;
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                   var js=JObject.Parse(jsonString);
                    string oldlast = null;
                   if (File.Exists(filelast))   oldlast = File.ReadAllText(filelast).ToString(CultureInfo.InvariantCulture);
                    var url = js["files"][0]["url"];
                    var last = js["files"][0]["lastModified"];
                    File.WriteAllText(filelast, last.ToString());
                  
                   if(last.ToString()!=oldlast)
                   {
                                         
                       var res =  await client.GetAsync(url.ToString());

                       var jsonString1 =  await res.Content.ReadAsStringAsync();
                      
                   
                       Save(fileitem, jsonString1);
                      




                   }
                  
                   GetDataFromFile(handler, realm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }

        public static void GetDataFromFile(AucResponseHandler handler, string realm)
        {
            if (handler == null)
                return;
            var fileitem = Path.Combine(@"AucInfo\", realm + ".json");
            if (!File.Exists(fileitem))
            {
                var ex = new Exception();

                var f = new FileNotFoundException();
                return;
            }
            //var jsarr = ReadFileInString(fileitem);

            //JsonTextReader reader = new JsonTextReader(new StringReader(jsarr)) {SupportMultipleContent = true};
            //var st = "";
            //    while (reader.Read())
            //    {
            //        if (reader.Value != null) st += reader.Value+" ";
            //        // Console.WriteLine("Token: {0}, Value: {1} Path {2}", reader.TokenType, reader.Value,reader.Path);
            //        else
            //            //Console.WriteLine("Token: {0}", reader.TokenType);
            //            if (reader.TokenType.ToString() == "StartObject" )
            //            {
            //                handler(st);
            //                st = "";
            //            }

            //        //  var a = reader.Value;
            //        //  handler(new WoWAucInfo(a));
            //    }

            var lists=new List<WoWAucInfo>();
            using ( var stream = new FileStream(fileitem, FileMode.Open))
            {


               
                var str = new StreamReader(stream);

                string f;
                
                while ((f = str.ReadLine()) != null)

                {
                    var s = f;
                    var t = s.Trim();
                    if (t.LastOrDefault() == ',') t = t.Remove(t.LastIndexOf(','));
                    var r = new Regex(@"\[");
                    var open = r.Matches(t).Count;

                    r = new Regex(@"\]");
                    var close = r.Matches(t).Count;
                    if (open != close)
                    {
                        t = t.Replace("]", "");
                        t = t.Remove(t.Length - 1);
                    }
                    if (t.FirstOrDefault() == '{' && t.LastOrDefault() == '}') lists.Add(new WoWAucInfo(t));
                    

                }
            }
            handler(lists);
        }


        public static async void GetPetInfoFromSpeciesId(int specId, PetResponseHandler handler)
        {
            if (handler == null)
                return;
            ClearBase(@"PetsInfo\");
            var fileitem = Path.Combine(@"PetsInfo\", specId + @".xml");
            if (File.Exists(fileitem))
            {
                var jsonString = ReadFileInString(fileitem);

                var info = new WoWPets(jsonString);

                handler(info);
                return;
            }
         
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("battlePet/species/" + specId + @"?locale=ru_RU");

                    if (!response.IsSuccessStatusCode)
                    {
                        handler(null);
                        return;
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                    Save(fileitem, jsonString);
                    var info = new WoWPets(jsonString);

                    handler(info);
                }
          

        }


        public static void GetTest(AucResponseHandler hand)
        {

            var str = @"{""auc"":1117916020,""item"":82800,""owner"":""»рника"",""ownerRealm"":""ясеневыйлес"",""bid"":61740500,""buyout"":64990000,""quantity"":1,""timeLeft"":""MEDIUM"",""rand"":0,""seed"":1984317312,""context"":0,""modifiers"":[{""type"":3,""value"":1332},{""type"":4,""value"":50331663},{""type"":5,""value"":25}],""petSpeciesId"":1332,""petBreedId"":15,""petLevel"":25,""petQualityId"":3}";
            var auc = new WoWAucInfo(str);
          //  hand(auc);

        }

        private static void Save(string filein, string listin)
        {
            var file = filein;
            var list = listin;

            using (var f = File.CreateText(file))
            {
                var writer = new XmlSerializer(list.GetType());

                writer.Serialize(f, list);
            }
        }

        public static int count;
        private static string ReadFileInString(string filein)
        {
            var file = filein;

            var l = string.Empty;
            if (file == null) return l;
            using (var f = new StreamReader(file))
            {
                var reader = new XmlSerializer(l.GetType());
                count++; 
                l = (string)reader.Deserialize(f);
            }

            return l;
        }

        public static void ClearBase(string unitinfo)
        {
           
            var list = new DirectoryInfo(unitinfo).GetFiles().Select(file => file.FullName).ToList(); 
           
            var listdel = new List<string>();
       
            foreach (var file in list)
            {
                using (var f = File.OpenText(file))
                {
                    string s;
                    while ((s = f.ReadLine()) != null)
                    {
                        if (s.Contains("DOCTYPE"))
                        {
                            listdel.Add(file);
                      
                        }
               
                    }
                }
            }

            if (!listdel.Any()) return;
            foreach (var file in listdel)
            {
                File.Delete(file);
            }
        }
    }
}