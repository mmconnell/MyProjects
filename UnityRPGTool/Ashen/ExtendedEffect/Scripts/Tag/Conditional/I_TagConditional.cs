﻿using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_TagConditional
    {
        bool Check(I_DeliveryTool owner, I_DeliveryTool target);
        string visualize();
    }
}