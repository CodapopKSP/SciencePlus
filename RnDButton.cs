using UnityEngine;
using System;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]

    public class RnDButton : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.onGUIRnDComplexSpawn.Add(RnDStuff);
            GameEvents.OnTechnologyResearched.Add(PressButtonCallback);

        }

        private void OnDestroy()
        {
            GameEvents.onGUIRnDComplexSpawn.Remove(RnDStuff);
            GameEvents.OnTechnologyResearched.Remove(PressButtonCallback);
        }

        private void RnDStuff()
        {
            ResearchAndDevelopment.Instance.SetScience(0, TransactionReasons.RnDs);
        }

        public void PressButtonCallback(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> targetAction)
        {
            DateTime now = DateTime.Now;
            string type1 = "noType";
            string type2 = "noType";

            ConfigNode TechNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModuleManager.TechTree");
            TechNode = TechNode.GetNode("TechTree");
            foreach (ConfigNode RDNode in TechNode.GetNodes("RDNode"))
            {
                if (RDNode.GetValue("id") == targetAction.host.techID)
                {
                    if (RDNode.GetValue("scitype1") != null)
                    {
                        type1 = RDNode.GetValue("scitype1");
                    }
                    if (RDNode.GetValue("scitype2") != null)
                    {
                        type2 = RDNode.GetValue("scitype2");
                    }
                }
            }

            if (ResearchAndDevelopment.Instance.GetTechState(targetAction.host.techID) == null)
            {
                bool colorcheck = true;
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
                {
                    if ((type1 == scienceType.type) | (type2 == scienceType.type))
                    {
                        if (scienceType.scienceBank < targetAction.host.scienceCost)
                        {
                            colorcheck = false;
                        }
                    }
                }
                if (colorcheck)
                {
                    ResearchAndDevelopment.Instance.AddScience(targetAction.host.scienceCost, TransactionReasons.RnDs);
                }
            }

            if (ResearchAndDevelopment.Instance.GetTechState(targetAction.host.techID) != null)
            {
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
                {
                    if ((type1 == scienceType.type) | (type2 == scienceType.type))
                    {
                        scienceType.scienceCache = scienceType.scienceCache - targetAction.host.scienceCost;
                    }
                }
            }
        }
    }
}
