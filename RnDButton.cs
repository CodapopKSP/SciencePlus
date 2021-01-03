using UnityEngine;

namespace TestAddon
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

        private void SpawnButton()
        {
            CoolUI.ShowGUI();
        }

        private void HideButton()
        {
            CoolUI.Destroy();
        }
    }
}
