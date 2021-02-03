using UnityEngine;
using System.Collections.Generic;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine.UI;

namespace SciencePlus
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    class ConditionalResearchButtonSetup : MonoBehaviour
    {
        private void Start()
        {
            RDController.OnRDTreeSpawn.Add(i => AddButton(i));
            Destroy(gameObject);
        }

        private static void AddButton(RDController instance)
        {
            instance.actionButton.gameObject.AddComponent<ConditionalResearchButton>();
        }
    }

    class ConditionalResearchButton : MonoBehaviour
    {
        public class Request
        {
            public RDNode node;
            public bool allowed;

            public void Deny() { allowed = false; }
        }

        public static EventData<Request> OnResearchButtonPresented = new EventData<ConditionalResearchButton.Request>("OnResearchButtonPresented");

        private Image _buttonImage;
        private Button _button;
        private Button.ButtonClickedEvent _originalEvent;

        private void Awake()
        {
            _buttonImage = GetComponent<Image>();
            _button = GetComponent<Button>();

            _originalEvent = _button.onClick;
            _button.onClick = new Button.ButtonClickedEvent();
            _button.onClick.AddListener(OnButtonClicked);
            GetComponent<UIStateButton>().onValueChanged.AddListener(_ => UpdateButtonStatus());
        }

        private void OnEnable()
        {
            UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            ToggleButton(!IsNodeResearchable() || AllowResearch());
        }

        private void ToggleButton(bool enable)
        {
            _button.enabled = enable;
            _buttonImage.color = enable ? Color.white : Color.grey;
        }

        private void OnButtonClicked()
        {
            if (IsNodeResearchable())
            {
                PopupDialog.SpawnPopupDialog(
                    new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                    new MultiOptionDialog(
                        "ConfirmResearchDialog", $"Are you sure you want to purchase {RDController.Instance.node_selected.tech.title}?",
                        "Confirm?",
                        UISkinManager.GetSkin("KSP window 7"),
                        new DialogGUIButton("Yes", UserConfirmedPurchase, true),
                        new DialogGUIButton("No", () => { }, true)),
                    false,
                    UISkinManager.GetSkin("KSP window 7"));
            }
            
            else
            {
                _originalEvent.Invoke();
            }
            
        }

        private void UserConfirmedPurchase()
        {
            _originalEvent.Invoke();
        }

        private bool IsNodeResearchable()
        {
            var node = RDController.Instance.node_selected;
            return node != null && node.state == RDNode.State.RESEARCHABLE;
        }

        private bool AllowResearch()
        {
            var request = new Request { node = RDController.Instance.node_selected, allowed = true };

            List<string> sciTypes = new List<string>();
            OnResearchButtonPresented.Fire(request);
            return request.allowed;
        }
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    class RnDScienceTypes : MonoBehaviour
    {
        private void Start()
        {
            ConditionalResearchButton.OnResearchButtonPresented.Add(CanAffordScienceTypes);
            GameEvents.OnTechnologyResearched.Add(SpendScienceTypes);
        }

        private void OnDestroy()
        {
            ConditionalResearchButton.OnResearchButtonPresented.Remove(CanAffordScienceTypes);
            GameEvents.OnTechnologyResearched.Remove(SpendScienceTypes);
        }

        private void CanAffordScienceTypes(ConditionalResearchButton.Request request)
        {
            List<string> sciTypes = new List<string>();
            ConfigNode TechNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModuleManager.TechTree");
            TechNode = TechNode.GetNode("TechTree");
            foreach (ConfigNode RDNode in TechNode.GetNodes("RDNode"))
            {
                if (RDNode.GetValue("id") == request.node.tech.techID)
                {
                    string[] sciTypesArray = RDNode.GetValues("scitype");
                    foreach (string scitype in sciTypesArray)
                    {
                        sciTypes.Add(scitype);
                    }
                }
            }

            bool typecheck = true;
            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
            {
                foreach (string scitype in sciTypes)
                {
                    if (typecheck)
                    {
                        if (scitype == scienceType.type)
                        {
                            if (scienceType.scienceBank < request.node.tech.scienceCost)
                            {
                                typecheck = false;
                            }
                        }
                    }
                }
            }

            if (!typecheck)
            {
                request.Deny();
            }
        }

        private void SpendScienceTypes(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> targetAction)
        {
            List<string> sciTypes = new List<string>();
            ConfigNode TechNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/ModuleManager.TechTree");
            TechNode = TechNode.GetNode("TechTree");
            foreach (ConfigNode RDNode in TechNode.GetNodes("RDNode"))
            {
                if (RDNode.GetValue("id") == targetAction.host.techID)
                {
                    string[] sciTypesArray = RDNode.GetValues("scitype");
                    foreach (string scitype in sciTypesArray)
                    {
                        sciTypes.Add(scitype);
                    }
                }
            }

            foreach (ScienceCounter.ScienceType scienceType in ScienceCounter.instance.allScienceTypes)
            {
                foreach (string scitype in sciTypes)
                {
                    if (scitype == scienceType.type)
                    {
                        scienceType.scienceCache = scienceType.scienceCache - targetAction.host.scienceCost;
                    }
                }
            }
        }
    }
}
