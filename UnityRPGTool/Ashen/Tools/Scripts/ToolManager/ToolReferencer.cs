using UnityEngine;
using System.Collections;
using Manager;

namespace Manager
{
    public class ToolReferencer : MonoBehaviour
    {
        public ToolManager reference;

        private void Awake()
        {
            ToolLookUp.Instance.Register(gameObject, reference);
        }
    }
}