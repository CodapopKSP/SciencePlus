using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]

    public class TestCounter : MonoBehaviour
    {
        void SaveTest(ConfigNode node)
        {
            bool flag = true;//File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.txt");
            if (!flag)
            {
                //File.Create(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Science+.txt");
                node.AddNode("TEST-NODE");
                ConfigNode node2 = node.GetNode("TEST-NODE");
                node2.AddValue("VALUE", number);
                Debug.Log("[--------TEST--------]: Save " + number);
            }
            else
            {
                ConfigNode node2 = node.GetNode("TEST-NODE");
                node2.SetValue("VALUE", 5);
                Debug.Log("[--------TEST--------]: Save 5");
            }
        }

        void LoadTest(ConfigNode node)
        {
            ConfigNode node2 = node.GetNode("TEST-NODE");
            int test = int.Parse(node2.GetValue("VALUE"));
            Debug.Log("[--------TEST--------]: Load " + test);
            number = number + 1;
        }

        private void Start()
        {
            GameEvents.onGameStateSave.Add(SaveTest);
            //GameEvents.onGameStateLoad.Add(LoadTest);
        }

        private void OnDestroy()
        {
            GameEvents.onGameStateSave.Remove(SaveTest);
            //GameEvents.onGameStateSave.Remove(LoadTest);
        }

        public int number = 0;

    }
}