using UnityEngine;

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
            if (ResearchAndDevelopment.Instance != null)
            {
                ResearchAndDevelopment.Instance.SetScience(0, TransactionReasons.RnDs);
            }
        }

        public void PressButtonCallback(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> targetAction)
        {
            string color1 = "noColor";
            string color2 = "noColor";

            ConfigNode TechNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/Squad/Resources/TechTree.cfg");
            TechNode = TechNode.GetNode("TechTree");
            foreach (ConfigNode RDNode in TechNode.GetNodes("RDNode"))
            {
                if (RDNode.GetValue("id") == targetAction.host.techID)
                {
                    if (RDNode.GetValue("color1") != null)
                    {
                        color1 = RDNode.GetValue("color1");
                    }
                    if (RDNode.GetValue("color2") != null)
                    {
                        color2 = RDNode.GetValue("color2");
                    }
                }
            }

            if (ResearchAndDevelopment.Instance.GetTechState(targetAction.host.techID) == null)
            {
                bool colorcheck = true;
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceColors)
                {
                    if ((color1 == scienceType.color) | (color2 == scienceType.color))
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
                foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceColors)
                {
                    if ((color1 == scienceType.color) | (color2 == scienceType.color))
                    {
                        scienceType.scienceCache = scienceType.scienceCache - targetAction.host.scienceCost;
                    }
                }
            }
        }
    }
}
