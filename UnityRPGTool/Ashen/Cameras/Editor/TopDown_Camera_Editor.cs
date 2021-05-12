using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ashen.Cameras
{
    [CustomEditor(typeof(TopDown_Camera))]
    public class IP_TopDown : Editor
    {
        #region Variables
        private TopDown_Camera targetCamera;
        #endregion

        #region Main Methods
        void OnEnable()
        {
            targetCamera = (TopDown_Camera)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            if (!targetCamera.m_Target)
            {
                return;
            }

            //storing target reference
            Transform camTarget = targetCamera.m_Target;

            //Draw distance disc
            Handles.color = new Color(1f, 0f, 0f, 0.15f);
            Handles.DrawSolidDisc(targetCamera.m_Target.position, Vector3.up, targetCamera.m_Distance);

            Handles.color = new Color(1f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(targetCamera.m_Target.position, Vector3.up, targetCamera.m_Distance);
            
            //Slider handles to adjust camera properties
            Handles.color = new Color(1f, 1f, 0f, 0.5f);
            targetCamera.m_Distance = Handles.ScaleSlider(targetCamera.m_Distance, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.m_Distance, 1f);
            targetCamera.m_Distance = Mathf.Clamp(targetCamera.m_Distance, 2f, float.MaxValue);

            Handles.color = new Color(0f, 0f, 1f, 0.5f);
            targetCamera.m_Height = Handles.ScaleSlider(targetCamera.m_Height, camTarget.position, Vector3.up, Quaternion.identity, targetCamera.m_Height, 1f);
            targetCamera.m_Height = Mathf.Clamp(targetCamera.m_Height, 5f, float.MaxValue);

            //Create Labels
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.UpperCenter;
            Handles.Label(camTarget.position + (-camTarget.forward * targetCamera.m_Distance), "Distance", labelStyle);

            labelStyle.alignment = TextAnchor.MiddleRight;
            Handles.Label(camTarget.position + (Vector3.up * targetCamera.m_Height), "Height", labelStyle);
        }

        #endregion


    }
}
