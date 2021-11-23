using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct AbilityTagChange
{
    public int priority;
    public string source;
    public List<AbilityTag> tags;
    public bool overwrite;
}
