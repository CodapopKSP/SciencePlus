using UnityEngine;

namespace SciencePlus
{


	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class WindowFlight : Window
	{

	}

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class WindowSpaceCenter : Window
	{

	}

	public class Window : MonoBehaviour
	{
		static Rect windowpos;
		private static bool gui_enabled;
		private static bool hide_ui;

		static Window instance;


		public static void ToggleGUI()
		{
			gui_enabled = !gui_enabled;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		public static void HideGUI()
		{
			gui_enabled = false;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		public static void ShowGUI()
		{
			gui_enabled = true;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		void UpdateGUIState()
		{
			enabled = !hide_ui && gui_enabled;
		}

		void onHideUI()
		{
			hide_ui = true;
			UpdateGUIState();
		}

		void onShowUI()
		{
			hide_ui = false;
			UpdateGUIState();
		}

		public void Awake()
		{
			instance = this;
			GameEvents.onHideUI.Add(onHideUI);
			GameEvents.onShowUI.Add(onShowUI);
		}

		void OnDestroy()
		{
			instance = null;
			GameEvents.onHideUI.Remove(onHideUI);
			GameEvents.onShowUI.Remove(onShowUI);
		}

		void Start()
		{
			UpdateGUIState();
		}

		void WindowGUI(int windowID)
		{
			GUILayout.BeginVertical();

			foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
            {
				GUILayout.Label(scienceType.scienceName + ": " + (scienceType.scienceBank + scienceType.scienceCache));
			}
			GUILayout.EndVertical();
			GUI.DragWindow(new Rect(0, 0, 1000, 200));
		}

		void OnGUI()
		{
			if (gui_enabled)
			{
				GUI.skin = HighLogic.Skin;
				windowpos = GUILayout.Window(GetInstanceID(), windowpos, WindowGUI, "Science+", GUILayout.Width(200));
			}
		}
	}
}