using System.Collections.Generic;
using UnityEngine;
using System;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]

    public class ScienceCounter : MonoBehaviour
    {
        static ConfigNode sciNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/Squad/Resources/ScienceDefs.cfg");
        ConfigNode[] sciNodes = sciNode.GetNodes("EXPERIMENT_DEFINITION");
        List<Experiment> ExperimentList = new List<Experiment>();

        private void Awake()
        {
            ScienceCounter.instance = this;
            foreach (ConfigNode experiment in sciNodes)
            {
                string expName = experiment.GetValue("title");
                int expbV = int.Parse(experiment.GetValue("baseValue"));
                ExperimentList.Add(new Experiment(expName, expbV));
            }
        }

        public class Experiment
        {
            public Experiment(string name, int baseValue)
            {
                this.name = name;
                this.baseValue = baseValue;
            }
            public string name;
            public int baseValue;
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




            foreach (Experiment expNode in ExperimentList)
            {
                if (expNode.name.Contains("#autoLOC_501009 //#autoLOC_501009 = "))
                {
                    Debug.Log("[--------SCIENCE+--------]: GOT IT");
                }
                Debug.Log("[--------SCIENCE+--------]: " + expNode.name);
                Debug.Log("[--------SCIENCE+--------]: " + expNode.baseValue);
            }




            bool hasPlanet = false;
            foreach (ScienceType scienceType in allScienceColors)
            {
                foreach (string body in scienceType.bodyList)
                {
                    if (sub.title.Contains(body))
                    {
                        float newTotal = scienceType.scienceCache + sub.science;
                        scienceType.scienceCache = newTotal;
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " add " + sub.science);
                        hasPlanet = true;
                    }
                }
            }

            if (!hasPlanet)
            {
                int randomNumber = random.Next(8);
                foreach (ScienceType scienceType in allScienceColors)
                {
                    if (randomNumber == scienceType.randInt)
                    {
                        float newTotal = scienceType.scienceCache + sub.science;
                        scienceType.scienceCache = newTotal;
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " random add  " + sub.science);
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