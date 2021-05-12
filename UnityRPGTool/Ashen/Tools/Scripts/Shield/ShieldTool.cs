using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class ShieldTool : A_EnumeratedTool<ShieldTool>
    {
        public List<ShieldHolder> permanentShields;
        public List<ShieldHolder> temporaryShields;


    }
}