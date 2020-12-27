﻿using System.Collections.Generic;
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
        public override void OnSave(ConfigNode node)
        {
            ConfigNode configNode = new ConfigNode("Science");
            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceColors)
            {
                scienceType.scienceBank = scienceType.scienceBank + scienceType.scienceCache;
                configNode.AddValue(scienceType.color, scienceType.scienceBank);
                scienceType.scienceCache = 0;
            }
            node.AddNode(configNode);
        }

        public override void OnLoad(ConfigNode node)
        {
            node = node.GetNode("Science");
            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceColors)
            {
                scienceType.scienceBank = int.Parse(node.GetValue(scienceType.color));
            }
        }
    }
}