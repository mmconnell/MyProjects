using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.Cameras
{
    public class Base_Camera : MonoBehaviour
    {
        #region Variables
        public Transform m_Target;
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }
        #endregion

        #region Helper Methods
        protected virtual void HandleCamera()
        {
            if (!m_Target)
            {
                return;
            }
        }
        #endregion
    }
}