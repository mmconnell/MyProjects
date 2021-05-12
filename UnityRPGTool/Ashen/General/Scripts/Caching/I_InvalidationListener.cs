using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;

public interface I_InvalidationListener 
{
    void Invalidate(I_DeliveryTool toolManager, string key);
}
