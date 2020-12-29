using UnityEngine;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]

    public class RnDButton : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.onGUIRnDComplexSpawn.Add(SpawnButton);
            GameEvents.onGUIRnDComplexDespawn.Add(HideButton);
        }

        private void OnDestroy()
        {
            GameEvents.onGUIRnDComplexSpawn.Remove(SpawnButton);
            GameEvents.onGUIRnDComplexDespawn.Remove(HideButton);
        }

        private Rect windowPosition = new Rect();

        private void SpawnButton()
        {

        }

        private void HideButton()
        {

        }

    }
}
