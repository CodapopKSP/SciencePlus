using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SciencePlus
{
    //[KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]
    [KSPScenario(ScenarioCreationOptions.AddToNewGames, GameScenes.SPACECENTER | GameScenes.FLIGHT)]
    //public class SciencePlus : ScenarioModule
    public class SciencePlus : MonoBehaviour
    {
        void SaveScience(ConfigNode node)
        {
            string name = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.sfs";
            if (File.Exists(name))
            {
                Debug.Log("[--------SCIENCE + --------]: HASFILE");
                node = ConfigNode.Load(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.sfs");
            }
            else
            {
                Debug.Log("[--------SCIENCE + --------]: NOT HASFILE");
                node = ConfigNode.Load(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs");
                node = node.GetNode("GAME");
                node.AddNode("SCIENCE+");
                node = node.GetNode("SCIENCE+");
                Debug.Log("[--------SCIENCE + --------]: Start Colors");
                foreach (ScienceType scienceType in allScienceColors)
                {
                    node.AddValue(scienceType.scienceName, "0");
                }
                Debug.Log("[--------SCIENCE + --------]: End Colors");
                node.Save(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.sfs");
                Debug.Log("[--------SCIENCE + --------]: Saved");
            }

            Debug.Log("[--------SCIENCE + --------]: " + node);
            Debug.Log("[--------SCIENCE + --------]: SAVE");

            foreach (ScienceType scienceType in allScienceColors)
            {
                Debug.Log("[--------SCIENCE + --------]: " + scienceType.color + "type");
                if (scienceType.scienceCache > 0)
                {
                    float data;
                    Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " cache: " + scienceType.scienceCache);
                    data = float.Parse(node.GetValue(scienceType.scienceName));
                    Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " old: " + data);
                    float newScienceValue = data + scienceType.scienceCache;
                    node.SetValue(scienceType.scienceName, newScienceValue);
                    Debug.Log("[--------SCIENCE + --------]: " + scienceType.scienceName + " new: " + newScienceValue);
                    scienceType.scienceCache = 0;
                }
                node.Save(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.sfs");
            }
        }

        private void Start()
        {
            GameEvents.OnScienceRecieved.Add(ScienceProcessingCallback);
            GameEvents.onGameStateSave.Add(SaveScience);
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
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " add to cache: " + sub.science);
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