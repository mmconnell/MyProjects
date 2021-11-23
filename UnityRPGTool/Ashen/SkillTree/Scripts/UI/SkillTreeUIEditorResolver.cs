using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Ashen.SkillTree;
using UnityEditor;

public class SkillTreeUIEditorResolver : MonoBehaviour
{
    public GameObject start;
    private SkillTreeUI skillTreeUi;

    public SkillTreeLineManger lineManager;
    public RequirementsManager requirementsManager;

    HashSet<SkillTreeNodeUI> foundNodes;
    Dictionary<SkillNode, SkillTreeNodeUI> nodesToUI;

    public void Update()
    {
        if (!skillTreeUi)
        {
            UpdateNodes();
        }
    }

    [Button]
    void UpdateNodes()
    {
        if (!skillTreeUi)
        {
            skillTreeUi = GetComponent<SkillTreeUI>();
        }
        if (!skillTreeUi)
        {
            return;
        }
        if (skillTreeUi.skillTreeUIs == null)
        {
            skillTreeUi.skillTreeUIs = new List<SkillTreeNodeUI>();
        }
        foundNodes = new HashSet<SkillTreeNodeUI>();
        nodesToUI = new Dictionary<SkillNode, SkillTreeNodeUI>();
        Transform initial = gameObject.transform;
        if (start)
        {
            initial = start.transform;
        }
        checkChildren(initial);
        lineManager.RepairLines();
        requirementsManager.RepairRequirements();
        for (int x = 0; x < skillTreeUi.skillTreeUIs.Count; x++)
        {
            SkillTreeNodeUI node = skillTreeUi.skillTreeUIs[x];
            if (!foundNodes.Contains(node))
            {
                skillTreeUi.skillTreeUIs.RemoveAt(x);
                x--;
            }
            else
            {
                if (node.skillNode)
                {
                    node.skillNameText.text = node.skillNode.skillName;
                    EditorUtility.SetDirty(node.skillNameText);
                    if (node.skillNode != null)
                    {
                        node.maxValueText.text = node.skillNode.maxRanks + "";
                        EditorUtility.SetDirty(node.maxValueText);
                    }
                }
                node.OnValidOptionSkill();
                if (node.skillNode.hasRequirements)
                {
                    foreach (I_SkillNodeRequirements requirements in node.skillNode.requirements)
                    {
                        OtherSkillNodeRequirement otherNodeRequirement = requirements as OtherSkillNodeRequirement;
                        if (otherNodeRequirement != null && nodesToUI.ContainsKey(otherNodeRequirement.skillNode))
                        {
                            lineManager.RegisterSkillNodeLine(nodesToUI[otherNodeRequirement.skillNode], node);
                            requirementsManager.RegisterSkillRequirements(nodesToUI[otherNodeRequirement.skillNode], node, otherNodeRequirement.levelRequired);
                        }
                    }
                }
            }
        }
    }

    public void checkChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;
            SkillTreeNodeUI node = go.GetComponent<SkillTreeNodeUI>();
            if (node)
            {
                foundNodes.Add(node);
                if (nodesToUI.ContainsKey(node.skillNode))
                {
                    nodesToUI[node.skillNode] = node;
                }
                else
                {
                    nodesToUI.Add(node.skillNode, node);
                }
                if (!skillTreeUi.skillTreeUIs.Contains(node))
                {
                    skillTreeUi.skillTreeUIs.Add(node);
                }
                if (!node.skillTreeUI)
                {
                    node.skillTreeUI = skillTreeUi;
                }
            }
            checkChildren(child);
        }
    }
} 
