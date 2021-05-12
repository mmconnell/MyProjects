using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Saveable
{
    object CaptureState();
    void RestoreState(object state);
}
