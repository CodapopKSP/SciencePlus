using UnityEngine;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]

    public class RnDButton : MonoBehaviour
    {
        private void Start()
        {
            //GameEvents.onGUIRnDComplexSpawn.Add(SpawnButton);
            GameEvents.OnTechnologyResearched.Add(PressButtonCallback);
        }

        private void OnDestroy()
        {
            //GameEvents.onGUIRnDComplexSpawn.Remove(SpawnButton);
        }

        private void PressButtonCallback(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> hello)
        {
            Debug.Log("[--------SCIENCE+--------]: There's your button!");
        }
    }
}
