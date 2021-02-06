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
            if (allScienceTypes.Count<1)
            {
                BuildTypes();
            }
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
            bool hasPlanet = false;
            foreach (ScienceType scienceType in allScienceTypes)
            {
                foreach (string body in scienceType.bodyList)
                {
                    if (sub.title.Contains(body))
                    {
                        float newTotal = scienceType.scienceCache + sciValue;
                        scienceType.scienceCache = newTotal;
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " increased by " + sciValue + "!");
                        hasPlanet = true;
                    }
                }
            }

            if (!hasPlanet)
            {
                int randomNumber = random.Next(allScienceTypes.Count);
                foreach (ScienceType scienceType in allScienceTypes)
                {
                    if (randomNumber == scienceType.randInt)
                    {
                        float newTotal = scienceType.scienceCache + sciValue;
                        scienceType.scienceCache = newTotal;
                        Debug.Log("[--------SCIENCE+--------]: " + scienceType.scienceName + " randomly increased by  " + sciValue + "!");
                    }
                }
            }
        }

        public class ScienceType
        {
            public ScienceType(string type, List<string> bodyList, int randInt, float scienceBank = 0, float scienceCache = 0)
            {
                this.type = type;
                this.scienceName = type + " Science";
                this.bodyList = bodyList;
                this.randInt = randInt;
                this.scienceBank = scienceBank;
                this.scienceCache = scienceCache;
            }
            public string type;
            public string scienceName;
            public List<string> bodyList;
            public int randInt;
            public float scienceBank;
            public float scienceCache;
        }

        public void BuildTypes()
        {
            allScienceTypes.Clear();
            ConfigNode SciencePlusNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/SciencePlus/Science+.cfg");
            SciencePlusNode = SciencePlusNode.GetNode("Science+");
            ConfigNode[] ScienceTypeNodes = SciencePlusNode.GetNodes();
            int counter = 0;

            foreach (ConfigNode typeNode in ScienceTypeNodes)
            {
                List<string> typeIDs = new List<string>();
                string[] typeIDsArray = typeNode.GetValues("id");
                foreach (string typeID in typeIDsArray)
                {
                    typeIDs.Add(typeID);
                }

                allScienceTypes.Add(new ScienceType(typeNode.GetValue("type"), typeIDs, counter));
                counter += 1;
            }
        }

        public List<ScienceType> allScienceTypes = new List<ScienceType>();
        private static readonly System.Random random = new System.Random();
        public static ScienceCounter instance;
    }
}




