using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestAddon
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CoolUILoader : MonoBehaviour
    {
        private static GameObject panelPrefab;

        public static GameObject PanelPrefab
        {
            get { return panelPrefab; }
        }

        private void Awake()
        {
            AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mycoolui.dat"));
            panelPrefab = prefabs.LoadAsset("MyCoolUIPanel") as GameObject;
        }
    }

    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    public class CoolUI : MonoBehaviour
    {
        public static GameObject CoolUICanvas = null;
        public static GameObject CoolUIText = null;

        public static void Destroy()
        {
            CoolUICanvas.DestroyGameObject();
            CoolUICanvas = null;
        }

        public static void ShowGUI()
        {
            CoolUICanvas = (GameObject)Instantiate(CoolUILoader.PanelPrefab);
            Debug.Log("[--------SCIENCE+--------]: 1");
            CoolUICanvas.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            Debug.Log("[--------SCIENCE+--------]: 2");
            CoolUICanvas.AddComponent<CoolUI>();
            Debug.Log("[--------SCIENCE+--------]: 3");

            CoolUIText = (GameObject)GameObject.Find("ImportantText");
            Debug.Log("[--------SCIENCE+--------]: 4");
            GameObject checkToggle = (GameObject)GameObject.Find("CheckThisOUt");
            Debug.Log("[--------SCIENCE+--------]: 5");
        }
    }
}