using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, true)]
    
    public class ScienceCounter : MonoBehaviour
    {
        void SaveScience(ConfigNode node)
        {
            foreach (ScienceType scienceType in allScienceColors)
            {
                Debug.Log("[--------SCIENCE+--------1]: " + scienceType.scienceName + " " + scienceType.scienceBank + " " + scienceType.scienceCache);
            }
            ConfigNode node2 = node.GetNode("SCIENCE+");
            bool flag = node2 == null;
            if (flag)
            {
                node.AddNode("SCIENCE+");
                node2 = node.GetNode("SCIENCE+");
                foreach (ScienceType scienceType in allScienceColors)
                {
                    node2.AddNode(scienceType.scienceName);
                    ConfigNode node3 = node2.GetNode(scienceType.scienceName);
                    node3.AddValue("SCI", 0);
                }

            }
            foreach (ScienceType scienceType in allScienceColors)
            {
                Debug.Log("[--------SCIENCE+--------2]: " + scienceType.scienceName + " " + scienceType.scienceBank + " " + scienceType.scienceCache);
            }

            foreach (ScienceType scienceType in allScienceColors)
            {
                ConfigNode node3 = node2.GetNode(scienceType.scienceName);
                node3.SetValue("SCI", scienceType.scienceBank + scienceType.scienceCache);
                Debug.Log("[--------SCIENCE+--------]: Saved " + scienceType.scienceName + " " + (scienceType.scienceBank + scienceType.scienceCache));
                scienceType.scienceBank = scienceType.scienceBank + scienceType.scienceCache;
                scienceType.scienceCache = 0;
            }
        }

        void LoadScience(ConfigNode node)
        {
            foreach (ScienceType scienceType in allScienceColors)
            {
                Debug.Log("[--------SCIENCE+--------3]: " + scienceType.scienceName + " " + scienceType.scienceBank + " " + scienceType.scienceCache);
            }
            ConfigNode node2 = node.GetNode("SCIENCE+");
            bool flag = node2 == null;
            if (!flag)
            {
                foreach (ScienceType scienceType in allScienceColors)
                {
                    ConfigNode node3 = node2.GetNode(scienceType.scienceName);
                    scienceType.scienceBank = float.Parse(node3.GetValue("SCI"));
                    Debug.Log("[--------SCIENCE+--------]: Loaded " + scienceType.scienceName + " " + scienceType.scienceBank);
                }
            }
        }

        private void Start()
        {
            GameEvents.OnScienceRecieved.Add(ScienceProcessingCallback);
            GameEvents.onGameStateSave.Add(SaveScience);
            GameEvents.onGameStateLoad.Add(LoadScience);
        }

        private void OnDestroy()
        {
            GameEvents.OnScienceRecieved.Remove(ScienceProcessingCallback);
            GameEvents.onGameStateSave.Remove(SaveScience);
            GameEvents.onGameStateLoad.Remove(LoadScience);
        }

        void ScienceProcessingCallback(float sciValue, ScienceSubject sub, ProtoVessel pv, bool test)
        {
            foreach (ScienceType scienceType in allScienceColors)
            {
                Debug.Log("[--------SCIENCE+--------4]: " + scienceType.scienceName + " " + scienceType.scienceBank + " " + scienceType.scienceCache);
            }
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