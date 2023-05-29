using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CandyMatch3.Game.Common
{
    /// <summary>
    /// The available limit types.
    /// </summary>
    public enum LimitType
    {
        Moves =0,
        Time
    }
    public class Level
    {
        public int id ;

        public int width ;
        public int height ;
        public List<LevelTile> tiles = new List<LevelTile>();
        public List<int> idBackground = new List<int>();

        public LimitType limitType;
        public int limit;
        public int limit_booster;

        //public List<Goal> goals = new List<Goal>();
        public List<Person> goals = new List<Person>();
        public List<PersonVip> customerVip = new List<PersonVip>();
        //public List<CandyColor> availableColors = new List<CandyColor>();
        public List<AvailableColor> availableColors = new List<AvailableColor>();
        public List<FailingHole> falling_hole = new List<FailingHole>();

        public int score1;
        public int score2;
        public int score3;

        public bool awardSpecialCandies;
        public AwardedSpecialCandyType awardedSpecialCandyType;

        public int collectableChance;

        //public Dictionary<BoosterType, bool> availableBoosters = new Dictionary<BoosterType, bool>();
        public Dictionary<BoosterType, List<BoosterResource>> availableBoosters = new Dictionary<BoosterType, List<BoosterResource>>();
        public Dictionary<BoosterType, BoosterLimitAndGift> availableBoosters_v2 = new Dictionary<BoosterType, BoosterLimitAndGift>();


        public void checkDataOldMap(string textJsonLoadFile)
        {
            string replaceCandyMatch3 = textJsonLoadFile.Replace("GameVanilla", "CandyMatch3");
            var jsonData = MiniJSON.Json.Deserialize(replaceCandyMatch3) as Dictionary<string, object>;
            id = int.Parse(jsonData["id"].ToString());
            width = int.Parse(jsonData["width"].ToString());
            height = int.Parse(jsonData["height"].ToString());
            List<object> tilesTemp = jsonData.GetList("tiles");
            foreach (var data in tilesTemp)
            {
                var serializer = new fsSerializer();
                LevelTile levelTile = FileUtils.ParseJsonFile<LevelTile>(serializer, MiniJSON.Json.Serialize(data));
                tiles.Add(levelTile);
            }    

            List<object> idBackGroundTEMP = jsonData.GetList("idBackground");
            if (idBackGroundTEMP != null)
            {
                foreach (var idBackGroundItem in idBackGroundTEMP)
                {
                    int id = idBackGroundItem.ToInt();
                    idBackground.Add(id);
                }
            }
            else idBackground = new List<int>();

            limitType = (LimitType)Enum.Parse(typeof(LimitType), jsonData["limitType"].ToString());
            limit = int.Parse(jsonData["limit"].ToString());
            if (jsonData.ContainsKey("limit_booster"))
            {
                limit_booster = int.Parse(jsonData["limit_booster"].ToString());
            }
            else limit_booster = 2;


            var jsonGoal = jsonData.GetList("goals");
            foreach(var goalItem in jsonGoal)
            {
                var goalItemTemp = goalItem as Dictionary<string, object>;
                if (goalItemTemp.ContainsKey("character"))
                {
                    Person person = new Person();
                    var character = goalItemTemp.GetDictionary("character");
                    foreach (var key in character.Keys)
                    {
                        person.character.Add(key, int.Parse(character[key].ToString()));
                    }

                    var gift = goalItemTemp.GetList("gift");
                    foreach(var giftItem in gift)
                    {
                        var serializer = new fsSerializer();
                        BoosterResource boosterResource = FileUtils.ParseJsonFile<BoosterResource>(serializer, MiniJSON.Json.Serialize(giftItem));
                        person.gift.Add(boosterResource);
                    }

                    var require = goalItemTemp.GetList("require");
                    foreach (var requireItem in require)
                    {
                        var serializer = new fsSerializer();
                        Goal goal = FileUtils.ParseJsonFile<Goal>(serializer, MiniJSON.Json.Serialize(requireItem));
                        person.require.Add(goal);
                    }
                    var skin = goalItemTemp.GetInt("skin");
                    person.skin = skin;
                    goals.Add(person);
                }
                else
                {
                    goals = new List<Person>();
                    break;
                }    
            }

            var jsonCustomerVip = jsonData.GetList("customerVip");
            foreach (var CustomerVipItem in jsonCustomerVip)
            {
                var customerVipTemp = CustomerVipItem as Dictionary<string, object>;
                if (customerVipTemp.ContainsKey("character"))
                {
                    PersonVip personVip = new PersonVip();
                    var character = customerVipTemp.GetDictionary("character");
                    foreach (var key in character.Keys)
                    {
                        personVip.character.Add(key, int.Parse(character[key].ToString()));
                    }

                    var gift = customerVipTemp.GetList("gift");
                    foreach (var giftItem in gift)
                    {
                        var serializer = new fsSerializer();
                        BoosterResource boosterResource = FileUtils.ParseJsonFile<BoosterResource>(serializer, MiniJSON.Json.Serialize(giftItem));
                        personVip.gift.Add(boosterResource);
                    }

                    var ratio = customerVipTemp.GetInt("ratio");
                    personVip.ratio = ratio;

                    var turn = customerVipTemp.GetInt("turn");
                    personVip.turn = turn;

                    var times = customerVipTemp.GetList("times");
                    List<int> Timestemp = new List<int>();
                    for(int k = 0; k<times.Count; k++)
                    {
                        int temp = times[k].ToInt();
                        Timestemp.Add(temp);
                    }    

                    personVip.times = Timestemp;

                    var skin = customerVipTemp.GetInt("skin");
                    personVip.turn = skin;

                    /*                    var require = customerVipTemp.GetList("random_require");
                                        foreach (var requireItem in require)
                                        {
                                            var serializer = new fsSerializer();
                                            Goal goal = FileUtils.ParseJsonFile<Goal>(serializer, MiniJSON.Json.Serialize(requireItem));
                                            personVip.random_required.Add(goal);
                                        }*/
                    customerVip.Add(personVip);
                }
                else
                {
                    goals = new List<Person>();
                    break;
                }
            }



            var jsonAvailableColors = jsonData.GetList("availableColors");
            if (jsonAvailableColors != null)
            {
                foreach (var colorItem in jsonAvailableColors)
                {
                    var colorItemTemp = colorItem as Dictionary<string, object>;
                    if(colorItemTemp == null)
                    {
                        availableColors = new List<AvailableColor>();
                        break;                         
                    }    

                    var serializer = new fsSerializer();
                    AvailableColor color = FileUtils.ParseJsonFile<AvailableColor>(serializer, MiniJSON.Json.Serialize(colorItem));
                    availableColors.Add(color);
                }
            }

            var jsonFailingHole = jsonData.GetList("falling_hole");
            if (jsonFailingHole != null)
            {
                foreach (var fallingHoleItem in jsonFailingHole)
                {
                    var failingHoleItemTemp = fallingHoleItem as Dictionary<string, object>;
                    if (failingHoleItemTemp == null)
                    {
                        falling_hole = new List<FailingHole>();
                        break;
                    }

                    var serializer = new fsSerializer();
                    FailingHole fallingHole = FileUtils.ParseJsonFile<FailingHole>(serializer, MiniJSON.Json.Serialize(fallingHoleItem));
                    falling_hole.Add(fallingHole);
                }
            }
            else falling_hole = new List<FailingHole>();
            
            score1 = int.Parse(jsonData["score1"].ToString());
            score2 = int.Parse(jsonData["score2"].ToString());
            score3 = int.Parse(jsonData["score3"].ToString());

            awardSpecialCandies = jsonData.GetBool("awardSpecialCandies");
            awardedSpecialCandyType = (AwardedSpecialCandyType)Enum.Parse(typeof(AwardedSpecialCandyType), jsonData["awardedSpecialCandyType"].ToString());
            collectableChance = int.Parse(jsonData["collectableChance"].ToString());

            var jsonBooster = jsonData.GetDictionary("availableBoosters");
            foreach(var key in jsonBooster.Keys)
            {
                if (key == "Bomb") continue; // bo truong hop bomb
                var boosterItem = jsonBooster.GetList(key);
                List<BoosterResource> resourceList = new List<BoosterResource>();
                if (boosterItem != null)
                {                  
                    foreach (var resourceItem in boosterItem)
                    {
                        var serializer = new fsSerializer();
                        BoosterResource boosterResource = FileUtils.ParseJsonFile<BoosterResource>(serializer, MiniJSON.Json.Serialize(resourceItem));
                        resourceList.Add(boosterResource);
                    }
                }
                else
                {
                    availableBoosters = new Dictionary<BoosterType, List<BoosterResource>>();
                    break;
                }
                availableBoosters.Add((BoosterType)Enum.Parse(typeof(BoosterType), key), resourceList);
            } 
            
            var jsonAvailableBooster2 = jsonData.GetDictionary("availableBoosters_v2");
            foreach(var key in jsonAvailableBooster2.Keys)
            {
                var boosterV2Item = jsonAvailableBooster2[key];
                BoosterLimitAndGift boosterLimitAndGift = new BoosterLimitAndGift();
                if (boosterV2Item != null)
                {
                    var serializer = new fsSerializer();
                    boosterLimitAndGift = FileUtils.ParseJsonFile<BoosterLimitAndGift>(serializer, MiniJSON.Json.Serialize(boosterV2Item));
                }  
                else
                {
                    availableBoosters_v2 = new Dictionary<BoosterType, BoosterLimitAndGift>();
                }
                availableBoosters_v2.Add((BoosterType)Enum.Parse(typeof(BoosterType), key), boosterLimitAndGift);


            }    
        }

    }
}
