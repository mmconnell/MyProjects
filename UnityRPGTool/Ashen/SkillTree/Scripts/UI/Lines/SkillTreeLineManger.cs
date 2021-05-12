using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeLineManger : MonoBehaviour
{
    public List<SkillNodeLine> skillLines;
    public RectTransform reference;
    public GameObject skillLinePrefab;

    public void RegisterSkillNodeLine(SkillTreeNodeUI from, SkillTreeNodeUI to)
    {
        if (skillLines == null)
        {
            skillLines = new List<SkillNodeLine>();
        }
        foreach (SkillNodeLine line in skillLines)
        {
            if (line.from == from && line.to == to)
            {
                return;
            }
        }
        SkillNodeLine newLine = new SkillNodeLine();
        newLine.from = from;
        newLine.to = to;
        GameObject newLineGameObject = Instantiate(skillLinePrefab, transform);
        newLineGameObject.name = from.skillNode.skillName + " To " + to.skillNode.skillName;
        SquareLineDrawerUI lineDrawer = newLineGameObject.GetComponent<SquareLineDrawerUI>();
        newLine.line = lineDrawer;
        skillLines.Add(newLine);

        lineDrawer.locations = new List<UILocation>();

        UILocation fromLocation = new UILocation();
        fromLocation.bothResolver = BothResolver.MIDXY;
        fromLocation.coord = Coord.BOTH;
        fromLocation.rectSide = RectSide.RIGHT;
        fromLocation.splitPercentage = 50;
        fromLocation.rectTransform = from.gameObject.GetComponent<RectTransform>();
        lineDrawer.locations.Add(fromLocation);

        UILocation toLocation = new UILocation();
        toLocation.bothResolver = BothResolver.MIDXY;
        toLocation.coord = Coord.BOTH;
        toLocation.rectSide = RectSide.LEFT;
        toLocation.splitPercentage = 50;
        toLocation.rectTransform = to.gameObject.GetComponent<RectTransform>();
        lineDrawer.locations.Add(toLocation);

        lineDrawer.reference = reference != null ? reference : GetComponent<RectTransform>();
    }

    public void RepairLines()
    {
        if (skillLines == null)
        {
            return;
        }
        for (int x = 0; x < skillLines.Count; x++)
        {
            SkillNodeLine line = skillLines[x];
            if (line.from == null || line.to == null || line.line == null)
            {
                skillLines.RemoveAt(x);
                x--;
            }
        }
    }
}
