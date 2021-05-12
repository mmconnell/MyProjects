using UnityEngine;
using UnityEditor;

public class Target : A_EnumSO<Target, Targets>
{
    public I_TargetHolder targetHolder;

    public I_TargetHolder BuildTargetHolder()
    {
        return targetHolder.Clone();
    }
}