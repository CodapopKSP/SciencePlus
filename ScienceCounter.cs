using System.Collections.Generic;
using UnityEngine;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]

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

        public void ScienceProcessingCallback(float sciValue, ScienceSubject sub, ProtoVessel pv, bool test)
        {
            Debug.Log("[--------SCIENCE+--------]: " + sub.title);
            
            bool hasPlanet = false;
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
                        hasPlanet = true;
                        Debug.Log("[--------SCIENCE+--------]: Bool = " + hasPlanet);
                    }
                }
            }

            if (!hasPlanet)
            {
                int randomNumber = random.Next(8);
                Debug.Log("[--------SCIENCE+--------]: RandomNumber = " + randomNumber);
                foreach (ScienceType scienceType in allScienceColors)
                {
                    if (randomNumber == scienceType.randInt)
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
            public ScienceType(string color, List<string> bodyList, int randInt, float scienceBank = 0, float scienceCache = 0)
            {
                this.color = color;
                this.scienceName = color + "Science";
                this.bodyList = bodyList;
                this.randInt = randInt;
                this.scienceBank = scienceBank;
                this.scienceCache = scienceCache;
            }
            public string color;
            public string scienceName;
            public List<string> bodyList;
            public int randInt;
            public float scienceBank;
            public float scienceCache;
        }

        public List<ScienceType> allScienceColors = new List<ScienceType>()
        {
            new ScienceType("Red",    new List<string>() { "Moho",    "Duna"            }, 0),
            new ScienceType("Orange", new List<string>() { "Dres",    "Vall"            }, 1),
            new ScienceType("Yellow", new List<string>() { "Mun",     "Pol"             }, 2),
            new ScienceType("Green",  new List<string>() { "Minmus",  "Ike"             }, 3),
            new ScienceType("Blue",   new List<string>() { "Kerbin",  "Eeloo"           }, 4),
            new ScienceType("Purple", new List<string>() { "Eve",     "Bop"             }, 5),
            new ScienceType("Gold",   new List<string>() { "Kerbol",  "Jool",   "Tylo"  }, 6),
            new ScienceType("Silver", new List<string>() { "Gilly",   "Laythe"          }, 7)
        };

        private static readonly System.Random random = new System.Random();
        public static ScienceCounter instance;
    }
}