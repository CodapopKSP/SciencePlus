namespace SciencePlus
{
    [KSPScenario(ScenarioCreationOptions.AddToNewGames, new GameScenes[]
    {
        GameScenes.SPACECENTER,
        GameScenes.FLIGHT,
        GameScenes.TRACKSTATION,
    })]

    public class SciencePlus : ScenarioModule
    {
        public override void OnSave(ConfigNode node)
        {
            ConfigNode configNode = new ConfigNode("Science");
            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
            {
                scienceType.scienceBank += scienceType.scienceCache;
                configNode.AddValue(scienceType.type, scienceType.scienceBank);
                scienceType.scienceCache = 0;
            }
            node.AddNode(configNode);
        }

        public override void OnLoad(ConfigNode node)
        {
            //ScienceCounter.instance.BuildTypes();
            node = node.GetNode("Science");
            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
            {
                scienceType.scienceBank = float.Parse(node.GetValue(scienceType.type));
            }
        }
    }
}