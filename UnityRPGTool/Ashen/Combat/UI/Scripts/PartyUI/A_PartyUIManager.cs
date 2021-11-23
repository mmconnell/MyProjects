using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Manager;
using UnityEngine.UI;

public abstract class A_PartyUIManager<T> : SingletonMonoBehaviour<T> where T : A_PartyUIManager<T>
{
    [OdinSerialize]
    public Dictionary<PartyPosition, A_CharacterSelector> positionToManager;

    protected A_CharacterSelector[] managers;

    protected virtual void Start()
    {
        managers = new A_CharacterSelector[PartyPositions.Count];
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (positionToManager.TryGetValue(position, out A_CharacterSelector manager))
            {
                managers[(int)position] = manager;
            }
        }
        A_PartyManager partyManager = GetPartyManager();
    }

    protected abstract A_PartyManager GetPartyManager();

    public virtual void Recalculate()
    {
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            Recalculate(position);
        }
    }

    private void Recalculate(PartyPosition position)
    {
        PartyPositions partyPositions = PartyPositions.Instance;
        A_CharacterSelector memberManager = positionToManager[position];
        if (!memberManager.HasRegisteredToolManager())
        {
            Navigation nav = memberManager.navigation;
            nav.selectOnDown = null;
            nav.selectOnLeft = null;
            nav.selectOnRight = null;
            nav.selectOnUp = null;
            memberManager.navigation = nav;
            return;
        }
        PartyPosition other = null;
        int index = -1;
        int matchingOther = -1;
        List<PartyPosition> list = null;
        List<PartyPosition> otherList = null;
        if (position.row == PartyRow.FRONT)
        {
            index = partyPositions.frontRow.IndexOf(position);
            other = partyPositions.backRow[index];
            matchingOther = (int)other;
            otherList = partyPositions.backRow;
            list = partyPositions.frontRow;
        }
        else
        {
            index = partyPositions.backRow.IndexOf(position);
            other = partyPositions.frontRow[index];
            matchingOther = (int)other;
            otherList = partyPositions.frontRow;
            list = partyPositions.backRow;
        }

        if (positionToManager[other].HasRegisteredToolManager())
        {
            SetVertical(memberManager, positionToManager[other]);
        }
        else
        {
            int searchIndex = index;
            int enumSearch = matchingOther;
            bool found = false;
            while (!found && searchIndex > 0)
            {
                searchIndex--;
                enumSearch--;
                if (positionToManager[otherList[searchIndex]].HasRegisteredToolManager())
                {
                    other = PartyPositions.Instance[enumSearch];
                    SetVertical(memberManager, positionToManager[other]);
                    found = true;
                }
            }
            searchIndex = index;
            enumSearch = matchingOther;
            while (!found & searchIndex < otherList.Count - 1)
            {
                searchIndex++;
                enumSearch++;
                if (positionToManager[otherList[searchIndex]].HasRegisteredToolManager())
                {
                    other = PartyPositions.Instance[enumSearch];
                    SetVertical(memberManager, positionToManager[other]);
                    found = true;
                }
            }
            if (!found)
            {
                SetVertical(memberManager, null);
            }
        }

        PartyPosition left = null;
        PartyPosition right = null;

        int sideSearch = index;
        int sideEnum = (int)position;
        while (left == null && sideSearch > 0)
        {
            sideSearch--;
            sideEnum--;
            if (positionToManager[list[sideSearch]].HasRegisteredToolManager())
            {
                left = PartyPositions.Instance[sideEnum];
            }
        }
        if (left == null)
        {
            List<PartyPosition> positions = position.row == PartyRow.FRONT ? partyPositions.frontRow : partyPositions.backRow;
            int startIndex = positions.Count - 1;
            while (startIndex >= 0 && left == null)
            {
                PartyPosition curPosition = positions[startIndex];
                if (positionToManager[curPosition].HasRegisteredToolManager())
                {
                    left = curPosition;
                }
                startIndex--;
            }
        }
        sideSearch = index;
        sideEnum = (int)position;
        while (right == null && sideSearch < list.Count - 1)
        {
            sideSearch++;
            sideEnum++;
            if (positionToManager[list[sideSearch]].HasRegisteredToolManager())
            {
                right = PartyPositions.Instance[sideEnum];
            }
        }
        if (right == null)
        {
            List<PartyPosition> positions = position.row == PartyRow.FRONT ? partyPositions.frontRow : partyPositions.backRow;
            int startIndex = 0;
            while (startIndex < positions.Count && right == null)
            {
                PartyPosition curPosition = positions[startIndex];
                if (positionToManager[curPosition].HasRegisteredToolManager())
                {
                    right = curPosition;
                }
                startIndex++;
            }
        }

        A_CharacterSelector rightMem = right != null ? positionToManager[right] : null;
        A_CharacterSelector leftMem = left != null ? positionToManager[left] : null;

        SetSides(leftMem, memberManager, rightMem);
    }

    public virtual void SetPartyMember(PartyPosition position, ToolManager toolManager)
    {
        managers[(int)position].RegisterToolManager(toolManager);
        Recalculate();
    }

    private void SetVertical(A_CharacterSelector top, A_CharacterSelector bottom)
    {
        if (bottom == null)
        {
            Navigation nav = top.navigation;
            nav.selectOnUp = null;
            nav.selectOnDown = null;
            top.navigation = nav;
            return;
        }
        Navigation topNav = top.navigation;
        Navigation botNav = bottom.navigation;
        topNav.selectOnUp = bottom;
        topNav.selectOnDown = bottom;
        botNav.selectOnDown = top;
        botNav.selectOnUp = top;
        top.navigation = topNav;
        bottom.navigation = botNav;
    }

    private void SetSides(A_CharacterSelector left, A_CharacterSelector mid, A_CharacterSelector right)
    {
        Navigation leftNav;
        Navigation midNav = mid.navigation;
        Navigation rightNav;
        
        rightNav = right.navigation;
        leftNav = left.navigation;
        
        leftNav.selectOnRight = mid;

        midNav.selectOnLeft = left;
        midNav.selectOnRight = right;

        rightNav.selectOnLeft = mid;

        mid.navigation = midNav;
        right.navigation = rightNav;
        left.navigation = leftNav;
    }
}
