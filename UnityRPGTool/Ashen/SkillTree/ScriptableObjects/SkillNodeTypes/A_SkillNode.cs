using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;

[Serializable]
public abstract class A_SkillNode<T> where T : class, new()
{
    [HideLabel, EnumToggleButtons, PropertyOrder(-1)]
    public ReplaceAbilitySkillTypeInspector type;

    [OdinSerialize, ReadOnly]
    private int skillLevel;
    public int SkillLevel
    {
        set
        {
            skillLevel = value;
        }
    }
    
    [ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), Hide]
    public int choice;

    public List<ValueDropdownItem<int>> BuildValueToggle()
    {
        List<ValueDropdownItem<int>> options = new List<ValueDropdownItem<int>>();
        for (int x = 1; x <= skillLevel; x++)
        {
            options.Add(new ValueDropdownItem<int>
            {
                Value = x,
                Text = "Level " + x,
            });
        }
        if (options.Count == 0)
        {
            choice = -1;
        }
        return options;
    }

    public T GetOverride(int level)
    {
        switch (level)
        {
            case 1:
                return level1;
            case 2:
                return level2;
            case 3:
                return level3;
            case 4:
                return level4;
            case 5:
                return level5;
            case 6:
                return level6;
            case 7:
                return level7;
            case 8:
                return level8;
            case 9:
                return level9;
            case 10:
                return level10;
        }
        return null;
    }

    [Hide, ShowIf("@" + nameof(choice) + " == 1"), Title("Level 1")]
    public T level1;
    [Hide, ShowIf("@" + nameof(choice) + " == 2"), Title("Level 2")]
    public T level2;
    [Hide, ShowIf("@" + nameof(choice) + " == 3"), Title("Level 3")]
    public T level3;
    [Hide, ShowIf("@" + nameof(choice) + " == 4"), Title("Level 4")]
    public T level4;
    [Hide, ShowIf("@" + nameof(choice) + " == 5"), Title("Level 5")]
    public T level5;
    [Hide, ShowIf("@" + nameof(choice) + " == 6"), Title("Level 6")]
    public T level6;
    [Hide, ShowIf("@" + nameof(choice) + " == 7"), Title("Level 7")]
    public T level7;
    [Hide, ShowIf("@" + nameof(choice) + " == 8"), Title("Level 8")]
    public T level8;
    [Hide, ShowIf("@" + nameof(choice) + " == 9"), Title("Level 9")]
    public T level9;
    [Hide, ShowIf("@" + nameof(choice) + " == 10"), Title("Level 10")]
    public T level10;
}
