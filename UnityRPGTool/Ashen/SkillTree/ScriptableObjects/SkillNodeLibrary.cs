using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ashen.SkillTree;
using Sirenix.OdinInspector;

public class SkillNodeLibrary : SingletonScriptableObject<SkillNodeLibrary>
{
    public Dictionary<string, SkillNode> idToSkillNode; 

    [Button]
    public void LoadSkillNodes()
    {
        if (idToSkillNode == null)
        {
            idToSkillNode = new Dictionary<string, SkillNode>();
        }
        List<SkillNode> skillNodes = StaticUtilities.FindAssetsByType<SkillNode>();
        foreach (SkillNode skillNode in skillNodes)
        {
            if (!idToSkillNode.ContainsValue(skillNode))
            {
                if (skillNode.skillName == null || skillNode.skillName == "")
                {
                    idToSkillNode.Add(skillNode.name, skillNode);
                }
                else
                {
                    idToSkillNode.Add(skillNode.skillName, skillNode);
                }
            }
        }
    }
}