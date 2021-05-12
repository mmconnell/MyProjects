using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Ashen.SkillTree;

public class RequirementsManager : MonoBehaviour
{
    public RectTransform reference;

    public RectTransformSide requiresNodeDefaultSide;
    public RectTransformSide sourceNodeDefaultSide;
    public DisplayType defaultDisplayType;

    public GameObject requirementsPrefab;
    public GameObject requirementsContainerPrefab;

    public List<RequirementsContainer> requirements;

    public void RegisterSkillRequirements(SkillTreeNodeUI requires, SkillTreeNodeUI source, int amount)
    {
        if (requirements == null)
        {
            requirements = new List<RequirementsContainer>();
        }
        RequirementsContainer containerGo = null;
        foreach (RequirementsContainer req in requirements)
        {
            if (req.source == source)
            {
                foreach (RequirementsPositionController control in req.controllers)
                {
                    if (control.requiresReference == requires)
                    {
                        control.text.text = "LV" + amount;
                        EditorUtility.SetDirty(control.text);
                        return;
                    }
                }
                containerGo = req;
            }
        }
        if (containerGo == null)
        {
            containerGo = Instantiate(requirementsContainerPrefab, transform).GetComponent<RequirementsContainer>();
            containerGo.gameObject.name = source.skillNode.skillName + " Requirements";
            containerGo.controllers = new List<RequirementsPositionController>();
            containerGo.displayType = defaultDisplayType;
            containerGo.source = source;
            requirements.Add(containerGo);
        }
        GameObject newRequirementsObject = Instantiate(requirementsPrefab, containerGo.transform);
        newRequirementsObject.name = requires.skillNode.skillName;
        RequirementsPositionController controller = newRequirementsObject.GetComponent<RequirementsPositionController>();
        controller.locationX = 25;
        controller.locationY = 25;
        controller.reference = reference;
        controller.requiresReference = requires;
        controller.source = source;
        controller.requiresBound = requiresNodeDefaultSide;
        controller.sourceBound = sourceNodeDefaultSide;
        controller.text.text = "LV" + amount;
        containerGo.controllers.Add(controller);
        containerGo.Reset();
        EditorUtility.SetDirty(controller.text);
    }

    public void UpdateRequirements(SkillNode requires, SkillNode source, bool requirementsMet)
    {
        foreach (RequirementsContainer req in requirements)
        {
            if (req.source.skillNode == source)
            {
                foreach (RequirementsPositionController control in req.controllers)
                {
                    if (control.requiresReference.skillNode == requires)
                    {
                        control.disable = requirementsMet;
                        req.Reset();
                        return;
                    }
                }
            }
        }
    }

    public void RepairRequirements()
    {
        if (requirements == null)
        {
            return;
        }
        for (int x = 0; x < requirements.Count; x++)
        {
            RequirementsContainer req = requirements[x];
            bool valid = true;
            if (req == null || req.source == null || req.controllers == null || req.controllers.Count == 0)
            {
                valid = false;
            }
            if (valid)
            {
                for (int y = 0; y < req.controllers.Count; y++)
                {
                    RequirementsPositionController control = req.controllers[y];
                    if (control.source == null || control.requiresReference == null)
                    {
                        req.controllers.RemoveAt(y);
                        y--;
                    }
                }
                if (req.controllers.Count == 0)
                {
                    valid = false;
                }
            }
            if (!valid)
            {
                requirements.RemoveAt(x);
                x--;
            }
            else
            {
                req.Reset();
            }
        }
    }
}
