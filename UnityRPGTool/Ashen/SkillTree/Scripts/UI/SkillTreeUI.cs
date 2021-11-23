using Ashen.SkillTree;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : SingletonMonoBehaviour<SkillTreeUI>
{
    public CharacterBinderUI characterBinder;
    public Material disabledMaterial;

    [ShowInInspector]
    private SkillTreeTool skillTreeTool;
    [InlineEditor]
    public List<SkillTreeNodeUI> skillTreeUIs;

    public ScrollRect skillViewer;

    [FoldoutGroup("Valid Node"), AutoPopulate, HideLabel]
    public SkillTreeNodeColorScheme validNode;
    [FoldoutGroup("Invalid Node"), AutoPopulate, HideLabel]
    public SkillTreeNodeColorScheme invalidNode;
    public Color selectedNodeColor;
    //[FoldoutGroup("Selected Node"), AutoPopulate, HideLabel]
    //public SkillTreeNodeColorScheme selectedNode;

    public Image noviceIndicator;
    public Image veteranIndicator;
    public Image masterIndicator;

    public TextMeshProUGUI noviceName;
    public TextMeshProUGUI veteranName;
    public TextMeshProUGUI masterName;

    public TextMeshProUGUI skillPoints;

    public Color selectedIndicator;
    public Color inactiveIndicator;

    public RequirementsManager reqManager;

    public GameObject noviceLeft;
    public GameObject noviceRight;
    public GameObject veteranLeft;
    public GameObject veteranRight;
    public GameObject masterLeft;
    public GameObject masterRight;

    public GameObject linesHolder;
    public GameObject linePrefab;

    public GameObject skillNode;
    public GameObject skillNodeContainer;

    public List<GameObject> containers;
    public List<GameObject> lines;

    public void RegisterSkillTree(SkillTreeTool skillTreeTool)
    {
        this.skillTreeTool = skillTreeTool;
        noviceName.text = skillTreeTool.name;
        veteranName.text = skillTreeTool.name;
        masterName.text = skillTreeTool.name;
        LoadSkillTree(skillTreeTool.skillTree);
        UpdateNodes();

    }

    public void RegisterNode(SkillTreeNodeUI node, bool batchUpdate = false)
    {
        if (skillTreeUIs == null)
        {
            skillTreeUIs = new List<SkillTreeNodeUI>();
        }
        if (!skillTreeUIs.Contains(node))
        {
            skillTreeUIs.Add(node);
        }
        if (!batchUpdate)
        {
            UpdateNodes();
        }
    }

    public void OnClickNode(SkillTreeNodeUI nodeUI, SkillNode node)
    {
        if (!skillTreeTool)
        {
            return;
        }
        NodeIncreaseRequestResponse response = skillTreeTool.RequestSkillNodeIncrease(node);
        if (response == NodeIncreaseRequestResponse.MAX_LEVEL || response == NodeIncreaseRequestResponse.SUCCESS)
        {
            int skilLevel = skillTreeTool.GetCurrentLevel(node);
            if (skilLevel == 1)
            {
                nodeUI.OnFirstSkillPoint();
            }
        }
        UpdateNodes();
    }

    public void UpdateScreenPosition(SkillLevel skillLevel)
    {
        noviceIndicator.color = inactiveIndicator;
        veteranIndicator.color = inactiveIndicator;
        masterIndicator.color = inactiveIndicator;
        switch (skillLevel)
        {
            case SkillLevel.Novice:
                skillViewer.horizontalNormalizedPosition = 0f;
                noviceIndicator.color = selectedIndicator;
                break;
            case SkillLevel.Veteran:
                skillViewer.horizontalNormalizedPosition = 0.5f;
                veteranIndicator.color = selectedIndicator;
                break;
            case SkillLevel.Master:
                skillViewer.horizontalNormalizedPosition = 1f;
                masterIndicator.color = selectedIndicator;
                break;
        }
    }

    public void OnClickRight(SkillTreeNodeUI skillTreeNodeUI, SkillNode skillNode)
    {
    }

    [Button]
    public void UpdateNodes()
    {
        if (skillTreeUIs == null)
        {
            return;
        }
        skillPoints.text = skillTreeTool.skillPoints.ToString();
        foreach (SkillTreeNodeUI node in skillTreeUIs)
        {
            if (!skillTreeTool)
            {
                if (node.skillNode.hasRequirements && node.skillNode.requirements?.Count > 0)
                {
                    node.OnDisabledSkill();
                }
                else
                {
                    node.OnValidOptionSkill();
                }
                noviceIndicator.color = selectedIndicator;
                veteranIndicator.color = inactiveIndicator;
                masterIndicator.color = inactiveIndicator;
                continue;
            }
            if (skillTreeTool.RequirementsMet(node.skillNode))
            {

            }
            if (skillTreeTool.GetCurrentLevel(node.skillNode) == 0)
            {
                if (!skillTreeTool.CanIncreaseSkillNode(node.skillNode))
                {
                    node.OnDisabledSkill();
                }
                else
                {
                    if (skillTreeTool.skillPoints > 0)
                    {
                        node.OnValidOptionSkill();
                    }
                    else
                    {
                        node.OnMissingSkillPoints();
                    }
                }
            }
            else
            {
                if (skillTreeTool.CanIncreaseSkillNode(node.skillNode))
                {
                    if (skillTreeTool.skillPoints > 0)
                    {
                        node.OnValidOptionSkill();
                    }
                    else
                    {
                        node.OnMissingSkillPoints();
                    }
                }
                else if (skillTreeTool.IsSkillMax(node.skillNode))
                {
                    node.OnMaxSkill();
                }
            }
            if (node.skillNode.hasRequirements)
            {
                foreach (I_SkillNodeRequirements req in node.skillNode.requirements)
                {
                    if (req is OtherSkillNodeRequirement other)
                    {
                        reqManager.UpdateRequirements(other.skillNode, node.skillNode, req.RequirementsMet(skillTreeTool));
                    }
                }
            }
            node.currentValueText.text = skillTreeTool.GetCurrentLevel(node.skillNode) + "";
            EditorUtility.SetDirty(node.currentValueText);
        }
    }

    [Button]
    private void LoadSkillTree(SkillTree skillTree)
    {
        if (skillTreeUIs == null)
        {
            skillTreeUIs = new List<SkillTreeNodeUI>();
        }
        skillTreeUIs.Clear();
        if (containers == null)
        {
            containers = new List<GameObject>();
        }
        foreach (GameObject container in containers)
        {
            DestroyImmediate(container);
        }
        containers.Clear();
        if (lines == null)
        {
            lines = new List<GameObject>();
        }
        foreach (GameObject line in lines)
        {
            DestroyImmediate(line);
        }
        lines.Clear();
        Dictionary<SkillNode, SkillTreeNodeUI> nodeConfigToElement = new Dictionary<SkillNode, SkillTreeNodeUI>();
        if (skillTree.noviceLeft != null)
        {
            LoadSKillsForColumn(skillTree.noviceLeft, noviceLeft, nodeConfigToElement, 1f);
        }
        if (skillTree.noviceRight != null)
        {
            LoadSKillsForColumn(skillTree.noviceRight, noviceRight, nodeConfigToElement, - 1f);
        }
        if (skillTree.veteranLeft != null)
        {
            LoadSKillsForColumn(skillTree.veteranLeft, veteranLeft, nodeConfigToElement, 1f);
        }
        if (skillTree.veteranRight != null)
        {
            LoadSKillsForColumn(skillTree.veteranRight, veteranRight, nodeConfigToElement, -1f);
        }
        if (skillTree.masterLeft != null)
        {
            LoadSKillsForColumn(skillTree.masterLeft, masterLeft, nodeConfigToElement, 1f);
        }
        if (skillTree.masterRight != null)
        {
            LoadSKillsForColumn(skillTree.masterRight, masterRight, nodeConfigToElement, -1f);
        }
        GenerateLines(nodeConfigToElement);
        UpdateNodes();
    }

    private void LoadSKillsForColumn(List<NodeUI> nodeUis, GameObject column, Dictionary<SkillNode, SkillTreeNodeUI> nodeConfigToElement, float widthMultiplier)
    {
        RectTransform referenceRect = (column.transform as RectTransform);
        float oneUnitHeight = referenceRect.rect.height / 7f;
        float width = referenceRect.rect.width;
        float height = referenceRect.rect.height;

        int usedSections = 0;
        foreach (NodeUI nodeUi in nodeUis)
        {
            if (usedSections + nodeUi.space > 7)
            {
                break;
            }
            GameObject container = Instantiate(skillNodeContainer, column.transform);
            containers.Add(container);
            RectTransform containerRect = container.transform as RectTransform;
            containerRect.localPosition = new Vector3(widthMultiplier * width/2f, (-usedSections * oneUnitHeight) + (height/2f), 0);
            containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, oneUnitHeight * nodeUi.space);
            if (nodeUi.type == NodeUI.NodeUiType.SkillNode)
            {
                GameObject skillNode = Instantiate(this.skillNode, container.transform);
                SkillTreeNodeUI skillTreeNodeUI = skillNode.GetComponent<SkillTreeNodeUI>();
                skillTreeNodeUI.RegisterSkillNode(nodeUi.skillNode);
                nodeConfigToElement.Add(nodeUi.skillNode, skillTreeNodeUI);
                skillTreeNodeUI.skillTreeUI = this;
                RegisterNode(skillTreeNodeUI, true);
                
            }
            usedSections += nodeUi.space;
        }
    }

    private void GenerateLines(Dictionary<SkillNode, SkillTreeNodeUI> nodeConfigToElement)
    {
        foreach (KeyValuePair<SkillNode, SkillTreeNodeUI> pair in nodeConfigToElement)
        {
            SkillNode skillNode = pair.Key;
            if (skillNode.hasRequirements && skillNode.requirements?.Count > 0)
            {
                foreach (I_SkillNodeRequirements requirements in skillNode.requirements)
                {
                    OtherSkillNodeRequirement otherSkillNodeRequirement = requirements as OtherSkillNodeRequirement;
                    if (otherSkillNodeRequirement == null || !nodeConfigToElement.ContainsKey(otherSkillNodeRequirement.skillNode))
                    {
                        continue;
                    }
                    GameObject lineHolder = Instantiate(linePrefab, linesHolder.transform);
                    lines.Add(lineHolder);
                    SquareLineDrawerUI drawer = lineHolder.GetComponent<SquareLineDrawerUI>();
                    drawer.reference = transform;
                    drawer.locations = new List<UILocation>();
                    if (otherSkillNodeRequirement.lineConfigurations?.Count > 0)
                    {
                        foreach (SkillNodeLineConfiguration config in otherSkillNodeRequirement.lineConfigurations)
                        {
                            drawer.locations.Add(new UILocation()
                            {
                                rectTransform = nodeConfigToElement[config.skillNode].transform as RectTransform,
                                bothResolver = config.configuration.bothResolver,
                                coord = config.configuration.coord,
                                rectSide = config.configuration.rectSide,
                                splitPercentage = config.configuration.splitPercentage,
                            });
                        }
                    }
                    else
                    {
                        drawer.locations.Add(new UILocation()
                        {
                            rectTransform  = nodeConfigToElement[otherSkillNodeRequirement.skillNode].transform as RectTransform,
                            rectSide = RectSide.RIGHT,
                            coord = Coord.X,
                        });
                        drawer.locations.Add(new UILocation()
                        {
                            rectTransform = pair.Value.transform as RectTransform,
                            rectSide = RectSide.LEFT,
                            coord = Coord.BOTH,
                            bothResolver = BothResolver.MIDXY,
                            splitPercentage = 50,
                        });
                    }
                }
            }
        }
    }
}