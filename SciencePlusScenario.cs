using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SciencePlus
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
    {
        GameScenes.SPACECENTER,
        GameScenes.FLIGHT,
        GameScenes.TRACKSTATION
    })]

    public class SciencePlus : ScenarioModule
    {
        int number;
        public override void OnSave(ConfigNode node)
        {


            ConfigNode configNode = new ConfigNode("Science");
            configNode.AddValue("Red", number);
            node.AddNode(configNode);

        }

        public override void OnLoad(ConfigNode node)
        {
            node = node.GetNode("Science");
            number = int.Parse(node.GetValue("Red"));
            number += 1;
        }
    }
}