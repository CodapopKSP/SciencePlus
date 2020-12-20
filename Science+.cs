using System.Collections.Generic;
using UnityEngine;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]
    //[KSPScenario(ScenarioCreationOptions.AddToNewGames, GameScenes.SPACECENTER | GameScenes.FLIGHT)]
    //public class SciencePlus : ScenarioModule
    public class SciencePlus : MonoBehaviour
    {
        //void SaveScience(ConfigNode node)

        if (File.Exists(path))
        //{
        //    Debug.Log("[--------SCIENCE + --------]: SAVE");
        //    if (node.HasNode("SCIENCE+"))
        //    {
        //        Debug.Log("[--------SCIENCE + --------]: IF");
        //        node = node.GetNode("SCIENCE+");
        //        foreach (ScienceType scienceType in allScienceColors)
        //        {
        //            if (scienceType.scienceCache > 0)
        //            {
        //                float data;
        //                Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " cache: " + scienceType.scienceCache);
        //                data = float.Parse(node.GetValue(scienceType.scienceName));
        //                Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " old: " + data);
        //                float newScienceValue = data + scienceType.scienceCache;
        //                node.SetValue(scienceType.scienceName, newScienceValue);
        //                Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " new: " + newScienceValue);
        //                scienceType.scienceCache = 0;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("[--------SCIENCE + --------]: ELSE");
        //        node.AddNode("SCIENCE+");
        //        node = node.GetNode("SCIENCE+");
        //        foreach (ScienceType scienceType in allScienceColors)
        //        {
        //            if (!(node.HasValue(scienceType.scienceName)))
        //            {
        //                node.AddValue(scienceType.scienceName, "0");
        //                Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " NODE ADDED");

        //            }
        //        }
        //    }
        //}

        private void Start()
        {
            GameEvents.OnScienceRecieved.Add(ScienceProcessingCallback);
            //GameEvents.onGameStateSave.Add(SaveScience);
        }

        private void OnDestroy()
        {
            GameEvents.OnScienceRecieved.Remove(ScienceProcessingCallback);
            //GameEvents.onGameStateSave.Remove(SaveScience);
        }

        void ScienceProcessingCallback(float sciValue, ScienceSubject sub, ProtoVessel pv, bool test)
        {
            foreach (ScienceType scienceType in allScienceColors)
            {
                foreach (string body in scienceType.bodyList)
                {
                    if (sub.title.Contains(body))
                    {
                        float newTotal = scienceType.scienceCache + sub.science;
                        scienceType.scienceCache = newTotal;
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " add: " + sub.science);
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " cache: " + scienceType.scienceCache);
                    }
                }
            }
        }

        public class ScienceType
        {
            public ScienceType(string color, List<string> bodyList, float scienceBank = 0, float scienceCache = 0)
            {
                this.color = color;
                this.scienceName = color + "Science";
                this.bodyList = bodyList;
                this.scienceBank = scienceBank;
                this.scienceCache = scienceCache;
            }
            public string color;
            public string scienceName;
            public List<string> bodyList;
            public float scienceBank;
            public float scienceCache;
        }

        List<ScienceType> allScienceColors = new List<ScienceType>()
        {
            new ScienceType("Red",    new List<string>() { "Moho",    "Duna"            }),
            new ScienceType("Orange", new List<string>() { "Dres",    "Vall"            }),
            new ScienceType("Yellow", new List<string>() { "Mun",     "Pol"             }),
            new ScienceType("Green",  new List<string>() { "Minmus",  "Ike"             }),
            new ScienceType("Blue",   new List<string>() { "Kerbin",  "Eeloo"           }),
            new ScienceType("Purple", new List<string>() { "Eve",     "Bop"             }),
            new ScienceType("Gold",   new List<string>() { "Kerbol",  "Jool",   "Tylo"  }),
            new ScienceType("Silver", new List<string>() { "Gilly",   "Laythe"          })
        };
    }
}