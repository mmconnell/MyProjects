using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;
using Ashen.EquationSystem;

public interface I_Cacheable
{
    void Recalculate(I_DeliveryTool toolManager, EquationArgumentPack extraArguments);
}
