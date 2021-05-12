using UnityEngine;
using System.Collections;

public class SkillPanelHandler : MonoBehaviour
{
    public GameObject skillsHolder;

    public GameObject enabler;

    public SkillSelector GetFirstSkill()
    {
        foreach (Transform child in skillsHolder.transform)
        {
            SkillSelector skillSelector = child.GetComponent<SkillSelector>();
            if (skillSelector != null)
            {
                return skillSelector;
            }
        }
        return null;
    }
}
