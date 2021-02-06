using UnityEngine;
using KSP.UI.Screens;
using System.IO;
using RUI.Icons.Selectable;

namespace SciencePlus
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class AppButton : MonoBehaviour
	{
		const ApplicationLauncher.AppScenes buttonScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT;
		private static ApplicationLauncherButton button = null;

		public static Callback Toggle = delegate { };

		public void Start()
		{
			GameObject.DontDestroyOnLoad(this);
			GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
		}

		void OnDestroy()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIAppLauncherReady);
		}

		void OnGUIAppLauncherReady()
		{
			if (ApplicationLauncher.Ready && button == null)
			{
				Texture tex = GameDatabase.Instance.GetTexture("SciencePlus/Assets/icon", false);
				button = ApplicationLauncher.Instance.AddModApplication(OnToggle, OnToggle, null, null, null, null, buttonScenes, tex);
				UpdateVisibility();
			}
		}

		static bool buttonVisible
		{
			get
			{
				return true;
			}
		}

		public static void UpdateVisibility()
		{
			if (button != null)
			{
				button.VisibleInScenes = buttonVisible ? buttonScenes : 0;
			}
		}

		private void OnToggle()
		{
			Toggle();
		}
	}

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class Toolbar_Flight : MonoBehaviour
	{
		public void Awake()
		{
			AppButton.Toggle += WindowFlight.ToggleGUI;
		}

		void OnDestroy()
		{
			AppButton.Toggle -= WindowFlight.ToggleGUI;
		}
	}

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class Toolbar_SpaceCenter : MonoBehaviour
	{
		public void Awake()
		{
			AppButton.Toggle += WindowSpaceCenter.ToggleGUI;
		}

		void OnDestroy()
		{
			AppButton.Toggle -= WindowSpaceCenter.ToggleGUI;
		}
	}
}














/*
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
*/