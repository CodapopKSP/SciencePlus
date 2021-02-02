using UnityEngine;
using System;
using System.Collections.Generic;

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
            List<string> sciTypes = new List<string>();

            ConfigNode TechNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModuleManager.TechTree");
            TechNode = TechNode.GetNode("TechTree");
            foreach (ConfigNode RDNode in TechNode.GetNodes("RDNode"))
            {
                if (RDNode.GetValue("id") == targetAction.host.techID)
                {
                    string[] sciTypesArray = RDNode.GetValues("scitype");
                    foreach (string scitype in sciTypesArray)
                    {
                        sciTypes.Add(scitype);
                    }
                }
            }

            if (ResearchAndDevelopment.Instance.GetTechState(targetAction.host.techID) == null)
            {
                bool typecheck = true;
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
                {
                    foreach (string scitype in sciTypes)
                    {
                        if (scitype==scienceType.type)
                        {
                            if (scienceType.scienceBank < targetAction.host.scienceCost)
                            {
                                typecheck = false;
                            }
                        }
                    }
                }
                if (typecheck)
                {
                    ResearchAndDevelopment.Instance.AddScience(targetAction.host.scienceCost, TransactionReasons.RnDs);
                }
            }

            if (ResearchAndDevelopment.Instance.GetTechState(targetAction.host.techID) != null)
            {
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
                {
                    foreach (string scitype in sciTypes)
                    {
                        if (scitype==scienceType.type)
                        {
                            scienceType.scienceCache = scienceType.scienceCache - targetAction.host.scienceCost;
                        }
                    }
                }
            }
        }
    }
}
