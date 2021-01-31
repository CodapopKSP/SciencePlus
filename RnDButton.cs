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
            //GameEvents.onGUIRnDComplexSpawn.Remove(SpawnButton);
        }

        private void RnDStuff()
        {
            if (ResearchAndDevelopment.Instance != null)
            {
                Debug.Log("[--------SCIENCE+--------]: testing 1 2");
                ResearchAndDevelopment.RefreshTechTreeUI();

            }
        }

        private void PressButtonCallback(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> targetAction)
        {
            Debug.Log("[--------SCIENCE+--------]: There's your button!" + targetAction.host.scienceCost);
            Debug.Log("[--------SCIENCE+--------]: There's your button!" + targetAction.host.techID);
            Debug.Log("[--------SCIENCE+--------]: There's your button!" + targetAction.host.title);
            Debug.Log("[--------SCIENCE+--------]: There's your button!" + targetAction.host.host);
        }
    }
}
