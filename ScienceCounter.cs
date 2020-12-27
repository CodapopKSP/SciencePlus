using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, true)]

    public class ScienceCounter : MonoBehaviour
    {
        private void Awake()
        {
            ScienceCounter.instance = this;
        }

        private void Start()
        {
            GameEvents.OnScienceRecieved.Add(ScienceProcessingCallback);
        }

        private void OnDestroy()
        {
            GameEvents.OnScienceRecieved.Remove(ScienceProcessingCallback);
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

        public List<ScienceType> allScienceColors = new List<ScienceType>()
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

        public static ScienceCounter instance;
    }
}