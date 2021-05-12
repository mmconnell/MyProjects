using UnityEngine;
using System.Collections;

public interface I_Poolable
{
    bool Enabled();
    void Disable();
    void Initialize();
}
