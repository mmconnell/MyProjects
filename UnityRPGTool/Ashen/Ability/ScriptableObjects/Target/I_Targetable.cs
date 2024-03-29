﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Manager;
using System.Collections.Generic;

public interface I_Targetable
{
    void Selected();
    void SelectedSecondary();
    void Deselected();
    Selectable GetSelectableObject();
    List<ToolManager> GetTargets();
    ToolManager GetTarget();
}
